using System;
using System.Web.UI;

namespace SIAP
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Entrar al Login (cerrar sesión, sesión expirada o primera vez) termina la sesión del servidor.
                // Antes "Cerrar sesión" solo limpiaba el navegador y la sesión seguía viva en el servidor.
                Session.Clear();
                Session.Abandon();
            }
            TxtUsuario.Focus();
        }

        protected void BtnLoguear_Click(object sender, EventArgs e)
        {
            try
            {
                string uid = this.TxtUsuario.Text;
                string pass = this.TextContrasenia.Text;
             
                string script = "";

                var repo = new SIAP.Repositories.UsuarioRepository();
                var user = repo.ValidarCredenciales(uid, pass);

                if (user != null)
                {
                    // Guardar en sesión de ASP.NET
                    Session["UsuarioId"] = user.Id;
                    Session["UsuarioNombre"] = user.NombreCompleto;
                    Session["UsuarioRol"] = user.Rol;
                    
                    // Guardar los accesos en sesión
                    Session["VerDashboard"] = user.VerDashboard || user.Rol == "Administrador";
                    Session["VerPresupuesto"] = user.VerPresupuesto || user.Rol == "Administrador";
                    Session["VerAdecuaciones"] = user.VerAdecuaciones || user.Rol == "Administrador";
                    Session["VerMinistraciones"] = user.VerMinistraciones || user.Rol == "Administrador";
                    Session["VerCatalogos"] = user.VerCatalogos || user.Rol == "Administrador";

                    // Registrar log de inicio de sesión
                    repo.RegistrarLog(user.Id, "Sistema", "Login", $"Inicio de sesión exitoso desde IP: {Request.UserHostAddress}");

                    // Guardar también en sessionStorage para mantener compatibilidad con JS que lo use
                    script += @" sessionStorage.setItem('name', '"+ user.NombreCompleto + "');";
                    script += @" sessionStorage.setItem('rol', '"+ user.Rol + "');";
                    script += @"window.location.href = 'Menu.aspx';";
                }
                else
                {
                    script += @"   
                                Swal.fire(
                                'Error',
                                'Usuario y/o Contraseña Incorrecta...',
                                'error'
                                    );
                                     ";
                }
                ScriptManager.RegisterStartupScript(this, typeof(Page), "login", script, true);               
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}