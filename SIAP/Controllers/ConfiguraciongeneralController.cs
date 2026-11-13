using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SIAP.Controllers
{
    public class ConfiguraciongeneralController : ApiController
    {
        //----------- Configuración General ---------------------
        public IEnumerable<Data.Configuracion> GetNumOfi()
        {
            IEnumerable<Data.Configuracion> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
            {
                // Antes estaba fijo el año '2022'; ahora toma el ejercicio más reciente capturado
                var sql = "SELECT * FROM tblc_SIAP_Configuracion WHERE Anio_ejercicio = (SELECT MAX(Anio_ejercicio) FROM tblc_SIAP_Configuracion);";
                list = con.Query<Data.Configuracion>(sql);
            }
            return list.ToArray().ToList();
        }
        // Actualizar la configuración
        [HttpPut]
        [Route("api/UpConfig/{id}")]
        public IHttpActionResult updateConfig(int id, Data.Configuracion configuracion)
        {
            int result = 0;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
            {
                var sqlquery = "UPDATE tblc_SIAP_Configuracion SET Titular = @Titular WHERE Idconfiguracion = @Id";
                result = con.Execute(sqlquery, new { configuracion.Titular, Id = id });
            }
            return Ok(result);
        }

    }
    public class FirmantesController : ApiController
    {
        //----------- Ver Firmantes ---------------------
        public IEnumerable<Data.Firmantes> GetNumOfi()
        {
            IEnumerable<Data.Firmantes> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
            {
                var sql = "SELECT * FROM tblc_SIAP_Firmantes ORDER BY Idfirmante;";
                list = con.Query<Data.Firmantes>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class UpFirmantesController : ApiController
    {
        //Actualizar datos de los firmantes
        [HttpPut]
        [Route("api/UpFirmantes/{id}")]
        public IHttpActionResult updateFirm(int id,Data.Firmantes firmantes)
        {
            int result = 0;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
            {
                var sqlquery = "UPDATE tblc_SIAP_Firmantes SET Nombre = @Nombre, Cargo = @Cargo WHERE Idfirmante = @Id";
                result = con.Execute(sqlquery, new { firmantes.Nombre, firmantes.Cargo, Id = id });
            }
            return Ok(result);
        }
        [HttpDelete]
        [Route("api/DelFirmantes/{id}")]
        public IHttpActionResult DeleteFIrm(int id)
        {
            //Eliminar un firmante
            int result = 0;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString()))
            {
                var sqlquery = "DELETE FROM tblc_SIAP_Firmantes WHERE Idfirmante = @Id";
                result = con.Execute(sqlquery, new { Id = id });
            }
            return Ok(result);
        }
    }
}
