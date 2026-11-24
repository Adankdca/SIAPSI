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

    /// <summary>Un mes del calendario de la clave.</summary>
    public class MesClaveDto
    {
        public int Mes { get; set; }
        public string Nombre { get; set; }
        public decimal Calendarizado { get; set; }
        public decimal Ministrado { get; set; }
        /// <summary>Calendarizado − ministrado del mes (los apartados no tienen mes todavía).</summary>
        public decimal Disponible { get; set; }
        public bool EsMesActual { get; set; }
    }

    public class MinistracionClaveDto
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public int Mes { get; set; }
        public decimal Importe { get; set; }
        public string Estatus { get; set; }
        public string Concepto { get; set; }
        public System.DateTime? Fecha { get; set; }
    }

    public class ApartadoClaveDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public string Oficio { get; set; }
        public string Motivo { get; set; }
        public System.DateTime? FechaApartado { get; set; }
    }

    /// <summary>Obra o acción de BDSICOFIN ligada a la clave.</summary>
    public class ObraSicofinDto
    {
        public int IdGeneral { get; set; }
        public string IdAccion { get; set; }
        public string IdProyecto { get; set; }
        public string Proyecto { get; set; }
        public string Accion { get; set; }
        public string TipoAccion { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string Estatus { get; set; }
        public string TipoObra { get; set; }
        public string Dependencia { get; set; }
        public string Fuente { get; set; }
        /// <summary>Nombre del proyecto y la acción, como se muestra.</summary>
        public string Nombre
        {
            get
            {
                string p = (Proyecto ?? "").Trim(), a = (Accion ?? "").Trim();
                return a.Length == 0 ? p : p.Length == 0 ? a : p + " — " + a;
            }
        }
    }

    /// <summary>Otra clave del mismo proyecto (mismos segmentos de UR a PT).</summary>
    public class ClaveProyectoDto
    {
        public int Id { get; set; }
        public string ClavePresupuestal { get; set; }
        public decimal Modificado { get; set; }
        public decimal Liberado { get; set; }
        public decimal PorLiberar { get; set; }
        public Dictionary<string, SegmentoCelda> Segmentos { get; set; } = new Dictionary<string, SegmentoCelda>();
    }

    /// <summary>
    /// Detalle de una clave. Los importes cuadran así:
    ///   Modificado = Aprobado + Aumentos − Disminuciones
    ///   Modificado = Ministrado + Apartado + Disponible + PorLiberar
    ///   (Liberado = suma de los meses = Ministrado + Apartado + Disponible)
    /// </summary>
    public class DetalleClaveDto
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
        public decimal Ministrado { get; set; }
        public decimal Apartado { get; set; }
        public decimal Disponible { get; set; }
        public decimal Devengado { get; set; }
        public decimal Pagado { get; set; }
        public int MesActual { get; set; }
        /// <summary>Nombre de la obra (BDSICOFIN): proyecto de la primera acción encontrada.</summary>
        public string NombreObra { get; set; }
        /// <summary>Acciones de BDSICOFIN ligadas a la clave (puede haber varias: obra, supervisión...).</summary>
        public List<ObraSicofinDto> Obras { get; set; } = new List<ObraSicofinDto>();
        /// <summary>Clave corta con la que se buscó en BDSICOFIN.</summary>
        public string ClaveCortaSicofin { get; set; }
        /// <summary>Aviso si no se pudo consultar BDSICOFIN (no configurado o error de conexión).</summary>
        public string AvisoSicofin { get; set; }
        public Dictionary<string, SegmentoCelda> Segmentos { get; set; } = new Dictionary<string, SegmentoCelda>();
        public List<MesClaveDto> Meses { get; set; } = new List<MesClaveDto>();
        public List<MinistracionClaveDto> Ministraciones { get; set; } = new List<MinistracionClaveDto>();
        public List<ApartadoClaveDto> Apartados { get; set; } = new List<ApartadoClaveDto>();
        /// <summary>Segmentos que definen el proyecto (de UR a PT) y las otras claves que los comparten.</summary>
        public string PrefijoProyecto { get; set; }
        public List<ClaveProyectoDto> ClavesDelProyecto { get; set; } = new List<ClaveProyectoDto>();
    }
}
