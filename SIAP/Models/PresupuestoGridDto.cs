using System;
using System.Collections.Generic;

namespace SIAP.Models
{
    // DTO (Data Transfer Object) para mostrar solo lo esencial en el Grid
    public class PresupuestoGridDto
    {
        public int Id { get; set; }
        public string ClavePresupuestal { get; set; }
        public string UrDescripcion { get; set; }
        public string ProgramaDescripcion { get; set; }
        public string ObjetoGastoDescripcion { get; set; }
        public decimal Aprobado { get; set; }
        public decimal Modificado { get; set; }
        public decimal Pagado { get; set; }
        public decimal Ministerio { get; set; }
        public decimal SaldoApartado { get; set; }
        public bool EsCalendarizada { get; set; }
        public string ClaveMunicipio { get; set; }
        public string NombreMunicipio { get; set; }
        public string NombreRegion { get; set; }
    }

    public class ApartadoDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public string Motivo { get; set; }
        public string Oficio { get; set; }
        public DateTime FechaApartado { get; set; }
    }

    public class PresupuestoDetalleDto
    {
        public string ClavePresupuestal { get; set; }
        public decimal Aprobado { get; set; }
        public decimal Modificado { get; set; }
        public decimal Devengado { get; set; }
        public decimal Pagado { get; set; }
        public decimal SaldoApartado { get; set; }
        public List<ApartadoDto> Apartados { get; set; } = new System.Collections.Generic.List<ApartadoDto>();
        

        public decimal GastadoPorAdecuacion { get; set; }
        
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

    public class SegmentoClaveDto
    {
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
    }

    public class ClaveDecodificadaDto
    {
        public System.Collections.Generic.List<SegmentoClaveDto> Segmentos { get; set; } = new System.Collections.Generic.List<SegmentoClaveDto>();
    }

    public class ApartarRequest
    {
        public string Clave { get; set; }
        public decimal Monto { get; set; }
        public string Motivo { get; set; }
        public string Oficio { get; set; }
    }

    public class CargaMasivaDto
    {
        public string Anio { get; set; }
        public string Oficio { get; set; }
        public System.Collections.Generic.List<PresupuestoDetalleDto> Claves { get; set; } = new System.Collections.Generic.List<PresupuestoDetalleDto>();
    }
}
