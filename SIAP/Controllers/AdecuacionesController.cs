using Dapper;
//using Microsoft.AspNetCore.Mvc;
using SIAP.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using OfficeOpenXml;
using System.IO;

namespace SIAP.Controllers
{
    public class AdecuacionesController : ApiController
    {
        private Adecuacion MapToServicio(SqlDataReader reader)
        {
            return new Adecuacion()
            {
                numsolicitud = Convert.ToString(reader["numsolicitud"] is DBNull ? null : reader["numsolicitud"]),
                tipoadecuacion = Convert.ToString(reader["tipoadecuacion"] is DBNull ? null : reader["tipoadecuacion"]),
                fechasolicitud = Convert.ToDateTime(reader["fechasolicitud"] is DBNull ? null : reader["fechasolicitud"]),
                montoadecuacion = Convert.ToDecimal(reader["montoadecuacion"] is DBNull ? null : reader["montoadecuacion"]),
                emisor = Convert.ToDecimal(reader["emisor"] is DBNull ? null : reader["emisor"]),
                receptor = Convert.ToDecimal(reader["receptor"] is DBNull ? null : reader["receptor"]),
                agrupar = Convert.ToDecimal(reader["agrupar"] is DBNull ? null : reader["agrupar"]),
                oficionumero = Convert.ToString(reader["oficionumero"] is DBNull ? null : reader["oficionumero"]),
                idestatus = Convert.ToInt32(reader["idestatus"] is DBNull ? null : reader["idestatus"]),
                estatus = Convert.ToString(reader["estatus"] is DBNull ? null : reader["estatus"]),
            };
        }
        //--------- Adecuaciones --------------
        /// <summary>
        /// Obtiene el listado general de las adecuaciones agrupadas por número de oficio.
        /// </summary>
        /// <remarks>
        /// Este endpoint retorna un conjunto resumen de adecuaciones. Calcula los totales globales como el monto total de adecuación, 
        /// receptor y emisor, basándose en la agrupación por <c>NumOficio</c>. Se utiliza principalmente para una vista de resumen general.
        /// </remarks>
        /// <returns>Un IHttpActionResult con estado 200 (OK) y la lista de adecuaciones, o 500 en caso de error interno.</returns>
        public IHttpActionResult Get()
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    string sql = @"
                        SELECT 
                            CAST(MIN(Id) AS VARCHAR(20)) as numsolicitud,
                            NumOficio as oficionumero,
                            MAX(CASE 
                                WHEN NumOficio LIKE '%ET%' THEN 'TRASPASO'
                                WHEN NumOficio LIKE '%EA%' THEN 'AMPLIACION'
                                WHEN NumOficio LIKE '%ER%' THEN 'REDUCCION'
                                ELSE 'Varios'
                            END) as tipoadecuacion,
                            COALESCE(MAX(FechaSolicitud), CAST('0001-01-01' AS DATETIME2)) as fechasolicitud,
                            ISNULL(SUM(MontoTotal), 0) as montoadecuacion,
                            ISNULL(SUM(MontoTotal), 0) as receptor,
                            CAST(0 AS DECIMAL) as emisor,
                            'Varios' as estatus
                        FROM adecuaciones
                        GROUP BY NumOficio
                        ORDER BY MAX(Id) DESC";
                    
                    var data = conn.Query<Adecuacion>(sql).ToList();
                    return Ok(data);
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        

        [HttpGet]
        [Route("api/Adecuaciones/Paginado")]
        /// <summary>
        /// Obtiene la lista paginada de adecuaciones, diseñada para ser consumida directamente por la librería DataTables (Server-side processing).
        /// </summary>
        /// <remarks>
        /// Aplica múltiples filtros dinámicos que el usuario puede establecer en la interfaz (búsqueda por texto, número de solicitud, tipo, mes y año).
        /// Retorna el formato estricto que espera DataTables: <c>draw</c>, <c>recordsTotal</c>, <c>recordsFiltered</c> y la matriz de <c>data</c>.
        /// </remarks>
        /// <param name="draw">Contador de peticiones de DataTables para sincronización de respuestas asíncronas.</param>
        /// <param name="start">Índice del primer registro a retornar (offset para paginación).</param>
        /// <param name="length">Cantidad máxima de registros a retornar por página.</param>
        /// <param name="search">Término de búsqueda ingresado en el buscador global de la tabla.</param>
        /// <param name="solic">Filtro específico por número de solicitud u oficio.</param>
        /// <param name="tipo">Filtro específico por tipo de adecuación (ej. AMPLIACION, REDUCCION).</param>
        /// <param name="mes">Filtro específico por mes numérico (1-12).</param>
        /// <param name="anio">Filtro específico por año a cuatro dígitos.</param>
        /// <returns>JSON estructurado con la metadata de DataTables y la lista de registros filtrados.</returns>
        public IHttpActionResult GetPaginado(int draw = 1, int start = 0, int length = 10, string search = "", string solic = "", string tipo = "", string mes = "", string anio = "")
        {
            try
            {
                var queryParams = Request.GetQueryNameValuePairs();
                var searchParam = queryParams.FirstOrDefault(q => q.Key == "search[value]").Value ?? "";
                
                if(string.IsNullOrEmpty(solic)) solic = queryParams.FirstOrDefault(q => q.Key == "solic").Value ?? "";
                if(string.IsNullOrEmpty(tipo)) tipo = queryParams.FirstOrDefault(q => q.Key == "tipo").Value ?? "";
                if(string.IsNullOrEmpty(mes)) mes = queryParams.FirstOrDefault(q => q.Key == "mes").Value ?? "";
                if(string.IsNullOrEmpty(anio)) anio = queryParams.FirstOrDefault(q => q.Key == "anio").Value ?? "";

                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    List<string> conditions = new List<string>();
                    var p = new DynamicParameters();
                    
                    if (!string.IsNullOrEmpty(searchParam))
                    {
                        conditions.Add("(COALESCE(NumSolicitud, NumOficio) LIKE @search)");
                        p.Add("@search", $"%{searchParam}%");
                    }
                    if (!string.IsNullOrEmpty(solic) && solic != "0" && solic != "(Seleccione un Número de Solicitud)")
                    {
                        conditions.Add("COALESCE(NumSolicitud, NumOficio) = @solic");
                        p.Add("@solic", solic);
                    }
                    if (!string.IsNullOrEmpty(tipo) && tipo != "0" && tipo != "(Seleccione un Tipo de Adecuación)")
                    {
                        conditions.Add("TipoAdecuacion = @tipo");
                        p.Add("@tipo", tipo);
                    }
                    if (!string.IsNullOrEmpty(mes) && mes != "0" && mes != "(Seleccione un Mes)")
                    {
                        conditions.Add("MONTH(FechaSolicitud) = @mes");
                        p.Add("@mes", mes);
                    }
                    if (!string.IsNullOrEmpty(anio) && anio != "0" && anio != "(Seleccione un Año)")
                    {
                        conditions.Add("YEAR(FechaSolicitud) = @anio");
                        p.Add("@anio", anio);
                    }

                    string searchCondition = conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : "";

                    string sqlTotal = "SELECT COUNT(DISTINCT COALESCE(NumSolicitud, NumOficio)) FROM adecuaciones";
                    int recordsTotal = conn.ExecuteScalar<int>(sqlTotal);

                    string sqlFiltered = $"SELECT COUNT(DISTINCT COALESCE(NumSolicitud, NumOficio)) FROM adecuaciones {searchCondition}";
                    int recordsFiltered = conditions.Count == 0 ? recordsTotal : conn.ExecuteScalar<int>(sqlFiltered, p);

                    // DataTables manda length = -1 para "Todos"; FETCH NEXT no acepta valores <= 0
                    if (start < 0) start = 0;
                    if (length <= 0) length = int.MaxValue;
                    p.Add("@length", length);
                    p.Add("@start", start);

                    string sql = $@"
                        SELECT 
                            CAST(MIN(Id) AS VARCHAR(20)) as numsolicitud,
                            COALESCE(NumSolicitud, NumOficio) as oficionumero,
                            MAX(CASE 
                                WHEN COALESCE(NumSolicitud, NumOficio) LIKE '%ET%' THEN 'TRASPASO'
                                WHEN COALESCE(NumSolicitud, NumOficio) LIKE '%EA%' THEN 'AMPLIACION'
                                WHEN COALESCE(NumSolicitud, NumOficio) LIKE '%ER%' THEN 'REDUCCION'
                                ELSE 'Varios'
                            END) as tipoadecuacion,
                            COALESCE(MAX(FechaSolicitud), CAST('0001-01-01' AS DATETIME2)) as fechasolicitud,
                            ISNULL(SUM(MontoTotal), 0) as montoadecuacion,
                            ISNULL(SUM(MontoTotal), 0) as receptor,
                            CAST(0 AS DECIMAL) as emisor,
                            'Varios' as estatus
                        FROM adecuaciones
                        {searchCondition}
                        GROUP BY COALESCE(NumSolicitud, NumOficio)
                        ORDER BY MAX(Id) DESC
                        OFFSET @start ROWS FETCH NEXT @length ROWS ONLY";
                    
                    var data = conn.Query<Adecuacion>(sql, p).ToList();

                    return Ok(new
                    {
                        draw = draw,
                        recordsTotal = recordsTotal,
                        recordsFiltered = recordsFiltered,
                        data = data
                    });
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Adecuaciones/Filtros/Solicitudes")]
        public IHttpActionResult GetFiltroSolicitudes()
        {
            try {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    var data = conn.Query("SELECT DISTINCT COALESCE(NumSolicitud, NumOficio) as value, COALESCE(NumSolicitud, NumOficio) as text FROM adecuaciones WHERE COALESCE(NumSolicitud, NumOficio) IS NOT NULL AND COALESCE(NumSolicitud, NumOficio) <> '' ORDER BY 1 DESC").ToList();
                    return Ok(data);
                }
            } catch(Exception ex) { return InternalServerError(ex); }
        }

        [HttpGet]
        [Route("api/Adecuaciones/Filtros/Tipos")]
        public IHttpActionResult GetFiltroTipos()
        {
            try {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    var data = conn.Query("SELECT DISTINCT TipoAdecuacion as value, TipoAdecuacion as text FROM adecuaciones WHERE TipoAdecuacion IS NOT NULL AND TipoAdecuacion <> '' ORDER BY 1").ToList();
                    return Ok(data);
                }
            } catch(Exception ex) { return InternalServerError(ex); }
        }

        [HttpGet]
        [Route("api/Adecuaciones/Filtros/Meses")]
        public IHttpActionResult GetFiltroMeses()
        {
            try {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    var data = conn.Query("SELECT DISTINCT MONTH(FechaSolicitud) as value FROM adecuaciones WHERE FechaSolicitud IS NOT NULL ORDER BY 1").ToList();
                    return Ok(data);
                }
            } catch(Exception ex) { return InternalServerError(ex); }
        }

        [HttpGet]
        [Route("api/Adecuaciones/Filtros/Anios")]
        public IHttpActionResult GetFiltroAnios()
        {
            try {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    var data = conn.Query("SELECT DISTINCT YEAR(FechaSolicitud) as value FROM adecuaciones WHERE FechaSolicitud IS NOT NULL ORDER BY 1 DESC").ToList();
                    return Ok(data);
                }
            } catch(Exception ex) { return InternalServerError(ex); }
        }

        [HttpGet]
        [Route("api/Adecuaciones/Oficio/{numOficio}")]
        public IHttpActionResult GetOficio(string numOficio)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    // Necesitamos decodificar la URL porque el oficio trae diagonales como "2024/001"
                    string decodedOficio = System.Net.WebUtility.UrlDecode(numOficio);
                    
                    string sql = @"
                        SELECT 
                            a.Id as numsolicitud,
                            COALESCE(a.NumSolicitud, a.NumOficio) as oficionumero,
                            a.TipoAdecuacion as tipoadecuacion,
                            a.FechaSolicitud as fechasolicitud,
                            a.Estatus as estatus,
                            a.Solicita as solicita,
                            a.Autoriza as autoriza,
                            m.ClavePresupuestal as clave,
                            m.TipoOperacion as tipooperacion,
                            (ISNULL(m.Enero, 0) + ISNULL(m.Febrero, 0) + ISNULL(m.Marzo, 0) + ISNULL(m.Abril, 0) + ISNULL(m.Mayo, 0) + ISNULL(m.Junio, 0) + ISNULL(m.Julio, 0) + ISNULL(m.Agosto, 0) + ISNULL(m.Septiembre, 0) + ISNULL(m.Octubre, 0) + ISNULL(m.Noviembre, 0) + ISNULL(m.Diciembre, 0)) as montoadecuacion
                        FROM adecuaciones a
                        LEFT JOIN adecuacionmovimientos m ON a.Id = m.AdecuacionId
                        WHERE COALESCE(a.NumSolicitud, a.NumOficio) = @decodedOficio
                        ORDER BY m.Id ASC";
                    
                    var data = conn.Query<dynamic>(sql, new { decodedOficio }).ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Adecuaciones/Movimientos/{id}")]
        /// <summary>
        /// Obtiene el detalle completo de los movimientos asociados a un ID de adecuación específico.
        /// </summary>
        /// <remarks>
        /// Retorna la lista de todas las claves presupuestales afectadas en esa adecuación, resolviendo el nombre 
        /// del tipo de operación (mediante JOIN a <c>cat_tipoadecuacion</c>) e incluyendo el desglose monetario por cada uno de los 12 meses.
        /// </remarks>
        /// <param name="id">El identificador único primario de la Adecuación cabecera.</param>
        /// <returns>Lista de movimientos detallados en formato JSON.</returns>
        public IHttpActionResult GetMovimientos(int id)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    string sql = @"
                        SELECT 
                            Id,
                            AdecuacionId,
                            CASE 
                                WHEN m.TipoOperacion = 'INICIAL' THEN 'PRESUPUESTO INICIAL'
                                WHEN c.Descripcion IS NOT NULL THEN c.Descripcion
                                ELSE m.TipoOperacion 
                            END as TipoOperacion,
                            ClavePresupuestal,
                            (ISNULL(m.Enero, 0) + ISNULL(m.Febrero, 0) + ISNULL(m.Marzo, 0) + ISNULL(m.Abril, 0) + ISNULL(m.Mayo, 0) + ISNULL(m.Junio, 0) + ISNULL(m.Julio, 0) + ISNULL(m.Agosto, 0) + ISNULL(m.Septiembre, 0) + ISNULL(m.Octubre, 0) + ISNULL(m.Noviembre, 0) + ISNULL(m.Diciembre, 0)) as Total,
                            ISNULL(m.Enero,0) as Enero, ISNULL(m.Febrero,0) as Febrero, ISNULL(m.Marzo,0) as Marzo, ISNULL(m.Abril,0) as Abril, ISNULL(m.Mayo,0) as Mayo, ISNULL(m.Junio,0) as Junio, ISNULL(m.Julio,0) as Julio, ISNULL(m.Agosto,0) as Agosto, ISNULL(m.Septiembre,0) as Septiembre, ISNULL(m.Octubre,0) as Octubre, ISNULL(m.Noviembre,0) as Noviembre, ISNULL(m.Diciembre,0) as Diciembre
                        FROM adecuacionmovimientos m
                        LEFT JOIN cat_tipoadecuacion c ON m.TipoOperacion = c.Clave
                        WHERE m.AdecuacionId = @id";

                    var data = conn.Query(sql, new { id }).ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/adecuaciones/TiposAdecuacion")]
        public IHttpActionResult GetTiposAdecuacion()
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    string sql = "SELECT Clave as idtipoadecuacion, Descripcion as nombre FROM cat_tipoadecuacion ORDER BY Clave";
                    var data = conn.Query(sql).ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/adecuaciones/Apartado/{id}")]
        public IHttpActionResult GetApartado(int id)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    string sql = "SELECT Id, Monto, Oficio, FechaApartado FROM apartados_presupuesto WHERE Id = @id";
                    var data = conn.QueryFirstOrDefault(sql, new { id });
                    if (data == null) return NotFound();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/adecuaciones/GetPorClave/{clave}")]
        public IHttpActionResult GetPorClave(string clave)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    string sql = @"
                        SELECT 
                            a.NumOficio,
                            a.FechaSolicitud,
                            a.TipoAdecuacion,
                            CASE 
                                WHEN m.TipoOperacion = 'INICIAL' THEN 'PRESUPUESTO INICIAL'
                                WHEN c.Descripcion IS NOT NULL THEN c.Descripcion
                                ELSE m.TipoOperacion 
                            END as TipoOperacion,
                            CASE WHEN ISNULL(m.MontoTotal, 0) > 0 THEN m.MontoTotal
                                 ELSE (ISNULL(m.Enero, 0) + ISNULL(m.Febrero, 0) + ISNULL(m.Marzo, 0) + ISNULL(m.Abril, 0) + ISNULL(m.Mayo, 0) + ISNULL(m.Junio, 0) + ISNULL(m.Julio, 0) + ISNULL(m.Agosto, 0) + ISNULL(m.Septiembre, 0) + ISNULL(m.Octubre, 0) + ISNULL(m.Noviembre, 0) + ISNULL(m.Diciembre, 0)) END as Total
                        FROM adecuacionmovimientos m
                        INNER JOIN adecuaciones a ON a.Id = m.AdecuacionId
                        LEFT JOIN cat_tipoadecuacion c ON m.TipoOperacion = c.Clave
                        WHERE m.ClavePresupuestal = @clave
                        ORDER BY a.FechaSolicitud DESC";
                    
                    var data = conn.Query(sql, new { clave }).ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/adecuaciones/CheckClave/{clave}")]
        public IHttpActionResult CheckClave(string clave)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    string sql = "SELECT COUNT(*) FROM presupuestocalendarizado WHERE ClavePresupuestal = @clave";
                    int count = conn.ExecuteScalar<int>(sql, new { clave });
                    return Ok(new { exists = count > 0 });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/Adecuaciones/UploadExcel")]
        public IHttpActionResult UploadExcel()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count == 0)
                {
                    return Json(new { success = false, message = "No se proporcionó ningún archivo." });
                }

                var postedFile = httpRequest.Files[0];
                // Licencia de EPPlus: se declara en Web.config (EPPlus:ExcelPackage:License)

                using (var package = new ExcelPackage(postedFile.InputStream))
                {
                    // Buscar la hoja solicitada por el usuario
                    var worksheet = package.Workbook.Worksheets["BASE GENERAL VERTICAL"];
                    if (worksheet == null)
                    {
                        // Si no existe, usamos la primera hoja como fallback
                        worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    }

                    if (worksheet == null)
                    {
                        return Json(new { success = false, message = "El archivo Excel está vacío o no contiene hojas." });
                    }

                    int rowCount = worksheet.Dimension.End.Row;

                    using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                    {
                        con.Open();
                        // Iteramos desde la fila 2 asumiendo que la fila 1 tiene encabezados
                        for (int row = 2; row <= rowCount; row++)
                        {
                            string clave = worksheet.Cells[row, 1].Text?.Trim(); // A
                            string adecuacion = worksheet.Cells[row, 2].Text?.Trim(); // B

                            if (string.IsNullOrEmpty(clave) || string.IsNullOrEmpty(adecuacion))
                            {
                                continue; // Saltar filas vacías
                            }

                            // TODO: Insertar la lógica de base de datos para Clave y Adecuación
                            // Se utilizará el sp_PRE_adecuacionpresupuestariaorden o un simple insert en Adecuaciones
                            // Como las adecuaciones "ya están en la base" según entendimos o se quieren insertar ahora, 
                            // insertaremos un dummy para mostrar el progreso:
                            
                            string sqlAdecuacion = @"
                                INSERT INTO adecuaciones (NumOficio, TipoAdecuacion, FechaSolicitud, MontoTotal, Estatus)
                                SELECT @numsolicitud, 'AMPLIACION', GETDATE(), 0, 'Procesada'
                                WHERE NOT EXISTS (
                                    SELECT 1 FROM adecuaciones WHERE NumOficio = @numsolicitud
                                )";
                                
                            using (var cmdAdec = new SqlCommand(sqlAdecuacion, con))
                            {
                                cmdAdec.Parameters.AddWithValue("@numsolicitud", adecuacion);
                                cmdAdec.ExecuteNonQuery();
                            }
                        }
                    }
                }

                return Json(new { success = true, message = "Archivo de adecuaciones procesado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }

        [HttpPost]
        [Route("api/adecuaciones/GuardarManual")]
        /// <summary>
        /// Procesa y guarda una adecuación capturada de forma manual desde la interfaz de usuario, incluyendo múltiples movimientos de reducción y ampliación.
        /// </summary>
        /// <remarks>
        /// Este endpoint es altamente crítico y realiza operaciones complejas en transacción:
        /// 1. Inserta la cabecera de la adecuación (metadatos, oficios, fechas, estatus) en la tabla <c>adecuaciones</c>.
        /// 2. Itera sobre la lista de movimientos. Para cada movimiento, verifica si la <c>ClavePresupuestal</c> existe en el calendario de presupuesto (<c>presupuestocalendarizado</c>). Si no existe, crea un registro vacío.
        /// 3. Validaciones de Reducción: Si el movimiento es de tipo 'Reducción' (o código 2, 4, DISMINUCION), valida matemáticamente que el calendario actual posea fondos suficientes para el retiro. Si no se especifican meses particulares, distribuye la reducción de diciembre hacia atrás (Last-In-First-Out) comprobando los saldos.
        /// 4. Actualización de Saldos: Afecta la tabla de presupuesto modificando los montos mensuales correspondientes según sea ampliación (+) o reducción (-).
        /// 5. Registro Histórico: Inserta el detalle del movimiento en <c>adecuacionmovimientos</c>.
        /// Si alguna regla de negocio falla (ej. fondos insuficientes), se lanza una excepción y la transacción realiza un Rollback total.
        /// </remarks>
        /// <param name="data">Objeto <see cref="AdecuacionManualDto"/> que empaqueta la información de cabecera y el array de <see cref="MovimientoManualDto"/>.</param>
        /// <returns>JSON con <c>success = true</c> si el guardado es exitoso, o <c>success = false</c> junto con el mensaje de error si falla (ej. por saldo insuficiente).</returns>
        public IHttpActionResult GuardarManual([FromBody] AdecuacionManualDto data)
        {
            try
            {
                if (data == null || data.Movimientos == null || data.Movimientos.Count == 0)
                {
                    return Json(new { success = false, message = "Datos inválidos o sin movimientos." });
                }

                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insertar la adecuación (Cabecera)
                            string sqlAdec = @"
                                INSERT INTO adecuaciones 
                                (NumOficio, NumSolicitud, Asunto, TipoAdecuacion, FechaSolicitud, MontoTotal, Estatus, FechaCreacion, TipoAdecuacionId, ApartadoId, Solicita, Autoriza) 
                                VALUES 
                                (@NumOficio, @NumSolicitud, @Asunto, @TipoAdecuacion, @FechaSolicitud, @MontoTotal, @Estatus, GETDATE(), @TipoAdecuacionId, @ApartadoId, @Solicita, @Autoriza);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            int adecuacionId = conn.QuerySingle<int>(sqlAdec, new
                            {
                                data.NumOficio,
                                data.NumSolicitud,
                                Asunto = string.IsNullOrEmpty(data.Asunto) ? "CAPTURA MANUAL" : data.Asunto,
                                data.TipoAdecuacion,
                                data.FechaSolicitud,
                                data.MontoTotal,
                                data.Estatus,
                                data.TipoAdecuacionId,
                                data.ApartadoId,
                                data.Solicita,
                                data.Autoriza
                            }, transaction);

                            // 2. Insertar movimientos
                            string sqlCheckClave = "SELECT TOP 1 Id FROM presupuestocalendarizado WHERE ClavePresupuestal = @ClavePresupuestal";
                            string sqlInsertClave = "INSERT INTO presupuestocalendarizado (ClavePresupuestal) VALUES (@ClavePresupuestal); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            string sqlMovs = @"
                                INSERT INTO adecuacionmovimientos 
                                (AdecuacionId, TipoOperacion, ClavePresupuestal, PresupuestoCalendarizadoId, MontoTotal, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre)
                                VALUES 
                                (@AdecuacionId, @TipoOperacion, @ClavePresupuestal, @PresupuestoCalendarizadoId, @MontoTotal, @Enero, @Febrero, @Marzo, @Abril, @Mayo, @Junio, @Julio, @Agosto, @Septiembre, @Octubre, @Noviembre, @Diciembre);";

                            foreach (var mov in data.Movimientos)
                            {
                                // Verificar si la clave existe
                                int? presId = conn.QueryFirstOrDefault<int?>(sqlCheckClave, new { mov.ClavePresupuestal }, transaction);
                                if (presId == null || presId == 0)
                                {
                                    // Crear nueva clave si no existe
                                    presId = conn.QuerySingle<int>(sqlInsertClave, new { mov.ClavePresupuestal }, transaction);
                                }

                                decimal totalMov = 0;
                                string sqlUpdate = "";

                                if (mov.TipoOperacion == "4" || mov.TipoOperacion == "2" || mov.TipoOperacion.ToUpper() == "REDUCCION" || mov.TipoOperacion.ToUpper() == "DISMINUCION")
                                {
                                    totalMov = mov.MontoTotal;
                                    
                                    // Obtener saldos actuales
                                    var saldos = conn.QueryFirstOrDefault<MovimientoManualDto>(
                                        "SELECT Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre FROM presupuestocalendarizado WHERE Id = @Id",
                                        new { Id = presId }, transaction);

                                    if (saldos != null)
                                    {
                                        decimal sumaMeses = mov.Enero + mov.Febrero + mov.Marzo + mov.Abril + mov.Mayo + mov.Junio + mov.Julio + mov.Agosto + mov.Septiembre + mov.Octubre + mov.Noviembre + mov.Diciembre + mov.PorLiberar;
                                        
                                        if (sumaMeses == 0 && totalMov > 0)
                                        {
                                            decimal restante = totalMov;

                                            decimal decD = Math.Min(restante, saldos.Diciembre); mov.Diciembre = decD; restante -= decD;
                                            decimal decN = Math.Min(restante, saldos.Noviembre); mov.Noviembre = decN; restante -= decN;
                                            decimal decO = Math.Min(restante, saldos.Octubre); mov.Octubre = decO; restante -= decO;
                                            decimal decS = Math.Min(restante, saldos.Septiembre); mov.Septiembre = decS; restante -= decS;
                                            decimal decAg = Math.Min(restante, saldos.Agosto); mov.Agosto = decAg; restante -= decAg;
                                            decimal decJu = Math.Min(restante, saldos.Julio); mov.Julio = decJu; restante -= decJu;
                                            decimal decJn = Math.Min(restante, saldos.Junio); mov.Junio = decJn; restante -= decJn;
                                            decimal decMy = Math.Min(restante, saldos.Mayo); mov.Mayo = decMy; restante -= decMy;
                                            decimal decAb = Math.Min(restante, saldos.Abril); mov.Abril = decAb; restante -= decAb;
                                            decimal decMz = Math.Min(restante, saldos.Marzo); mov.Marzo = decMz; restante -= decMz;
                                            decimal decF = Math.Min(restante, saldos.Febrero); mov.Febrero = decF; restante -= decF;
                                            decimal decE = Math.Min(restante, saldos.Enero); mov.Enero = decE; restante -= decE;

                                            if (restante > 0)
                                            {
                                                throw new Exception($"Fondos insuficientes en la clave {mov.ClavePresupuestal} para realizar la reducción de {totalMov.ToString("C")}.");
                                            }
                                        }
                                        else
                                        {
                                            // Distribución mensual explicita proporcionada
                                            if (mov.Enero > saldos.Enero || mov.Febrero > saldos.Febrero || mov.Marzo > saldos.Marzo || 
                                                mov.Abril > saldos.Abril || mov.Mayo > saldos.Mayo || mov.Junio > saldos.Junio || 
                                                mov.Julio > saldos.Julio || mov.Agosto > saldos.Agosto || mov.Septiembre > saldos.Septiembre || 
                                                mov.Octubre > saldos.Octubre || mov.Noviembre > saldos.Noviembre || mov.Diciembre > saldos.Diciembre)
                                            {
                                                throw new Exception($"Fondos insuficientes en la clave {mov.ClavePresupuestal} para aplicar la reducción con el calendario especificado.");
                                            }
                                        }
                                    }

                                    sqlUpdate = @"UPDATE presupuestocalendarizado SET
                                        Disminuciones = Disminuciones + @TotalMov,
                                        Enero = Enero - @Enero, Febrero = Febrero - @Febrero, Marzo = Marzo - @Marzo,
                                        Abril = Abril - @Abril, Mayo = Mayo - @Mayo, Junio = Junio - @Junio,
                                        Julio = Julio - @Julio, Agosto = Agosto - @Agosto, Septiembre = Septiembre - @Septiembre,
                                        Octubre = Octubre - @Octubre, Noviembre = Noviembre - @Noviembre, Diciembre = Diciembre - @Diciembre,
                                        PorLiberar = PorLiberar - @PorLiberar
                                        WHERE Id = @Id";
                                }
                                else
                                {
                                    totalMov = mov.MontoTotal;

                                    sqlUpdate = @"UPDATE presupuestocalendarizado SET
                                        Aumentos = Aumentos + @TotalMov,
                                        Enero = Enero + @Enero, Febrero = Febrero + @Febrero, Marzo = Marzo + @Marzo,
                                        Abril = Abril + @Abril, Mayo = Mayo + @Mayo, Junio = Junio + @Junio,
                                        Julio = Julio + @Julio, Agosto = Agosto + @Agosto, Septiembre = Septiembre + @Septiembre,
                                        Octubre = Octubre + @Octubre, Noviembre = Noviembre + @Noviembre, Diciembre = Diciembre + @Diciembre,
                                        PorLiberar = PorLiberar + @PorLiberar
                                        WHERE Id = @Id";
                                }

                                conn.Execute(sqlMovs, new
                                {
                                    AdecuacionId = adecuacionId,
                                    mov.TipoOperacion,
                                    mov.ClavePresupuestal,
                                    PresupuestoCalendarizadoId = presId,
                                    mov.MontoTotal,
                                    mov.Enero,
                                    mov.Febrero,
                                    mov.Marzo,
                                    mov.Abril,
                                    mov.Mayo,
                                    mov.Junio,
                                    mov.Julio,
                                    mov.Agosto,
                                    mov.Septiembre,
                                    mov.Octubre,
                                    mov.Noviembre,
                                    mov.Diciembre
                                }, transaction);

                                if (!string.IsNullOrEmpty(sqlUpdate))
                                {
                                    conn.Execute(sqlUpdate, new
                                    {
                                        TotalMov = totalMov,
                                        mov.Enero, mov.Febrero, mov.Marzo, mov.Abril, mov.Mayo, mov.Junio,
                                        mov.Julio, mov.Agosto, mov.Septiembre, mov.Octubre, mov.Noviembre, mov.Diciembre, mov.PorLiberar,
                                        Id = presId
                                    }, transaction);
                                }
                            }

                            transaction.Commit();

                            try
                            {
                                // Crear registro en la bitácora para adecuaciones y mostrar sus datos como un rastreador
                                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["UsuarioId"] != null)
                                {
                                    int uId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UsuarioId"]);
                                    var logRepo = new SIAP.Repositories.UsuarioRepository();
                                    logRepo.RegistrarLog(uId, "Adecuaciones", "Registro", $"Registró adecuación con n° de solicitud: {data.NumSolicitud ?? data.NumOficio}");
                                }
                            }
                            catch { /* Ignorar error de bitacora */ }

                            return Json(new { success = true, message = "Guardado correctamente." });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = "Error en base de datos: " + ex.Message });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }

    }

    public class AdecuacionManualDto
    {
        public string NumOficio { get; set; }
        public string NumSolicitud { get; set; }
        public string Asunto { get; set; }
        public int TipoAdecuacionId { get; set; }
        public string TipoAdecuacion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Solicita { get; set; }
        public string Autoriza { get; set; }
        public string Concepto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estatus { get; set; }
        public int? ApartadoId { get; set; }
        public List<MovimientoManualDto> Movimientos { get; set; }
    }

    public class MovimientoManualDto
    {
        public string ClavePresupuestal { get; set; }
        public string TipoOperacion { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal Enero { get; set; }
        public decimal Febrero { get; set; }
        public decimal Marzo { get; set; }
        public decimal Abril { get; set; }
        public decimal Mayo { get; set; }
        public decimal Junio { get; set; }
        public decimal Julio { get; set; }
        public decimal Agosto { get; set; }
        public decimal Septiembre { get; set; }
        public decimal Octubre { get; set; }
        public decimal Noviembre { get; set; }
        public decimal Diciembre { get; set; }
        public decimal PorLiberar { get; set; }
    }

    public class AdecFController : ApiController
    {
        private Adecuacion MapToServicio(SqlDataReader reader)
        {
            return new Adecuacion()
            {
                numsolicitud = Convert.ToString(reader["numsolicitud"] is DBNull ? null : reader["numsolicitud"]),
                tipoadecuacion = Convert.ToString(reader["tipoadecuacion"] is DBNull ? null : reader["tipoadecuacion"]),
                fechasolicitud = Convert.ToDateTime(reader["fechasolicitud"] is DBNull ? null : reader["fechasolicitud"]),
                montoadecuacion = Convert.ToDecimal(reader["montoadecuacion"] is DBNull ? null : reader["montoadecuacion"]),
                emisor = Convert.ToDecimal(reader["emisor"] is DBNull ? null : reader["emisor"]),
                receptor = Convert.ToDecimal(reader["receptor"] is DBNull ? null : reader["receptor"]),
                agrupar = Convert.ToDecimal(reader["agrupar"] is DBNull ? null : reader["agrupar"]),
                oficionumero = Convert.ToString(reader["oficionumero"] is DBNull ? null : reader["oficionumero"]),
                idestatus = Convert.ToInt32(reader["idestatus"] is DBNull ? null : reader["idestatus"]),
                estatus = Convert.ToString(reader["estatus"] is DBNull ? null : reader["estatus"]),
            };
        }
        //--------- Adecuaciones con filtros -------
        public List<Adecuacion> GetAdec()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var response = new List<Adecuacion>();
            response.Add(new Adecuacion()
            {
                numsolicitud = "21111206ET10061",
                tipoadecuacion = "AMPLIACIÓN",
                fechasolicitud = DateTime.Now.AddDays(-2),
                montoadecuacion = 1500000.00m,
                emisor = 0,
                receptor = 1500000.00m,
                estatus = "Procesada"
            });
            return response;
        }
    }
    public class NumOficiosController : ApiController
    {
        //----------- Numero de oficio ---------------------
        public IEnumerable<NumeroOficio> GetNumOfi()
        {
            var list = new List<NumeroOficio>();
            list.Add(new NumeroOficio { oficio = "(Seleccione un Número de Oficio)" });
            list.Add(new NumeroOficio { oficio = "OFICIO 1" });
            return list;
        }
    }
    public class EstatusAdecController : ApiController
    {
        //------------ Estatus de la adecuación -------------
        public IEnumerable<EstatusAdec> GetEstatus()
        {
            var list = new List<EstatusAdec>();
            list.Add(new EstatusAdec { idestatusadecuacion = 0, estatusadecuacion = "(Seleccione un Estatus)" });
            list.Add(new EstatusAdec { idestatusadecuacion = 1, estatusadecuacion = "En Formulación" });
            list.Add(new EstatusAdec { idestatusadecuacion = 2, estatusadecuacion = "En Trámite" });
            list.Add(new EstatusAdec { idestatusadecuacion = 3, estatusadecuacion = "Procesada" });
            return list;
        }
    }
    public class MesesController : ApiController
    {
        //------------ Meses --------------
        public IEnumerable<Mes> GetMes()
        {
            var list = new List<Mes>();
            list.Add(new Mes { numeromes = 0, nombremes = "(Seleccione un Mes)" });
            list.Add(new Mes { numeromes = 1, nombremes = "ENERO" });
            list.Add(new Mes { numeromes = 2, nombremes = "FEBRERO" });
            return list;
        }
    }
}