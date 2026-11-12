using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAP.Data
{
    public class Correspondencia
    {
        public int row { get; set; }
        public int idcorrespondencia { get; set; }
        public string numoficio { get; set; }
        public DateTime fechaoficio { get; set; }
        public string origen { get; set; }
        public string nombre { get; set; }
        public string cargo { get; set; }
        public string asunto { get; set; }
        public DateTime fechaacuerdo { get; set; }
        public DateTime fechatermino { get; set; }
        public string anexo { get; set; }
        public int iddependencia { get; set; }
        public int idsecretaria { get; set; }
        public int idcordinacion { get; set; }
        public int idsubsecretaria { get; set; }
        public int iddireccion { get; set; }
        public int idarea { get; set; }
        public int idestatus { get; set; }
        public string instruccion { get; set; }
        public string prioridad { get; set; }
        public int tiempo { get; set; }
        public DateTime fechacierre { get; set; }
        public DateTime fechaalta { get; set; }
        public int idusuario { get; set; }
        public string analista { get; set; }
        public string instruccionanalista { get; set; }
        public string descripcionanalista { get; set; }
        public string archivoanalista { get; set; }
        public string descripcionconcluido { get; set; }
        public string archivoconcluido { get; set; }
        public string descripcionrespuesta { get; set; }
        public string archivorespuesta { get; set; }
        public string instrucciondir { get; set; }
        public string prioridaddir { get; set; }
        public string tiempodir { get; set; }
        public DateTime fechaacuerdodir { get; set; }
        public DateTime fechaterminodir { get; set; }
        public DateTime fechaatendido { get; set; }
        public string Nofolio { get; set; }
        public string Tipo { get; set; }
        public int dias { get; set; }
        public int CveMunicipio { get; set; }
        public int IDLocalidad { get; set; }
        public int Agenda { get; set; }
        public string prioridadsec { get; set; }
        public DateTime fechaterminosec { get; set; }
        public string prioridadcoor { get; set; }
        public DateTime fechaterminocoor { get; set; }
        public string instruccionsec { get; set; }
        public string instruccioncoor { get; set; }
        public DateTime fechaterminosub { get; set; }
        public string archivocrea { get; set; }
        public string estatus { get; set; }
        public string Turnado { get; set; }
        public string EstatusActual { get; set; }
        public bool Atencion { get; set; }
        public bool Apoyo { get; set; }
        public bool Conocimiento { get; set; }
        public bool Informar { get; set; }
        public bool Archivo {get;set;}
        public string TurnadoT { get; set; }
        public string prioridadGen { get; set; }
    }
}