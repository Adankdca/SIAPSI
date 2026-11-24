using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Data.SqlClient;
using SIAP.Models;

namespace SIAP.Repositories
{
    public class PresupuestoRepository
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString;

        private IDbConnection DbConnection
        {
            get { return new SqlConnection(_connectionString); }
        }

        /// <summary>
        /// Obtiene todos los registros de la matriz de presupuesto para el ejercicio actual.
        /// Realiza subconsultas con 'TOP 1' a catálogos asociados a los segmentos de la Clave Presupuestal 
        /// (como UR, Programa, Objeto de Gasto, Municipio y Región) para enriquecer la respuesta, evitando filas duplicadas en caso de catálogos sucios.
        /// También calcula si una clave está calendarizada sumando los meses, y obtiene el saldo disponible en los apartados activos de los últimos 7 días.
        /// </summary>
        /// <returns>Una enumeración de objetos <see cref="PresupuestoGridDto"/> que representan la matriz presupuestal con descripciones asociadas.</returns>
        public IEnumerable<PresupuestoGridDto> ObtenerMatrizPresupuesto()
        {
            using (IDbConnection db = DbConnection)
            {
                int mesCorte = 6;
                try
                {
                    var settingsMes = System.Configuration.ConfigurationManager.AppSettings["MesCorte"];
                    if (int.TryParse(settingsMes, out int parsedMes))
                    {
                        mesCorte = parsedMes;
                    }
                }
                catch { }

                // Las descripciones salen de los segmentos guardados de cada clave (presupuesto_segmentos),
                // que ya resolvieron el catálogo correcto (UR dentro del organismo, etc.).
                // Si una clave no tiene segmentos, se usa la búsqueda anterior por posición.
                string query = @"
                    SELECT 
                        p.Id, 
                        p.ClavePresupuestal,
                        COALESCE(ur.Descripcion, (SELECT TOP 1 Descripcion FROM cat_ur WHERE Clave = SUBSTRING(p.ClavePresupuestal, 1, 2))) AS UrDescripcion,
                        COALESCE(pp.Descripcion, (SELECT TOP 1 Descripcion FROM cat_programapresupuestario WHERE Clave = SUBSTRING(p.ClavePresupuestal, 11, 3))) AS ProgramaDescripcion,
                        COALESCE(og.Descripcion, (SELECT TOP 1 Descripcion FROM cat_objetogastointegrar WHERE Clave = SUBSTRING(p.ClavePresupuestal, 29, 5))) AS ObjetoGastoDescripcion,
                        ISNULL(p.Aprobado, 0) AS Aprobado,
                        (ISNULL(p.Aprobado, 0) + ISNULL(p.Aumentos, 0) - ISNULL(p.Disminuciones, 0)) AS Modificado,
                        ISNULL(p.Pagado, 0) AS Pagado,
                        ISNULL(p.Pagado, 0) AS Ministerio,
                        (SELECT ISNULL(SUM(Monto), 0) FROM apartados_presupuesto a WHERE a.ClavePresupuestal = p.ClavePresupuestal AND a.Activo = 1) AS SaldoApartado,
                        CASE WHEN (ISNULL(p.Enero,0) + ISNULL(p.Febrero,0) + ISNULL(p.Marzo,0) + ISNULL(p.Abril,0) + ISNULL(p.Mayo,0) + ISNULL(p.Junio,0) + ISNULL(p.Julio,0) + ISNULL(p.Agosto,0) + ISNULL(p.Septiembre,0) + ISNULL(p.Octubre,0) + ISNULL(p.Noviembre,0) + ISNULL(p.Diciembre,0)) > 0 THEN 1 ELSE 0 END AS EsCalendarizada,
                        COALESCE(sg.MpioValor, SUBSTRING(p.ClavePresupuestal, 23, 3)) AS ClaveMunicipio,
                        COALESCE(m.Nombre, (SELECT TOP 1 Nombre FROM cat_municipio WHERE Clave = SUBSTRING(p.ClavePresupuestal, 23, 3))) AS NombreMunicipio,
                        COALESCE(rg.Descripcion, (SELECT TOP 1 r.Descripcion FROM cat_region r JOIN cat_municipio m2 ON r.Id = m2.RegionId WHERE m2.Clave = SUBSTRING(p.ClavePresupuestal, 23, 3))) AS NombreRegion
                    FROM presupuestocalendarizado p
                    OUTER APPLY (
                        SELECT MAX(CASE WHEN s.Abreviatura = N'UR'   THEN s.CatalogoId END) AS UrId,
                               MAX(CASE WHEN s.Abreviatura = N'PP'   THEN s.CatalogoId END) AS PpId,
                               MAX(CASE WHEN s.Abreviatura = N'OG'   THEN s.CatalogoId END) AS OgId,
                               MAX(CASE WHEN s.Abreviatura = N'MPIO' THEN s.CatalogoId END) AS MpioId,
                               MAX(CASE WHEN s.Abreviatura = N'MPIO' THEN s.Valor END) AS MpioValor
                        FROM presupuesto_segmentos s
                        WHERE s.PresupuestoCalendarizadoId = p.Id
                    ) sg
                    LEFT JOIN cat_ur ur ON ur.Id = sg.UrId
                    LEFT JOIN cat_programapresupuestario pp ON pp.Id = sg.PpId
                    LEFT JOIN cat_objetogastointegrar og ON og.Id = sg.OgId
                    LEFT JOIN cat_municipio m ON m.Id = sg.MpioId
                    LEFT JOIN cat_region rg ON rg.Id = m.RegionId;
                ";
                return db.Query<PresupuestoGridDto>(query);
            }
        }


        /// <summary>
        /// Consulta los montos detallados de una Clave Presupuestal en específico, incluyendo montos aprobados, modificados, devengados, pagados,
        /// así como la calendarización mensual (Enero a Diciembre) y el saldo total en apartados vigentes.
        /// Además, si la clave existe, llena su propiedad 'Apartados' con el listado histórico de los apartados de los últimos 7 días.
        /// </summary>
        /// <param name="clave">La Clave Presupuestal de 56 caracteres a consultar.</param>
        /// <returns>Un objeto <see cref="PresupuestoDetalleDto"/> con la información detallada de la clave o null si no se encuentra.</returns>
        public PresupuestoDetalleDto ObtenerDetalleClave(string clave)
        {
            using (IDbConnection db = DbConnection)
            {
                string query = @"
                    SELECT TOP 1
                        p.ClavePresupuestal,
                        p.Aprobado, 
                         (ISNULL(p.Aprobado, 0) + ISNULL(p.Aumentos, 0) - ISNULL(p.Disminuciones, 0)) AS Modificado, 
                        p.Devengado, p.Pagado, p.Disminuciones AS GastadoPorAdecuacion,
                        p.Enero, p.Febrero, p.Marzo, p.Abril, p.Mayo, p.Junio,
                        p.Julio, p.Agosto, p.Septiembre, p.Octubre, p.Noviembre, p.Diciembre,
                        (SELECT ISNULL(SUM(Monto), 0) FROM apartados_presupuesto WHERE ClavePresupuestal = @Clave AND Activo = 1) AS SaldoApartado
                    FROM presupuestocalendarizado p
                    WHERE p.ClavePresupuestal = @Clave;
                ";
                var dto = db.QueryFirstOrDefault<PresupuestoDetalleDto>(query, new { Clave = clave });
                
                if (dto != null)
                {
                    int anioCorte = 2025;
                    int mesCorte = 6;
                    
                    try
                    {
                        var settingsAnio = System.Configuration.ConfigurationManager.AppSettings["AnioCorte"];
                        var settingsMes = System.Configuration.ConfigurationManager.AppSettings["MesCorte"];
                        if (!string.IsNullOrEmpty(settingsAnio)) anioCorte = int.Parse(settingsAnio);
                        if (!string.IsNullOrEmpty(settingsMes)) mesCorte = int.Parse(settingsMes);
                    }
                    catch { }

                    string cpStr = clave.Length >= 5 ? clave.Substring(clave.Length - 5, 4) : "";
                    int anioClave = anioCorte;
                    int.TryParse(cpStr, out anioClave);

                    // Poblamos la lista de apartados activos
                    string apQuery = "SELECT Id, Monto, Motivo, Oficio, FechaApartado FROM apartados_presupuesto WHERE ClavePresupuestal = @Clave AND Activo = 1 ORDER BY FechaApartado DESC";
                    dto.Apartados = db.Query<ApartadoDto>(apQuery, new { Clave = clave }).AsList();


                }
                return dto;
            }
        }

        /// <summary>
        /// Separa una clave presupuestal en sus segmentos según la estructura de su ejercicio
        /// (cat_estructuraclave) y busca la descripción de cada uno en su catálogo.
        /// El ejercicio se detecta por la longitud de la clave y su segmento CP.
        /// </summary>
        public ClaveDecodificadaDto DecodificarClave(string clave)
        {
            return new SIAP.Services.ClavePresupuestalService().Decodificar(clave);
        }

        /// <summary>
        /// Actualiza los montos mensuales programados (calendarización) en la tabla 'presupuestocalendarizado' para una clave presupuestal.
        /// Realiza un UPDATE en base de datos asignando los valores de los meses (Enero a Diciembre) enviados en la solicitud.
        /// </summary>
        /// <param name="req">Objeto <see cref="PresupuestoDetalleDto"/> con los montos mensuales y la Clave Presupuestal.</param>
        public void GuardarCalendarizacion(PresupuestoDetalleDto req)
        {
            using (IDbConnection db = DbConnection)
            {
                string query = @"
                    UPDATE presupuestocalendarizado SET
                        Enero = @Enero, Febrero = @Febrero, Marzo = @Marzo, Abril = @Abril,
                        Mayo = @Mayo, Junio = @Junio, Julio = @Julio, Agosto = @Agosto,
                        Septiembre = @Septiembre, Octubre = @Octubre, Noviembre = @Noviembre, Diciembre = @Diciembre
                    WHERE ClavePresupuestal = @ClavePresupuestal
                ";
                db.Execute(query, req);
            }
        }




        public IEnumerable<PresupuestoDetalleDto> ObtenerClavesPorObra(string prefijo22Digitos)
        {
            if (string.IsNullOrEmpty(prefijo22Digitos) || prefijo22Digitos.Length < 22)
            {
                return Enumerable.Empty<PresupuestoDetalleDto>();
            }

            using (IDbConnection db = DbConnection)
            {
                // Solo traemos las claves que empiezan con los mismos 22 caracteres
                string query = @"
                    SELECT 
                        ClavePresupuestal,
                        Aprobado, 
                        (ISNULL(Aprobado, 0) + ISNULL(Aumentos, 0) - ISNULL(Disminuciones, 0)) AS Modificado, 
                        Devengado, Pagado
                    FROM presupuestocalendarizado
                    WHERE ClavePresupuestal LIKE @Prefijo
                    ORDER BY ClavePresupuestal ASC;
                ";
                return db.Query<PresupuestoDetalleDto>(query, new { Prefijo = prefijo22Digitos + "%" });
            }
        }

        public bool RegistrarApartado(string clave, decimal monto, string motivo, string oficio)
        {
            using (IDbConnection db = DbConnection)
            {
                // La validación de saldo disponible ya se hace en el front/controller.
                // Insertamos directamente el apartado.
                string sql = @"
                    INSERT INTO apartados_presupuesto (ClavePresupuestal, Monto, FechaApartado, Activo, Motivo, Oficio)
                    VALUES (@Clave, @Monto, GETDATE(), 1, @Motivo, @Oficio);
                ";
                int rows = db.Execute(sql, new { Clave = clave, Monto = monto, Motivo = motivo, Oficio = oficio });
                return rows > 0;
            }
        }

        public bool CancelarApartado(string clave)
        {
            using (IDbConnection db = DbConnection)
            {
                string sql = "UPDATE apartados_presupuesto SET Activo = 0 WHERE ClavePresupuestal = @Clave";
                int rows = db.Execute(sql, new { Clave = clave });
                return rows > 0;
            }
        }

        public bool CancelarApartadoPorId(int id)
        {
            using (IDbConnection db = DbConnection)
            {
                string sql = "UPDATE apartados_presupuesto SET Activo = 0 WHERE Id = @Id";
                int rows = db.Execute(sql, new { Id = id });
                return rows > 0;
            }
        }
    }
}
