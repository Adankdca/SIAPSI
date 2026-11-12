using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SIAP.Services;
using System.Collections.Concurrent;
using System.Configuration;
using Dapper;
using MySql.Data.MySqlClient;

namespace SIAP.Controllers
{
    public class UploadOficioController : ApiController
    {
        // Almacen temporal en memoria para oficios procesados antes de confirmar
        private static ConcurrentDictionary<string, OficioLecturaResult> OficiosPendientes = new ConcurrentDictionary<string, OficioLecturaResult>();

        [HttpPost]
        [Route("api/UploadOficio")]
        public async Task<IHttpActionResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported media type.");
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                var file = provider.Contents.FirstOrDefault();
                if (file == null)
                {
                    return BadRequest("No file uploaded.");
                }

                string filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();

                var service = new LectorOficiosService();
                using (var ms = new MemoryStream(buffer))
                {
                    var result = service.ProcesarArchivo(ms, filename);
                    
                    if (result.Success)
                    {
                        bool isDuplicate = false;
                        if (!string.IsNullOrWhiteSpace(result.Oficio))
                        {
                            using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                            {
                                conn.Open();
                                string sqlDuplicate = "SELECT COUNT(1) FROM adecuaciones WHERE NumSolicitud = @oficio OR NumOficio = @oficio";
                                int countDup = conn.ExecuteScalar<int>(sqlDuplicate, new { oficio = result.Oficio });
                                if (countDup > 0)
                                {
                                    isDuplicate = true;
                                }
                            }
                        }

                        // Save the physical file
                        string safeOficioName = string.Join("_", result.Oficio?.Split(Path.GetInvalidFileNameChars()) ?? new string[0]);
                        if (string.IsNullOrWhiteSpace(safeOficioName) || safeOficioName == "DESCONOCIDO") {
                            safeOficioName = "Oficio_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            result.Oficio = safeOficioName;
                        }
                        
                        string uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/Oficios/Adecuaciones/");
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }
                        
                        string filePath = Path.Combine(uploadDir, safeOficioName + ".pdf");
                        File.WriteAllBytes(filePath, buffer);

                        string token = Guid.NewGuid().ToString();

                        // Verificar si las claves existen
                        if (result.Movimientos != null && result.Movimientos.Count > 0)
                        {
                            using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                            {
                                conn.Open();
                                string sqlCheck = "SELECT COUNT(1) FROM presupuestocalendarizado WHERE ClavePresupuestal = @clave";
                                foreach (var mov in result.Movimientos)
                                {
                                    int count = conn.ExecuteScalar<int>(sqlCheck, new { clave = mov.ClavePresupuestal });
                                    mov.EsNuevaClave = (count == 0);
                                }
                            }
                        }

                        OficiosPendientes.TryAdd(token, result);
                        return Ok(new { success = true, token = token, isDuplicate = isDuplicate, data = result });
                    }
                    else
                    {
                        return Ok(new { success = false, message = result.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/UploadOficio/Aplicar")]
        public IHttpActionResult Aplicar(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || !OficiosPendientes.TryGetValue(token, out OficioLecturaResult result))
                {
                    return Ok(new { success = false, message = "Token inválido o expirado." });
                }

                // Aquí llamaríamos a AdecuacionesController para guardar en DB
                // Para este ejemplo, llamaremos a la lógica de guardado masivo en AdecuacionesController
                var adecuacionesController = new AdecuacionesController();
                
                // Formateamos los datos para que AdecuacionesController los acepte (Payload)
                var payload = new SIAP.Controllers.AdecuacionManualDto
                {
                    NumSolicitud = result.Oficio,
                    Asunto = "Oficio importado " + result.Oficio,
                    TipoAdecuacionId = 2,
                    TipoAdecuacion = "Ampliación", // O lo que detecte
                    FechaSolicitud = DateTime.Now,
                    Concepto = "Importado masivamente mediante lector de PDF/Excel",
                    MontoTotal = result.TotalAmpliacion > 0 ? result.TotalAmpliacion : result.TotalReduccion,
                    Estatus = "Registrada",
                    ApartadoId = 1,
                    Movimientos = result.Movimientos.Select(m => new SIAP.Controllers.MovimientoManualDto 
                    {
                        ClavePresupuestal = m.ClavePresupuestal,
                        TipoOperacion = m.MontoAmpliacion > 0 ? "Ampliación" : "Reducción",
                        MontoTotal = m.MontoAmpliacion > 0 ? m.MontoAmpliacion : m.MontoReduccion
                    }).ToList()
                };

                var saveResult = adecuacionesController.GuardarManual(payload);
                
                // Removemos el token para que no se use doble
                OficiosPendientes.TryRemove(token, out _);
                
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/UploadOficio/AplicarPersonalizado")]
        public IHttpActionResult AplicarPersonalizado([FromBody] SIAP.Controllers.AdecuacionManualDto dto)
        {
            try
            {
                if (dto == null || dto.Movimientos == null || dto.Movimientos.Count == 0)
                {
                    return Json(new { success = false, message = "Datos inválidos o sin movimientos." });
                }

                // Validar duplicidad estricta aquí antes de guardar
                if (!string.IsNullOrWhiteSpace(dto.NumSolicitud))
                {
                    using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
                    {
                        conn.Open();
                        string sqlDuplicate = "SELECT COUNT(1) FROM adecuaciones WHERE NumSolicitud = @oficio OR NumOficio = @oficio";
                        int countDup = conn.ExecuteScalar<int>(sqlDuplicate, new { oficio = dto.NumSolicitud });
                        if (countDup > 0)
                        {
                            return Json(new { success = false, message = "El número de solicitud '" + dto.NumSolicitud + "' ya existe en la base de datos. Por favor, cambie el número de solicitud en la pantalla de revisión." });
                        }
                    }
                }

                var adecuacionesController = new AdecuacionesController();
                adecuacionesController.ControllerContext = this.ControllerContext;
                
                
                
                // Mapear los tipos de operacion seleccionados por el usuario a IDs o claves
                foreach(var mov in dto.Movimientos)
                {
                    if (mov.TipoOperacion == "Ampliación") mov.TipoOperacion = "3";
                    else if (mov.TipoOperacion == "Reducción") mov.TipoOperacion = "4";
                    else if (mov.TipoOperacion == "Traspaso Incremento") mov.TipoOperacion = "1";
                    else if (mov.TipoOperacion == "Traspaso Decremento") mov.TipoOperacion = "2";
                }

                // Calcular totales
                decimal totalReduccion = 0;
                decimal totalAmpliacion = 0;
                bool hasTraspasos = false;

                foreach (var mov in dto.Movimientos)
                {
                    if (mov.TipoOperacion == "4" || mov.TipoOperacion == "2") totalReduccion += mov.MontoTotal;
                    else totalAmpliacion += mov.MontoTotal;
                }

                if (totalAmpliacion > 0 && totalReduccion > 0) hasTraspasos = true;

                // Set headers based on logic
                dto.Asunto = "Oficio importado " + dto.NumSolicitud;
                
                if (hasTraspasos) {
                    dto.TipoAdecuacionId = 1; // 1 = Traspaso
                    dto.TipoAdecuacion = "Traspaso";
                } else if (totalAmpliacion > 0 && totalReduccion == 0) {
                    dto.TipoAdecuacionId = 3; // Ampliacion
                    dto.TipoAdecuacion = "Ampliación";
                } else if (totalReduccion > 0 && totalAmpliacion == 0) {
                    dto.TipoAdecuacionId = 4; // Reduccion
                    dto.TipoAdecuacion = "Reducción";
                } else {
                    dto.TipoAdecuacionId = 1; // Default
                    dto.TipoAdecuacion = "Mixto / Traspaso";
                }
                
                // Actualizar las operaciones de los movimientos si es traspaso
                foreach (var mov in dto.Movimientos)
                {
                    if (hasTraspasos)
                    {
                        if (mov.TipoOperacion == "Ampliación" || mov.TipoOperacion == "3") 
                            mov.TipoOperacion = "Traspaso Incremento"; // 1
                        if (mov.TipoOperacion == "Reducción" || mov.TipoOperacion == "4") 
                            mov.TipoOperacion = "Traspaso Decremento"; // 2
                    }
                }
                
                if (dto.FechaSolicitud == default(DateTime))
                {
                    dto.FechaSolicitud = DateTime.Now;
                }
                dto.Concepto = "Importado y revisado manualmente";
                dto.MontoTotal = totalAmpliacion > 0 ? totalAmpliacion : totalReduccion;
                dto.Estatus = "Registrada";
                dto.ApartadoId = 1;
                dto.NumOficio = dto.NumSolicitud;

                var saveResult = adecuacionesController.GuardarManual(dto);
                return saveResult; // Regresa el Ok de GuardarManual
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }
    }
}
