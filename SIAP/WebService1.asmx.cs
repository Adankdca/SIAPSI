using System;
using System.Collections.Generic;
using System.Data;//
using System.Web;
using System.Web.Services;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Data.SqlClient;

namespace SIAP
{
    /// <summary>
    /// Descripción breve de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        Cnn cn;
        Controlador cnx = new Controlador();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
        [WebMethod]
        public List<Controlador.tipoadecuacion> llenartipoadecuacion()
        {
            //combos BDEtica = new combos();
            DataTable dt = new DataTable();
            dt = cnx.Cargartipoadecuacion();
            List<Controlador.tipoadecuacion> data = new List<Controlador.tipoadecuacion>();
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(new Controlador.tipoadecuacion
                {
                    idtipoadecuacion = dr["idtipoadecuacion"].ToString(),
                    nombre = dr["nombre"].ToString()
                });
            }
            return data;
        }
        [WebMethod]
        public List<Controlador.estatusadec> llenarestatus()
        {
            //combos BDEtica = new combos();
            DataTable dt = new DataTable();
            dt = cnx.Cargarestatus();
            List<Controlador.estatusadec> data = new List<Controlador.estatusadec>();
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(new Controlador.estatusadec
                {
                    idestatusadecuacion = dr["idestatusadecuacion"].ToString(),
                    estatusadecuacion = dr["estatusadecuacion"].ToString()
                });
            }
            return data;
        }
        [WebMethod]
        public string[] Busquedaclave(string prefix, string idtipo)
        {
            List<string> result = new List<string>();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString;
            
            using (var con = new MySql.Data.MySqlClient.MySqlConnection(connString))
            {
                string Ssql = "";
                if (prefix != "")
                {
                    if (idtipo.ToString().Trim() == "NU")
                    {
                        Ssql = " SELECT distinct ClavePresupuestal as cve FROM presupuestocalendarizado WHERE ClavePresupuestal LIKE CONCAT('%', @SearchText, '%') LIMIT 20";
                    }
                    if (idtipo.ToString().Trim() == "EX")
                    {
                        Ssql = " SELECT distinct ClavePresupuestal as cve FROM presupuestocalendarizado WHERE ClavePresupuestal LIKE CONCAT('%', @SearchText, '%') LIMIT 20";
                    }
                }

                if (!string.IsNullOrEmpty(Ssql))
                {
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(Ssql, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@SearchText", prefix);
                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                result.Add(string.Format("{0}", dr["cve"].ToString().Trim()));
                            }
                        }
                    }
                }
            }
            return result.ToArray();
        }
        [WebMethod]
        public List<Controlador.noliberadocve> Noliberado(string clavepresupuestal)
        {
            //combos BDEtica = new combos();
            DataTable dt = new DataTable();
            dt = cnx.datonoliberado(clavepresupuestal);
            List<Controlador.noliberadocve> data = new List<Controlador.noliberadocve>();
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(new Controlador.noliberadocve
                {
                    noliberado = dr["noliberado"].ToString(),
                    liberado = dr["liberado"].ToString()
                });
            }
            return data;
        }

        [WebMethod]
        public string GuardarAdecuacion(Models.Adecuacion adecuacion)
        {
            try
            {
                var repo = new Repositories.AdecuacionRepository();
                int idGuardado = repo.GuardarAdecuacionConTransaccion(adecuacion);
                return "OK|" + idGuardado.ToString();
            }
            catch (Exception ex)
            {
                // Devolver el mensaje exacto para mostrarlo en un SweetAlert en el Frontend
                return "ERROR|" + ex.Message;
            }
        }
    }
}
