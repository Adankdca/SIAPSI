using System;

namespace SIAP.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public int PresupuestoCalendarizadoId { get; set; }
        public string Folio { get; set; }
        public string UnificaContrato { get; set; }
        public string Beneficiario { get; set; }
        public string Factura { get; set; }
        public string ConceptoGasto { get; set; }
        public string Ministracion { get; set; }
        public decimal Importe { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
