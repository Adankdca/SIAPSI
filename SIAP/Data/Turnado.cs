using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAP.Data
{
    public class Turnado
    {
        public int idcorrespondencia { get; set; }
        public string Director { get; set; }
        public string instruccion { get; set; }
        public bool Atencion { get; set; }
        public bool Apoyo { get; set; }
        public bool Conocimiento { get; set; }
        public bool Informar { get; set; }
        public bool Archivo { get; set; }
    }
}