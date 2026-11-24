using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using System.Data.SqlClient;
using SIAP.Models;
using SIAP.Repositories;

namespace SIAP
{
    public partial class Usuarios : System.Web.UI.Page
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Sin sesión: al Login con aviso de sesión expirada. Sin permiso: al Menú.
            Seguridad.Requerir(this, soloAdministrador: true);

            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Usuario AS Username, NombreCompleto, Rol, Activo FROM usuarios ORDER BY Id";
                var lista = db.Query<Usuario>(query).ToList();
                gvUsuarios.DataSource = lista;
                gvUsuarios.DataBind();
            }
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                using (var db = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT 
                            u.Id, 
                            u.Usuario AS Username, 
                            u.NombreCompleto, 
                            u.Rol, 
                            u.Activo,
                            a.VerDashboard,
                            a.VerPresupuesto,
                            a.VerAdecuaciones,
                            a.VerMinistraciones,
                            a.VerCatalogos
                        FROM usuarios u
                        LEFT JOIN usuarios_accesos a ON u.Id = a.UsuarioId
                        WHERE u.Id = @Id;
                    ";
                    var user = db.QueryFirstOrDefault<Usuario>(query, new { Id = id });
                    
                    if (user != null)
                    {
                        hfUsuarioId.Value = user.Id.ToString();
                        txtUsername.Text = user.Username;
                        txtPassword.Text = ""; // No mostrar por seguridad
                        txtNombreCompleto.Text = user.NombreCompleto;
                        ddlRol.SelectedValue = user.Rol;
                        chkActivo.Checked = user.Activo;
                        
                        chkVerDashboard.Checked = user.VerDashboard;
                        chkVerPresupuesto.Checked = user.VerPresupuesto;
                        chkVerAdecuaciones.Checked = user.VerAdecuaciones;
                        chkVerMinistraciones.Checked = user.VerMinistraciones;
                        chkVerCatalogos.Checked = user.VerCatalogos;

                        ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "window.addEventListener('load', function() { mostrarModal(); });", true);
                    }
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                try
                {
                    using (var db = new SqlConnection(_connectionString))
                    {
                        // Obtener nombre de usuario para el log
                        string username = db.ExecuteScalar<string>("SELECT Usuario FROM usuarios WHERE Id = @Id", new { Id = id });
                        
                        // Eliminar primero dependencias en usuarios_accesos
                        db.Execute("DELETE FROM usuarios_accesos WHERE UsuarioId = @Id", new { Id = id });
                        
                        // Eliminar usuario
                        db.Execute("DELETE FROM usuarios WHERE Id = @Id", new { Id = id });
                        
                        new UsuarioRepository().RegistrarLog(Convert.ToInt32(Session["UsuarioId"]), "Usuarios", "Eliminar", string.Format("Usuario Eliminado: {0}", username));
                        MostrarMensaje("success", "Usuario eliminado con éxito.");
                        CargarUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensaje("error", "Error al eliminar: " + ex.Message.Replace("'", ""));
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(hfUsuarioId.Value);
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();
                string nombre = txtNombreCompleto.Text.Trim();
                string rol = ddlRol.SelectedValue;
                bool activo = chkActivo.Checked;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(nombre))
                {
                    MostrarMensaje("error", "Usuario y Nombre son obligatorios");
                    return;
                }

                using (var db = new SqlConnection(_connectionString))
                {
                    if (id == 0) // Nuevo usuario
                    {
                        if (string.IsNullOrEmpty(password))
                        {
                            MostrarMensaje("error", "La contraseña es obligatoria para usuarios nuevos");
                            return;
                        }

                        // Verificar si existe el usuario
                        int count = db.ExecuteScalar<int>("SELECT COUNT(*) FROM usuarios WHERE Usuario = @User", new { User = username });
                        if (count > 0)
                        {
                            MostrarMensaje("error", "El nombre de usuario ya existe.");
                            return;
                        }

                        string insertUser = "INSERT INTO usuarios (Usuario, PasswordHash, NombreCompleto, Rol, Activo) VALUES (@User, @Pass, @Nombre, @Rol, @Activo); SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        int newId = db.QuerySingle<int>(insertUser, new { User = username, Pass = password, Nombre = nombre, Rol = rol, Activo = activo });

                        string insertAcceso = "INSERT INTO usuarios_accesos (UsuarioId, VerDashboard, VerPresupuesto, VerAdecuaciones, VerMinistraciones, VerCatalogos) VALUES (@Id, @D, @P, @A, @M, @C)";
                        db.Execute(insertAcceso, new 
                        { 
                            Id = newId, 
                            D = rol == "Administrador" ? true : chkVerDashboard.Checked,
                            P = rol == "Administrador" ? true : chkVerPresupuesto.Checked,
                            A = rol == "Administrador" ? true : chkVerAdecuaciones.Checked,
                            M = rol == "Administrador" ? true : chkVerMinistraciones.Checked,
                            C = rol == "Administrador" ? true : chkVerCatalogos.Checked
                        });

                        new UsuarioRepository().RegistrarLog(Convert.ToInt32(Session["UsuarioId"]), "Usuarios", "Crear", string.Format("Usuario Creado: {0}", username));
                        MostrarMensaje("success", "Usuario creado con éxito.");
                    }
                    else // Actualizar
                    {
                        string updateQuery = "UPDATE usuarios SET Usuario = @User, NombreCompleto = @Nombre, Rol = @Rol, Activo = @Activo";
                        if (!string.IsNullOrEmpty(password))
                        {
                            updateQuery += ", PasswordHash = @Pass";
                        }
                        updateQuery += " WHERE Id = @Id";
                        
                        db.Execute(updateQuery, new { User = username, Nombre = nombre, Rol = rol, Activo = activo, Pass = password, Id = id });

                        // Borrar y reinsertar accesos o actualizar
                        db.Execute("DELETE FROM usuarios_accesos WHERE UsuarioId = @Id", new { Id = id });
                        string insertAcceso = "INSERT INTO usuarios_accesos (UsuarioId, VerDashboard, VerPresupuesto, VerAdecuaciones, VerMinistraciones, VerCatalogos) VALUES (@Id, @D, @P, @A, @M, @C)";
                        db.Execute(insertAcceso, new 
                        { 
                            Id = id, 
                            D = rol == "Administrador" ? true : chkVerDashboard.Checked,
                            P = rol == "Administrador" ? true : chkVerPresupuesto.Checked,
                            A = rol == "Administrador" ? true : chkVerAdecuaciones.Checked,
                            M = rol == "Administrador" ? true : chkVerMinistraciones.Checked,
                            C = rol == "Administrador" ? true : chkVerCatalogos.Checked
                        });

                        new UsuarioRepository().RegistrarLog(Convert.ToInt32(Session["UsuarioId"]), "Usuarios", "Actualizar", string.Format("Usuario Modificado: {0}", username));
                        MostrarMensaje("success", "Usuario actualizado con éxito.");
                    }

                    CargarUsuarios();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("error", "Error: " + ex.Message.Replace("'", ""));
            }
        }

        private void MostrarMensaje(string tipo, string msj)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", string.Format("mostrarMensaje('{0}', '{1}');", tipo, msj), true);
        }
    }
}
