using System.Web.UI;

namespace SIAP
{
    /// <summary>
    /// Validación de sesión y permisos para las páginas.
    ///  - Sin sesión (expiró o nunca entró): al Login, con aviso de sesión expirada.
    ///  - Con sesión pero sin permiso: de regreso al Menú (no se cierra la sesión).
    /// </summary>
    public static class Seguridad
    {
        public const string UrlSesionExpirada = "Login.aspx?expirada=1";

        public static bool EsAdministrador(Page page)
        {
            var s = page.Session;
            return s != null && s["UsuarioRol"] != null && s["UsuarioRol"].ToString() == "Administrador";
        }

        /// <param name="permiso">Nombre de la variable de sesión del permiso (VerPresupuesto, VerAdecuaciones...). Null = solo sesión.</param>
        /// <param name="soloAdministrador">True si la página es solo para el rol Administrador.</param>
        public static void Requerir(Page page, string permiso = null, bool soloAdministrador = false)
        {
            var s = page.Session;
            if (s == null || s["UsuarioId"] == null)
            {
                page.Response.Redirect(UrlSesionExpirada, true);
                return;
            }
            bool admin = EsAdministrador(page);
            if (soloAdministrador && !admin)
            {
                page.Response.Redirect("Menu.aspx", true);
                return;
            }
            if (permiso != null && !admin && !(s[permiso] is bool && (bool)s[permiso]))
            {
                page.Response.Redirect("Menu.aspx", true);
            }
        }
    }
}
