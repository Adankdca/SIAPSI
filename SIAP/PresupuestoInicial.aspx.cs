using System;

namespace SIAP
{
    public partial class PresupuestoInicial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null || Session["VerPresupuesto"] == null || !(bool)Session["VerPresupuesto"])
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}
