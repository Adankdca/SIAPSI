using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAP.Data
{
    public class Configuracion
    {
        public int Idconfiguracion { get; set; }
        public string Nombre_dependencia { get; set; }
        public string Clave { get; set; }
        public string Titular { get; set; }
        public int idanio { get; set; }
        public string Anio_ejercicio { get; set; }
    }
    public class Firmantes
    {
        public int Idfirmante { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
    }
}