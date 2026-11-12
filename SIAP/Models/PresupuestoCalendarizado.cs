using System;

namespace SIAP.Models
{
    public class PresupuestoCalendarizado
    {
        public int Id { get; set; }
        public string Secuencia { get; set; }
        public string Marca { get; set; }
        public string ClavePresupuestal { get; set; }
        
        // Llaves Foráneas (Se pueden agregar las propiedades de navegación de los catálogos después)
        public int? UrId { get; set; }
        public int? UroId { get; set; }
        public int? FinalidadId { get; set; }
        public int? FuncionId { get; set; }
        // ... (las demás llaves foráneas)
        
        // Montos Globales
        public decimal Aprobado { get; set; }
        public decimal PorLiberar { get; set; }
        public decimal Liberado { get; set; }
        
        // Calendarización
        public decimal Enero { get; set; }
        public decimal Febrero { get; set; }
        public decimal Marzo { get; set; }
        public decimal Abril { get; set; }
        public decimal Mayo { get; set; }
        public decimal Junio { get; set; }
        public decimal Julio { get; set; }
        public decimal Agosto { get; set; }
        public decimal Septiembre { get; set; }
        public decimal Octubre { get; set; }
        public decimal Noviembre { get; set; }
        public decimal Diciembre { get; set; }
        
        // Ajustes y Avance
        public decimal Aumentos { get; set; }
        public decimal Disminuciones { get; set; }
        public decimal Modificado { get; set; } // Ojo: en DB es una columna autocalculada (STORED)
        public decimal Ministrado { get; set; }
        public decimal Devengado { get; set; }
        public decimal Pagado { get; set; }
        public decimal Disponibilidad { get; set; } // Ojo: en DB es autocalculada
        public decimal Economias { get; set; }
    }
}
