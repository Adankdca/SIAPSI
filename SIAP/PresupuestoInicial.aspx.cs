using System;

namespace SIAP
{
    public partial class PresupuestoInicial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Sin sesión: al Login con aviso de sesión expirada. Sin permiso: al Menú.
            Seguridad.Requerir(this, "VerPresupuesto");
        }
    }
}
