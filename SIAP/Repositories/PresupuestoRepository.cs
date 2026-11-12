using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using SIAP.Models;

namespace SIAP.Repositories
{
    public class PresupuestoRepository
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString;

        private IDbConnection DbConnection
        {
            get { return new MySqlConnection(_connectionString); }
        }

        /// <summary>
        /// Obtiene todos los registros de la matriz de presupuesto para el ejercicio actual.
        /// Realiza subconsultas con 'LIMIT 1' a catálogos asociados a los segmentos de la Clave Presupuestal 
        /// (como UR, Programa, Objeto de Gasto, Municipio y Región) para enriquecer la respuesta, evitando filas duplicadas en caso de catálogos sucios.
        /// También calcula si una clave está calendarizada sumando los meses, y obtiene el saldo disponible en los apartados activos de los últimos 7 días.
        /// </summary>
        /// <returns>Una enumeración de objetos <see cref="PresupuestoGridDto"/> que representan la matriz presupuestal con descripciones asociadas.</returns>
        public IEnumerable<PresupuestoGridDto> ObtenerMatrizPresupuesto()
        {
            using (IDbConnection db = DbConnection)
            {
                int mesCorte = 6;
                try
                {
                    var settingsMes = System.Configuration.ConfigurationManager.AppSettings["MesCorte"];
                    if (int.TryParse(settingsMes, out int parsedMes))
                    {
                        mesCorte = parsedMes;
                    }
                }
                catch { }

                // Usamos subconsultas con LIMIT 1 para evitar filas duplicadas en caso de catálogos sucios
                string query = $@"
                    SELECT 
                        p.Id, 
                        p.ClavePresupuestal,
                        (SELECT Descripcion FROM cat_ur WHERE Clave = SUBSTRING(p.ClavePresupuestal, 1, 2) LIMIT 1) AS UrDescripcion,
                        (SELECT Descripcion FROM cat_programapresupuestario WHERE Clave = SUBSTRING(p.ClavePresupuestal, 11, 3) LIMIT 1) AS ProgramaDescripcion,
                        (SELECT Descripcion FROM cat_objetogastointegrar WHERE Clave = SUBSTRING(p.ClavePresupuestal, 29, 5) LIMIT 1) AS ObjetoGastoDescripcion,
                        p.Aprobado,
                         (IFNULL(p.Aprobado, 0) + IFNULL(p.Aumentos, 0) - IFNULL(p.Disminuciones, 0)) AS Modificado,
                        p.Pagado,
                        p.Pagado AS Ministerio,
                        (SELECT IFNULL(SUM(Monto), 0) FROM apartados_presupuesto a WHERE a.ClavePresupuestal = p.ClavePresupuestal AND a.Activo = 1 AND a.FechaApartado >= DATE_SUB(NOW(), INTERVAL 7 DAY)) AS SaldoApartado,
                        CASE WHEN (p.Enero + p.Febrero + p.Marzo + p.Abril + p.Mayo + p.Junio + p.Julio + p.Agosto + p.Septiembre + p.Octubre + p.Noviembre + p.Diciembre) > 0 THEN 1 ELSE 0 END AS EsCalendarizada,
                        SUBSTRING(p.ClavePresupuestal, 23, 3) AS ClaveMunicipio,
                        (SELECT Nombre FROM cat_municipio WHERE Clave = SUBSTRING(p.ClavePresupuestal, 23, 3) LIMIT 1) AS NombreMunicipio,
                        (SELECT r.Descripcion FROM cat_region r JOIN cat_municipio m ON r.Id = m.RegionId WHERE m.Clave = SUBSTRING(p.ClavePresupuestal, 23, 3) LIMIT 1) AS NombreRegion
                    FROM presupuestocalendarizado p;
                ";
                return db.Query<PresupuestoGridDto>(query);
            }
        }

        /// <summary>
        /// Obtiene la matriz presupuestal de manera paginada, ideal para cargar grandes volúmenes de datos en DataTables (server-side processing).
        /// Permite filtrar los registros por término de búsqueda (que coincida con la Clave Presupuestal), municipio, región y estado de calendarización.
        /// Primero ejecuta una consulta de COUNT para obtener el total de registros filtrados, y luego ejecuta la consulta principal aplicando LIMIT y ORDER BY.
        /// </summary>
        /// <param name="start">Índice del primer registro a devolver (para paginación).</param>
        /// <param name="length">Número máximo de registros a devolver por página.</param>
        /// <param name="search">Texto de búsqueda parcial sobre la Clave Presupuestal.</param>
        /// <param name="mpio">Filtro exacto por el nombre del Municipio (segmento 23-3 de la clave).</param>
        /// <param name="region">Filtro exacto por el nombre de la Región asociada al Municipio.</param>
        /// <param name="estado">Filtro por estado ('calendarizadas' o 'porliberar'). Evalúa si la suma de los meses de enero a diciembre es mayor a cero o igual a cero.</param>
        /// <returns>Una tupla donde el primer elemento es la lista de <see cref="PresupuestoGridDto"/> correspondiente a la página y el segundo es el conteo total de registros que coinciden con los filtros.</returns>
        public (IEnumerable<PresupuestoGridDto>, int) ObtenerMatrizPresupuestoPaginado(int start, int length, string search, string mpio = "", string region = "", string estado = "")
        {
            using (IDbConnection db = DbConnection)
            {
                int mesCorte = 6;
                try
                {
                    var settingsMes = System.Configuration.ConfigurationManager.AppSettings["MesCorte"];
                    if (int.TryParse(settingsMes, out int parsedMes))
                    {
                        mesCorte = parsedMes;
                    }
                }
                catch { }

                var parameters = new DynamicParameters();
                parameters.Add("Start", start);
                parameters.Add("Length", length);

                string whereClause = "WHERE 1=1";
                if (!string.IsNullOrEmpty(search))
                {
                    whereClause += " AND p.ClavePresupuestal LIKE @Search";
                    parameters.Add("Search", "%" + search + "%");
                }
                if (!string.IsNullOrEmpty(mpio))
                {
                    whereClause += " AND (SELECT Nombre FROM cat_municipio WHERE Clave = SUBSTRING(p.ClavePresupuestal, 23, 3) LIMIT 1) = @Mpio";
                    parameters.Add("Mpio", mpio);
                }
                if (!string.IsNullOrEmpty(region))
                {
                    whereClause += " AND (SELECT r.Descripcion FROM cat_region r JOIN cat_municipio m ON r.Id = m.RegionId WHERE m.Clave = SUBSTRING(p.ClavePresupuestal, 23, 3) LIMIT 1) = @Region";
                    parameters.Add("Region", region);
                }
                if (!string.IsNullOrEmpty(estado) && estado != "all")
                {
                    if (estado == "calendarizadas")
                    {
                        whereClause += " AND (p.Enero + p.Febrero + p.Marzo + p.Abril + p.Mayo + p.Junio + p.Julio + p.Agosto + p.Septiembre + p.Octubre + p.Noviembre + p.Diciembre) > 0";
                    }
                    else if (estado == "porliberar")
                    {
                        whereClause += " AND (p.Enero + p.Febrero + p.Marzo + p.Abril + p.Mayo + p.Junio + p.Julio + p.Agosto + p.Septiembre + p.Octubre + p.Noviembre + p.Diciembre) = 0";
                    }
                }

                string countQuery = $"SELECT COUNT(1) FROM presupuestocalendarizado p {whereClause}";
                int totalCount = db.ExecuteScalar<int>(countQuery, parameters);

                string query = $@"
                    SELECT 
                        p.Id, 
                        p.ClavePresupuestal,
                        (SELECT Descripcion FROM cat_ur WHERE Clave = SUBSTRING(p.ClavePresupuestal, 1, 2) LIMIT 1) AS UrDescripcion,
                        (SELECT Descripcion FROM cat_programapresupuestario WHERE Clave = SUBSTRING(p.ClavePresupuestal, 11, 3) LIMIT 1) AS ProgramaDescripcion,
                        (SELECT Descripcion FROM cat_objetogastointegrar WHERE Clave = SUBSTRING(p.ClavePresupuestal, 29, 5) LIMIT 1) AS ObjetoGastoDescripcion,
                        p.Aprobado,
                         (IFNULL(p.Aprobado, 0) + IFNULL(p.Aumentos, 0) - IFNULL(p.Disminuciones, 0)) AS Modificado,
                        p.Pagado,
                        p.Pagado AS Ministerio,
                        (SELECT IFNULL(SUM(Monto), 0) FROM apartados_presupuesto a WHERE a.ClavePresupuestal = p.ClavePresupuestal AND a.Activo = 1 AND a.FechaApartado >= DATE_SUB(NOW(), INTERVAL 7 DAY)) AS SaldoApartado,
                        CASE WHEN (p.Enero + p.Febrero + p.Marzo + p.Abril + p.Mayo + p.Junio + p.Julio + p.Agosto + p.Septiembre + p.Octubre + p.Noviembre + p.Diciembre) > 0 THEN 1 ELSE 0 END AS EsCalendarizada,
                        SUBSTRING(p.ClavePresupuestal, 23, 3) AS ClaveMunicipio,
                        (SELECT Nombre FROM cat_municipio WHERE Clave = SUBSTRING(p.ClavePresupuestal, 23, 3) LIMIT 1) AS NombreMunicipio,
                        (SELECT r.Descripcion FROM cat_region r JOIN cat_municipio m ON r.Id = m.RegionId WHERE m.Clave = SUBSTRING(p.ClavePresupuestal, 23, 3) LIMIT 1) AS NombreRegion
                    FROM presupuestocalendarizado p
                    {whereClause}
                    ORDER BY p.Id DESC
                    LIMIT @Start, @Length;
                ";
                var data = db.Query<PresupuestoGridDto>(query, parameters);
                
                return (data, totalCount);
            }
        }

        /// <summary>
        /// Consulta los montos detallados de una Clave Presupuestal en específico, incluyendo montos aprobados, modificados, devengados, pagados,
        /// así como la calendarización mensual (Enero a Diciembre) y el saldo total en apartados vigentes.
        /// Además, si la clave existe, llena su propiedad 'Apartados' con el listado histórico de los apartados de los últimos 7 días.
        /// </summary>
        /// <param name="clave">La Clave Presupuestal de 56 caracteres a consultar.</param>
        /// <returns>Un objeto <see cref="PresupuestoDetalleDto"/> con la información detallada de la clave o null si no se encuentra.</returns>
        public PresupuestoDetalleDto ObtenerDetalleClave(string clave)
        {
            using (IDbConnection db = DbConnection)
            {
                string query = @"
                    SELECT 
                        p.ClavePresupuestal,
                        p.Aprobado, 
                         (IFNULL(p.Aprobado, 0) + IFNULL(p.Aumentos, 0) - IFNULL(p.Disminuciones, 0)) AS Modificado, 
                        p.Devengado, p.Pagado, p.Disminuciones AS GastadoPorAdecuacion,
                        p.Enero, p.Febrero, p.Marzo, p.Abril, p.Mayo, p.Junio,
                        p.Julio, p.Agosto, p.Septiembre, p.Octubre, p.Noviembre, p.Diciembre,
                        (SELECT IFNULL(SUM(Monto), 0) FROM apartados_presupuesto WHERE ClavePresupuestal = @Clave AND Activo = 1 AND FechaApartado >= DATE_SUB(NOW(), INTERVAL 7 DAY)) AS SaldoApartado
                    FROM presupuestocalendarizado p
                    WHERE p.ClavePresupuestal = @Clave
                    LIMIT 1;
                ";
                var dto = db.QueryFirstOrDefault<PresupuestoDetalleDto>(query, new { Clave = clave });
                
                if (dto != null)
                {
                    int anioCorte = 2025;
                    int mesCorte = 6;
                    
                    try
                    {
                        var settingsAnio = System.Configuration.ConfigurationManager.AppSettings["AnioCorte"];
                        var settingsMes = System.Configuration.ConfigurationManager.AppSettings["MesCorte"];
                        if (!string.IsNullOrEmpty(settingsAnio)) anioCorte = int.Parse(settingsAnio);
                        if (!string.IsNullOrEmpty(settingsMes)) mesCorte = int.Parse(settingsMes);
                    }
                    catch { }

                    string cpStr = clave.Length >= 5 ? clave.Substring(clave.Length - 5, 4) : "";
                    int anioClave = anioCorte;
                    int.TryParse(cpStr, out anioClave);

                    // Poblamos la lista de apartados activos
                    string apQuery = "SELECT Id, Monto, Motivo, Oficio, FechaApartado FROM apartados_presupuesto WHERE ClavePresupuestal = @Clave AND Activo = 1 AND FechaApartado >= DATE_SUB(NOW(), INTERVAL 7 DAY) ORDER BY FechaApartado DESC";
                    dto.Apartados = db.Query<ApartadoDto>(apQuery, new { Clave = clave }).AsList();


                }
                return dto;
            }
        }

        /// <summary>
        /// Descompone una Clave Presupuestal de 56 caracteres en sus segmentos lógicos (UR, Programa, Objeto de Gasto, Municipio, etc.).
        /// Recorre una configuración predefinida de segmentos y su longitud, consultando en base de datos la descripción 
        /// de cada segmento desde su tabla de catálogo respectiva para generar un objeto detallado por segmento.
        /// Tiene lógica específica para el Ciclo Presupuestal y para los Proyectos Estratégicos.
        /// </summary>
        /// <param name="clave">La Clave Presupuestal que se desea decodificar y separar en segmentos.</param>
        /// <returns>Un objeto <see cref="ClaveDecodificadaDto"/> que contiene una lista de segmentos, cada uno con su valor y descripción extraída de los catálogos.</returns>
        public ClaveDecodificadaDto DecodificarClave(string clave)
        {
            var res = new ClaveDecodificadaDto();
            if (string.IsNullOrEmpty(clave)) return res;

            var segmentosConfig = new [] {
                new { N = "Unidad Responsable", A = "UR", L = 2, T = "cat_ur" },
                new { N = "Unidad Responsable Op.", A = "UR0", L = 3, T = "cat_unidadresponsableoperativa" },
                new { N = "Finalidad", A = "FI", L = 1, T = "cat_finalidad" },
                new { N = "Función", A = "F", L = 1, T = "cat_funcion" },
                new { N = "Subfunción", A = "SF", L = 1, T = "cat_subfuncion" },
                new { N = "Subsubfunción", A = "SSF", L = 1, T = "cat_subsubfuncion" },
                new { N = "Programa Sectorial", A = "PS", L = 1, T = "cat_programasectorial" },
                new { N = "Programa Presupuestario", A = "PP", L = 3, T = "cat_programapresupuestario" },
                new { N = "Origen de Asignación", A = "OA", L = 2, T = "cat_origenasignacion" },
                new { N = "Actividad Institucional", A = "AI", L = 3, T = "cat_actividadinstitucional" },
                new { N = "Proyecto Estratégico", A = "PT", L = 4, T = "cat_proyectoestrategico" },
                new { N = "Municipio", A = "MPIO", L = 3, T = "cat_municipio" },
                new { N = "Grupo Asignación", A = "GA", L = 1, T = "cat_asignaciongasto" },
                new { N = "Clasif. Objeto Gasto", A = "COGC", L = 2, T = "cat_objetogasto" },
                new { N = "Gasto", A = "GASTO", L = 5, T = "cat_objetogastointegrar" },
                new { N = "Gasto (Objeto)", A = "GTO", L = 2, T = "cat_orientaciongasto" },
                new { N = "Año de Gasto", A = "AG", L = 2, T = "cat_aniogasto" },
                new { N = "Tipo de Gasto", A = "TG", L = 1, T = "cat_tipogasto" },
                new { N = "Tipo de Recurso", A = "TR", L = 1, T = "cat_tiporecurso" },
                new { N = "Fuente Financiamiento", A = "FF", L = 1, T = "cat_fuentefinanciamiento" },
                new { N = "Subfuente Financiamiento", A = "SFTE", L = 1, T = "cat_subfuentefinanciamiento" },
                new { N = "Ramo", A = "RM", L = 2, T = "cat_ramo" },
                new { N = "Programa Fondo", A = "PF", L = 5, T = "cat_programafondo" },
                new { N = "Rubro de Ingresos", A = "RI", L = 3, T = "cat_rubroingresos" },
                new { N = "Ciclo Presupuestal", A = "CP", L = 4, T = "" },
                new { N = "Dígito Ministración", A = "DM", L = 1, T = "cat_digitosministracion" }
            };

            int currentIndex = 0;
            using (IDbConnection db = DbConnection)
            {
                foreach (var seg in segmentosConfig)
                {
                    if (currentIndex + seg.L > clave.Length) break;

                    string valor = clave.Substring(currentIndex, seg.L);
                    string descripcion = "";
                    
                    try
                    {
                        if (seg.A == "CP")
                        {
                            // Es el Ciclo Presupuestal (ej. 2025), el valor literal es la descripción
                            descripcion = $"Año {valor}";
                        }
                        else if (seg.A == "PT" && valor.Length == 4)
                        {
                            // Lógica especial para Proyecto Estratégico (ej. A001)
                            string letraCatalogo = valor.Substring(0, 1);
                            string numeroProyecto = valor.Substring(1, 3);
                            string query = $"SELECT Descripcion FROM {seg.T} WHERE Clave = @c LIMIT 1";
                            string descCatalogo = db.QueryFirstOrDefault<string>(query, new { c = letraCatalogo });
                            
                            if (!string.IsNullOrEmpty(descCatalogo))
                            {
                                descripcion = $"{descCatalogo} - Proyecto {numeroProyecto}";
                            }
                            else
                            {
                                descripcion = $"Proyecto {numeroProyecto}";
                            }
                        }
                        else
                        {
                            string colName = seg.T == "cat_municipio" ? "Nombre" : "Descripcion";
                            string query = $"SELECT {colName} FROM {seg.T} WHERE Clave = @c LIMIT 1";
                            descripcion = db.QueryFirstOrDefault<string>(query, new { c = valor });
                        }
                    }
                    catch (Exception)
                    {
                        // Ignore table not found or column not found
                    }

                    res.Segmentos.Add(new SegmentoClaveDto
                    {
                        Nombre = seg.N,
                        Abreviatura = seg.A,
                        Valor = valor,
                        Descripcion = descripcion ?? ""
                    });

                    currentIndex += seg.L;
                }
            }
            return res;
        }

        /// <summary>
        /// Actualiza los montos mensuales programados (calendarización) en la tabla 'presupuestocalendarizado' para una clave presupuestal.
        /// Realiza un UPDATE en base de datos asignando los valores de los meses (Enero a Diciembre) enviados en la solicitud.
        /// </summary>
        /// <param name="req">Objeto <see cref="PresupuestoDetalleDto"/> con los montos mensuales y la Clave Presupuestal.</param>
        public void GuardarCalendarizacion(PresupuestoDetalleDto req)
        {
            using (IDbConnection db = DbConnection)
            {
                string query = @"
                    UPDATE presupuestocalendarizado SET
                        Enero = @Enero, Febrero = @Febrero, Marzo = @Marzo, Abril = @Abril,
                        Mayo = @Mayo, Junio = @Junio, Julio = @Julio, Agosto = @Agosto,
                        Septiembre = @Septiembre, Octubre = @Octubre, Noviembre = @Noviembre, Diciembre = @Diciembre
                    WHERE ClavePresupuestal = @ClavePresupuestal
                ";
                db.Execute(query, req);
            }
        }

        public void InsertarNuevaClaveManual(string clave, decimal aprobado)
        {
            using (IDbConnection db = DbConnection)
            {
                int existe = db.QueryFirstOrDefault<int>("SELECT COUNT(*) FROM presupuestocalendarizado WHERE ClavePresupuestal = @c", new { c = clave });
                if (existe > 0) throw new Exception("La Clave Presupuestal ya existe en la base de datos.");

                string query = @"
                    INSERT INTO presupuestocalendarizado 
                    (ClavePresupuestal, Aprobado, Modificado, Devengado, Pagado,
                     Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre)
                    VALUES 
                    (@c, @a, @a, 0, 0,
                     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                ";
                db.Execute(query, new { c = clave, a = aprobado });
            }
        }

        public void InsertarPresupuestoMasivo(CargaMasivaDto masiva, string usuarioId)
        {
            using (IDbConnection db = DbConnection)
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. Guardar el registro del archivo/carga en el historial
                        string queryHistorial = @"
                            INSERT INTO tblp_pre_historialcargamasiva 
                            (anio, oficionumeroinicial, fechacarga, usuariocreo, montototalaprobado)
                            VALUES 
                            (@anio, @oficio, NOW(), @usuario, @monto)
                        ";
                        
                        decimal montoTotal = masiva.Claves.Sum(c => c.Aprobado);
                        db.Execute(queryHistorial, new { anio = masiva.Anio, oficio = masiva.Oficio, usuario = usuarioId, monto = montoTotal }, transaction);

                        // 2. Insertar claves masivamente
                        string queryInsert = @"
                            INSERT INTO presupuestocalendarizado 
                            (ClavePresupuestal, Aprobado, Devengado, Pagado,
                             Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre)
                            VALUES 
                            (@ClavePresupuestal, @Aprobado, 0, 0,
                             @Enero, @Febrero, @Marzo, @Abril, @Mayo, @Junio, @Julio, @Agosto, @Septiembre, @Octubre, @Noviembre, @Diciembre)
                            ON DUPLICATE KEY UPDATE 
                                Aprobado = VALUES(Aprobado),
                                Enero = VALUES(Enero), Febrero = VALUES(Febrero), Marzo = VALUES(Marzo), Abril = VALUES(Abril),
                                Mayo = VALUES(Mayo), Junio = VALUES(Junio), Julio = VALUES(Julio), Agosto = VALUES(Agosto),
                                Septiembre = VALUES(Septiembre), Octubre = VALUES(Octubre), Noviembre = VALUES(Noviembre), Diciembre = VALUES(Diciembre)
                        ";
                        
                        db.Execute(queryInsert, masiva.Claves, transaction);
                        
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        public IEnumerable<PresupuestoDetalleDto> ObtenerClavesPorObra(string prefijo22Digitos)
        {
            if (string.IsNullOrEmpty(prefijo22Digitos) || prefijo22Digitos.Length < 22)
            {
                return Enumerable.Empty<PresupuestoDetalleDto>();
            }

            using (IDbConnection db = DbConnection)
            {
                // Solo traemos las claves que empiezan con los mismos 22 caracteres
                string query = @"
                    SELECT 
                        ClavePresupuestal,
                        Aprobado, 
                        (IFNULL(Aprobado, 0) + IFNULL(Aumentos, 0) - IFNULL(Disminuciones, 0)) AS Modificado, 
                        Devengado, Pagado
                    FROM presupuestocalendarizado
                    WHERE ClavePresupuestal LIKE @Prefijo
                    ORDER BY ClavePresupuestal ASC;
                ";
                return db.Query<PresupuestoDetalleDto>(query, new { Prefijo = prefijo22Digitos + "%" });
            }
        }

        public bool RegistrarApartado(string clave, decimal monto, string motivo, string oficio)
        {
            using (IDbConnection db = DbConnection)
            {
                // La validación de saldo disponible ya se hace en el front/controller.
                // Insertamos directamente el apartado.
                string sql = @"
                    INSERT INTO apartados_presupuesto (ClavePresupuestal, Monto, FechaApartado, Activo, Motivo, Oficio)
                    VALUES (@Clave, @Monto, NOW(), 1, @Motivo, @Oficio);
                ";
                int rows = db.Execute(sql, new { Clave = clave, Monto = monto, Motivo = motivo, Oficio = oficio });
                return rows > 0;
            }
        }

        public bool CancelarApartado(string clave)
        {
            using (IDbConnection db = DbConnection)
            {
                string sql = "UPDATE apartados_presupuesto SET Activo = 0 WHERE ClavePresupuestal = @Clave";
                int rows = db.Execute(sql, new { Clave = clave });
                return rows > 0;
            }
        }

        public bool CancelarApartadoPorId(int id)
        {
            using (IDbConnection db = DbConnection)
            {
                string sql = "UPDATE apartados_presupuesto SET Activo = 0 WHERE Id = @Id";
                int rows = db.Execute(sql, new { Id = id });
                return rows > 0;
            }
        }
    }
}
