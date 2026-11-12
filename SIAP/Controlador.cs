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
        Cnn cn;
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

            DataTable dt = null;
            try
            {
                AbrirConexion();
                SqlDataAdapter da = new SqlDataAdapter("Get_All_SIAP_tipoadeacuacion", conexion);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Clear();
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception) { }
            finally { CerrarConexion(); }

            return dt;
        }
        public DataTable Cargarestatus()
        {

            DataTable dt = null;
            try
            {
                AbrirConexion();
                SqlDataAdapter da = new SqlDataAdapter("Get_All_SIAP_estatusadeacuacion", conexion);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Clear();
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
            DataTable dt = null;
            try
            {
                AbrirConexion();
                SqlDataAdapter da = new SqlDataAdapter("Get_All_SIAP_noliberado", conexion);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Clear();
                da.SelectCommand.Parameters.AddWithValue("@clavepresupuestal", Convert.ToString(clavepresupuestal));
                da.SelectCommand.ExecuteNonQuery();
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