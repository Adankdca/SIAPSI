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

                // Usamos subconsultas con TOP 1 para evitar filas duplicadas en caso de catálogos sucios
                string query = $@"
                    SELECT 
                        p.Id, 
                        p.ClavePresupuestal,
                        (SELECT TOP 1 Descripcion FROM cat_ur WHERE Clave = SUBSTRING(p.ClavePresupuestal, 1, 2)) AS UrDescripcion,
                        (SELECT TOP 1 Descripcion FROM cat_programapresupuestario WHERE Clave = SUBSTRING(p.ClavePresupuestal, 11, 3)) AS ProgramaDescripcion,
                        (SELECT TOP 1 Descripcion FROM cat_objetogastointegrar WHERE Clave = SUBSTRING(p.ClavePresupuestal, 29, 5)) AS ObjetoGastoDescripcion,
                        p.Aprobado,
                         (ISNULL(p.Aprobado, 0) + ISNULL(p.Aumentos, 0) - ISNULL(p.Disminuciones, 0)) AS Modificado,
                        p.Pagado,
                        p.Pagado AS Ministerio,
                        (SELECT ISNULL(SUM(Monto), 0) FROM apartados_presupuesto a WHERE a.ClavePresupuestal = p.ClavePresupuestal AND a.Activo = 1 AND a.FechaApartado >= DATEADD(DAY, -7, GETDATE())) AS SaldoApartado,
                        CASE WHEN (p.Enero + p.Febrero + p.Marzo + p.Abril + p.Mayo + p.Junio + p.Julio + p.Agosto + p.Septiembre + p.Octubre + p.Noviembre + p.Diciembre) > 0 THEN 1 ELSE 0 END AS EsCalendarizada,
                        SUBSTRING(p.ClavePresupuestal, 23, 3) AS ClaveMunicipio,
                        (SELECT TOP 1 Nombre FROM cat_municipio WHERE Clave = SUBSTRING(p.ClavePresupuestal, 23, 3)) AS NombreMunicipio,
                        (SELECT TOP 1 r.Descripcion FROM cat_region r JOIN cat_municipio m ON r.Id = m.RegionId WHERE m.Clave = SUBSTRING(p.ClavePresupuestal, 23, 3)) AS NombreRegion
                    FROM presupuestocalendarizado p;
                ";
                return db.Query<PresupuestoGridDto>(query);
            }
        }

        /// <summary>
        /// Obtiene la matriz presupuestal de manera paginada, ideal para cargar grandes volúmenes de datos en DataTables (server-side processing).
        /// Permite filtrar los registros por término de búsqueda (que coincida con la Clave Presupuestal), municipio, región y estado de calendarización.
        /// Primero ejecuta una consulta de COUNT para obtener el total de registros filtrados, y luego ejecuta la consulta principal aplicando ORDER BY con OFFSET/FETCH.
        /// </summary>
        /// <param name="start">Índice del primer registro a devolver (para paginación).</param>
        /// <param name="length">Número máximo de registros a devolver por página.</param>
        /// <param name="search">Texto de búsqueda parcial sobre la Clave Presupuestal.</param>
        /// <param name="mpio">Filtro exacto por el nombre del Municipio (segmento 23-3 de la clave).</param>
        /// <param name="region">Filtro exacto por el nombre de la Región asociada al Municipio.</param>
        /// <param name="estado">Filtro por estado ('calendarizadas' o 'porliberar'). Evalúa si la suma de los meses de enero a diciembre es mayor a cero o igual a cero.</param>
        /// <returns>Una tupla donde el primer elemento es la lista de <see cref="PresupuestoGridDto"/> correspondiente a la página y el segundo es el conteo total de registros que coinciden con los filtros.</returns>
        public (IEnumerable<PresupuestoGridDto>, int) ObtenerMatrizPresupuestoPaginado(int start, int length, string search, string mpio = "", string region = "", string estado = "")
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

                // DataTables manda length = -1 cuando se elige "Todos"; FETCH NEXT no acepta valores <= 0
                if (start < 0) start = 0;
                if (length <= 0) length = int.MaxValue;

                var parameters = new DynamicParameters();
                parameters.Add("Start", start);
                parameters.Add("Length", length);

                string whereClause = "WHERE 1=1";
                if (!string.IsNullOrEmpty(search))
                {
                    whereClause += " AND p.ClavePresupuestal LIKE @Search";
                    parameters.Add("Search", "%" + search + "%");
                }
                if (!string.IsNullOrEmpty(mpio))
                {
                    whereClause += " AND (SELECT TOP 1 Nombre FROM cat_municipio WHERE Clave = SUBSTRING(p.ClavePresupuestal, 23, 3)) = @Mpio";
                    parameters.Add("Mpio", mpio);
                }
                if (!string.IsNullOrEmpty(region))
                {
                    whereClause += " AND (SELECT TOP 1 r.Descripcion FROM cat_region r JOIN cat_municipio m ON r.Id = m.RegionId WHERE m.Clave = SUBSTRING(p.ClavePresupuestal, 23, 3)) = @Region";
                    parameters.Add("Region", region);
                }
                if (!string.IsNullOrEmpty(estado) && estado != "all")
                {
                    if (estado == "calendarizadas")
                    {
                        whereClause += " AND (p.Enero + p.Febrero + p.Marzo + p.Abril + p.Mayo + p.Junio + p.Julio + p.Agosto + p.Septiembre + p.Octubre + p.Noviembre + p.Diciembre) > 0";
                    }
                    else if (estado == "porliberar")
                    {
                        whereClause += " AND (p.Enero + p.Febrero + p.Marzo + p.Abril + p.Mayo + p.Junio + p.Julio + p.Agosto + p.Septiembre + p.Octubre + p.Noviembre + p.Diciembre) = 0";
                    }
                }

                string countQuery = $"SELECT COUNT(1) FROM presupuestocalendarizado p {whereClause}";
                int totalCount = db.ExecuteScalar<int>(countQuery, parameters);

                string query = $@"
                    SELECT 
                        p.Id, 
                        p.ClavePresupuestal,
                        (SELECT TOP 1 Descripcion FROM cat_ur WHERE Clave = SUBSTRING(p.ClavePresupuestal, 1, 2)) AS UrDescripcion,
                        (SELECT TOP 1 Descripcion FROM cat_programapresupuestario WHERE Clave = SUBSTRING(p.ClavePresupuestal, 11, 3)) AS ProgramaDescripcion,
                        (SELECT TOP 1 Descripcion FROM cat_objetogastointegrar WHERE Clave = SUBSTRING(p.ClavePresupuestal, 29, 5)) AS ObjetoGastoDescripcion,
                        p.Aprobado,
                         (ISNULL(p.Aprobado, 0) + ISNULL(p.Aumentos, 0) - ISNULL(p.Disminuciones, 0)) AS Modificado,
                        p.Pagado,
                        p.Pagado AS Ministerio,
                        (SELECT ISNULL(SUM(Monto), 0) FROM apartados_presupuesto a WHERE a.ClavePresupuestal = p.ClavePresupuestal AND a.Activo = 1 AND a.FechaApartado >= DATEADD(DAY, -7, GETDATE())) AS SaldoApartado,
                        CASE WHEN (p.Enero + p.Febrero + p.Marzo + p.Abril + p.Mayo + p.Junio + p.Julio + p.Agosto + p.Septiembre + p.Octubre + p.Noviembre + p.Diciembre) > 0 THEN 1 ELSE 0 END AS EsCalendarizada,
                        SUBSTRING(p.ClavePresupuestal, 23, 3) AS ClaveMunicipio,
                        (SELECT TOP 1 Nombre FROM cat_municipio WHERE Clave = SUBSTRING(p.ClavePresupuestal, 23, 3)) AS NombreMunicipio,
                        (SELECT TOP 1 r.Descripcion FROM cat_region r JOIN cat_municipio m ON r.Id = m.RegionId WHERE m.Clave = SUBSTRING(p.ClavePresupuestal, 23, 3)) AS NombreRegion
                    FROM presupuestocalendarizado p
                    {whereClause}
                    ORDER BY p.Id DESC
                    OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY;
                ";
                var data = db.Query<PresupuestoGridDto>(query, parameters);
                
                return (data, totalCount);
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
                        (SELECT ISNULL(SUM(Monto), 0) FROM apartados_presupuesto WHERE ClavePresupuestal = @Clave AND Activo = 1 AND FechaApartado >= DATEADD(DAY, -7, GETDATE())) AS SaldoApartado
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
                    string apQuery = "SELECT Id, Monto, Motivo, Oficio, FechaApartado FROM apartados_presupuesto WHERE ClavePresupuestal = @Clave AND Activo = 1 AND FechaApartado >= DATEADD(DAY, -7, GETDATE()) ORDER BY FechaApartado DESC";
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

        public void InsertarNuevaClaveManual(string clave, decimal aprobado)
        {
            using (IDbConnection db = DbConnection)
            {
                int existe = db.QueryFirstOrDefault<int>("SELECT COUNT(*) FROM presupuestocalendarizado WHERE ClavePresupuestal = @c", new { c = clave });
                if (existe > 0) throw new Exception("La Clave Presupuestal ya existe en la base de datos.");

                string query = @"
                    INSERT INTO presupuestocalendarizado 
                    (ClavePresupuestal, Aprobado, Devengado, Pagado,
                     Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre)
                    VALUES 
                    (@c, @a, 0, 0,
                     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                ";
                db.Execute(query, new { c = clave, a = aprobado });
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
