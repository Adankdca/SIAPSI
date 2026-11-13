using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using OfficeOpenXml;
using SIAP.Models;
using SIAP.Repositories;

namespace SIAP.Services
{
    /// <summary>
    /// Validación e importación del presupuesto inicial desde el Excel (hoja MAESTRO).
    ///
    /// Reglas:
    ///  - Las columnas se buscan por su encabezado, no por su posición.
    ///  - La longitud de la clave y sus segmentos salen de cat_estructuraclave para el año elegido.
    ///  - Si cualquier fila tiene error, se rechaza el archivo completo y se informa fila por fila.
    ///  - Un segmento que no existe en su catálogo NO detiene la carga: queda como advertencia.
    ///  - Un ejercicio solo se carga una vez (para volver a cargarlo, primero se limpia).
    /// </summary>
    public class PresupuestoInicialService
    {
        public static readonly string[] Meses =
            { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" };
        private static readonly string[] ColumnasMes =
            { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

        /// <summary>Máximo de errores por fila que se devuelven a la pantalla (el total se informa aparte).</summary>
        public const int MaxErroresMostrados = 500;

        private readonly EstructuraClaveRepository _estructuraRepo = new EstructuraClaveRepository();

        /// <summary>Fila de totales al final de la hoja (sin clave), si la trae. Solo se usa para cotejar.</summary>
        public FilaPresupuestoInicial FilaTotales { get; private set; }

        // ------------------------------------------------------------------ lectura del Excel

        private static string Normalizar(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            string d = s.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in d)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(c);
                if (cat == UnicodeCategory.NonSpacingMark) continue;
                if (char.IsWhiteSpace(c) || c == '_') continue;
                sb.Append(c);
            }
            return sb.ToString();
        }

        private static string Texto(object v)
        {
            if (v == null) return "";
            if (v is double) return ((double)v).ToString("0.##########", CultureInfo.InvariantCulture);
            return Convert.ToString(v, CultureInfo.InvariantCulture).Trim();
        }

        /// <summary>Convierte el valor de una celda a decimal redondeado a centavos. False si no es numérico.</summary>
        private static bool Importe(object v, out decimal valor)
        {
            valor = 0m;
            if (v == null) return true;
            try
            {
                if (v is double) { valor = Math.Round(Convert.ToDecimal((double)v), 2, MidpointRounding.AwayFromZero); return true; }
                if (v is decimal || v is int || v is long || v is float || v is short)
                {
                    valor = Math.Round(Convert.ToDecimal(v, CultureInfo.InvariantCulture), 2, MidpointRounding.AwayFromZero); return true;
                }
            }
            catch (OverflowException) { return false; }

            string s = Convert.ToString(v, CultureInfo.InvariantCulture).Trim().Replace("$", "").Replace(",", "").Replace(" ", "");
            if (s.Length == 0 || s == "-") return true;
            decimal d;
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out d))
            {
                valor = Math.Round(d, 2, MidpointRounding.AwayFromZero); return true;
            }
            return false;
        }

        /// <summary>
        /// Lee la hoja MAESTRO. Devuelve las filas con clave o importes; los errores de estructura
        /// del archivo van a erroresGenerales y los de cada fila a erroresFila.
        /// </summary>
        public List<FilaPresupuestoInicial> LeerExcel(Stream archivo, List<SegmentoEstructura> estructura,
                                                      List<string> erroresGenerales, List<ErrorFila> erroresFila)
        {
            var filas = new List<FilaPresupuestoInicial>();
            using (var package = new ExcelPackage(archivo))
            {
                var hojas = package.Workbook.Worksheets;
                ExcelWorksheet ws = hojas["MAESTRO"];
                if (ws == null && hojas.Count == 1) ws = hojas.First();
                if (ws == null)
                {
                    erroresGenerales.Add("El archivo debe tener una hoja llamada MAESTRO.");
                    return filas;
                }
                if (ws.Dimension == null)
                {
                    erroresGenerales.Add("La hoja MAESTRO está vacía.");
                    return filas;
                }

                int ultimaFila = ws.Dimension.End.Row;
                int ultimaCol = ws.Dimension.End.Column;

                // Encabezados (fila 1)
                var columnas = new Dictionary<string, int>();
                for (int c = 1; c <= ultimaCol; c++)
                {
                    string h = Normalizar(Texto(ws.Cells[1, c].Value));
                    if (h.Length > 0 && !columnas.ContainsKey(h)) columnas[h] = c;
                }

                int colClave = columnas.ContainsKey("claves") ? columnas["claves"] : columnas.ContainsKey("clave") ? columnas["clave"] : 0;
                var faltan = new List<string>();
                if (colClave == 0) faltan.Add("claves");
                foreach (var req in new[] { "aprobado", "noliberado" }.Concat(Meses))
                    if (!columnas.ContainsKey(req)) faltan.Add(req);
                if (faltan.Count > 0)
                {
                    erroresGenerales.Add("Faltan columnas en la hoja MAESTRO: " + string.Join(", ", faltan) + ".");
                    return filas;
                }

                // Columnas de segmento que traiga el archivo (UR, URO, FI...), para cotejarlas con la clave
                var colSegmento = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                foreach (var seg in estructura)
                {
                    int c;
                    if (columnas.TryGetValue(Normalizar(seg.Abreviatura), out c)) colSegmento[seg.Abreviatura] = c;
                }

                for (int r = 2; r <= ultimaFila; r++)
                {
                    string clave = Texto(ws.Cells[r, colClave].Value);
                    object vAprobado = ws.Cells[r, columnas["aprobado"]].Value;

                    bool filaVacia = clave.Length == 0 && (vAprobado == null || Texto(vAprobado).Length == 0);
                    if (filaVacia) continue;

                    var fila = new FilaPresupuestoInicial { Fila = r, Clave = clave };
                    var motivos = new List<string>();

                    decimal d;
                    if (Importe(vAprobado, out d)) fila.Aprobado = d; else motivos.Add("El aprobado no es un número.");
                    if (Importe(ws.Cells[r, columnas["noliberado"]].Value, out d)) fila.NoLiberado = d; else motivos.Add("El no liberado no es un número.");
                    for (int m = 0; m < 12; m++)
                    {
                        if (Importe(ws.Cells[r, columnas[Meses[m]]].Value, out d)) fila.Meses[m] = d;
                        else motivos.Add("El importe de " + Meses[m] + " no es un número.");
                    }

                    foreach (var kv in colSegmento)
                        fila.SegmentosExcel[kv.Key] = Texto(ws.Cells[r, kv.Value].Value);

                    fila.ErroresLectura = motivos;
                    filas.Add(fila);
                }
            }

            // Las filas sin clave al final de la hoja son la fila de totales: no se importan.
            FilaTotales = null;
            while (filas.Count > 0 && filas[filas.Count - 1].Clave.Length == 0)
            {
                FilaTotales = filas[filas.Count - 1];
                filas.RemoveAt(filas.Count - 1);
            }
            foreach (var f in filas)
                foreach (var mtv in f.ErroresLectura)
                    erroresFila.Add(new ErrorFila { Fila = f.Fila, Clave = f.Clave, Motivo = mtv });
            return filas;
        }

        // ------------------------------------------------------------------ validación

        public ResultadoValidacionPresupuesto Validar(Stream archivo, int anio, string oficio, string nombreArchivo,
                                                     out List<FilaPresupuestoInicial> filas)
        {
            var res = new ResultadoValidacionPresupuesto { Anio = anio, Oficio = oficio, Archivo = nombreArchivo };
            filas = new List<FilaPresupuestoInicial>();

            if (string.IsNullOrWhiteSpace(oficio)) res.ErroresGenerales.Add("Capture el número de oficio.");

            var estructura = _estructuraRepo.ObtenerEstructura(anio);
            if (estructura.Count == 0)
            {
                res.ErroresGenerales.Add("No hay estructura de clave configurada para " + anio +
                                         ". Agréguela en cat_estructuraclave antes de cargar.");
                return Cerrar(res);
            }
            res.LongitudClave = estructura[0].LongitudClave;

            var yaCargado = EjercicioCargado(anio);
            if (yaCargado != null) res.ErroresGenerales.Add(yaCargado);

            var errores = new List<ErrorFila>();
            filas = LeerExcel(archivo, estructura, res.ErroresGenerales, errores);
            if (filas.Count == 0 && res.ErroresGenerales.Count == 0)
                res.ErroresGenerales.Add("El archivo no tiene claves.");

            var servicioClave = new ClavePresupuestalService();
            var cp = estructura.FirstOrDefault(s => s.Abreviatura == "CP");
            var vistas = new Dictionary<string, int>();
            var sinCatalogo = new Dictionary<string, AdvertenciaCatalogo>();

            foreach (var f in filas)
            {
                // Clave
                if (f.Clave.Length == 0)
                {
                    errores.Add(new ErrorFila { Fila = f.Fila, Clave = "", Motivo = "La clave está vacía (solo la fila de totales, al final, puede ir sin clave)." });
                    continue;
                }
                if (f.Clave.Any(char.IsWhiteSpace))
                    errores.Add(new ErrorFila { Fila = f.Fila, Clave = f.Clave, Motivo = "La clave tiene espacios." });
                if (f.Clave.Length != res.LongitudClave)
                {
                    errores.Add(new ErrorFila
                    {
                        Fila = f.Fila, Clave = f.Clave,
                        Motivo = "La clave tiene " + f.Clave.Length + " caracteres; para " + anio + " deben ser " + res.LongitudClave + "."
                    });
                    continue;
                }

                int otra;
                if (vistas.TryGetValue(f.Clave, out otra))
                    errores.Add(new ErrorFila { Fila = f.Fila, Clave = f.Clave, Motivo = "Clave repetida (también está en la fila " + otra + ")." });
                else
                    vistas[f.Clave] = f.Fila;

                f.Segmentos = servicioClave.Separar(f.Clave, estructura);

                if (cp != null)
                {
                    var vcp = f.Segmentos.FirstOrDefault(s => s.Abreviatura == "CP");
                    if (vcp != null && vcp.Valor != anio.ToString())
                        errores.Add(new ErrorFila
                        {
                            Fila = f.Fila, Clave = f.Clave,
                            Motivo = "El ciclo presupuestal de la clave es " + vcp.Valor + " y se está cargando " + anio + "."
                        });
                }

                // Columnas de segmento del Excel contra la clave
                foreach (var sv in f.Segmentos)
                {
                    string enExcel;
                    if (!f.SegmentosExcel.TryGetValue(sv.Abreviatura, out enExcel) || enExcel.Length == 0) continue;
                    string comparar = enExcel;
                    long num;
                    if (comparar.Length < sv.Valor.Length && long.TryParse(comparar, out num))
                        comparar = comparar.PadLeft(sv.Valor.Length, '0'); // la celda perdió los ceros a la izquierda
                    if (!string.Equals(comparar, sv.Valor, StringComparison.OrdinalIgnoreCase))
                        errores.Add(new ErrorFila
                        {
                            Fila = f.Fila, Clave = f.Clave,
                            Motivo = "La columna " + sv.Abreviatura + " dice '" + enExcel + "' pero en la clave es '" + sv.Valor + "'."
                        });
                }

                // Importes
                if (f.Aprobado < 0 || f.NoLiberado < 0 || f.Meses.Any(m => m < 0))
                    errores.Add(new ErrorFila { Fila = f.Fila, Clave = f.Clave, Motivo = "Hay importes negativos." });

                decimal suma = f.Liberado + f.NoLiberado;
                if (suma != f.Aprobado)
                    errores.Add(new ErrorFila
                    {
                        Fila = f.Fila, Clave = f.Clave,
                        Motivo = string.Format(CultureInfo.GetCultureInfo("es-MX"),
                            "Meses ({0:C2}) + no liberado ({1:C2}) = {2:C2}, pero el aprobado es {3:C2}. Diferencia: {4:C2}.",
                            f.Liberado, f.NoLiberado, suma, f.Aprobado, f.Aprobado - suma)
                    });

                // Catálogos (solo advertencia)
                servicioClave.Resolver(f.Segmentos, estructura);
                foreach (var sv in f.Segmentos.Where(s => s.TablaCatalogo != null && !s.CatalogoId.HasValue))
                {
                    string k = sv.Abreviatura + "|" + sv.Valor;
                    AdvertenciaCatalogo adv;
                    if (!sinCatalogo.TryGetValue(k, out adv))
                    {
                        adv = new AdvertenciaCatalogo { Abreviatura = sv.Abreviatura, Nombre = sv.Nombre, Valor = sv.Valor };
                        sinCatalogo[k] = adv;
                    }
                    adv.Claves++;
                    res.SegmentosSinCatalogo++;
                }

                res.TotalAprobado += f.Aprobado;
                res.TotalLiberado += f.Liberado;
                res.TotalNoLiberado += f.NoLiberado;
            }

            res.TotalFilas = filas.Count;
            if (FilaTotales != null && FilaTotales.Aprobado != res.TotalAprobado)
                res.AdvertenciasGenerales.Add(string.Format(CultureInfo.GetCultureInfo("es-MX"),
                    "La fila de totales (fila {0}) dice {1:C2} de aprobado, pero la suma de las claves es {2:C2}.",
                    FilaTotales.Fila, FilaTotales.Aprobado, res.TotalAprobado));
            res.Advertencias = sinCatalogo.Values.OrderBy(a => a.Abreviatura).ThenBy(a => a.Valor).ToList();
            if (estructura.Any(s => string.Equals(s.SegmentoPadre, ClavePresupuestalService.SegmentoOrganismo, StringComparison.OrdinalIgnoreCase))
                && !servicioClave.OrgPubId.HasValue)
                res.AdvertenciasGenerales.Add("No está configurado el organismo (tblc_SIAP_Configuracion.Clave), por eso las UR no se pudieron buscar en su catálogo.");
            foreach (var t in servicioClave.CatalogosInvalidos)
                res.AdvertenciasGenerales.Add("El catálogo " + t + " configurado en cat_estructuraclave no existe o no tiene las columnas indicadas.");

            res.TotalErrores = errores.Count;
            res.Errores = errores.OrderBy(e => e.Fila).Take(MaxErroresMostrados).ToList();
            return Cerrar(res);
        }

        private static ResultadoValidacionPresupuesto Cerrar(ResultadoValidacionPresupuesto res)
        {
            res.TotalErrores = Math.Max(res.TotalErrores, res.Errores.Count);
            res.Valido = res.ErroresGenerales.Count == 0 && res.TotalErrores == 0;
            return res;
        }

        /// <summary>Mensaje si el ejercicio ya tiene presupuesto inicial cargado; null si no.</summary>
        private class InfoEjercicio
        {
            public int Claves { get; set; }
            public string Oficio { get; set; }
            public DateTime? Fecha { get; set; }
        }

        private class ClaveId
        {
            public string Clave { get; set; }
            public int Id { get; set; }
        }

        private class RegistroCarga
        {
            public int Id { get; set; }
            public string Anio { get; set; }
            public string Archivo { get; set; }
            public string RutaArchivo { get; set; }
        }

        public string EjercicioCargado(int anio)
        {
            using (var db = DbConnectionFactory.GetConnection())
            {
                var info = db.QueryFirstOrDefault<InfoEjercicio>(@"
                    SELECT COUNT(*) AS Claves, MAX(h.oficionumeroinicial) AS Oficio, MAX(h.fechacarga) AS Fecha
                    FROM dbo.presupuestocalendarizado p
                    LEFT JOIN dbo.tblp_pre_historialcargamasiva h ON h.id = p.CargaId
                    WHERE p.Ejercicio = @anio", new { anio });
                if (info == null || info.Claves == 0) return null;
                string fecha = info.Fecha.HasValue ? " el " + info.Fecha.Value.ToString("dd/MM/yyyy") : "";
                string of = string.IsNullOrEmpty(info.Oficio) ? "" : " con el oficio " + info.Oficio;
                return "El ejercicio " + anio + " ya tiene presupuesto inicial (" + info.Claves + " claves, cargado" + fecha + of +
                       "). Para volver a cargarlo, primero hay que limpiar los datos del ejercicio.";
            }
        }

        // ------------------------------------------------------------------ importación

        /// <summary>
        /// Importa filas ya validadas en una sola transacción. Devuelve el Id de la carga en el historial.
        /// </summary>
        public int Importar(List<FilaPresupuestoInicial> filas, ResultadoValidacionPresupuesto validacion,
                            string usuarioId, string rutaRelativa)
        {
            if (!validacion.Valido) throw new InvalidOperationException("El archivo no pasó la validación.");

            var columnasPresupuesto = _estructuraRepo.ColumnasDeTabla("presupuestocalendarizado");
            // Nombre exacto de cada columna (SqlBulkCopy distingue mayúsculas en los mapeos)
            Func<string, string> nombreReal = c => columnasPresupuesto.FirstOrDefault(x => string.Equals(x, c, StringComparison.OrdinalIgnoreCase));
            foreach (var f in filas)
                foreach (var sv in f.Segmentos)
                    if (sv.ColumnaFK != null) sv.ColumnaFK = nombreReal(sv.ColumnaFK); // null si la columna no existe
            // Columnas FK configuradas en la estructura que sí existen en la tabla
            var columnasFK = filas.SelectMany(f => f.Segmentos)
                .Where(s => s.ColumnaFK != null)
                .Select(s => s.ColumnaFK).Distinct().ToList();

            using (var conn = DbConnectionFactory.GetSqlConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        int cargaId = conn.QuerySingle<int>(@"
                            INSERT INTO dbo.tblp_pre_historialcargamasiva
                                (anio, oficionumeroinicial, fechacarga, usuariocreo, montototalaprobado,
                                 archivo, rutaarchivo, totalclaves, totalliberado, totalnoliberado, segmentossincatalogo)
                            VALUES (@anio, @oficio, GETDATE(), @usuario, @aprobado,
                                    @archivo, @ruta, @claves, @liberado, @noliberado, @sinCatalogo);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);",
                            new
                            {
                                anio = validacion.Anio.ToString(),
                                oficio = validacion.Oficio,
                                usuario = usuarioId,
                                aprobado = validacion.TotalAprobado,
                                archivo = validacion.Archivo,
                                ruta = rutaRelativa,
                                claves = filas.Count,
                                liberado = validacion.TotalLiberado,
                                noliberado = validacion.TotalNoLiberado,
                                sinCatalogo = validacion.SegmentosSinCatalogo
                            }, tx);

                        // 1. Claves
                        var tabla = new DataTable();
                        tabla.Columns.Add("ClavePresupuestal", typeof(string));
                        tabla.Columns.Add("Ejercicio", typeof(int));
                        tabla.Columns.Add("CargaId", typeof(int));
                        tabla.Columns.Add("Aprobado", typeof(decimal));
                        tabla.Columns.Add("PorLiberar", typeof(decimal));
                        tabla.Columns.Add("Liberado", typeof(decimal));
                        foreach (var mes in ColumnasMes) tabla.Columns.Add(mes, typeof(decimal));
                        foreach (var cero in new[] { "Aumentos", "Disminuciones", "Ministrado", "Devengado", "Pagado" })
                            if (columnasPresupuesto.Contains(cero)) tabla.Columns.Add(cero, typeof(decimal));
                        foreach (var fk in columnasFK) tabla.Columns.Add(fk, typeof(int));

                        foreach (var f in filas)
                        {
                            var row = tabla.NewRow();
                            row["ClavePresupuestal"] = f.Clave;
                            row["Ejercicio"] = validacion.Anio;
                            row["CargaId"] = cargaId;
                            row["Aprobado"] = f.Aprobado;
                            row["PorLiberar"] = f.NoLiberado;
                            row["Liberado"] = f.Liberado;
                            for (int m = 0; m < 12; m++) row[ColumnasMes[m]] = f.Meses[m];
                            foreach (var cero in new[] { "Aumentos", "Disminuciones", "Ministrado", "Devengado", "Pagado" })
                                if (tabla.Columns.Contains(cero)) row[cero] = 0m;
                            foreach (var fk in columnasFK)
                            {
                                var sv = f.Segmentos.FirstOrDefault(s => string.Equals(s.ColumnaFK, fk, StringComparison.OrdinalIgnoreCase));
                                row[fk] = sv != null && sv.CatalogoId.HasValue ? (object)sv.CatalogoId.Value : DBNull.Value;
                            }
                            tabla.Rows.Add(row);
                        }
                        Copiar(conn, tx, "dbo.presupuestocalendarizado", tabla);

                        // 2. Ids asignados
                        var ids = conn.Query<ClaveId>(
                            "SELECT ClavePresupuestal AS Clave, Id FROM dbo.presupuestocalendarizado WHERE CargaId = @cargaId",
                            new { cargaId }, tx).ToDictionary(k => k.Clave, k => k.Id);
                        if (ids.Count != filas.Count)
                            throw new InvalidOperationException("Se esperaban " + filas.Count + " claves y se insertaron " + ids.Count + ".");

                        // 3. Segmentos de cada clave
                        var segs = new DataTable();
                        segs.Columns.Add("PresupuestoCalendarizadoId", typeof(int));
                        segs.Columns.Add("Orden", typeof(int));
                        segs.Columns.Add("Abreviatura", typeof(string));
                        segs.Columns.Add("Valor", typeof(string));
                        segs.Columns.Add("TablaCatalogo", typeof(string));
                        segs.Columns.Add("CatalogoId", typeof(int));
                        foreach (var f in filas)
                        {
                            int pid = ids[f.Clave];
                            foreach (var sv in f.Segmentos)
                            {
                                segs.Rows.Add(pid, sv.Orden, sv.Abreviatura, sv.Valor,
                                    (object)sv.TablaCatalogo ?? DBNull.Value,
                                    sv.CatalogoId.HasValue ? (object)sv.CatalogoId.Value : DBNull.Value);
                            }
                        }
                        Copiar(conn, tx, "dbo.presupuesto_segmentos", segs);

                        tx.Commit();
                        return cargaId;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        private static void Copiar(SqlConnection conn, SqlTransaction tx, string destino, DataTable datos)
        {
            using (var bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.CheckConstraints, tx))
            {
                bulk.DestinationTableName = destino;
                bulk.BatchSize = 2000;
                bulk.BulkCopyTimeout = 600;
                foreach (DataColumn c in datos.Columns) bulk.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                bulk.WriteToServer(datos);
            }
        }

        // ------------------------------------------------------------------ historial

        public List<HistorialCargaDto> Historial()
        {
            using (var db = DbConnectionFactory.GetConnection())
            {
                return db.Query<HistorialCargaDto>(@"
                    SELECT h.id AS Id, h.anio AS Anio, h.oficionumeroinicial AS Oficio, h.archivo AS Archivo,
                           CAST(CASE WHEN h.rutaarchivo IS NULL THEN 0 ELSE 1 END AS BIT) AS TieneArchivo,
                           ISNULL(h.montototalaprobado, 0) AS MontoTotalAprobado, h.totalliberado AS TotalLiberado,
                           h.totalnoliberado AS TotalNoLiberado, h.totalclaves AS TotalClaves,
                           h.segmentossincatalogo AS SegmentosSinCatalogo, h.fechacarga AS FechaCarga,
                           ISNULL(u.NombreCompleto, h.usuariocreo) AS Usuario
                    FROM dbo.tblp_pre_historialcargamasiva h
                    LEFT JOIN dbo.usuarios u ON CAST(u.Id AS NVARCHAR(50)) = h.usuariocreo
                    ORDER BY h.fechacarga DESC, h.id DESC").ToList();
            }
        }

        public HistorialCargaDto ObtenerCarga(int id, out string rutaRelativa)
        {
            using (var db = DbConnectionFactory.GetConnection())
            {
                var r = db.QueryFirstOrDefault<RegistroCarga>(@"
                    SELECT id AS Id, anio AS Anio, archivo AS Archivo, rutaarchivo AS RutaArchivo
                    FROM dbo.tblp_pre_historialcargamasiva WHERE id = @id", new { id });
                if (r == null) { rutaRelativa = null; return null; }
                rutaRelativa = r.RutaArchivo;
                return new HistorialCargaDto { Id = r.Id, Anio = r.Anio, Archivo = r.Archivo };
            }
        }
    }
}
