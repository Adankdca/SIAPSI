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
using System.Data.SqlClient;

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
        /// Consulta General paginada para DataTables, por ejercicio. Los filtros por segmento llegan en
        /// "seg" como UR:08|PP:H04|MPIO:101. Las descripciones salen de los segmentos guardados de cada clave.
        /// </summary>
        [HttpGet]
        [Route("matriz/paginado")]
        public IHttpActionResult GetMatrizPaginado(int anio, int draw = 1, int start = 0, int length = 10,
                                                   string seg = "", string region = "", string estado = "")
        {
            try
            {
                var qs = System.Web.HttpContext.Current.Request.QueryString;
                var filtro = new FiltroMatriz
                {
                    Anio = anio, Start = start, Length = length,
                    Search = qs["search[value]"], Estado = estado, Region = region
                };
                foreach (var par in (seg ?? "").Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var kv = par.Split(new[] { ':' }, 2);
                    if (kv.Length == 2 && kv[1].Length > 0) filtro.Segmentos[kv[0]] = kv[1];
                }
                string colIdx = qs["order[0][column]"];
                if (!string.IsNullOrEmpty(colIdx))
                {
                    filtro.OrdenColumna = qs["columns[" + colIdx + "][name]"];
                    filtro.OrdenDescendente = string.Equals(qs["order[0][dir]"], "desc", StringComparison.OrdinalIgnoreCase);
                }

                var res = new SIAP.Services.PresupuestoConsultaService().Matriz(filtro);
                return Ok(new
                {
                    draw = draw,
                    recordsTotal = res.Total,
                    recordsFiltered = res.Filtradas,
                    totales = res.Totales,
                    data = res.Filas
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>Ejercicios con presupuesto cargado o con estructura de clave configurada.</summary>
        [HttpGet]
        [Route("ejercicios")]
        public IHttpActionResult GetEjercicios()
        {
            try { return Ok(new SIAP.Services.PresupuestoConsultaService().Ejercicios()); }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        /// <summary>Valores de un segmento (UR, PP, FF, MPIO...) usados en el ejercicio, para los filtros.</summary>
        [HttpGet]
        [Route("filtros/segmento")]
        public IHttpActionResult GetOpcionesSegmento(int anio, string abrev)
        {
            try { return Ok(new SIAP.Services.PresupuestoConsultaService().OpcionesSegmento(anio, abrev)); }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        /// <summary>Valores de segmento del ejercicio que no están en su catálogo.</summary>
        [HttpGet]
        [Route("sincatalogo")]
        public IHttpActionResult GetSinCatalogo(int anio)
        {
            try { return Ok(new SIAP.Services.PresupuestoConsultaService().ValoresSinCatalogo(anio)); }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        /// <summary>
        /// Vuelve a buscar en los catálogos los segmentos de todas las claves del ejercicio
        /// (después de dar de alta los valores que faltaban). No cambia importes.
        /// </summary>
        [HttpPost]
        [Route("reprocesar-catalogos")]
        public IHttpActionResult ReprocesarCatalogos(int anio)
        {
            try
            {
                var session = System.Web.HttpContext.Current?.Session;
                if (session == null || session["UsuarioId"] == null)
                    return Content(System.Net.HttpStatusCode.Unauthorized, "La sesión expiró. Vuelva a iniciar sesión.");
                var res = new SIAP.Services.PresupuestoConsultaService().ReprocesarCatalogos(anio);
                RegistrarLog("ReprocesarCatalogos", string.Format("Ejercicio {0}: {1} claves, {2} segmentos actualizados, sin catálogo {3} → {4}",
                    anio, res.Claves, res.SegmentosActualizados, res.SinCatalogoAntes, res.SinCatalogoDespues));
                return Ok(res);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (Exception ex) { return InternalServerError(ex); }
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
                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString))
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
                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString))
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

        /// <summary>
        /// Alta manual de una clave en el ejercicio. Valida la longitud y el CP según la estructura del año;
        /// el aprobado queda por liberar. Devuelve los segmentos que no están en su catálogo (no impiden el alta).
        /// </summary>
        [HttpPost]
        [Route("nueva")]
        public IHttpActionResult NuevaClave([FromBody] NuevaClaveRequest req)
        {
            try
            {
                var session = System.Web.HttpContext.Current?.Session;
                if (session == null || session["UsuarioId"] == null)
                    return Content(System.Net.HttpStatusCode.Unauthorized, "La sesión expiró. Vuelva a iniciar sesión.");

                var segs = new SIAP.Services.PresupuestoConsultaService().InsertarClaveManual(req);
                var sinCatalogo = segs.Where(s => s.TablaCatalogo != null && !s.CatalogoId.HasValue)
                                      .Select(s => s.Abreviatura + " " + s.Valor).ToList();
                RegistrarLog("NuevaClave", string.Format("Ejercicio {0}, clave {1}, aprobado {2:N2}, oficio {3}",
                    req.Anio, req.ClavePresupuestal, req.Aprobado, req.Oficio));
                return Ok(new { success = true, sinCatalogo });
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                return BadRequest("La clave ya existe en el presupuesto.");
            }
            catch (Exception ex) { return InternalServerError(ex); }
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



    }
}
