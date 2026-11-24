using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIAP
{
    public partial class Adecuaciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Sin sesión: al Login con aviso de sesión expirada. Sin permiso: al Menú.
            Seguridad.Requerir(this, "VerAdecuaciones");

            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //this.lbldondeestoy.Text = "ADECUACIONES " + "<i class='fa fa-angle-double-right'></i>" + "DETALLES";
                Session["filenameimportacionadecuacion"] = "";
            }
        }
    }
}