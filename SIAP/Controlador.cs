using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace SIAP
{
    public class Controlador
    {
        string sql, html;
        string urlnew;
        DataTable bloque1;
        public String CadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString;
        public SqlConnection conexion;
        //==========================================conexion==========================================================================
        public Boolean AbrirConexion()
        {
            Boolean exito;
            try
            {
                conexion = new SqlConnection(CadenaConexion);
                conexion.Open();
                exito = true;
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }
        public void CerrarConexion()
        {
            if (conexion == null) { return; }
            conexion.Close();
            conexion = null;
        }

        //==========================================conexion==========================================================================



        //==========================================FUNCIÓN CATÁLOGOS==================================================================
        public DataTable Cargartipoadecuacion()
        {

            DataTable dt = new DataTable(); // vacía si falla, para no romper a quien recorre dt.Rows
            try
            {
                AbrirConexion();
                // El procedimiento Get_All_SIAP_tipoadeacuacion no existe en SIAP_DB; se consulta el catálogo directamente
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT CAST(Id AS VARCHAR(10)) AS idtipoadecuacion, Descripcion AS nombre FROM cat_tipoadecuacion ORDER BY Descripcion", conexion);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception) { }
            finally { CerrarConexion(); }

            return dt;
        }
        public DataTable Cargarestatus()
        {

            DataTable dt = new DataTable(); // vacía si falla, para no romper a quien recorre dt.Rows
            try
            {
                AbrirConexion();
                // El procedimiento Get_All_SIAP_estatusadeacuacion no existe; no hay catálogo de estatus,
                // así que se devuelven los estatus que ya se usan en adecuaciones
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT DISTINCT Estatus AS idestatusadecuacion, Estatus AS estatusadecuacion FROM adecuaciones WHERE Estatus IS NOT NULL AND Estatus <> '' ORDER BY Estatus", conexion);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception) { }
            finally { CerrarConexion(); }

            return dt;
        }
        //==========================================FUNCIÓN CATÁLOGOS==================================================================

        //==========================================FUNCIÓN CONSULTAS==================================================================
        public DataTable datonoliberado(string clavepresupuestal)
        {
            DataTable dt = new DataTable(); // vacía si falla, para no romper a quien recorre dt.Rows
            try
            {
                AbrirConexion();
                // El procedimiento Get_All_SIAP_noliberado no existe; se calcula desde presupuestocalendarizado.
                // No liberado = PorLiberar (mes cero); Liberado = suma de enero a diciembre.
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT TOP 1
                        ISNULL(PorLiberar, 0) AS noliberado,
                        ISNULL(Enero,0) + ISNULL(Febrero,0) + ISNULL(Marzo,0) + ISNULL(Abril,0) + ISNULL(Mayo,0) + ISNULL(Junio,0)
                      + ISNULL(Julio,0) + ISNULL(Agosto,0) + ISNULL(Septiembre,0) + ISNULL(Octubre,0) + ISNULL(Noviembre,0) + ISNULL(Diciembre,0) AS liberado
                    FROM presupuestocalendarizado
                    WHERE ClavePresupuestal = @clavepresupuestal", conexion);
                da.SelectCommand.Parameters.Add("@clavepresupuestal", SqlDbType.NVarChar, 150).Value = Convert.ToString(clavepresupuestal);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception) { }
            finally { CerrarConexion(); }
            return dt;
        }

        
        //==========================================FUNCIÓN CONSULTAS==================================================================

        //==========================================CLASES============================================================================
        public class tipoadecuacion
        {
            public String idtipoadecuacion { get; set; }
            public String nombre { get; set; }
        }
        public class estatusadec
        {
            public String idestatusadecuacion { get; set; }
            public String estatusadecuacion { get; set; }
        }
        public class noliberadocve
        {
            public String noliberado { get; set; }
            public String liberado { get; set; }
        }
        //==========================================CLASES============================================================================
    }
}