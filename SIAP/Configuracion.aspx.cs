using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIAP
{
    public partial class Configuracion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Sin sesión: al Login con aviso de sesión expirada. Sin permiso: al Menú.
            Seguridad.Requerir(this, soloAdministrador: true);
        }
    }
}