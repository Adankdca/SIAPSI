using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIAP
{
    public partial class Catalogos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null || Session["VerCatalogos"] == null || !(bool)Session["VerCatalogos"])
            {
                Response.Redirect("Login.aspx");
                return;
            }
        }
    }
}