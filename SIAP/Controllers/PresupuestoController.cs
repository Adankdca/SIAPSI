using System;
using System.Linq;
using System.Web.Http;
using SIAP.Models;
using SIAP.Repositories;
using System.IO;
using System.Web;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Globalization;

namespace SIAP.Controllers
{
    [RoutePrefix("api/presupuesto")]
    public class PresupuestoController : ApiController
    {
        private PresupuestoRepository _repository;

        private void RegistrarLog(string accion, string detalle)
        {
            try
            {
                var session = System.Web.HttpContext.Current?.Session;
                if (session != null && session["UsuarioId"] != null)
                {
                    int usuarioId = Convert.ToInt32(session["UsuarioId"]);
                    new UsuarioRepository().RegistrarLog(usuarioId, "Presupuesto", accion, detalle);
                }
            }
            catch { /* Ignorar errores de log */ }
        }

        public PresupuestoController()
        {
            _repository = new PresupuestoRepository();
        }

        // GET: api/presupuesto/matriz
        /// <summary>
        /// Obtiene la matriz presupuestal completa.
        /// Este endpoint no paginado consulta todas las claves presupuestales del ejercicio con su estado financiero (aprobado, modificado, etc.).
        /// </summary>
        /// <returns>Resultado HTTP con la lista de datos presupuestales, o InternalServerError en caso de excepción.</returns>
        [HttpGet]
        [Route("matriz")]
        public IHttpActionResult GetMatriz()
        {
            try
            {
                var matriz = _repository.ObtenerMatrizPresupuesto();
                return Ok(matriz);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Obtiene la matriz presupuestal paginada, diseñada específicamente para su consumo por DataTables.
        /// Intercepta el parámetro 'search[value]' nativo de DataTables para filtrar y aplica filtros adicionales de la vista.
        /// </summary>
        /// <param name="draw">Contador de peticiones de DataTables para asegurar el orden de las respuestas asíncronas.</param>
        /// <param name="start">Índice del primer registro a devolver en la página actual.</param>
        /// <param name="length">Número de registros a mostrar por página.</param>
        /// <param name="search">Texto de búsqueda parcial ingresado por el usuario.</param>
        /// <param name="mpio">Filtro opcional para buscar por Municipio exacto.</param>
        /// <param name="region">Filtro opcional para buscar por Región exacta.</param>
        /// <param name="estado">Filtro opcional de estado (ej: 'calendarizadas', 'porliberar', o vacío).</param>
        /// <returns>Objeto JSON con el formato requerido por DataTables, incluyendo draw, recordsTotal, recordsFiltered y data.</returns>
        [HttpGet]
        [Route("matriz/paginado")]
        public IHttpActionResult GetMatrizPaginado(int draw = 1, int start = 0, int length = 10, string search = "", string mpio = "", string region = "", string estado = "")
        {
            try
            {
                // Leer el valor de búsqueda nativo de DataTables
                var searchVal = System.Web.HttpContext.Current.Request.QueryString["search[value]"] ?? search;

                var (data, totalCount) = _repository.ObtenerMatrizPresupuestoPaginado(start, length, searchVal, mpio, region, estado);
                
                return Ok(new
                {
                    draw = draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount, 
                    data = data
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Obtiene la información detallada de una clave presupuestal.
        /// Incluye los saldos (aprobado, devengado, pagado, etc.), los montos programados por mes y el historial de apartados de los últimos 7 días.
        /// </summary>
        /// <param name="clave">La Clave Presupuestal de 56 caracteres a consultar.</param>
        /// <returns>Objeto con el detalle presupuestal o NotFound si la clave no existe.</returns>
        [HttpGet]
        [Route("detalle")]
        public IHttpActionResult GetDetalle(string clave)
        {
            try
            {
                var detalle = _repository.ObtenerDetalleClave(clave);
                if (detalle == null) return NotFound();
                return Ok(detalle);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("filtros/municipios")]
        public IHttpActionResult GetFiltrosMunicipios(string region = "")
        {
            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString))
                {
                    string sql = "SELECT DISTINCT Nombre FROM cat_municipio WHERE Nombre IS NOT NULL AND Nombre <> ''";
                    if (!string.IsNullOrEmpty(region))
                    {
                        sql = "SELECT DISTINCT m.Nombre FROM cat_municipio m JOIN cat_region r ON m.RegionId = r.Id WHERE m.Nombre IS NOT NULL AND m.Nombre <> '' AND r.Descripcion = @Region";
                    }
                    sql += " ORDER BY Nombre";
                    var data = Dapper.SqlMapper.Query<string>(conn, sql, new { Region = region }).ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("filtros/regiones")]
        public IHttpActionResult GetFiltrosRegiones()
        {
            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString))
                {
                    var data = Dapper.SqlMapper.Query<string>(conn, "SELECT DISTINCT Descripcion FROM cat_region WHERE Descripcion IS NOT NULL AND Descripcion <> '' ORDER BY Descripcion").ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Genera un nuevo apartado (reserva) de recurso en una clave presupuestal dada.
        /// Registra la operación indicando el monto, motivo y oficio que ampara el apartado.
        /// Si es exitoso, deja una traza en el log del sistema del usuario que realizó la acción.
        /// </summary>
        /// <param name="req">Petición con los datos del apartado: clave, monto, motivo y oficio.</param>
        /// <returns>Objeto indicando 'success: true' si se realizó correctamente, o BadRequest si falló la validación o el registro.</returns>
        [HttpPost]
        [Route("apartar")]
        public IHttpActionResult Apartar([FromBody] ApartarRequest req)
        {
            try
            {
                if (req == null || string.IsNullOrEmpty(req.Clave) || req.Monto <= 0)
                {
                    return BadRequest("Datos de apartado inválidos.");
                }
                bool ok = _repository.RegistrarApartado(req.Clave, req.Monto, req.Motivo, req.Oficio);
                if (ok) 
                {
                    RegistrarLog("Apartar", string.Format("Clave: {0}, Monto: {1}", req.Clave, req.Monto));
                    return Ok(new { success = true });
                }
                return BadRequest("Ya existe un apartado en curso para esta clave.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("cancelar-apartado")]
        public IHttpActionResult CancelarApartado([FromBody] ApartarRequest req)
        {
            try
            {
                if (req == null || string.IsNullOrEmpty(req.Clave))
                {
                    return BadRequest("Clave no proporcionada.");
                }
                bool ok = _repository.CancelarApartado(req.Clave);
                if (ok) RegistrarLog("CancelarApartado", string.Format("Clave: {0}", req.Clave));
                return Ok(new { success = ok });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public class CancelarApartadoIdRequest
        {
            public int Id { get; set; }
            public string Clave { get; set; } // Opcional, para actualizar la vista después
        }

        [HttpPost]
        [Route("cancelar-apartado-id")]
        public IHttpActionResult CancelarApartadoPorId([FromBody] CancelarApartadoIdRequest req)
        {
            try
            {
                if (req == null || req.Id <= 0)
                {
                    return BadRequest("Id no proporcionado.");
                }
                bool ok = _repository.CancelarApartadoPorId(req.Id);
                if (ok) RegistrarLog("CancelarApartadoPorId", string.Format("ID Apartado: {0}", req.Id));
                return Ok(new { success = ok });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Descompone una clave presupuestal de 56 caracteres en sus segmentos conformantes (UR, Función, Fondo, etc.) y obtiene la descripción de cada uno consultando a los catálogos correspondientes en base de datos.
        /// </summary>
        /// <param name="clave">La Clave Presupuestal completa.</param>
        /// <returns>El objeto decodificado con una lista de segmentos y sus descripciones en texto claro.</returns>
        [HttpGet]
        [Route("decodificar")]
        public IHttpActionResult Decodificar(string clave)
        {
            try
            {
                var decodificada = _repository.DecodificarClave(clave);
                return Ok(decodificada);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Actualiza la calendarización (distribución mensual del gasto) de una clave presupuestal en base de datos.
        /// Asigna los montos de enero a diciembre definidos en la petición para la clave en cuestión y registra un log del evento.
        /// </summary>
        /// <param name="req">Objeto con la clave y los montos a actualizar por mes.</param>
        /// <returns>Respuesta HTTP de Ok en caso de éxito, o InternalServerError en caso de fallo.</returns>
        [HttpPost]
        [Route("calendarizar")]
        public IHttpActionResult Calendarizar([FromBody] PresupuestoDetalleDto req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.ClavePresupuestal)) return BadRequest("Clave requerida.");
                _repository.GuardarCalendarizacion(req);
                RegistrarLog("Calendarizar", string.Format("Clave: {0}", req.ClavePresupuestal));
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("nueva")]
        public IHttpActionResult NuevaClave([FromBody] PresupuestoDetalleDto req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.ClavePresupuestal) || req.ClavePresupuestal.Length != 56) 
                    return BadRequest("La clave debe tener 56 caracteres exactos.");
                if (req.Aprobado <= 0)
                    return BadRequest("El monto aprobado debe ser mayor a 0.");

                _repository.InsertarNuevaClaveManual(req.ClavePresupuestal, req.Aprobado);
                RegistrarLog("NuevaClave", string.Format("Clave: {0}, Aprobado: {1}", req.ClavePresupuestal, req.Aprobado));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("obra")]
        public IHttpActionResult GetClavesRelacionadas(string prefijo)
        {
            try
            {
                var claves = _repository.ObtenerClavesPorObra(prefijo);
                return Ok(claves);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("previewExcel")]
        public IHttpActionResult PreviewExcel()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count == 0)
                    return BadRequest("No se recibió ningún archivo.");

                var postedFile = httpRequest.Files[0];
                var anio = httpRequest.Form["anio"];
                var oficio = httpRequest.Form["oficio"];

                string ext = Path.GetExtension(postedFile.FileName).ToLower();
                if (ext != ".xlsx")
                    return BadRequest("El archivo debe ser formato .xlsx");

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<PresupuestoDetalleDto> lista = new List<PresupuestoDetalleDto>();

                using (var package = new ExcelPackage(postedFile.InputStream))
                {
                    var worksheet = package.Workbook.Worksheets["MAESTRO"];
                    if (worksheet == null) return BadRequest("La hoja de Excel debe llamarse 'MAESTRO'.");

                    int rows = worksheet.Dimension.Rows;
                    int cols = worksheet.Dimension.Columns;

                    var headerMap = new Dictionary<string, int>();
                    for (int col = 1; col <= cols; col++)
                    {
                        string headerName = worksheet.Cells[1, col].Text.Trim().ToLower();
                        if (!headerMap.ContainsKey(headerName))
                            headerMap.Add(headerName, col);
                    }

                    string[] camposObligatorios = {
                        "claves","ur","uro","fi","fu","sf","ssf","ps","pp","oa","ai","pt",
                        "mpio","ga","ogc","og","gto","ag","tg","tr","ff","sff","ra",
                        "pf","ri","cp","dm"
                    };

                    foreach (var reqCol in camposObligatorios)
                    {
                        if (!headerMap.ContainsKey(reqCol))
                            return BadRequest($"No se encontró la columna obligatoria '{reqCol}'.");
                    }

                    for (int row = 2; row <= rows; row++)
                    {
                        var claveVal = worksheet.Cells[row, headerMap["claves"]].Text.Trim();
                        if (string.IsNullOrEmpty(claveVal)) continue;
                        if (claveVal.Length != 56) return BadRequest($"Error en fila {row}: La clave debe tener 56 dígitos (Tiene {claveVal.Length}). Clave: {claveVal}");

                        var item = new PresupuestoDetalleDto { ClavePresupuestal = claveVal };
                        
                        item.Aprobado = LeerDecimal(worksheet, row, "aprobado", headerMap);
                        item.Enero = LeerDecimal(worksheet, row, "enero", headerMap);
                        item.Febrero = LeerDecimal(worksheet, row, "febrero", headerMap);
                        item.Marzo = LeerDecimal(worksheet, row, "marzo", headerMap);
                        item.Abril = LeerDecimal(worksheet, row, "abril", headerMap);
                        item.Mayo = LeerDecimal(worksheet, row, "mayo", headerMap);
                        item.Junio = LeerDecimal(worksheet, row, "junio", headerMap);
                        item.Julio = LeerDecimal(worksheet, row, "julio", headerMap);
                        item.Agosto = LeerDecimal(worksheet, row, "agosto", headerMap);
                        item.Septiembre = LeerDecimal(worksheet, row, "septiembre", headerMap);
                        item.Octubre = LeerDecimal(worksheet, row, "octubre", headerMap);
                        item.Noviembre = LeerDecimal(worksheet, row, "noviembre", headerMap);
                        item.Diciembre = LeerDecimal(worksheet, row, "diciembre", headerMap);
                        
                        lista.Add(item);
                    }
                }

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private decimal LeerDecimal(ExcelWorksheet worksheet, int row, string colName, Dictionary<string, int> headerMap)
        {
            if (!headerMap.ContainsKey(colName)) return 0m;
            int col = headerMap[colName];
            string val = worksheet.Cells[row, col].Text.Trim();
            if (string.IsNullOrEmpty(val)) return 0m;
            if (double.TryParse(val, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double temp))
            {
                if (Math.Abs(temp) < 0.000001) return 0m;
                return Math.Round((decimal)temp, 2);
            }
            return 0m;
        }

        [HttpPost]
        [Route("saveMasivo")]
        public IHttpActionResult SaveMasivoExcel([FromBody] CargaMasivaDto req)
        {
            try
            {
                if (req == null || req.Claves == null || req.Claves.Count == 0)
                    return BadRequest("No hay datos para guardar.");
                
                var session = System.Web.HttpContext.Current?.Session;
                string usuariocreo = session != null && session["UsuarioId"] != null ? session["UsuarioId"].ToString() : "0";

                _repository.InsertarPresupuestoMasivo(req, usuariocreo);
                
                RegistrarLog("CargaMasiva", $"Se cargaron {req.Claves.Count} claves. Oficio: {req.Oficio}, Año: {req.Anio}");
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
