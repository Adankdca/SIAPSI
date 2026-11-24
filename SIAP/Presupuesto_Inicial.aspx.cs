using AjaxControlToolkit;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

//using ClosedXML.Excel;
//using Microsoft.Office.Interop.Excel;
//using DataTable = System.Data.DataTable;

namespace SIAP
{
    public partial class Presupuesto_Inicial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Sin sesión: al Login con aviso de sesión expirada. Sin permiso: al Menú.
            Seguridad.Requerir(this, "VerPresupuesto");

            if(!IsPostBack)
            {
                Session["filename"] = "";
                // Deshabilitado temporalmente porque usaba SQL Server (legacy)
                // leerarchivo();
                // llenarcombo();

            }
        }

        //protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        //{
        //    // Función obsoleta
        //}
        private void msginformar(string msg)
        {
            string someScript = "";
            someScript = "<script type='text/javascript'>swal.fire({position: 'top-right', type: 'error',title: '" + msg.ToString() + "'})</script>";
            ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "Aceptar", someScript, false);
        }
        private void msgguardar(string msg)
        {
            string someScript1 = "";
            someScript1 = "<script type='text/javascript'>swal.fire({position: 'top-right', type:'success', title: '" + msg.ToString() + "'})</script>";
            ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "Aceptar", someScript1, false);
        }
    }
}
