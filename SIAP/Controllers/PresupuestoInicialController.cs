using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using SIAP.Models;
using SIAP.Repositories;
using SIAP.Services;

namespace SIAP.Controllers
{
    /// <summary>
    /// Carga del presupuesto inicial: validar el Excel, confirmar la importación, historial de cargas.
    /// El archivo se valida y se guarda en el servidor; la importación vuelve a leer ese mismo archivo,
    /// así lo que se guarda es exactamente lo que se validó.
    /// </summary>
    [RoutePrefix("api/presupuestoinicial")]
    public class PresupuestoInicialController : ApiController
    {
        private const string CarpetaBase = "~/Uploads/PresupuestoInicial";

        private static string UsuarioId()
        {
            var session = HttpContext.Current == null ? null : HttpContext.Current.Session;
            return session != null && session["UsuarioId"] != null ? session["UsuarioId"].ToString() : null;
        }

        private static void Log(string accion, string detalle)
        {
            try
            {
                string u = UsuarioId();
                if (u != null) new UsuarioRepository().RegistrarLog(Convert.ToInt32(u), "Presupuesto Inicial", accion, detalle);
            }
            catch { /* la bitácora no debe detener la operación */ }
        }

        private static string RutaFisica(string relativa)
        {
            return HttpContext.Current.Server.MapPath(relativa);
        }

        private static string CarpetaPendientes()
        {
            string dir = RutaFisica(CarpetaBase + "/pendientes");
            Directory.CreateDirectory(dir);
            // Limpia archivos validados que nunca se confirmaron (más de un día)
            try
            {
                foreach (var f in Directory.GetFiles(dir, "*.xlsx"))
                    if (File.GetCreationTime(f) < DateTime.Now.AddDays(-1)) File.Delete(f);
            }
            catch { }
            return dir;
        }

        private static string NombreSeguro(string nombre)
        {
            string n = Path.GetFileName(nombre ?? "presupuesto.xlsx");
            n = Regex.Replace(n, @"[^\w\.\-]+", "_");
            return n.Length > 120 ? n.Substring(n.Length - 120) : n;
        }

        [HttpGet]
        [Route("anios")]
        public IHttpActionResult Anios()
        {
            try { return Ok(new EstructuraClaveRepository().AniosConfigurados()); }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [HttpGet]
        [Route("estructura")]
        public IHttpActionResult Estructura(int anio)
        {
            try { return Ok(new EstructuraClaveRepository().ObtenerEstructura(anio)); }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [HttpGet]
        [Route("historial")]
        public IHttpActionResult Historial()
        {
            try { return Ok(new PresupuestoInicialService().Historial()); }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        /// <summary>
        /// Valida el Excel (multipart: anio, oficio, archivo). Si es válido lo guarda y devuelve un token
        /// para confirmar la importación. Si no, devuelve los errores por fila y no guarda nada.
        /// </summary>
        [HttpPost]
        [Route("validar")]
        public IHttpActionResult Validar()
        {
            try
            {
                if (UsuarioId() == null) return Content(HttpStatusCode.Unauthorized, "La sesión expiró. Vuelva a iniciar sesión.");

                var req = HttpContext.Current.Request;
                if (req.Files.Count == 0) return BadRequest("No se recibió ningún archivo.");
                var archivo = req.Files[0];
                if (!string.Equals(Path.GetExtension(archivo.FileName), ".xlsx", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("El archivo debe ser de Excel (.xlsx).");

                int anio;
                if (!int.TryParse(req.Form["anio"], out anio)) return BadRequest("Seleccione el año.");
                string oficio = (req.Form["oficio"] ?? "").Trim();

                // Se trabaja sobre una copia en memoria para poder leerla y luego guardarla
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    archivo.InputStream.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                var servicio = new PresupuestoInicialService();
                List<FilaPresupuestoInicial> filas;
                ResultadoValidacionPresupuesto res;
                using (var ms = new MemoryStream(bytes))
                    res = servicio.Validar(ms, anio, oficio, Path.GetFileName(archivo.FileName), out filas);

                if (res.Valido)
                {
                    res.Token = Guid.NewGuid().ToString("N");
                    File.WriteAllBytes(Path.Combine(CarpetaPendientes(), res.Token + ".xlsx"), bytes);
                }

                Log("Validar", string.Format("Año {0}, oficio {1}, archivo {2}: {3} filas, {4} errores",
                    anio, oficio, res.Archivo, res.TotalFilas, res.TotalErrores + res.ErroresGenerales.Count));
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "No se pudo leer el archivo: " + ex.Message);
            }
        }

        /// <summary>Importa un archivo ya validado. Vuelve a validarlo antes de guardar.</summary>
        [HttpPost]
        [Route("importar")]
        public IHttpActionResult Importar([FromBody] ImportarPresupuestoRequest req)
        {
            string destinoFisico = null;
            try
            {
                string usuario = UsuarioId();
                if (usuario == null) return Content(HttpStatusCode.Unauthorized, "La sesión expiró. Vuelva a iniciar sesión.");
                Guid token;
                if (req == null || !Guid.TryParseExact(req.Token ?? "", "N", out token))
                    return BadRequest("Vuelva a validar el archivo antes de importarlo.");

                string pendiente = Path.Combine(CarpetaPendientes(), token.ToString("N") + ".xlsx");
                if (!File.Exists(pendiente)) return BadRequest("La validación expiró. Vuelva a validar el archivo.");

                var servicio = new PresupuestoInicialService();
                List<FilaPresupuestoInicial> filas;
                ResultadoValidacionPresupuesto res;
                using (var fs = File.OpenRead(pendiente))
                    res = servicio.Validar(fs, req.Anio, (req.Oficio ?? "").Trim(), NombreSeguro(req.Archivo), out filas);
                if (!res.Valido) return Content(HttpStatusCode.BadRequest, res);

                // El Excel original se conserva para poder descargarlo desde el historial
                string carpetaAnio = CarpetaBase + "/" + req.Anio;
                Directory.CreateDirectory(RutaFisica(carpetaAnio));
                string nombreFinal = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + NombreSeguro(req.Archivo);
                string relativa = carpetaAnio.TrimStart('~', '/') + "/" + nombreFinal;
                destinoFisico = RutaFisica(carpetaAnio + "/" + nombreFinal);
                File.Copy(pendiente, destinoFisico);

                int cargaId = servicio.Importar(filas, res, usuario, relativa);
                try { File.Delete(pendiente); } catch { }

                Log("Importar", string.Format("Año {0}, oficio {1}: {2} claves, aprobado {3:N2}. Carga {4}",
                    req.Anio, res.Oficio, filas.Count, res.TotalAprobado, cargaId));
                return Ok(new { success = true, cargaId, claves = filas.Count, aprobado = res.TotalAprobado });
            }
            catch (Exception ex)
            {
                if (destinoFisico != null) { try { File.Delete(destinoFisico); } catch { } }
                return Content(HttpStatusCode.InternalServerError, "No se importó nada. " + ex.Message);
            }
        }

        /// <summary>Descarga el Excel original de una carga del historial.</summary>
        [HttpGet]
        [Route("archivo/{id:int}")]
        public HttpResponseMessage Archivo(int id)
        {
            if (UsuarioId() == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            string relativa;
            var carga = new PresupuestoInicialService().ObtenerCarga(id, out relativa);
            if (carga == null || string.IsNullOrEmpty(relativa)) return new HttpResponseMessage(HttpStatusCode.NotFound);

            string baseFisica = Path.GetFullPath(RutaFisica(CarpetaBase));
            string ruta = Path.GetFullPath(RutaFisica("~/" + relativa));
            if (!ruta.StartsWith(baseFisica, StringComparison.OrdinalIgnoreCase) || !File.Exists(ruta))
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var resp = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(File.ReadAllBytes(ruta)) };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = carga.Archivo ?? "presupuesto.xlsx" };
            return resp;
        }
    }
}
