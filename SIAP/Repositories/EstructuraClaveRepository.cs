using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SIAP.Models;

namespace SIAP.Repositories
{
    /// <summary>
    /// Lee la estructura de la clave presupuestal por ejercicio (cat_estructuraclave)
    /// y los catálogos que se usan para describir cada segmento.
    /// </summary>
    public class EstructuraClaveRepository
    {
        private IDbConnection Conexion() { return DbConnectionFactory.GetConnection(); }

        /// <summary>Segmentos del ejercicio en orden, con su posición. Lista vacía si el año no está configurado.</summary>
        public List<SegmentoEstructura> ObtenerEstructura(int anio)
        {
            using (var db = Conexion())
            {
                // SELECT * para que funcione con o sin las columnas que agrega el script 08
                return db.Query<SegmentoEstructura>(@"
                    SELECT *
                    FROM dbo.vw_estructuraclave
                    WHERE Anio = @anio
                    ORDER BY Orden", new { anio }).ToList();
            }
        }

        /// <summary>Ejercicios que tienen estructura de clave configurada, del más reciente al más antiguo.</summary>
        public List<int> AniosConfigurados()
        {
            using (var db = Conexion())
            {
                return db.Query<int>("SELECT DISTINCT Anio FROM dbo.cat_estructuraclave WHERE Activo = 1 ORDER BY Anio DESC").ToList();
            }
        }

        /// <summary>
        /// Ejercicio al que pertenece una clave: el año cuya longitud coincide y cuyo segmento CP es ese año.
        /// Si ninguno coincide en CP, el más reciente con la misma longitud. Null si ninguno.
        /// </summary>
        public int? DetectarAnio(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave)) return null;
            clave = clave.Trim();
            using (var db = Conexion())
            {
                var filas = db.Query<SegmentoEstructura>(@"
                    SELECT Anio, Abreviatura, Posicion, Longitud, LongitudClave
                    FROM dbo.vw_estructuraclave
                    WHERE LongitudClave = @largo", new { largo = clave.Length }).ToList();
                if (filas.Count == 0) return null;

                foreach (var cp in filas.Where(f => f.Abreviatura == "CP").OrderByDescending(f => f.Anio))
                {
                    if (cp.Posicion - 1 + cp.Longitud <= clave.Length &&
                        clave.Substring(cp.Posicion - 1, cp.Longitud) == cp.Anio.ToString())
                        return cp.Anio;
                }
                return filas.Max(f => f.Anio);
            }
        }

        /// <summary>
        /// Id del organismo público configurado (tblc_SIAP_Configuracion.Clave contra cat_orgpub.Clave).
        /// Se usa para buscar la UR dentro de cat_ur, que tiene las UR de todos los organismos.
        /// </summary>
        public int? ObtenerOrgPubId()
        {
            try
            {
                using (var db = Conexion())
                {
                    return db.QueryFirstOrDefault<int?>(@"
                        SELECT TOP 1 o.Id
                        FROM dbo.tblc_SIAP_Configuracion c
                        INNER JOIN dbo.cat_orgpub o ON LTRIM(RTRIM(o.Clave)) = LTRIM(RTRIM(c.Clave))
                        ORDER BY c.Anio_ejercicio DESC");
                }
            }
            catch
            {
                return null; // la tabla de configuración aún no existe
            }
        }

        /// <summary>Columnas de una tabla de dbo (para validar nombres antes de usarlos en SQL dinámico).</summary>
        public HashSet<string> ColumnasDeTabla(string tabla)
        {
            using (var db = Conexion())
            {
                var cols = db.Query<string>(
                    "SELECT c.name FROM sys.columns c WHERE c.object_id = OBJECT_ID(N'dbo.' + QUOTENAME(@tabla))",
                    new { tabla });
                return new HashSet<string>(cols, StringComparer.OrdinalIgnoreCase);
            }
        }

        public class RegistroCatalogo
        {
            public int Id { get; set; }
            public string Clave { get; set; }
            public int? PadreId { get; set; }
            public string Descripcion { get; set; }
        }

        /// <summary>
        /// Lee un catálogo completo: Id, clave, Id del padre (si es jerárquico) y descripción.
        /// Los nombres de tabla y columnas se validan contra sys.columns antes de armar la consulta.
        /// Devuelve null si la tabla o alguna columna no existe.
        /// </summary>
        public List<RegistroCatalogo> LeerCatalogo(string tabla, string columnaClave, string columnaPadre,
                                                   string columnaAnio = null, int? anio = null)
        {
            if (string.IsNullOrWhiteSpace(tabla)) return null;
            if (tabla.Contains("]") || (columnaClave ?? "").Contains("]") || (columnaPadre ?? "").Contains("]")
                || (columnaAnio ?? "").Contains("]")) return null;
            var cols = ColumnasDeTabla(tabla);
            if (cols.Count == 0 || !cols.Contains("Id")) return null;
            if (string.IsNullOrWhiteSpace(columnaClave)) columnaClave = "Clave";
            if (!cols.Contains(columnaClave)) return null;
            bool conPadre = !string.IsNullOrWhiteSpace(columnaPadre);
            if (conPadre && !cols.Contains(columnaPadre)) return null;
            bool conAnio = !string.IsNullOrWhiteSpace(columnaAnio) && anio.HasValue;
            if (conAnio && !cols.Contains(columnaAnio)) return null;

            string colDesc = cols.Contains("Descripcion") ? "[Descripcion]" : cols.Contains("Nombre") ? "[Nombre]" : "CAST(NULL AS NVARCHAR(10))";
            string sql =
                "SELECT [Id], LTRIM(RTRIM(CAST([" + columnaClave + "] AS NVARCHAR(50)))) AS Clave, " +
                (conPadre ? "TRY_CAST([" + columnaPadre + "] AS INT)" : "CAST(NULL AS INT)") + " AS PadreId, " +
                colDesc + " AS Descripcion FROM dbo.[" + tabla + "]" +
                (conAnio ? " WHERE [" + columnaAnio + "] = @anio" : "") + " ORDER BY [Id]";

            using (var db = Conexion())
            {
                return db.Query<RegistroCatalogo>(sql, new { anio }).ToList();
            }
        }
    }
}
