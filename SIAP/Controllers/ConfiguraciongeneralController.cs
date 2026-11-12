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
                var sql = "select * from tblc_SIAP_Configuracion where Anio_ejercicio='2022';";
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
                var sqlquery = "Update tblc_SIAP_Configuracion set Titular=@Titular where Idconfiguracion=" + id;
                result = con.Execute(sqlquery, configuracion);
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
                var sql = "select * from tblc_SIAP_Firmantes;";
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
                var sqlquery = "Update tblc_SIAP_Firmantes set Nombre=@Nombre, Cargo=@Cargo where Idfirmante=" + id;
                result = con.Execute(sqlquery, firmantes);
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
                var sqlquery = "Delete from tblc_SIAP_Firmantes where Idfirmante=" + id;
                result = con.Execute(sqlquery);
            }
            return Ok(result);
        }
    }
}
