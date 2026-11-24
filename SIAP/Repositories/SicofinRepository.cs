using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SIAP.Models;

namespace SIAP.Repositories
{
    /// <summary>
    /// Consultas de solo lectura a BDSICOFIN (sistema de planeación de obra).
    ///
    /// En BDSICOFIN la obra (tblp_SP_general) se liga a sus datos financieros (tblp_SP_financiero)
    /// por idgeneral, y en tblp_SP_financiero la clave presupuestal está en cuatro columnas
    /// (estatal/federal, obra/indirecto). Desde 2026 ahí no se guarda la clave completa sino una
    /// clave corta de 39 caracteres con algunos segmentos:
    ///     UR URO FI FU SF SSF PS PP OA AI PT MPIO + FF RA PF + CP
    /// Los segmentos de la clave corta se configuran en Web.config (BDSICOFIN:SegmentosClaveCorta).
    /// </summary>
    public class SicofinRepository
    {
        private const string NombreConexion = "BDSICOFINConnectionString";
        private const string SegmentosPorOmision = "UR,URO,FI,FU,SF,SSF,PS,PP,OA,AI,PT,MPIO,FF,RA,PF,CP";

        /// <summary>True si Web.config tiene la cadena de conexión a BDSICOFIN.</summary>
        public static bool Configurado
        {
            get
            {
                var cs = ConfigurationManager.ConnectionStrings[NombreConexion];
                return cs != null && !string.IsNullOrWhiteSpace(cs.ConnectionString);
            }
        }

        private static SqlConnection Conexion()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[NombreConexion].ConnectionString);
        }

        /// <summary>
        /// Partes de la clave completa que forman la clave corta: (posición desde 1, longitud),
        /// juntando las que van seguidas. Null si la estructura del año no tiene algún segmento.
        /// </summary>
        public static List<KeyValuePair<int, int>> RangosClaveCorta(List<SegmentoEstructura> estructura)
        {
            string lista = ConfigurationManager.AppSettings["BDSICOFIN:SegmentosClaveCorta"];
            if (string.IsNullOrWhiteSpace(lista)) lista = SegmentosPorOmision;
            var rangos = new List<KeyValuePair<int, int>>();
            foreach (var abrev in lista.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var seg = estructura.FirstOrDefault(x => string.Equals(x.Abreviatura, abrev.Trim(), StringComparison.OrdinalIgnoreCase));
                if (seg == null) return null;
                if (rangos.Count > 0 && rangos[rangos.Count - 1].Key + rangos[rangos.Count - 1].Value == seg.Posicion)
                    rangos[rangos.Count - 1] = new KeyValuePair<int, int>(rangos[rangos.Count - 1].Key, rangos[rangos.Count - 1].Value + seg.Longitud);
                else
                    rangos.Add(new KeyValuePair<int, int>(seg.Posicion, seg.Longitud));
            }
            return rangos;
        }

        /// <summary>Arma la clave corta de BDSICOFIN a partir de la clave completa.</summary>
        public static string ClaveCorta(string claveCompleta, List<SegmentoEstructura> estructura)
        {
            var rangos = RangosClaveCorta(estructura);
            if (rangos == null || string.IsNullOrEmpty(claveCompleta)) return null;
            var sb = new System.Text.StringBuilder();
            foreach (var r in rangos)
            {
                if (r.Key - 1 + r.Value > claveCompleta.Length) return null;
                sb.Append(claveCompleta.Substring(r.Key - 1, r.Value));
            }
            return sb.ToString().ToUpperInvariant();
        }

        /// <summary>
        /// Obras/acciones de BDSICOFIN ligadas a una clave del ejercicio. Una clave puede tener varias
        /// acciones (p. ej. la obra y su supervisión).
        /// En BDSICOFIN unas claves están capturadas completas y otras cortas; las dos se comparan
        /// por su clave corta: una clave completa capturada allá con otra partida u otro dato que
        /// no forma parte de la clave corta también encuentra la obra.
        /// </summary>
        public List<ObraSicofinDto> ObrasDeClave(string claveCompleta, int anio, List<SegmentoEstructura> estructura)
        {
            var rangos = RangosClaveCorta(estructura);
            string corta = ClaveCorta(claveCompleta, estructura);
            if (!Configurado || rangos == null || corta == null) return new List<ObraSicofinDto>();
            int largo = estructura[0].LongitudClave;

            // Para cada columna: igual a la clave corta, igual a la completa, o completa del mismo largo
            // cuya clave corta coincide. Posiciones y largo salen de la estructura (son números).
            var condiciones = new List<string>();
            foreach (var col in new[] { "cvepresupuestalsiaheestatalobra", "cvepresupuestalsiahefederalobra",
                                        "cvepresupuestalsiaheestatalindirecto", "cvepresupuestalsiahefederalindirecto" })
            {
                string v = "UPPER(LTRIM(RTRIM(f." + col + ")))";
                string corte = string.Join(" + ", rangos.Select(r => "SUBSTRING(" + v + ", " + r.Key + ", " + r.Value + ")"));
                condiciones.Add("(" + v + " IN (@corta, @completa) OR (LEN(" + v + ") = " + largo + " AND " + corte + " = @corta))");
            }
            string sql = @"
                SELECT g.idgeneral AS IdGeneral, g.idaccion AS IdAccion, g.idproyecto AS IdProyecto,
                       g.nombreproyecto AS Proyecto, g.accion AS Accion, g.tipoaccion AS TipoAccion,
                       g.municipio AS Municipio, g.localidad AS Localidad, g.estatus AS Estatus,
                       g.tipo_obra AS TipoObra, g.Dependencia AS Dependencia,
                       CAST(f.fuentefinanciamiento AS NVARCHAR(500)) AS Fuente
                FROM dbo.tblp_SP_financiero f
                JOIN dbo.tblp_SP_general g ON g.idgeneral = f.idgeneral
                WHERE f.anio = @anio
                  AND (" + string.Join("\n                       OR ", condiciones) + @")
                ORDER BY g.idgeneral";
            using (var db = Conexion())
            {
                var lista = db.Query<ObraSicofinDto>(sql, new
                {
                    anio = anio.ToString(),
                    corta,
                    completa = (claveCompleta ?? "").Trim().ToUpperInvariant()
                }, commandTimeout: 20).ToList();
                // Un idgeneral puede venir repetido si tiene varias fuentes: se queda uno
                return lista.GroupBy(o => o.IdGeneral).Select(g => g.First()).ToList();
            }
        }
    }
}
