using System;
using System.Collections.Generic;

namespace SIAP.Models
{
    public class Adecuacion
    {
        public int Id { get; set; }
        public string NumOficio { get; set; }
        public string NumSolicitud { get; set; }
        public string Asunto { get; set; }
        public string TipoAdecuacion { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Movimientos asociados a la adecuación
        public List<AdecuacionMovimiento> Movimientos { get; set; }
        
        public Adecuacion()
        {
            Movimientos = new List<AdecuacionMovimiento>();
        }
    }

    public class AdecuacionMovimiento
    {
        public int Id { get; set; }
        public int AdecuacionId { get; set; }
        public string TipoOperacion { get; set; } // 'AUMENTO' o 'DISMINUCION'
        public string ClavePresupuestal { get; set; }
        public int? PresupuestoCalendarizadoId { get; set; }
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
    }
}
