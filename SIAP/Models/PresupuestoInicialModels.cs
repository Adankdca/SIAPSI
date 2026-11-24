using System;
using System.Collections.Generic;

namespace SIAP.Models
{
    /// <summary>
    /// Un segmento de la clave presupuestal para un ejercicio (tabla cat_estructuraclave / vista vw_estructuraclave).
    /// </summary>
    public class SegmentoEstructura
    {
        public int Anio { get; set; }
        public int Orden { get; set; }
        public string Abreviatura { get; set; }
        public string Nombre { get; set; }
        public int Longitud { get; set; }
        /// <summary>Posición inicial dentro de la clave, empezando en 1.</summary>
        public int Posicion { get; set; }
        public int LongitudClave { get; set; }
        public string TablaCatalogo { get; set; }
        public string ColumnaClave { get; set; }
        public string ColumnaPadre { get; set; }
        public string SegmentoPadre { get; set; }
        public string ColumnaFK { get; set; }
        /// <summary>
        /// Segmentos que se anteponen al valor para buscarlo (p. ej. "TR,FF,RA" para el programa fondo:
        /// se busca 1528C001-00 en la columna ColumnaClave). Null = se busca el valor solo.
        /// </summary>
        public string SegmentosLlave { get; set; }
        /// <summary>Columna del catálogo con el año: si se indica, solo se leen los registros del ejercicio.</summary>
        public string ColumnaAnio { get; set; }
    }

    /// <summary>Valor de un segmento ya separado de la clave y buscado en su catálogo.</summary>
    public class SegmentoValor
    {
        public int Orden { get; set; }
        public string Abreviatura { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string TablaCatalogo { get; set; }
        public int? CatalogoId { get; set; }
        public string Descripcion { get; set; }
        public string ColumnaFK { get; set; }
    }

    /// <summary>Una fila del Excel del presupuesto inicial, ya leída.</summary>
    public class FilaPresupuestoInicial
    {
        public int Fila { get; set; }
        public string Clave { get; set; }
        public decimal Aprobado { get; set; }
        public decimal NoLiberado { get; set; }

        /// <summary>Errores de lectura de la celda (texto donde va un importe). Uso interno.</summary>
        public List<string> ErroresLectura { get; set; } = new List<string>();
        public decimal[] Meses { get; set; } = new decimal[12];
        public decimal Liberado
        {
            get { decimal s = 0; foreach (var m in Meses) s += m; return s; }
        }
        /// <summary>Valores de las columnas de segmento del Excel (UR, URO...), si vienen.</summary>
        public Dictionary<string, string> SegmentosExcel { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public List<SegmentoValor> Segmentos { get; set; } = new List<SegmentoValor>();
    }

    public class ErrorFila
    {
        public int Fila { get; set; }
        public string Clave { get; set; }
        public string Motivo { get; set; }
    }

    public class AdvertenciaCatalogo
    {
        public string Abreviatura { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public int Claves { get; set; }
    }

    /// <summary>Resultado de validar un archivo de presupuesto inicial.</summary>
    public class ResultadoValidacionPresupuesto
    {
        public bool Valido { get; set; }
        /// <summary>Identificador del archivo validado; se usa para confirmar la importación.</summary>
        public string Token { get; set; }
        public int Anio { get; set; }
        public string Oficio { get; set; }
        public string Archivo { get; set; }
        public int LongitudClave { get; set; }
        public int TotalFilas { get; set; }
        public decimal TotalAprobado { get; set; }
        public decimal TotalLiberado { get; set; }
        public decimal TotalNoLiberado { get; set; }
        public int SegmentosSinCatalogo { get; set; }
        public List<string> ErroresGenerales { get; set; } = new List<string>();
        public List<ErrorFila> Errores { get; set; } = new List<ErrorFila>();
        public int TotalErrores { get; set; }
        public List<AdvertenciaCatalogo> Advertencias { get; set; } = new List<AdvertenciaCatalogo>();
        public List<string> AdvertenciasGenerales { get; set; } = new List<string>();
    }

    public class ImportarPresupuestoRequest
    {
        public string Token { get; set; }
        public int Anio { get; set; }
        public string Oficio { get; set; }
        public string Archivo { get; set; }
    }

    public class HistorialCargaDto
    {
        public int Id { get; set; }
        public string Anio { get; set; }
        public string Oficio { get; set; }
        public string Archivo { get; set; }
        public bool TieneArchivo { get; set; }
        public decimal MontoTotalAprobado { get; set; }
        public decimal? TotalLiberado { get; set; }
        public decimal? TotalNoLiberado { get; set; }
        public int? TotalClaves { get; set; }
        public int? SegmentosSinCatalogo { get; set; }
        public DateTime? FechaCarga { get; set; }
        public string Usuario { get; set; }
    }
}
