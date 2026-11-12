using SIAP.Models;
using SIAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SIAP.Controllers
{
    /// <summary>
    /// Controlador principal para el módulo de Ministraciones.
    /// Expone los endpoints REST para registrar, consultar y administrar el dinero ministrado.
    /// Funciona como intermediario entre la vista (JS) y la capa de datos (MinistracionesRepository).
    /// </summary>
    public class MinistracionesController : ApiController
    {
        private MinistracionesRepository _repo = new MinistracionesRepository();

        //============================================================================================================
        //==================================CATALOGOS=================================================================
        //============================================================================================================

        [HttpGet]
        [Route("api/Ministraciones/getTipoministracion")]
        public IHttpActionResult GetTipoministracion()
        {
            try
            {
                return Ok(_repo.GetTiposMinistracion());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/getDigitoMinistracion")]
        public IHttpActionResult GetDigitoMinistracion()
        {
            try
            {
                return Ok(_repo.GetDigitosMinistracion());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/getFuenteFinanciamiento")]
        public IHttpActionResult GetFuenteFinanciamiento()
        {
            try
            {
                return Ok(_repo.GetFuentesFinanciamiento());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/getProgramasFondo")]
        public IHttpActionResult GetProgramasFondo()
        {
            try
            {
                return Ok(_repo.GetProgramasFondo());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetMinistracionesCatalogo")]
        public IHttpActionResult GetMinistracionesCatalogo()
        {
            try
            {
                return Ok(_repo.GetMinistracionesCatalogo());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/getmes")]
        public IHttpActionResult GetMes()
        {
            try
            {
                return Ok(_repo.Meses);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // =============================================
        // NUEVOS CATÁLOGOS
        // =============================================

        [HttpGet]
        [Route("api/Ministraciones/GetTiposMinistracionFolio")]
        public IHttpActionResult GetTiposMinistracionFolio()
        {
            try
            {
                return Ok(_repo.GetTiposMinistracionFolio());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetSubFuentesFinanciamiento")]
        public IHttpActionResult GetSubFuentesFinanciamiento()
        {
            try
            {
                return Ok(_repo.GetSubFuentesFinanciamiento());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetProyectosEstrategicos")]
        public IHttpActionResult GetProyectosEstrategicos()
        {
            try
            {
                return Ok(_repo.GetProyectosEstrategicos());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetRamos")]
        public IHttpActionResult GetRamos()
        {
            try
            {
                return Ok(_repo.GetRamos());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetTiposRecurso")]
        public IHttpActionResult GetTiposRecurso()
        {
            try
            {
                return Ok(_repo.GetTiposRecurso());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetProgramaFondoById")]
        public IHttpActionResult GetProgramaFondoById(int id)
        {
            try
            {
                var result = _repo.GetProgramaFondoById(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetSiguienteFolio")]
        public IHttpActionResult GetSiguienteFolio(int anio, int mes, int idTipoFolio)
        {
            try
            {
                var result = _repo.GetSiguienteFolio(anio, mes, idTipoFolio);
                return Ok(new { folio = result });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en GetSiguienteFolio: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        //============================================================================================================
        //==================================CONSULTAS=================================================================
        //============================================================================================================

        [HttpGet]
        [Route("api/Ministraciones/getMinistraciones")]
        public IHttpActionResult GetMinistraciones(
            string busqueda = "",
            string ministracion = "",
            string mes = "",
            string tipo = "",
            string programaFondo = "",
            string estatus = "")
        {
            try
            {
                int? mesInt = string.IsNullOrEmpty(mes) ? (int?)null : int.Parse(mes);
                int? tipoInt = string.IsNullOrEmpty(tipo) ? (int?)null : int.Parse(tipo);
                int? programaFondoInt = string.IsNullOrEmpty(programaFondo) ? (int?)null : int.Parse(programaFondo);

                var datos = _repo.GetMinistraciones(
                    filtro: busqueda,
                    mes: mesInt,
                    tipoMinistracionId: tipoInt,
                    programaFondoId: programaFondoInt,
                    estatus: string.IsNullOrEmpty(estatus) ? null : estatus
                );

                if (!string.IsNullOrEmpty(ministracion))
                {
                    datos = datos.Where(x => x.Id.ToString() == ministracion).ToList();
                }

                return Ok(datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en GetMinistraciones: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetMinistracionById")]
        public IHttpActionResult GetMinistracionById(int id)
        {
            try
            {
                var result = _repo.GetMinistracionById(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en GetMinistracionById: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetMinistracionDetalle")]
        public IHttpActionResult GetMinistracionDetalle(int id)
        {
            try
            {
                var detalle = _repo.GetMinistracionDetalle(id);
                if (detalle == null) return NotFound();
                return Ok(detalle);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetMinistracionDetalleCompleto")]
        public IHttpActionResult GetMinistracionDetalleCompleto(int id)
        {
            try
            {
                var detalle = _repo.GetMinistracionDetalleCompleto(id);
                if (detalle == null) return NotFound();
                return Ok(detalle);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en GetMinistracionDetalleCompleto: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetClavesPresupuestales")]
        public IHttpActionResult GetClavesPresupuestales(string filtro = "")
        {
            try
            {
                return Ok(_repo.GetClavesPresupuestales(filtro));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetClavesPresupuestalesPorMes")]
        public IHttpActionResult GetClavesPresupuestalesPorMes(int? mes = null, int? anio = null)
        {
            try
            {
                int mesActual = mes ?? DateTime.Now.Month;
                int anioActual = anio ?? DateTime.Now.Year;

                var result = _repo.GetClavesPresupuestalesPorMes(mesActual, anioActual);
                return Ok(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en GetClavesPresupuestalesPorMes: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetClavesPresupuestalesOtrosMeses")]
        public IHttpActionResult GetClavesPresupuestalesOtrosMeses(int mesActual, int anio)
        {
            try
            {
                var result = _repo.GetClavesPresupuestalesOtrosMeses(mesActual, anio);
                return Ok(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en GetClavesPresupuestalesOtrosMeses: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetSumaClavesPorMes")]
        public IHttpActionResult GetSumaClavesPorMes(int mes, string clave)
        {
            try
            {
                var result = _repo.GetSumaClavesPorMes(mes, clave);
                return Ok(new { suma = result });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en GetSumaClavesPorMes: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetRecursosPorMesYFondo")]
        public IHttpActionResult GetRecursosPorMesYFondo(int mes, int fondo)
        {
            try
            {
                var result = _repo.GetRecursosPorMesYFondo(mes, fondo);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetEstadisticas")]
        public IHttpActionResult GetEstadisticas()
        {
            try
            {
                return Ok(_repo.GetEstadisticas());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/GetComparacionMes")]
        public IHttpActionResult GetComparacionMes(int id, int mes, int anio)
        {
            try
            {
                var result = _repo.GetComparacionMes(id, mes, anio);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //============================================================================================================
        //==================================CRUD======================================================================
        //============================================================================================================

        [HttpPost]
        [Route("api/Ministraciones/GuardarMinistracion")]
        public IHttpActionResult GuardarMinistracion([FromBody] ModelMinistracionGuardar model)
        {
            try
            {
                if (model == null) return BadRequest("Datos inválidos");

                if (string.IsNullOrEmpty(model.folio))
                    return Ok(new { success = false, message = "El folio es requerido" });

                if (model.mesAplicacion <= 0)
                    return Ok(new { success = false, message = "Seleccione un mes válido" });

                if (model.anio <= 2000)
                    return Ok(new { success = false, message = "Ingrese un año válido" });

                if (string.IsNullOrEmpty(model.clavePresupuestal))
                    return Ok(new { success = false, message = "Seleccione una clave presupuestal" });

                if (string.IsNullOrEmpty(model.importeTotal) || decimal.Parse(model.importeTotal.Replace(",", "").Replace("$", "").Trim()) <= 0)
                    return Ok(new { success = false, message = "Ingrese un importe total válido" });

                int idMinistracion = _repo.GuardarMinistracion(model);
                if (idMinistracion > 0)
                {
                    try
                    {
                        // Crear registro en la bitácora para ministraciones (cuando se agrega) y mostrar sus datos como un rastreador
                        if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["UsuarioId"] != null)
                        {
                            int uId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UsuarioId"]);
                            var logRepo = new SIAP.Repositories.UsuarioRepository();
                            logRepo.RegistrarLog(uId, "Ministraciones", "Creación", $"Registró nueva ministración con folio: {model.folio}");
                        }
                    }
                    catch { }

                    return Ok(new { success = true, message = "Ministración guardada correctamente", id = idMinistracion });
                }
                return Ok(new { success = false, message = "No se guardó la ministración" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en GuardarMinistracion: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/Ministraciones/CancelarMinistracion")]
        public IHttpActionResult CancelarMinistracion([FromBody] ModelCancelarMinistracion model)
        {
            try
            {
                if (model == null) return BadRequest("Datos inválidos");

                if (string.IsNullOrEmpty(model.Motivo))
                    return Ok(new { success = false, message = "El motivo de cancelación es requerido" });

                if (model.Monto <= 0)
                    return Ok(new { success = false, message = "El monto a cancelar debe ser mayor a 0" });

                bool success = _repo.CancelarMinistracion(model.Id, model.Motivo, model.Monto);
                if (success)
                {
                    try
                    {
                        // Crear registro en la bitácora para ministraciones (cuando se cancela) y mostrar sus datos como un rastreador
                        if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["UsuarioId"] != null)
                        {
                            int uId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UsuarioId"]);
                            var logRepo = new SIAP.Repositories.UsuarioRepository();
                            logRepo.RegistrarLog(uId, "Ministraciones", "Cancelación", $"Canceló la ministración ID: {model.Id}. Motivo: {model.Motivo}");
                        }
                    }
                    catch { }

                    return Ok(new { success = true, message = "Ministración cancelada correctamente" });
                }
                else
                    return Ok(new { success = false, message = "No se pudo cancelar la ministración. Verifique que esté en estado REGISTRADA o INICIADO" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en CancelarMinistracion: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/Ministraciones/AprobarMinistracion")]
        public IHttpActionResult AprobarMinistracion(int id)
        {
            try
            {
                bool success = _repo.AprobarMinistracion(id);
                if (success)
                {
                    try
                    {
                        // Crear registro en la bitácora para ministraciones (cuando se aprueba) y mostrar sus datos como un rastreador
                        if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["UsuarioId"] != null)
                        {
                            int uId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UsuarioId"]);
                            var logRepo = new SIAP.Repositories.UsuarioRepository();
                            logRepo.RegistrarLog(uId, "Ministraciones", "Aprobación", $"Inició/Aprobó la ministración ID: {id}");
                        }
                    }
                    catch { }

                    return Ok(new { success = true, message = "Ministración iniciada correctamente" });
                }
                else
                    return Ok(new { success = false, message = "No se pudo iniciar la ministración. Verifique que esté en estado REGISTRADA" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en AprobarMinistracion: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/exportarExcel")]
        public IHttpActionResult ExportarExcel()
        {
            try
            {
                return Ok(new { success = true, message = "Exportación iniciada" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Ministraciones/Test")]
        public IHttpActionResult Test()
        {
            try
            {
                return Ok(new { success = true, message = "API funcionando correctamente", timestamp = DateTime.Now });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}