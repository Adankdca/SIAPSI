using System.Collections.Generic;

namespace SIAP.Models
{
    /// <summary>Valor de un segmento de la clave con su descripción de catálogo.</summary>
    public class SegmentoCelda
    {
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        /// <summary>True si el segmento tiene catálogo pero el valor no está en él.</summary>
        public bool SinCatalogo { get; set; }
    }

    /// <summary>Renglón de la Consulta General (matriz) del presupuesto.</summary>
    public class MatrizFilaDto
    {
        public int Id { get; set; }
        public string ClavePresupuestal { get; set; }
        public int? Ejercicio { get; set; }
        public decimal Aprobado { get; set; }
        public decimal Aumentos { get; set; }
        public decimal Disminuciones { get; set; }
        public decimal Modificado { get; set; }
        public decimal Liberado { get; set; }
        public decimal PorLiberar { get; set; }
        public decimal SaldoApartado { get; set; }
        public bool EsCalendarizada { get; set; }
        /// <summary>Segmentos de la clave por abreviatura (UR, PP, OG, MPIO...).</summary>
        public Dictionary<string, SegmentoCelda> Segmentos { get; set; } = new Dictionary<string, SegmentoCelda>();
    }

    public class TotalesMatrizDto
    {
        public int Claves { get; set; }
        public decimal Aprobado { get; set; }
        public decimal Modificado { get; set; }
        public decimal Liberado { get; set; }
        public decimal PorLiberar { get; set; }
    }

    /// <summary>Parámetros de la matriz (vienen de DataTables más los filtros de la pantalla).</summary>
    public class FiltroMatriz
    {
        public int Anio { get; set; }
        public int Start { get; set; }
        public int Length { get; set; } = 10;
        public string Search { get; set; }
        public string Estado { get; set; }
        public string Region { get; set; }
        /// <summary>Filtros por segmento: abreviatura → valor (p. ej. UR → 08).</summary>
        public Dictionary<string, string> Segmentos { get; set; } = new Dictionary<string, string>();
        public string OrdenColumna { get; set; }
        public bool OrdenDescendente { get; set; }
    }

    public class ResultadoMatriz
    {
        public int Total { get; set; }
        public int Filtradas { get; set; }
        public TotalesMatrizDto Totales { get; set; }
        public List<MatrizFilaDto> Filas { get; set; } = new List<MatrizFilaDto>();
    }

    /// <summary>Opción de un filtro por segmento: valor, descripción y cuántas claves lo usan.</summary>
    public class OpcionSegmentoDto
    {
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public int Claves { get; set; }
    }

    public class EjercicioDto
    {
        public int Anio { get; set; }
        public int Claves { get; set; }
        public bool TieneEstructura { get; set; }
    }

    /// <summary>Valor de segmento que no está en su catálogo dentro de un ejercicio.</summary>
    public class ValorSinCatalogoDto
    {
        public int Orden { get; set; }
        public string Abreviatura { get; set; }
        public string Nombre { get; set; }
        public string TablaCatalogo { get; set; }
        public string Valor { get; set; }
        /// <summary>Para catálogos dependientes: valor del segmento padre (p. ej. la OGC de una OG).</summary>
        public string SegmentoPadre { get; set; }
        public string ValorPadre { get; set; }
        public int Claves { get; set; }
        public string EjemploClave { get; set; }
    }

    public class ResultadoReproceso
    {
        public int Claves { get; set; }
        public int SegmentosActualizados { get; set; }
        public int SinCatalogoAntes { get; set; }
        public int SinCatalogoDespues { get; set; }
    }

    /// <summary>Alta manual de una clave (captura individual).</summary>
    public class NuevaClaveRequest
    {
        public int Anio { get; set; }
        public string ClavePresupuestal { get; set; }
        public decimal Aprobado { get; set; }
        public string Oficio { get; set; }
    }
}
