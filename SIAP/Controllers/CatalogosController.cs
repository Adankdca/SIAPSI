using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIAP.Models;
using SIAP.Repositories;
using Newtonsoft.Json;

namespace SIAP.Controllers
{
    [RoutePrefix("api/catalogos")]
    /// <summary>
    /// Controlador dinámico para la gestión de TODOS los catálogos del sistema (cat_*).
    /// En lugar de tener un endpoint por cada tabla, utiliza un enfoque genérico donde 
    /// el nombre de la tabla se pasa como parámetro en la URL ({tableName}).
    /// Esto permite administrar decenas de catálogos sin inflar el código.
    /// </summary>
    public class CatalogosController : ApiController
    {
        private CatalogosRepository _repository;

        public CatalogosController()
        {
            _repository = new CatalogosRepository();
        }

        [HttpGet]
        [Route("list")]
        /// <summary>
        /// Obtiene una lista predefinida de todos los catálogos administrables desde la Interfaz de Usuario.
        /// </summary>
        public IHttpActionResult GetCatalogsList()
        {
            try
            {
                var list = _repository.GetCatalogs();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{tableName}")]
        /// <summary>
        /// Obtiene todos los registros de la tabla especificada.
        /// Este endpoint lee directamente de MySQL las tablas maestras de catálogos (cat_*).
        /// </summary>
        /// <param name="tableName">Nombre de la tabla en MySQL (ej. cat_region).</param>
        public IHttpActionResult GetTableRecords(string tableName)
        {
            try
            {
                var records = _repository.GetRecords(tableName);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{tableName}/schema")]
        public IHttpActionResult GetTableSchema(string tableName)
        {
            try
            {
                var schema = _repository.GetSchema(tableName);
                return Ok(schema);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{tableName}")]
        public IHttpActionResult SaveTableRecord(string tableName, [FromBody] Dictionary<string, object> data)
        {
            try
            {
                _repository.SaveRecord(tableName, data);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Backward compatibility for existing UI
        [HttpGet]
        [Route("regiones")]
        public IHttpActionResult GetRegiones()
        {
            try
            {
                // We use GetRecords dynamically since we removed ObtenerRegiones
                var regiones = _repository.GetRecords("cat_region");
                return Ok(regiones);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
