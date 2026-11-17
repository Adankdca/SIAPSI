using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using SIAP.Models;
using SIAP.Repositories;

namespace SIAP.Services
{
    /// <summary>
    /// Consulta del presupuesto por ejercicio usando los segmentos guardados de cada clave
    /// (presupuesto_segmentos), no posiciones fijas dentro de la cadena. Así funciona igual
    /// para claves de 56, 58 o cualquier longitud que tenga el año.
    /// </summary>
    public class PresupuestoConsultaService
    {
        private readonly EstructuraClaveRepository _estructuraRepo = new EstructuraClaveRepository();
        private readonly ClavePresupuestalService _claves = new ClavePresupuestalService();

        private const string SumaMeses =
            "(ISNULL(p.Enero,0)+ISNULL(p.Febrero,0)+ISNULL(p.Marzo,0)+ISNULL(p.Abril,0)+ISNULL(p.Mayo,0)+ISNULL(p.Junio,0)+" +
            "ISNULL(p.Julio,0)+ISNULL(p.Agosto,0)+ISNULL(p.Septiembre,0)+ISNULL(p.Octubre,0)+ISNULL(p.Noviembre,0)+ISNULL(p.Diciembre,0))";

        private class SegmentoGuardado
        {
            public int Id { get; set; }
            public int PresupuestoCalendarizadoId { get; set; }
            public string Abreviatura { get; set; }
            public string Valor { get; set; }
            public int? CatalogoId { get; set; }
        }

        private class ClaveGuardada
        {
            public int Id { get; set; }
            public string ClavePresupuestal { get; set; }
        }

        private class OpcionCruda
        {
            public string Valor { get; set; }
            public int? CatalogoId { get; set; }
            public int Claves { get; set; }
        }

        // ------------------------------------------------------------------ ejercicios

        public List<EjercicioDto> Ejercicios()
        {
            var configurados = _estructuraRepo.AniosConfigurados();
            using (var db = DbConnectionFactory.GetConnection())
            {
                var conDatos = db.Query<EjercicioDto>(@"
                    SELECT Ejercicio AS Anio, COUNT(*) AS Claves
                    FROM dbo.presupuestocalendarizado
                    WHERE Ejercicio IS NOT NULL
                    GROUP BY Ejercicio").ToList();
                foreach (var a in configurados.Where(a => conDatos.All(e => e.Anio != a)))
                    conDatos.Add(new EjercicioDto { Anio = a, Claves = 0 });
                foreach (var e in conDatos) e.TieneEstructura = configurados.Contains(e.Anio);
                return conDatos.OrderByDescending(e => e.Anio).ToList();
            }
        }

        // ------------------------------------------------------------------ matriz

        private static readonly Dictionary<string, string> ColumnasOrden = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "clave", "p.ClavePresupuestal" },
            { "aprobado", "p.Aprobado" },
            { "modificado", "p.Modificado" },
            { "liberado", "p.Liberado" },
            { "porliberar", "p.PorLiberar" }
        };

        public ResultadoMatriz Matriz(FiltroMatriz f)
        {
            var estructura = _estructuraRepo.ObtenerEstructura(f.Anio);
            var porAbrev = estructura.ToDictionary(s => s.Abreviatura, StringComparer.OrdinalIgnoreCase);

            var prm = new DynamicParameters();
            prm.Add("Anio", f.Anio);
            var where = new StringBuilder("WHERE p.Ejercicio = @Anio");

            if (!string.IsNullOrWhiteSpace(f.Search))
            {
                where.Append(" AND p.ClavePresupuestal LIKE @Search");
                prm.Add("Search", "%" + f.Search.Trim() + "%");
            }
            if (f.Estado == "calendarizadas") where.Append(" AND " + SumaMeses + " > 0");
            else if (f.Estado == "porliberar") where.Append(" AND " + SumaMeses + " = 0");

            int i = 0;
            foreach (var kv in f.Segmentos.Where(k => porAbrev.ContainsKey(k.Key) && !string.IsNullOrWhiteSpace(k.Value)).Take(10))
            {
                where.AppendFormat(" AND EXISTS (SELECT 1 FROM dbo.presupuesto_segmentos s{0} WHERE s{0}.PresupuestoCalendarizadoId = p.Id AND s{0}.Abreviatura = @fa{0} AND s{0}.Valor = @fv{0})", i);
                prm.Add("fa" + i, porAbrev[kv.Key].Abreviatura);
                prm.Add("fv" + i, kv.Value.Trim());
                i++;
            }
            if (!string.IsNullOrWhiteSpace(f.Region))
            {
                where.Append(@" AND EXISTS (SELECT 1 FROM dbo.presupuesto_segmentos sr
                                  JOIN dbo.cat_municipio m ON m.Id = sr.CatalogoId
                                  JOIN dbo.cat_region r ON r.Id = m.RegionId
                                  WHERE sr.PresupuestoCalendarizadoId = p.Id AND sr.Abreviatura = N'MPIO' AND r.Descripcion = @Region)");
                prm.Add("Region", f.Region);
            }

            // Orden: columnas de importe o un segmento ("seg:UR")
            string orden;
            string col;
            if (!string.IsNullOrEmpty(f.OrdenColumna) && ColumnasOrden.TryGetValue(f.OrdenColumna, out col))
                orden = col;
            else if (f.OrdenColumna != null && f.OrdenColumna.StartsWith("seg:", StringComparison.OrdinalIgnoreCase)
                     && porAbrev.ContainsKey(f.OrdenColumna.Substring(4)))
            {
                orden = "(SELECT TOP 1 so.Valor FROM dbo.presupuesto_segmentos so WHERE so.PresupuestoCalendarizadoId = p.Id AND so.Abreviatura = @OrdAbrev)";
                prm.Add("OrdAbrev", porAbrev[f.OrdenColumna.Substring(4)].Abreviatura);
            }
            else orden = "p.ClavePresupuestal";
            orden += f.OrdenDescendente ? " DESC" : " ASC";
            if (!orden.StartsWith("p.ClavePresupuestal")) orden += ", p.ClavePresupuestal";

            int start = Math.Max(0, f.Start);
            int length = f.Length <= 0 ? int.MaxValue : f.Length;
            prm.Add("Start", start);
            prm.Add("Length", length);

            var res = new ResultadoMatriz();
            using (var db = DbConnectionFactory.GetConnection())
            {
                res.Total = db.ExecuteScalar<int>("SELECT COUNT(*) FROM dbo.presupuestocalendarizado WHERE Ejercicio = @Anio", new { f.Anio });
                res.Totales = db.QueryFirst<TotalesMatrizDto>(@"
                    SELECT COUNT(*) AS Claves,
                           ISNULL(SUM(ISNULL(p.Aprobado,0)),0) AS Aprobado,
                           ISNULL(SUM(ISNULL(p.Modificado,0)),0) AS Modificado,
                           ISNULL(SUM(ISNULL(p.Liberado,0)),0) AS Liberado,
                           ISNULL(SUM(ISNULL(p.PorLiberar,0)),0) AS PorLiberar
                    FROM dbo.presupuestocalendarizado p " + where, prm);
                res.Filtradas = res.Totales.Claves;

                res.Filas = db.Query<MatrizFilaDto>(@"
                    SELECT p.Id, p.ClavePresupuestal, p.Ejercicio,
                           ISNULL(p.Aprobado,0) AS Aprobado, ISNULL(p.Aumentos,0) AS Aumentos,
                           ISNULL(p.Disminuciones,0) AS Disminuciones, ISNULL(p.Modificado,0) AS Modificado,
                           ISNULL(p.Liberado,0) AS Liberado, ISNULL(p.PorLiberar,0) AS PorLiberar,
                           (SELECT ISNULL(SUM(a.Monto),0) FROM dbo.apartados_presupuesto a
                             WHERE a.ClavePresupuestal = p.ClavePresupuestal AND a.Activo = 1
                               AND a.FechaApartado >= DATEADD(DAY, -7, GETDATE())) AS SaldoApartado,
                           CAST(CASE WHEN " + SumaMeses + @" > 0 THEN 1 ELSE 0 END AS BIT) AS EsCalendarizada
                    FROM dbo.presupuestocalendarizado p " + where + @"
                    ORDER BY " + orden + @"
                    OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY", prm).ToList();

                LlenarSegmentos(db, res.Filas, porAbrev);
            }
            return res;
        }

        private void LlenarSegmentos(IDbConnection db, List<MatrizFilaDto> filas, Dictionary<string, SegmentoEstructura> porAbrev)
        {
            if (filas.Count == 0) return;
            var porId = filas.ToDictionary(x => x.Id);
            // De 1000 en 1000 para no rebasar el límite de 2100 parámetros de SQL Server
            for (int k = 0; k < filas.Count; k += 1000)
            {
                var ids = filas.Skip(k).Take(1000).Select(x => x.Id).ToList();
                var segs = db.Query<SegmentoGuardado>(@"
                    SELECT Id, PresupuestoCalendarizadoId, Abreviatura, Valor, CatalogoId
                    FROM dbo.presupuesto_segmentos WHERE PresupuestoCalendarizadoId IN @ids", new { ids });
                foreach (var s in segs)
                {
                    SegmentoEstructura est;
                    porAbrev.TryGetValue(s.Abreviatura, out est);
                    bool tieneCatalogo = est != null && !string.IsNullOrWhiteSpace(est.TablaCatalogo);
                    string desc = s.CatalogoId.HasValue ? _claves.DescripcionPorId(est, s.CatalogoId.Value) : null;
                    porId[s.PresupuestoCalendarizadoId].Segmentos[s.Abreviatura] = new SegmentoCelda
                    {
                        Valor = s.Valor,
                        Descripcion = desc,
                        SinCatalogo = tieneCatalogo && !s.CatalogoId.HasValue
                    };
                }
            }
        }

        /// <summary>Valores distintos de un segmento en el ejercicio, con su descripción, para los filtros.</summary>
        public List<OpcionSegmentoDto> OpcionesSegmento(int anio, string abreviatura)
        {
            var est = _estructuraRepo.ObtenerEstructura(anio)
                .FirstOrDefault(s => string.Equals(s.Abreviatura, abreviatura, StringComparison.OrdinalIgnoreCase));
            if (est == null) return new List<OpcionSegmentoDto>();
            using (var db = DbConnectionFactory.GetConnection())
            {
                var crudas = db.Query<OpcionCruda>(@"
                    SELECT s.Valor, MIN(s.CatalogoId) AS CatalogoId, COUNT(*) AS Claves
                    FROM dbo.presupuesto_segmentos s
                    JOIN dbo.presupuestocalendarizado p ON p.Id = s.PresupuestoCalendarizadoId
                    WHERE p.Ejercicio = @anio AND s.Abreviatura = @abrev
                    GROUP BY s.Valor ORDER BY s.Valor", new { anio, abrev = est.Abreviatura });
                return crudas.Select(c => new OpcionSegmentoDto
                {
                    Valor = c.Valor,
                    Claves = c.Claves,
                    Descripcion = c.CatalogoId.HasValue ? _claves.DescripcionPorId(est, c.CatalogoId.Value) : null
                }).ToList();
            }
        }

        // ------------------------------------------------------------------ catálogos

        /// <summary>Valores de segmento del ejercicio que no se encontraron en su catálogo.</summary>
        public List<ValorSinCatalogoDto> ValoresSinCatalogo(int anio)
        {
            using (var db = DbConnectionFactory.GetConnection())
            {
                return db.Query<ValorSinCatalogoDto>(@"
                    SELECT s.Orden, s.Abreviatura, e.Nombre, s.TablaCatalogo, s.Valor,
                           NULLIF(e.SegmentoPadre, N'ORGPUB') AS SegmentoPadre, sp.Valor AS ValorPadre,
                           COUNT(*) AS Claves, MIN(p.ClavePresupuestal) AS EjemploClave
                    FROM dbo.presupuesto_segmentos s
                    JOIN dbo.presupuestocalendarizado p ON p.Id = s.PresupuestoCalendarizadoId
                    LEFT JOIN dbo.cat_estructuraclave e ON e.Anio = p.Ejercicio AND e.Abreviatura = s.Abreviatura
                    LEFT JOIN dbo.presupuesto_segmentos sp ON sp.PresupuestoCalendarizadoId = s.PresupuestoCalendarizadoId
                                                         AND sp.Abreviatura = e.SegmentoPadre
                    WHERE p.Ejercicio = @anio AND s.TablaCatalogo IS NOT NULL AND s.CatalogoId IS NULL
                    GROUP BY s.Orden, s.Abreviatura, e.Nombre, s.TablaCatalogo, s.Valor, e.SegmentoPadre, sp.Valor
                    ORDER BY s.Orden, s.Valor, sp.Valor", new { anio }).ToList();
            }
        }

        /// <summary>
        /// Vuelve a buscar en los catálogos los segmentos de todas las claves del ejercicio.
        /// Sirve después de dar de alta valores que faltaban: actualiza CatalogoId en
        /// presupuesto_segmentos y las columnas Id de presupuestocalendarizado. No cambia importes.
        /// </summary>
        public ResultadoReproceso ReprocesarCatalogos(int anio)
        {
            var estructura = _estructuraRepo.ObtenerEstructura(anio);
            if (estructura.Count == 0) throw new ArgumentException("No hay estructura de clave para " + anio + ".");
            int longitud = estructura[0].LongitudClave;
            var columnasPresupuesto = _estructuraRepo.ColumnasDeTabla("presupuestocalendarizado");
            var res = new ResultadoReproceso();

            using (var conn = DbConnectionFactory.GetSqlConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        var claves = conn.Query<ClaveGuardada>(
                            "SELECT Id, ClavePresupuestal FROM dbo.presupuestocalendarizado WHERE Ejercicio = @anio",
                            new { anio }, tx).ToList();
                        var guardados = conn.Query<SegmentoGuardado>(@"
                            SELECT s.Id, s.PresupuestoCalendarizadoId, s.Abreviatura, s.Valor, s.CatalogoId
                            FROM dbo.presupuesto_segmentos s
                            JOIN dbo.presupuestocalendarizado p ON p.Id = s.PresupuestoCalendarizadoId
                            WHERE p.Ejercicio = @anio", new { anio }, tx)
                            .GroupBy(s => s.PresupuestoCalendarizadoId)
                            .ToDictionary(g => g.Key, g => g.ToDictionary(s => s.Abreviatura, StringComparer.OrdinalIgnoreCase));

                        res.Claves = claves.Count;
                        res.SinCatalogoAntes = guardados.Values.SelectMany(d => d.Values)
                            .Count(s => !s.CatalogoId.HasValue && estructura.Any(e => e.Abreviatura == s.Abreviatura && !string.IsNullOrWhiteSpace(e.TablaCatalogo)));

                        var cambios = new DataTable();
                        cambios.Columns.Add("Id", typeof(int));
                        cambios.Columns.Add("CatalogoId", typeof(int));
                        var nuevos = new DataTable();
                        foreach (var c in new[] { "PresupuestoCalendarizadoId", "Orden" }) nuevos.Columns.Add(c, typeof(int));
                        foreach (var c in new[] { "Abreviatura", "Valor", "TablaCatalogo" }) nuevos.Columns.Add(c, typeof(string));
                        nuevos.Columns.Add("CatalogoId", typeof(int));

                        foreach (var c in claves)
                        {
                            if (c.ClavePresupuestal == null || c.ClavePresupuestal.Length != longitud) continue;
                            var segs = _claves.Separar(c.ClavePresupuestal, estructura);
                            _claves.Resolver(segs, estructura);
                            Dictionary<string, SegmentoGuardado> previos;
                            guardados.TryGetValue(c.Id, out previos);
                            foreach (var sv in segs)
                            {
                                SegmentoGuardado g = null;
                                if (previos != null && previos.TryGetValue(sv.Abreviatura, out g))
                                {
                                    if (g.CatalogoId != sv.CatalogoId)
                                        cambios.Rows.Add(g.Id, sv.CatalogoId.HasValue ? (object)sv.CatalogoId.Value : DBNull.Value);
                                }
                                else
                                {
                                    // Clave guardada sin sus segmentos (p. ej. capturada antes de este cambio)
                                    nuevos.Rows.Add(c.Id, sv.Orden, sv.Abreviatura, sv.Valor,
                                        (object)sv.TablaCatalogo ?? DBNull.Value,
                                        sv.CatalogoId.HasValue ? (object)sv.CatalogoId.Value : DBNull.Value);
                                }
                            }
                        }

                        if (cambios.Rows.Count > 0)
                        {
                            conn.Execute("CREATE TABLE #cambios (Id INT PRIMARY KEY, CatalogoId INT NULL);", null, tx);
                            using (var bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tx) { DestinationTableName = "#cambios" })
                                bulk.WriteToServer(cambios);
                            conn.Execute(@"UPDATE s SET s.CatalogoId = t.CatalogoId
                                           FROM dbo.presupuesto_segmentos s JOIN #cambios t ON t.Id = s.Id;
                                           DROP TABLE #cambios;", null, tx);
                        }
                        if (nuevos.Rows.Count > 0)
                        {
                            using (var bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.CheckConstraints, tx) { DestinationTableName = "dbo.presupuesto_segmentos" })
                            {
                                foreach (DataColumn dc in nuevos.Columns) bulk.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                                bulk.WriteToServer(nuevos);
                            }
                        }
                        res.SegmentosActualizados = cambios.Rows.Count + nuevos.Rows.Count;

                        // Columnas Id de la tabla de presupuesto (UroId, FuncionId, ...) según la estructura
                        foreach (var seg in estructura.Where(s => !string.IsNullOrWhiteSpace(s.ColumnaFK)))
                        {
                            string colReal = columnasPresupuesto.FirstOrDefault(x => string.Equals(x, seg.ColumnaFK, StringComparison.OrdinalIgnoreCase));
                            if (colReal == null || colReal.Contains("]")) continue;
                            conn.Execute(@"
                                UPDATE p SET p.[" + colReal + @"] = s.CatalogoId
                                FROM dbo.presupuestocalendarizado p
                                JOIN dbo.presupuesto_segmentos s ON s.PresupuestoCalendarizadoId = p.Id AND s.Abreviatura = @abrev
                                WHERE p.Ejercicio = @anio AND ISNULL(p.[" + colReal + @"], -1) <> ISNULL(s.CatalogoId, -1)",
                                new { anio, abrev = seg.Abreviatura }, tx);
                        }

                        res.SinCatalogoDespues = conn.ExecuteScalar<int>(@"
                            SELECT COUNT(*) FROM dbo.presupuesto_segmentos s
                            JOIN dbo.presupuestocalendarizado p ON p.Id = s.PresupuestoCalendarizadoId
                            WHERE p.Ejercicio = @anio AND s.TablaCatalogo IS NOT NULL AND s.CatalogoId IS NULL",
                            new { anio }, tx);
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            return res;
        }

        // ------------------------------------------------------------------ captura manual

        /// <summary>
        /// Da de alta una clave individual en el ejercicio: todo el aprobado queda por liberar
        /// (sin calendario) hasta que una adecuación de liberación lo calendarice.
        /// Devuelve los segmentos de la clave para mostrar los que no están en su catálogo.
        /// </summary>
        public List<SegmentoValor> InsertarClaveManual(NuevaClaveRequest req)
        {
            if (req == null) throw new ArgumentException("No se recibieron datos.");
            string clave = (req.ClavePresupuestal ?? "").Trim().ToUpperInvariant();
            var estructura = _estructuraRepo.ObtenerEstructura(req.Anio);
            if (estructura.Count == 0) throw new ArgumentException("No hay estructura de clave para " + req.Anio + ".");
            int longitud = estructura[0].LongitudClave;

            if (clave.Length == 0) throw new ArgumentException("Capture la clave.");
            if (clave.Any(char.IsWhiteSpace)) throw new ArgumentException("La clave no debe tener espacios.");
            if (clave.Length != longitud)
                throw new ArgumentException("La clave tiene " + clave.Length + " caracteres; para " + req.Anio + " deben ser " + longitud + ".");
            if (req.Aprobado <= 0) throw new ArgumentException("El monto aprobado debe ser mayor a 0.");
            decimal aprobado = Math.Round(req.Aprobado, 2, MidpointRounding.AwayFromZero);

            var segs = _claves.Separar(clave, estructura);
            var cp = segs.FirstOrDefault(s => s.Abreviatura == "CP");
            if (cp != null && cp.Valor != req.Anio.ToString())
                throw new ArgumentException("El ciclo presupuestal de la clave es " + cp.Valor + " y se está capturando en " + req.Anio + ".");
            _claves.Resolver(segs, estructura);

            var columnasPresupuesto = _estructuraRepo.ColumnasDeTabla("presupuestocalendarizado");
            var fks = new List<KeyValuePair<string, int?>>();
            foreach (var sv in segs.Where(s => s.ColumnaFK != null))
            {
                string colReal = columnasPresupuesto.FirstOrDefault(x => string.Equals(x, sv.ColumnaFK, StringComparison.OrdinalIgnoreCase));
                if (colReal != null && !colReal.Contains("]") && fks.All(k => k.Key != colReal))
                    fks.Add(new KeyValuePair<string, int?>(colReal, sv.CatalogoId));
            }

            using (var conn = DbConnectionFactory.GetSqlConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        int existe = conn.ExecuteScalar<int>(
                            "SELECT COUNT(*) FROM dbo.presupuestocalendarizado WITH (UPDLOCK, HOLDLOCK) WHERE ClavePresupuestal = @clave",
                            new { clave }, tx);
                        if (existe > 0) throw new ArgumentException("La clave ya existe en el presupuesto.");

                        var prm = new DynamicParameters();
                        prm.Add("clave", clave);
                        prm.Add("anio", req.Anio);
                        prm.Add("aprobado", aprobado);
                        var cols = new StringBuilder();
                        var vals = new StringBuilder();
                        int n = 0;
                        foreach (var fk in fks)
                        {
                            cols.Append(", [").Append(fk.Key).Append("]");
                            vals.Append(", @fk").Append(n);
                            prm.Add("fk" + n, fk.Value);
                            n++;
                        }
                        int id = conn.QuerySingle<int>(@"
                            INSERT INTO dbo.presupuestocalendarizado
                                (ClavePresupuestal, Ejercicio, Aprobado, PorLiberar, Liberado,
                                 Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre,
                                 Aumentos, Disminuciones, Ministrado, Devengado, Pagado" + cols + @")
                            VALUES (@clave, @anio, @aprobado, @aprobado, 0,
                                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                    0, 0, 0, 0, 0" + vals + @");
                            SELECT CAST(SCOPE_IDENTITY() AS INT);", prm, tx);

                        conn.Execute(@"
                            INSERT INTO dbo.presupuesto_segmentos (PresupuestoCalendarizadoId, Orden, Abreviatura, Valor, TablaCatalogo, CatalogoId)
                            VALUES (@Pid, @Orden, @Abreviatura, @Valor, @TablaCatalogo, @CatalogoId)",
                            segs.Select(s => new { Pid = id, s.Orden, s.Abreviatura, s.Valor, s.TablaCatalogo, s.CatalogoId }), tx);
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            return segs;
        }
    }
}
