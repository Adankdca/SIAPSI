using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAP.Data
{
    public class Adecuacion
    {
        public string numsolicitud { get; set; }
        public string tipoadecuacion { get; set; }
        public DateTime fechasolicitud { get; set; }
        public decimal montoadecuacion { get; set; }
        public decimal emisor { get; set; }
        public decimal receptor { get; set; }
        public decimal agrupar { get; set; }
        public string oficionumero { get; set; }
        public int idestatus { get; set; }
        public string estatus { get; set; }
    }
    public class NumeroOficio
    {
        public string oficio { get; set; }
    }
    public class EstatusAdec
    {
        public int idestatusadecuacion { get; set; }
        public string estatusadecuacion { get; set; }
    }
    public class Mes
    {
        public int numeromes { get; set; }
        public string nombremes { get; set; }
    }
}