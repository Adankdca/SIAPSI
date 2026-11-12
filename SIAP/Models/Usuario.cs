using System;

namespace SIAP.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; }
        
        // Permisos Modulares
        public bool VerDashboard { get; set; }
        public bool VerPresupuesto { get; set; }
        public bool VerAdecuaciones { get; set; }
        public bool VerMinistraciones { get; set; }
        public bool VerCatalogos { get; set; }
    }

    public class LogEntry
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string Detalle { get; set; }
        public DateTime Fecha { get; set; }
    }
}
