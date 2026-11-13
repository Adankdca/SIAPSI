using System;
using System.Data;
using System.Linq;
using Dapper;
using System.Data.SqlClient;
using SIAP.Models;

namespace SIAP.Repositories
{
    /// <summary>
    /// Repositorio encargado de la gestión de sesión de usuarios y la bitácora de auditoría.
    /// Valida credenciales contra la base de datos y recupera los permisos de acceso (Roles/Accesos).
    /// </summary>
    public class UsuarioRepository
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString;

        private IDbConnection DbConnection
        {
            get { return new SqlConnection(_connectionString); }
        }

        /// <summary>
        /// Valida si el usuario y la contraseña existen en la base de datos y si el usuario está Activo.
        /// Realiza un LEFT JOIN con la tabla de `usuarios_accesos` para traer todos los permisos 
        /// (VerDashboard, VerPresupuesto, etc.) que tiene habilitados.
        /// IMPORTANTE: La validación de la contraseña está implementada en texto plano para esta versión, 
        /// pero debe cambiarse a un sistema de HASH (ej. BCrypt) antes de salir a producción pública.
        /// </summary>
        /// <returns>Modelo de usuario con sus permisos, o NULL si las credenciales son inválidas.</returns>
        public Usuario ValidarCredenciales(string usuario, string password)
        {
            using (IDbConnection db = DbConnection)
            {
                // NOTA: Para producción real, se debe usar bcrypt u otro hash fuerte para comparar.
                // Como es una versión inicial, se asume que passwordHash = password.
                string query = @"
                    SELECT TOP 1
                        u.Id, 
                        u.Usuario AS Username, 
                        u.PasswordHash, 
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
                    WHERE u.Usuario = @User AND u.PasswordHash = @Pass AND u.Activo = 1;
                ";

                var user = db.QueryFirstOrDefault<Usuario>(query, new { User = usuario, Pass = password });
                return user;
            }
        }
        
        /// <summary>
        /// Inserta un registro en la bitácora de auditoría (logs) de la base de datos.
        /// Muy útil para rastrear quién eliminó una ministración o quién aprobó una adecuación.
        /// </summary>
        /// <param name="usuarioId">ID del usuario que ejecutó la acción.</param>
        /// <param name="modulo">Nombre del módulo (ej. 'Ministraciones').</param>
        /// <param name="accion">Acción realizada (ej. 'DELETE', 'INSERT').</param>
        /// <param name="detalle">Texto descriptivo del movimiento (ej. 'Eliminó ministración ID 45').</param>
        public void RegistrarLog(int usuarioId, string modulo, string accion, string detalle)
        {
            using (IDbConnection db = DbConnection)
            {
                string query = @"
                    INSERT INTO bitacora_logs (UsuarioId, Modulo, Accion, Detalle, Fecha)
                    VALUES (@User, @Mod, @Accion, @Detalle, GETDATE());
                ";
                db.Execute(query, new { User = usuarioId, Mod = modulo, Accion = accion, Detalle = detalle });
            }
        }

        public System.Collections.Generic.List<LogEntry> ObtenerLogs()
        {
            using (IDbConnection db = DbConnection)
            {
                string query = @"
                    SELECT 
                        b.Id,
                        b.UsuarioId,
                        u.NombreCompleto AS NombreUsuario,
                        b.Modulo,
                        b.Accion,
                        b.Detalle,
                        b.Fecha
                    FROM bitacora_logs b
                    LEFT JOIN usuarios u ON b.UsuarioId = u.Id
                    ORDER BY b.Fecha DESC;
                ";
                return db.Query<LogEntry>(query).ToList();
            }
        }
    }
}
