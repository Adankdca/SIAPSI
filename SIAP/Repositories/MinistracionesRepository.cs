using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SIAP.Models;

namespace SIAP.Repositories
{
    public class MinistracionesRepository
    {
        private string connectionString;

        public MinistracionesRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ToString();
        }

        // =============================================
        // CATÁLOGOS (NUEVOS Y EXISTENTES)
        // =============================================

        public List<Modeltipoministracion> GetTiposMinistracion()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Clave, Descripcion 
                               FROM cat_tipoministracion 
                               ORDER BY Clave";
                return con.Query<Modeltipoministracion>(sql).ToList();
            }
        }

        // NUEVO: Obtener tipos de ministración folio
        public List<ModelTipoMinistracionFolio> GetTiposMinistracionFolio()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Clave, Descripcion 
                               FROM cat_tipoministracionfolio 
                               ORDER BY Clave";
                return con.Query<ModelTipoMinistracionFolio>(sql).ToList();
            }
        }

        public string GetSiguienteFolio(int anio, int mes, int idTipoFolio)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var tipoFolio = con.QueryFirstOrDefault<ModelTipoMinistracionFolio>("SELECT * FROM cat_tipoministracionfolio WHERE Id = @Id", new { Id = idTipoFolio });
                if (tipoFolio == null) return "";

                // Formato: 202501-MM
                string prefijo = anio.ToString() + mes.ToString("D2") + "-" + tipoFolio.Clave;
                string sql = "SELECT MAX(Folio) FROM ministraciones WHERE Folio LIKE @Prefijo";
                string maxFolio = con.ExecuteScalar<string>(sql, new { Prefijo = prefijo + "%" });
                
                int consecutivo = 1;
                if (!string.IsNullOrEmpty(maxFolio))
                {
                    string num = maxFolio.Substring(prefijo.Length);
                    if (int.TryParse(num, out int maxConsecutivo))
                    {
                        consecutivo = maxConsecutivo + 1;
                    }
                }
                
                // Formato final: 202501-MM001
                return prefijo + consecutivo.ToString("D3");
            }
        }

        // NUEVO: Obtener sub-fuentes de financiamiento
        public List<ModelSubFuenteFinanciamiento> GetSubFuentesFinanciamiento()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Clave, Descripcion 
                               FROM cat_subfuentefinanciamiento 
                               ORDER BY Clave";
                return con.Query<ModelSubFuenteFinanciamiento>(sql).ToList();
            }
        }

        // NUEVO: Obtener proyectos estratégicos
        public List<ModelProyectoEstrategico> GetProyectosEstrategicos()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Clave, Descripcion 
                               FROM cat_proyectoestrategico 
                               ORDER BY Clave";
                return con.Query<ModelProyectoEstrategico>(sql).ToList();
            }
        }

        public List<Modelministracion> GetMinistracionesCatalogo()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id as idministracion, Folio as numministracion 
                               FROM ministraciones 
                               WHERE Folio IS NOT NULL
                               ORDER BY Id DESC";
                return con.Query<Modelministracion>(sql).ToList();
            }
        }

        public List<Modelnummes> GetMeses()
        {
            return new List<Modelnummes>
            {
                new Modelnummes { numeromes = 1, nombremes = "Enero" },
                new Modelnummes { numeromes = 2, nombremes = "Febrero" },
                new Modelnummes { numeromes = 3, nombremes = "Marzo" },
                new Modelnummes { numeromes = 4, nombremes = "Abril" },
                new Modelnummes { numeromes = 5, nombremes = "Mayo" },
                new Modelnummes { numeromes = 6, nombremes = "Junio" },
                new Modelnummes { numeromes = 7, nombremes = "Julio" },
                new Modelnummes { numeromes = 8, nombremes = "Agosto" },
                new Modelnummes { numeromes = 9, nombremes = "Septiembre" },
                new Modelnummes { numeromes = 10, nombremes = "Octubre" },
                new Modelnummes { numeromes = 11, nombremes = "Noviembre" },
                new Modelnummes { numeromes = 12, nombremes = "Diciembre" }
            };
        }

        public List<ModelDigitoMinistracion> GetDigitosMinistracion()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Clave, Clave as DM, Descripcion 
                               FROM cat_digitosministracion 
                               ORDER BY Clave";
                return con.Query<ModelDigitoMinistracion>(sql).ToList();
            }
        }

        public List<ModelFuenteFinanciamiento> GetFuentesFinanciamiento()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Clave, Descripcion 
                               FROM cat_fuentefinanciamiento 
                               ORDER BY Clave";
                return con.Query<ModelFuenteFinanciamiento>(sql).ToList();
            }
        }

        public List<ModelProgramaFondo> GetProgramasFondo()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"
                    SELECT 
                        pf.Id, 
                        pf.Clave, 
                        pf.Descripcion,
                        ctr.Clave as TR, 
                        ctr.Descripcion as TipoRecurso,
                        cff.Clave as FF, 
                        cff.Descripcion as FuenteFinanciamiento,
                        cr.Clave as Ramo, 
                        cr.Descripcion as DescripcionRamo
                    FROM cat_programafondo pf
                    LEFT JOIN cat_tiporecurso ctr ON pf.TipoRecursoId = ctr.Id
                    LEFT JOIN cat_fuentefinanciamiento cff ON pf.FuenteFinanciamientoId = cff.Id
                    LEFT JOIN cat_ramo cr ON pf.RamoId = cr.Id
                    ORDER BY pf.Clave
                ";
                return con.Query<ModelProgramaFondo>(sql).ToList();
            }
        }

        public List<ModelRamo> GetRamos()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Clave, Descripcion 
                               FROM cat_ramo 
                               ORDER BY Clave";
                return con.Query<ModelRamo>(sql).ToList();
            }
        }

        public List<ModelTipoRecurso> GetTiposRecurso()
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Clave, Descripcion 
                               FROM cat_tiporecurso 
                               ORDER BY Clave";
                return con.Query<ModelTipoRecurso>(sql).ToList();
            }
        }

        public ModelProgramaFondo GetProgramaFondoById(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"
                    SELECT 
                        pf.Id, 
                        pf.Clave, 
                        pf.Descripcion,
                        ctr.Id as TipoRecursoId,
                        ctr.Clave as TR, 
                        ctr.Descripcion as TipoRecurso,
                        cff.Id as FuenteFinanciamientoId,
                        cff.Clave as FF, 
                        cff.Descripcion as FuenteFinanciamiento,
                        cr.Id as RamoId,
                        cr.Clave as Ramo, 
                        cr.Descripcion as DescripcionRamo
                    FROM cat_programafondo pf
                    LEFT JOIN cat_tiporecurso ctr ON pf.TipoRecursoId = ctr.Id
                    LEFT JOIN cat_fuentefinanciamiento cff ON pf.FuenteFinanciamientoId = cff.Id
                    LEFT JOIN cat_ramo cr ON pf.RamoId = cr.Id
                    WHERE pf.Id = @id
                ";

                return con.QueryFirstOrDefault<ModelProgramaFondo>(sql, new { id });
            }
        }

        public List<Modelnummes> Meses => GetMeses();

        // =============================================
        // CONSULTA PRINCIPAL CON FILTROS (ACTUALIZADA)
        // =============================================

        public List<ModelMinistracionCompleta> GetMinistraciones(
            string filtro = "",
            int? mes = null,
            int? tipoMinistracionId = null,
            int? programaFondoId = null,
            string estatus = null)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT 
                        m.Id,
                        m.Folio,
                        m.PresupuestoCalendarizadoId,
                        m.ClavePresupuestal,
                        m.Mes,
                        m.Anio,
                        m.Importe_total as ImporteTotal,
                        m.Concepto,
                        m.Estatus,
                        m.FechaCreacion,
                        m.UsuarioCreacion,
                        
                        -- Campos existentes (FKs)
                        m.id_digito_ministracion as DigitoMinistracionId,
                        m.id_tipo_ministracion as TipoMinistracionId,
                        m.id_programa_fondo as ProgramaFondoId,
                        m.id_fuente_financiamiento as FuenteFinanciamientoId,
                        
                        -- NUEVOS CAMPOS
                        m.id_tipoministracion_folio,
                        m.id_tipo_recurso,
                        m.Id_sub_fuente_financiamiento,
                        m.Id_proyecto_estrategico,
                        m.Id_ramo,
                        
                        -- Descripciones de catálogos existentes
                        dm.Clave as DigitoClave,
                        dm.Descripcion as DigitoDescripcion,
                        tm.Clave as TipoClave,
                        tm.Descripcion as TipoDescripcion,
                        ff.Clave as FuenteClave,
                        ff.Descripcion as FuenteDescripcion,
                        pf.Clave as ProgramaFondoClave,
                        pf.Descripcion as ProgramaFondoDescripcion,
                        
                        -- NUEVAS DESCRIPCIONES
                        tmf.Clave as TipoMinistracionFolioClave,
                        tmf.Descripcion as TipoMinistracionFolioDescripcion,
                        tr.Clave as TipoRecursoClave,
                        tr.Descripcion as TipoRecursoDescripcion,
                        sf.Clave as SubFuenteClave,
                        sf.Descripcion as SubFuenteDescripcion,
                        pe.Clave as ProyectoEstrategicoClave,
                        pe.Descripcion as ProyectoEstrategicoDescripcion,
                        r.Clave as RamoClave,
                        r.Descripcion as RamoDescripcion,
                        
                        -- Campos para programa fondo (ya existentes)
                        ctr.Clave as ProgramaFondoTR,
                        cff.Clave as ProgramaFondoFF,
                        cr.Clave as ProgramaFondoRamo
                        
                    FROM ministraciones m
                    
                    -- Joins existentes
                    LEFT JOIN cat_digitosministracion dm ON m.id_digito_ministracion = dm.Id
                    LEFT JOIN cat_tipoministracion tm ON m.id_tipo_ministracion = tm.Id
                    LEFT JOIN cat_programafondo pf ON m.id_programa_fondo = pf.Id
                    LEFT JOIN cat_fuentefinanciamiento ff ON m.id_fuente_financiamiento = ff.Id
                    
                    -- NUEVOS JOINS
                    LEFT JOIN cat_tipoministracionfolio tmf ON m.id_tipoministracion_folio = tmf.Id
                    LEFT JOIN cat_tiporecurso tr ON m.id_tipo_recurso = tr.Id
                    LEFT JOIN cat_subfuentefinanciamiento sf ON m.Id_sub_fuente_financiamiento = sf.Id
                    LEFT JOIN cat_proyectoestrategico pe ON m.Id_proyecto_estrategico = pe.Id
                    LEFT JOIN cat_ramo r ON m.Id_ramo = r.Id
                    
                    -- Joins para programa fondo (existentes)
                    LEFT JOIN cat_tiporecurso ctr ON pf.TipoRecursoId = ctr.Id 
                    LEFT JOIN cat_fuentefinanciamiento cff ON pf.FuenteFinanciamientoId = cff.Id 
                    LEFT JOIN cat_ramo cr ON pf.RamoId = cr.Id
                    
                    WHERE 1=1
                ";

                var parameters = new DynamicParameters();

                if (mes.HasValue)
                {
                    sql += " AND m.Mes = @mes";
                    parameters.Add("@mes", mes.Value);
                }

                if (tipoMinistracionId.HasValue)
                {
                    sql += " AND m.id_tipo_ministracion = @tipoMinistracionId";
                    parameters.Add("@tipoMinistracionId", tipoMinistracionId.Value);
                }

                if (programaFondoId.HasValue)
                {
                    sql += " AND m.id_programa_fondo = @programaFondoId";
                    parameters.Add("@programaFondoId", programaFondoId.Value);
                }

                if (!string.IsNullOrEmpty(estatus))
                {
                    sql += " AND m.Estatus = @estatus";
                    parameters.Add("@estatus", estatus);
                }

                if (!string.IsNullOrEmpty(filtro))
                {
                    sql += @" AND (m.Folio LIKE @filtro 
                                OR m.Concepto LIKE @filtro 
                                OR m.ClavePresupuestal LIKE @filtro)";
                    parameters.Add("@filtro", "%" + filtro + "%");
                }

                sql += " ORDER BY m.Id DESC";

                try
                {
                    var resultados = con.Query<ModelMinistracionCompleta>(sql, parameters).ToList();

                    // Obtener el presupuesto calendarizado para cada ministración
                    foreach (var item in resultados)
                    {
                        var columnaMes = ObtenerNombreColumnaMes(item.Mes);
                        var claves = item.ClavePresupuestal?.Split(',').Select(c => c.Trim()).Where(c => !string.IsNullOrEmpty(c)).ToArray();
                        if (claves != null && claves.Length > 0)
                        {
                            var sqlPresupuesto = $@"
                                SELECT COALESCE(SUM({columnaMes}), 0) 
                                FROM presupuestocalendarizado pc 
                                WHERE pc.ClavePresupuestal IN (SELECT LTRIM(RTRIM(value)) FROM STRING_SPLIT(@claves, ','))
                            ";
                            item.PresupuestoCalendarizado = con.QueryFirstOrDefault<decimal>(sqlPresupuesto, new { claves = string.Join(",", claves) });
                        }
                        else
                        {
                            item.PresupuestoCalendarizado = 0;
                        }

                        // Propiedades de apoyo
                        item.numministracion = item.Folio;
                        item.tipoministracion = item.TipoDescripcion ?? "N/A";
                        item.importe = item.ImporteTotal;
                        item.enlace = item.ClavePresupuestal;
                        item.numeromes = item.Mes;
                        item.estatus = item.Estatus ?? "REGISTRADA";
                        item.anio = item.Anio;
                        item.concepto = item.Concepto ?? "";
                        item.fuenteFinanciamiento = item.FuenteDescripcion ?? "N/A";
                        item.fuenteFinanciamientoId = item.FuenteFinanciamientoId;
                        item.programaFondoDescripcion = item.ProgramaFondoDescripcion ?? "N/A";
                        item.tienePDF = false;
                        item.nombremes = ObtenerNombreMes(item.Mes);
                        item.fecharegistro = item.FechaCreacion.ToString("dd/MM/yyyy");
                        item.digitoClave = item.DigitoClave;
                        item.digitoDescripcion = item.DigitoDescripcion;
                        item.digitoMinistracion = item.DigitoDescripcion;
                        item.digitoDM = item.DigitoClave;
                        item.tipoClave = item.TipoClave;
                    }

                    return resultados;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en GetMinistraciones: " + ex.Message);
                    throw;
                }
            }
        }

        private string ObtenerNombreMes(int mes)
        {
            string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                               "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            return mes >= 1 && mes <= 12 ? meses[mes - 1] : "N/A";
        }

        // =============================================
        // CLAVES PRESUPUESTALES
        // =============================================

        public List<ModelClavePresupuestal> GetClavesPresupuestales(string filtro = "")
        {
            using (var con = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT DISTINCT
                        pc.ClavePresupuestal,
                        '' as Descripcion,
                        'Presupuesto' as Tipo,
                        pc.Aprobado as Monto,
                        '' as Nombremes,
                        NULL as Numeromes
                    FROM presupuestocalendarizado pc
                    WHERE pc.ClavePresupuestal IS NOT NULL 
                      AND pc.ClavePresupuestal != ''
                      AND pc.ClavePresupuestal != 'Total general'
                    
                    UNION ALL
                    
                    SELECT DISTINCT
                        m.ClavePresupuestal,
                        m.Concepto as Descripcion,
                        'Ministración' as Tipo,
                        m.Importe_total as Monto,
                        '' as Nombremes,
                        m.Mes as Numeromes
                    FROM ministraciones m
                    WHERE m.ClavePresupuestal IS NOT NULL 
                      AND m.ClavePresupuestal != ''
                      AND m.ClavePresupuestal != 'Total general'
                ";

                if (!string.IsNullOrEmpty(filtro))
                {
                    var sqlWithFilter = @"
                        SELECT * FROM (
                            " + sql + @"
                        ) AS claves
                        WHERE ClavePresupuestal LIKE @filtro 
                           OR Descripcion LIKE @filtro
                        ORDER BY ClavePresupuestal
                    ";
                    return con.Query<ModelClavePresupuestal>(sqlWithFilter, new { filtro = "%" + filtro + "%" }).ToList();
                }

                return con.Query<ModelClavePresupuestal>(sql).ToList();
            }
        }

        // =============================================
        // CLAVES PRESUPUESTALES POR MES
        // =============================================

        public List<ModelClavePresupuestalPorMes> GetClavesPresupuestalesPorMes(int mes, int anio)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var columna = ObtenerNombreColumnaMes(mes);

                var sql = $@"
                    SELECT DISTINCT
                        pc.ClavePresupuestal,
                        '' as Descripcion,
                        'Presupuesto' as Tipo,
                        COALESCE(pc.{columna}, 0) as Monto,
                        @mes as Numeromes,
                        pc.ProgramaFondoId,
                        pf.Descripcion as ProgramaFondoDescripcion,
                        ff.Clave as FuenteFinanciamiento,
                        ff.Id as FuenteFinanciamientoId,
                        CASE 
                            WHEN ctr.Clave = '2' THEN 'Etiquetado'
                            ELSE 'No Etiquetado'
                        END as FuenteFinanciamiento
                    FROM presupuestocalendarizado pc
                    LEFT JOIN cat_programafondo pf ON pc.ProgramaFondoId = pf.Id 
                    LEFT JOIN cat_tiporecurso ctr ON pf.TipoRecursoId = ctr.Id 
                    LEFT JOIN cat_fuentefinanciamiento cff ON pf.FuenteFinanciamientoId = cff.Id 
                    LEFT JOIN cat_ramo cr ON pf.RamoId = cr.Id
                    LEFT JOIN cat_fuentefinanciamiento ff ON pc.FuenteFinanciamientoId = ff.Id
                    WHERE pc.ClavePresupuestal IS NOT NULL 
                      AND pc.ClavePresupuestal != ''
                      AND pc.ClavePresupuestal != 'Total general'
                    ORDER BY pc.ClavePresupuestal
                ";

                try
                {
                    var resultados = con.Query<ModelClavePresupuestalPorMes>(sql, new { mes }).ToList();
                    foreach (var item in resultados)
                    {
                        item.nombremes = ObtenerNombreMes(mes);
                    }
                    return resultados;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en GetClavesPresupuestalesPorMes: " + ex.Message);
                    return new List<ModelClavePresupuestalPorMes>();
                }
            }
        }

        // =============================================
        // CLAVES PRESUPUESTALES DE MESES POSTERIORES
        // =============================================

        public List<ModelClavePresupuestalPorMes> GetClavesPresupuestalesOtrosMeses(int mesActual, int anio)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var mesesPosteriores = new List<int>();
                for (int m = mesActual + 1; m <= 12; m++)
                {
                    mesesPosteriores.Add(m);
                }

                if (mesesPosteriores.Count == 0)
                    return new List<ModelClavePresupuestalPorMes>();

                var condiciones = new List<string>();
                var parameters = new DynamicParameters();
                parameters.Add("@anio", anio);

                foreach (var m in mesesPosteriores)
                {
                    var columna = ObtenerNombreColumnaMes(m);
                    condiciones.Add($"pc.{columna} > 0");
                    parameters.Add($"@mes_{m}", m);
                }

                var primerMes = mesesPosteriores[0];
                var columnaPrimerMes = ObtenerNombreColumnaMes(primerMes);

                var sql = $@"
                    SELECT DISTINCT
                        pc.ClavePresupuestal,
                        '' as Descripcion,
                        'Presupuesto' as Tipo,
                        COALESCE(pc.{columnaPrimerMes}, 0) as Monto,
                        @mes_{primerMes} as Numeromes,
                        pc.ProgramaFondoId,
                        pf.Descripcion as ProgramaFondoDescripcion,
                        ff.Clave as FuenteFinanciamiento,
                        ff.Id as FuenteFinanciamientoId
                    FROM presupuestocalendarizado pc
                    LEFT JOIN cat_programafondo pf ON pc.ProgramaFondoId = pf.Id 
                    LEFT JOIN cat_tiporecurso ctr ON pf.TipoRecursoId = ctr.Id 
                    LEFT JOIN cat_fuentefinanciamiento cff ON pf.FuenteFinanciamientoId = cff.Id 
                    LEFT JOIN cat_ramo cr ON pf.RamoId = cr.Id
                    LEFT JOIN cat_fuentefinanciamiento ff ON pc.FuenteFinanciamientoId = ff.Id
                    WHERE pc.ClavePresupuestal IS NOT NULL 
                      AND pc.ClavePresupuestal != ''
                      AND pc.ClavePresupuestal != 'Total general'
                      AND ({string.Join(" OR ", condiciones)})
                    ORDER BY pc.ClavePresupuestal
                ";

                try
                {
                    var resultados = con.Query<ModelClavePresupuestalPorMes>(sql, parameters).ToList();
                    foreach (var item in resultados)
                    {
                        item.nombremes = ObtenerNombreMes(item.Numeromes ?? 0);
                    }
                    return resultados;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en GetClavesPresupuestalesOtrosMeses: " + ex.Message);
                    return new List<ModelClavePresupuestalPorMes>();
                }
            }
        }

        private string ObtenerNombreColumnaMes(int mes)
        {
            string[] columnas = {
                "",          // índice 0
                "Enero",     // índice 1
                "Febrero",   // índice 2
                "Marzo",     // índice 3
                "Abril",     // índice 4
                "Mayo",      // índice 5
                "Junio",     // índice 6
                "Julio",     // índice 7
                "Agosto",    // índice 8
                "Septiembre",// índice 9
                "Octubre",   // índice 10
                "Noviembre", // índice 11
                "Diciembre"  // índice 12
            };
            return mes >= 1 && mes <= 12 ? columnas[mes] : "";
        }

        // =============================================
        // SUMA DE CLAVES POR MES
        // =============================================

        public decimal GetSumaClavesPorMes(int mes, string clave)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var columna = ObtenerNombreColumnaMes(mes);
                var sql = $@"
                    SELECT COALESCE(SUM({columna}), 0) as Total
                    FROM presupuestocalendarizado pc
                    WHERE pc.ClavePresupuestal = @clave
                ";

                try
                {
                    return con.QueryFirstOrDefault<decimal>(sql, new { clave });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en GetSumaClavesPorMes: " + ex.Message);
                    return 0;
                }
            }
        }

        // =============================================
        // OBTENER UNA MINISTRACIÓN POR ID (ACTUALIZADA)
        // =============================================

        public ModelMinistracionCompleta GetMinistracionById(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT 
                        m.Id,
                        m.Folio,
                        m.PresupuestoCalendarizadoId,
                        m.ClavePresupuestal,
                        m.Mes,
                        m.Anio,
                        m.Importe_total as ImporteTotal,
                        m.Concepto,
                        m.Estatus,
                        m.FechaCreacion,
                        m.UsuarioCreacion,
                        
                        -- Campos existentes
                        m.id_digito_ministracion as DigitoMinistracionId,
                        m.id_tipo_ministracion as TipoMinistracionId,
                        m.id_programa_fondo as ProgramaFondoId,
                        m.id_fuente_financiamiento as FuenteFinanciamientoId,
                        
                        -- NUEVOS CAMPOS
                        m.id_tipoministracion_folio,
                        m.id_tipo_recurso,
                        m.Id_sub_fuente_financiamiento,
                        m.Id_proyecto_estrategico,
                        m.Id_ramo,
                        
                        -- Descripciones existentes
                        dm.Clave as DigitoClave,
                        dm.Descripcion as DigitoDescripcion,
                        tm.Clave as TipoClave,
                        tm.Descripcion as TipoDescripcion,
                        ff.Clave as FuenteClave,
                        ff.Descripcion as FuenteDescripcion,
                        pf.Clave as ProgramaFondoClave,
                        pf.Descripcion as ProgramaFondoDescripcion,
                        
                        -- NUEVAS DESCRIPCIONES
                        tmf.Clave as TipoMinistracionFolioClave,
                        tmf.Descripcion as TipoMinistracionFolioDescripcion,
                        tr.Clave as TipoRecursoClave,
                        tr.Descripcion as TipoRecursoDescripcion,
                        sf.Clave as SubFuenteClave,
                        sf.Descripcion as SubFuenteDescripcion,
                        pe.Clave as ProyectoEstrategicoClave,
                        pe.Descripcion as ProyectoEstrategicoDescripcion,
                        r.Clave as RamoClave,
                        r.Descripcion as RamoDescripcion,
                        
                        -- Programa fondo (existente)
                        ctr.Clave as ProgramaFondoTR,
                        cff.Clave as ProgramaFondoFF,
                        cr.Clave as ProgramaFondoRamo
                        
                    FROM ministraciones m
                    LEFT JOIN cat_digitosministracion dm ON m.id_digito_ministracion = dm.Id
                    LEFT JOIN cat_tipoministracion tm ON m.id_tipo_ministracion = tm.Id
                    LEFT JOIN cat_programafondo pf ON m.id_programa_fondo = pf.Id
                    LEFT JOIN cat_fuentefinanciamiento ff ON m.id_fuente_financiamiento = ff.Id
                    
                    -- NUEVOS JOINS
                    LEFT JOIN cat_tipoministracionfolio tmf ON m.id_tipoministracion_folio = tmf.Id
                    LEFT JOIN cat_tiporecurso tr ON m.id_tipo_recurso = tr.Id
                    LEFT JOIN cat_subfuentefinanciamiento sf ON m.Id_sub_fuente_financiamiento = sf.Id
                    LEFT JOIN cat_proyectoestrategico pe ON m.Id_proyecto_estrategico = pe.Id
                    LEFT JOIN cat_ramo r ON m.Id_ramo = r.Id
                    
                    LEFT JOIN cat_tiporecurso ctr ON pf.TipoRecursoId = ctr.Id 
                    LEFT JOIN cat_fuentefinanciamiento cff ON pf.FuenteFinanciamientoId = cff.Id 
                    LEFT JOIN cat_ramo cr ON pf.RamoId = cr.Id
                    
                    WHERE m.Id = @id
                ";

                try
                {
                    var resultado = con.QueryFirstOrDefault<ModelMinistracionCompleta>(sql, new { id });
                    if (resultado != null)
                    {
                        var columnaMes = ObtenerNombreColumnaMes(resultado.Mes);
                        var claves = resultado.ClavePresupuestal?.Split(',').Select(c => c.Trim()).Where(c => !string.IsNullOrEmpty(c)).ToArray();
                        if (claves != null && claves.Length > 0)
                        {
                            var sqlPresupuesto = $@"
                                SELECT COALESCE(SUM({columnaMes}), 0) 
                                FROM presupuestocalendarizado pc 
                                WHERE pc.ClavePresupuestal IN (SELECT LTRIM(RTRIM(value)) FROM STRING_SPLIT(@claves, ','))
                            ";
                            resultado.PresupuestoCalendarizado = con.QueryFirstOrDefault<decimal>(sqlPresupuesto, new { claves = string.Join(",", claves) });
                        }
                        else
                        {
                            resultado.PresupuestoCalendarizado = 0;
                        }

                        resultado.numministracion = resultado.Folio;
                        resultado.tipoministracion = resultado.TipoDescripcion ?? "N/A";
                        resultado.importe = resultado.ImporteTotal;
                        resultado.enlace = resultado.ClavePresupuestal;
                        resultado.numeromes = resultado.Mes;
                        resultado.estatus = resultado.Estatus ?? "REGISTRADA";
                        resultado.anio = resultado.Anio;
                        resultado.concepto = resultado.Concepto ?? "";
                        resultado.fuenteFinanciamiento = resultado.FuenteDescripcion ?? "N/A";
                        resultado.fuenteFinanciamientoId = resultado.FuenteFinanciamientoId;
                        resultado.programaFondoDescripcion = resultado.ProgramaFondoDescripcion ?? "N/A";
                        resultado.tienePDF = false;
                        resultado.nombremes = ObtenerNombreMes(resultado.Mes);
                        resultado.fecharegistro = resultado.FechaCreacion.ToString("dd/MM/yyyy");
                        resultado.digitoClave = resultado.DigitoClave;
                        resultado.digitoDescripcion = resultado.DigitoDescripcion;
                        resultado.digitoMinistracion = resultado.DigitoDescripcion;
                        resultado.digitoDM = resultado.DigitoClave;
                        resultado.tipoClave = resultado.TipoClave;
                    }
                    return resultado;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en GetMinistracionById: " + ex.Message);
                    throw;
                }
            }
        }

        // =============================================
        // DETALLE COMPLETO DE MINISTRACIÓN (ACTUALIZADA)
        // =============================================

        public ModelMinistracionDetalleCompleto GetMinistracionDetalleCompleto(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT 
                        m.Id,
                        m.Folio,
                        m.PresupuestoCalendarizadoId,
                        m.ClavePresupuestal,
                        m.Mes,
                        m.Anio,
                        m.Importe_total as ImporteTotal,
                        m.Concepto,
                        m.Estatus,
                        m.FechaCreacion,
                        m.UsuarioCreacion,
                        
                        -- Campos existentes
                        m.id_digito_ministracion as DigitoMinistracionId,
                        m.id_tipo_ministracion as TipoMinistracionId,
                        m.id_programa_fondo as ProgramaFondoId,
                        m.id_fuente_financiamiento as FuenteFinanciamientoId,
                        
                        -- NUEVOS CAMPOS
                        m.id_tipoministracion_folio,
                        m.id_tipo_recurso,
                        m.Id_sub_fuente_financiamiento,
                        m.Id_proyecto_estrategico,
                        m.Id_ramo,
                        
                        -- Descripciones existentes
                        dm.Clave as DigitoClave,
                        dm.Descripcion as DigitoDescripcion,
                        tm.Clave as TipoClave,
                        tm.Descripcion as TipoDescripcion,
                        ff.Clave as FuenteClave,
                        ff.Descripcion as FuenteDescripcion,
                        pf.Clave as ProgramaFondoClave,
                        pf.Descripcion as ProgramaFondoDescripcion,
                        
                        -- NUEVAS DESCRIPCIONES
                        tmf.Clave as TipoMinistracionFolioClave,
                        tmf.Descripcion as TipoMinistracionFolioDescripcion,
                        tr.Clave as TipoRecursoClave,
                        tr.Descripcion as TipoRecursoDescripcion,
                        sf.Clave as SubFuenteClave,
                        sf.Descripcion as SubFuenteDescripcion,
                        pe.Clave as ProyectoEstrategicoClave,
                        pe.Descripcion as ProyectoEstrategicoDescripcion,
                        r.Clave as RamoClave,
                        r.Descripcion as RamoDescripcion,
                        
                        -- Programa fondo (existente)
                        ctr.Clave as ProgramaFondoTR,
                        cff.Clave as ProgramaFondoFF,
                        cr.Clave as ProgramaFondoRamo,
                        
                        -- Tipo recurso descripción (para etiquetado)
                        CASE WHEN tr.Clave = '2' THEN 'Etiquetado' ELSE 'No Etiquetado' END as TipoRecursoDescripcion
                        
                    FROM ministraciones m
                    LEFT JOIN cat_digitosministracion dm ON m.id_digito_ministracion = dm.Id
                    LEFT JOIN cat_tipoministracion tm ON m.id_tipo_ministracion = tm.Id
                    LEFT JOIN cat_programafondo pf ON m.id_programa_fondo = pf.Id
                    LEFT JOIN cat_fuentefinanciamiento ff ON m.id_fuente_financiamiento = ff.Id
                    
                    -- NUEVOS JOINS
                    LEFT JOIN cat_tipoministracionfolio tmf ON m.id_tipoministracion_folio = tmf.Id
                    LEFT JOIN cat_tiporecurso tr ON m.id_tipo_recurso = tr.Id
                    LEFT JOIN cat_subfuentefinanciamiento sf ON m.Id_sub_fuente_financiamiento = sf.Id
                    LEFT JOIN cat_proyectoestrategico pe ON m.Id_proyecto_estrategico = pe.Id
                    LEFT JOIN cat_ramo r ON m.Id_ramo = r.Id
                    
                    LEFT JOIN cat_tiporecurso ctr ON pf.TipoRecursoId = ctr.Id 
                    LEFT JOIN cat_fuentefinanciamiento cff ON pf.FuenteFinanciamientoId = cff.Id 
                    LEFT JOIN cat_ramo cr ON pf.RamoId = cr.Id
                    
                    WHERE m.Id = @id
                ";

                try
                {
                    var resultado = con.QueryFirstOrDefault<ModelMinistracionDetalleCompleto>(sql, new { id });
                    if (resultado != null)
                    {
                        var columnaMes = ObtenerNombreColumnaMes(resultado.Mes);
                        var claves = resultado.ClavePresupuestal?.Split(',').Select(c => c.Trim()).Where(c => !string.IsNullOrEmpty(c)).ToArray();
                        if (claves != null && claves.Length > 0)
                        {
                            var sqlPresupuesto = $@"
                                SELECT COALESCE(SUM({columnaMes}), 0) 
                                FROM presupuestocalendarizado pc 
                                WHERE pc.ClavePresupuestal IN (SELECT LTRIM(RTRIM(value)) FROM STRING_SPLIT(@claves, ','))
                            ";
                            resultado.PresupuestoCalendarizado = con.QueryFirstOrDefault<decimal>(sqlPresupuesto, new { claves = string.Join(",", claves) });
                        }
                        else
                        {
                            resultado.PresupuestoCalendarizado = 0;
                        }

                        resultado.numministracion = resultado.Folio;
                        resultado.tipoministracion = resultado.TipoDescripcion ?? "N/A";
                        resultado.importe = resultado.ImporteTotal;
                        resultado.enlace = resultado.ClavePresupuestal;
                        resultado.numeromes = resultado.Mes;
                        resultado.estatus = resultado.Estatus ?? "REGISTRADA";
                        resultado.anio = resultado.Anio;
                        resultado.concepto = resultado.Concepto ?? "";
                        resultado.fuenteFinanciamiento = resultado.FuenteDescripcion ?? "N/A";
                        resultado.fuenteFinanciamientoId = resultado.FuenteFinanciamientoId;
                        resultado.programaFondoDescripcion = resultado.ProgramaFondoDescripcion ?? "N/A";
                        resultado.tienePDF = false;
                        resultado.nombremes = ObtenerNombreMes(resultado.Mes);
                        resultado.fecharegistro = resultado.FechaCreacion.ToString("dd/MM/yyyy");
                        resultado.DigitoMinistracion = resultado.DigitoDescripcion ?? "N/A";
                        resultado.digitoClave = resultado.DigitoClave;
                        resultado.digitoDescripcion = resultado.DigitoDescripcion;
                        resultado.digitoDM = resultado.DigitoClave;
                        resultado.tipoClave = resultado.TipoClave;

                        var sqlClaves = @"
                            SELECT TOP 10 * FROM (
                            SELECT DISTINCT
                                pc.ClavePresupuestal,
                                '' as Descripcion,
                                pc.Aprobado as Monto,
                                '' as Nombremes,
                                NULL as Mes
                            FROM presupuestocalendarizado pc
                            WHERE pc.ClavePresupuestal = @clavePresupuestal
                            UNION ALL
                            SELECT DISTINCT
                                m2.ClavePresupuestal,
                                m2.Concepto as Descripcion,
                                m2.Importe_total as Monto,
                                '' as Nombremes,
                                m2.Mes
                            FROM ministraciones m2
                            WHERE m2.ClavePresupuestal = @clavePresupuestal
                              AND m2.Id != @id
                              AND m2.Estatus != 'CANCELADA'
                            ) x
                            ORDER BY Mes DESC
                        ";

                        resultado.ClavesRelacionadas = con.Query<ModelClaveRelacionada>(sqlClaves, new
                        {
                            clavePresupuestal = resultado.ClavePresupuestal,
                            id
                        }).ToList();

                        if (claves != null && claves.Length > 0)
                        {
                            var sqlDesglose = $@"
                                SELECT 
                                    pc.ClavePresupuestal,
                                    '' as Descripcion,
                                    COALESCE({columnaMes}, 0) as Monto,
                                    '' as Nombremes,
                                    0 as Mes
                                FROM presupuestocalendarizado pc
                                WHERE pc.ClavePresupuestal IN (SELECT LTRIM(RTRIM(value)) FROM STRING_SPLIT(@claves, ','))
                            ";
                            resultado.DesgloseClaves = con.Query<ModelClaveRelacionada>(sqlDesglose, new { claves = string.Join(",", claves) }).ToList();
                        }
                        else
                        {
                            resultado.DesgloseClaves = new List<ModelClaveRelacionada>();
                        }

                        resultado.SumatoriaClaves = resultado.ClavesRelacionadas?.Sum(c => c.Monto) ?? 0;
                        resultado.ClavesRelacionadas = resultado.ClavesRelacionadas ?? new List<ModelClaveRelacionada>();
                        resultado.Cancelaciones = new List<ModelCancelacionMinistracion>();
                        resultado.MinistracionesConjuntas = new List<ModelMinistracionConjunta>();
                        resultado.Movimientos = new List<ModelMovimientoAdecuacion>();
                    }
                    return resultado;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en GetMinistracionDetalleCompleto: " + ex.Message);
                    throw;
                }
            }
        }

        public ModelMinistracionDetalleCompleto GetMinistracionDetalle(int id)
        {
            return GetMinistracionDetalleCompleto(id);
        }

        // =============================================
        // COMPARACIÓN POR MES
        // =============================================

        public object GetComparacionMes(int id, int mes, int anio)
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    var columna = ObtenerNombreColumnaMes(mes);

                    var sqlClaves = $@"
                        SELECT DISTINCT
                            pc.ClavePresupuestal as Clave,
                            '' as Descripcion,
                            COALESCE(pc.{columna}, 0) as Monto
                        FROM presupuestocalendarizado pc
                        WHERE pc.ClavePresupuestal IS NOT NULL 
                          AND pc.ClavePresupuestal != ''
                          AND pc.ClavePresupuestal != 'Total general'
                        ORDER BY pc.ClavePresupuestal
                    ";

                    var claves = con.Query<dynamic>(sqlClaves).ToList();

                    var sqlMinistraciones = @"
                        SELECT 
                            m.ClavePresupuestal as Clave,
                            m.Concepto as Descripcion,
                            SUM(m.Importe_total) as Monto
                        FROM ministraciones m
                        WHERE m.Mes = @mes 
                          AND m.Anio = @anio
                          AND m.Estatus != 'CANCELADA'
                          AND m.ClavePresupuestal IS NOT NULL 
                          AND m.ClavePresupuestal != ''
                          AND m.ClavePresupuestal != 'Total general'
                        GROUP BY m.ClavePresupuestal, m.Concepto
                        ORDER BY m.ClavePresupuestal
                    ";

                    var ministraciones = con.Query<dynamic>(sqlMinistraciones, new { mes, anio }).ToList();

                    var totalPresupuesto = 0m;
                    var totalMinistrado = 0m;
                    var resultado = new List<dynamic>();

                    var dictMinistraciones = new Dictionary<string, dynamic>();
                    foreach (var min in ministraciones)
                    {
                        var claveStr = (string)min.Clave;
                        if (!dictMinistraciones.ContainsKey(claveStr))
                        {
                            dictMinistraciones[claveStr] = new
                            {
                                Monto = (decimal)min.Monto,
                                Descripcion = (string)min.Descripcion ?? "N/A"
                            };
                        }
                    }

                    foreach (var clave in claves)
                    {
                        var claveStr = (string)clave.Clave;
                        var montoPresupuesto = (decimal)clave.Monto;
                        var montoMinistrado = dictMinistraciones.ContainsKey(claveStr)
                            ? dictMinistraciones[claveStr].Monto
                            : 0m;
                        var descripcion = dictMinistraciones.ContainsKey(claveStr)
                            ? dictMinistraciones[claveStr].Descripcion
                            : "N/A";

                        totalPresupuesto += montoPresupuesto;
                        totalMinistrado += montoMinistrado;

                        resultado.Add(new
                        {
                            Clave = claveStr,
                            Descripcion = descripcion,
                            Presupuesto = montoPresupuesto,
                            Ministrado = montoMinistrado,
                            Diferencia = montoPresupuesto - montoMinistrado
                        });
                    }

                    foreach (var min in ministraciones)
                    {
                        var claveStr = (string)min.Clave;
                        if (!resultado.Any(r => (string)r.Clave == claveStr))
                        {
                            totalMinistrado += (decimal)min.Monto;
                            resultado.Add(new
                            {
                                Clave = claveStr,
                                Descripcion = (string)min.Descripcion ?? "N/A",
                                Presupuesto = 0m,
                                Ministrado = (decimal)min.Monto,
                                Diferencia = -(decimal)min.Monto
                            });
                        }
                    }

                    var clavesFinal = resultado.Select(r => new
                    {
                        Clave = (string)r.Clave,
                        Descripcion = (string)r.Descripcion,
                        Ministrado = (decimal)r.Ministrado,
                        Monto = (decimal)r.Presupuesto,
                        Mes = mes,
                        Anio = anio
                    }).ToList();

                    return new
                    {
                        mes = mes,
                        anio = anio,
                        nombremes = ObtenerNombreMes(mes),
                        claves = clavesFinal,
                        totalPresupuesto = totalPresupuesto,
                        totalMinistrado = totalMinistrado,
                        diferencia = totalPresupuesto - totalMinistrado
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR en GetComparacionMes: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("StackTrace: " + ex.StackTrace);

                return new
                {
                    error = true,
                    mensaje = ex.Message,
                    stackTrace = ex.StackTrace,
                    mes = mes,
                    anio = anio,
                    claves = new List<dynamic>(),
                    totalPresupuesto = 0m,
                    totalMinistrado = 0m,
                    diferencia = 0m
                };
            }
        }

        // =============================================
        // GUARDAR MINISTRACIÓN (ACTUALIZADA)
        // =============================================

        public int GuardarMinistracion(ModelMinistracionGuardar model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    decimal importeTotal = 0;

                    try
                    {
                        importeTotal = decimal.Parse(model.importeTotal?.Replace(",", "").Replace("$", "").Trim() ?? "0");
                    }
                    catch { importeTotal = 0; }

                    if (model.programaFondoId.HasValue && model.programaFondoId.Value > 0)
                    {
                        var pf = con.QueryFirstOrDefault<ModelProgramaFondo>(
                            "SELECT TipoRecursoId, FuenteFinanciamientoId, RamoId FROM cat_programafondo WHERE Id = @Id", 
                            new { Id = model.programaFondoId.Value },
                            transaction: transaction
                        );
                        if (pf != null)
                        {
                            model.id_tipo_recurso = model.id_tipo_recurso ?? pf.TipoRecursoId;
                            model.fuenteFinanciamientoId = model.fuenteFinanciamientoId ?? pf.FuenteFinanciamientoId;
                            model.Id_ramo = model.Id_ramo ?? pf.RamoId;
                        }
                    }

                    if ((model.presupuestoCalendarizadoId == null || model.presupuestoCalendarizadoId == 0) && !string.IsNullOrEmpty(model.clavePresupuestal))
                    {
                        var primerClave = model.clavePresupuestal.Split(',').Select(c => c.Trim()).FirstOrDefault(c => !string.IsNullOrEmpty(c));
                        if (!string.IsNullOrEmpty(primerClave))
                        {
                            var pcId = con.QueryFirstOrDefault<int?>(
                                "SELECT TOP 1 Id FROM presupuestocalendarizado WHERE ClavePresupuestal = @Clave",
                                new { Clave = primerClave },
                                transaction: transaction
                            );
                            if (pcId.HasValue)
                            {
                                model.presupuestoCalendarizadoId = pcId.Value;
                            }
                        }
                    }

                    if (model.accion == "NUEVO")
                    {
                        string folio = model.folio;
                        if (string.IsNullOrEmpty(folio))
                        {
                            folio = "MM" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1, 9999).ToString("D4");
                        }

                        string sql = @"
                            INSERT INTO ministraciones 
                            (
                                Folio, 
                                PresupuestoCalendarizadoId, 
                                id_programa_fondo, 
                                id_digito_ministracion, 
                                id_tipo_ministracion,
                                id_fuente_financiamiento, 
                                ClavePresupuestal, 
                                Mes, 
                                Anio, 
                                Importe_total,
                                Concepto, 
                                Estatus, 
                                UsuarioCreacion, 
                                FechaCreacion,
                                
                                -- NUEVOS CAMPOS
                                id_tipoministracion_folio,
                                id_tipo_recurso,
                                Id_sub_fuente_financiamiento,
                                Id_proyecto_estrategico,
                                Id_ramo
                            )
                            VALUES 
                            (
                                @Folio, 
                                @PresupuestoCalendarizadoId, 
                                @ProgramaFondoId, 
                                @DigitoMinistracionId, 
                                @TipoMinistracionId,
                                @FuenteFinanciamientoId, 
                                @ClavePresupuestal, 
                                @MesAplicacion, 
                                @Anio, 
                                @ImporteTotal,
                                @Concepto, 
                                @Estatus, 
                                @UsuarioCreacion, 
                                GETDATE(),
                                
                                -- NUEVOS VALORES
                                @id_tipoministracion_folio,
                                @id_tipo_recurso,
                                @Id_sub_fuente_financiamiento,
                                @Id_proyecto_estrategico,
                                @Id_ramo
                            );
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                        ";

                        var id = con.QueryFirst<int>(sql, new
                        {
                            Folio = folio,
                            PresupuestoCalendarizadoId = model.presupuestoCalendarizadoId,
                            ProgramaFondoId = model.programaFondoId,
                            DigitoMinistracionId = model.digitoMinistracionId,
                            TipoMinistracionId = model.tipoMinistracionId,
                            FuenteFinanciamientoId = model.fuenteFinanciamientoId,
                            ClavePresupuestal = model.clavePresupuestal ?? "",
                            MesAplicacion = model.mesAplicacion,
                            Anio = model.anio > 0 ? model.anio : DateTime.Now.Year,
                            ImporteTotal = importeTotal,
                            Concepto = model.concepto ?? "",
                            Estatus = model.estatus ?? "REGISTRADA",
                            UsuarioCreacion = "SISTEMA",

                            // NUEVOS PARÁMETROS
                            id_tipoministracion_folio = model.id_tipoministracion_folio,
                            id_tipo_recurso = model.id_tipo_recurso,
                            Id_sub_fuente_financiamiento = model.Id_sub_fuente_financiamiento,
                            Id_proyecto_estrategico = model.Id_proyecto_estrategico,
                            Id_ramo = model.Id_ramo
                        }, transaction);

                        transaction.Commit();
                        return id;
                    }
                    else if (model.accion == "EDITAR")
                    {
                        string sql = @"
                            UPDATE ministraciones SET
                                PresupuestoCalendarizadoId = @PresupuestoCalendarizadoId,
                                id_programa_fondo = @ProgramaFondoId,
                                id_digito_ministracion = @DigitoMinistracionId,
                                id_tipo_ministracion = @TipoMinistracionId,
                                id_fuente_financiamiento = @FuenteFinanciamientoId,
                                ClavePresupuestal = @ClavePresupuestal,
                                Mes = @MesAplicacion,
                                Anio = @Anio,
                                Importe_total = @ImporteTotal,
                                Concepto = @Concepto,
                                Estatus = @Estatus,
                                FechaModificacion = GETDATE(),
                                
                                -- NUEVOS CAMPOS
                                id_tipoministracion_folio = @id_tipoministracion_folio,
                                id_tipo_recurso = @id_tipo_recurso,
                                Id_sub_fuente_financiamiento = @Id_sub_fuente_financiamiento,
                                Id_proyecto_estrategico = @Id_proyecto_estrategico,
                                Id_ramo = @Id_ramo
                            WHERE Id = @id
                        ";

                        con.Execute(sql, new
                        {
                            id = model.id,
                            PresupuestoCalendarizadoId = model.presupuestoCalendarizadoId,
                            ProgramaFondoId = model.programaFondoId,
                            DigitoMinistracionId = model.digitoMinistracionId,
                            TipoMinistracionId = model.tipoMinistracionId,
                            FuenteFinanciamientoId = model.fuenteFinanciamientoId,
                            ClavePresupuestal = model.clavePresupuestal ?? "",
                            MesAplicacion = model.mesAplicacion,
                            Anio = model.anio > 0 ? model.anio : DateTime.Now.Year,
                            ImporteTotal = importeTotal,
                            Concepto = model.concepto ?? "",
                            Estatus = model.estatus ?? "REGISTRADA",

                            // NUEVOS PARÁMETROS
                            id_tipoministracion_folio = model.id_tipoministracion_folio,
                            id_tipo_recurso = model.id_tipo_recurso,
                            Id_sub_fuente_financiamiento = model.Id_sub_fuente_financiamiento,
                            Id_proyecto_estrategico = model.Id_proyecto_estrategico,
                            Id_ramo = model.Id_ramo
                        }, transaction);

                        transaction.Commit();
                        return model.id;
                    }

                    return 0;
                }
            }
        }

        // =============================================
        // CANCELAR MINISTRACIÓN
        // =============================================

        public bool CancelarMinistracion(int id, string motivo, decimal monto)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    var sqlCheck = @"
                        SELECT Id, Estatus, Mes, Importe_total as MontoAutorizado 
                        FROM ministraciones 
                        WHERE Id = @id
                    ";

                    var ministracion = con.QueryFirstOrDefault(sqlCheck, new { id }, transaction);

                    if (ministracion == null)
                        return false;

                    if (ministracion.Estatus != "REGISTRADA" && ministracion.Estatus != "INICIADO")
                        return false;

                    var montoAutorizado = Convert.ToDecimal(ministracion.MontoAutorizado);
                    if (monto > montoAutorizado)
                        return false;

                    string folioCancelacion = "CAN" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1, 9999).ToString("D4");

                    string sqlInsert = @"
                        INSERT INTO cancelaciones_ministraciones 
                        (MinistracionId, FolioCancelacion, MontoCancelado, MotivoConfirmacion, UsuarioCancela)
                        VALUES 
                        (@MinistracionId, @FolioCancelacion, @MontoCancelado, @MotivoConfirmacion, @UsuarioCancela)
                    ";

                    con.Execute(sqlInsert, new
                    {
                        MinistracionId = id,
                        FolioCancelacion = folioCancelacion,
                        MontoCancelado = monto,
                        MotivoConfirmacion = motivo,
                        UsuarioCancela = "SISTEMA"
                    }, transaction);

                    string sqlUpdate = @"
                        UPDATE ministraciones 
                        SET Estatus = 'CANCELADA' 
                        WHERE Id = @id
                    ";

                    con.Execute(sqlUpdate, new { id }, transaction);

                    transaction.Commit();
                    return true;
                }
            }
        }

        // =============================================
        // RECURSOS POR MES Y FONDO
        // =============================================

        public ModelRecursosPorMesFondo GetRecursosPorMesYFondo(int mes, int programaFondoId)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var sqlFondo = @"
                    SELECT Id, Clave, Descripcion 
                    FROM cat_programafondo 
                    WHERE Id = @programaFondoId
                ";

                var fondo = con.QueryFirstOrDefault(sqlFondo, new { programaFondoId });

                if (fondo == null)
                    return null;

                var sqlMinistraciones = @"
                    SELECT 
                        Id,
                        Folio,
                        ClavePresupuestal,
                        Importe_total as Monto,
                        Estatus
                    FROM ministraciones
                    WHERE Mes = @mes 
                      AND id_programa_fondo = @programaFondoId
                      AND Estatus != 'CANCELADA'
                    ORDER BY Id DESC
                ";

                var ministraciones = con.Query<ModelMinistracionResumen>(sqlMinistraciones, new { mes, programaFondoId }).ToList();
                var total = ministraciones.Sum(m => m.Monto);

                string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                   "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                string nombreMes = mes >= 1 && mes <= 12 ? meses[mes - 1] : "N/A";

                return new ModelRecursosPorMesFondo
                {
                    Mes = mes,
                    Nombremes = nombreMes,
                    ProgramaFondoId = programaFondoId,
                    FondoDescripcion = fondo.Descripcion,
                    Total = total,
                    Ministraciones = ministraciones
                };
            }
        }

        public bool EliminarMinistracion(int id)
        {
            return false;
        }

        // =============================================
        // APROBAR MINISTRACIÓN
        // =============================================

        public bool AprobarMinistracion(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = "UPDATE ministraciones SET Estatus = 'INICIADO' WHERE Id = @id AND Estatus = 'REGISTRADA'";
                return con.Execute(sql, new { id }) > 0;
            }
        }

        // =============================================
        // ESTADÍSTICAS COMPLETAS (ACTUALIZADA)
        // =============================================

        public ModelEstadisticasMinistraciones GetEstadisticas()
        {
            using (var con = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT 
                        COUNT(*) as Total,
                        SUM(CASE WHEN Estatus = 'REGISTRADA' THEN 1 ELSE 0 END) as Registradas,
                        SUM(CASE WHEN Estatus = 'INICIADO' THEN 1 ELSE 0 END) as Iniciados,
                        SUM(CASE WHEN Estatus = 'CANCELADA' THEN 1 ELSE 0 END) as Canceladas,
                        SUM(Importe_total) as TotalAutorizado,
                        0 as TotalSolicitado,
                        SUM(CASE WHEN id_tipo_recurso = 2 THEN Importe_total ELSE 0 END) as TotalEtiquetado,
                        SUM(CASE WHEN id_tipo_recurso != 2 OR id_tipo_recurso IS NULL THEN Importe_total ELSE 0 END) as TotalNoEtiquetado,
                        @MesActual as MesActual,
                        SUM(CASE WHEN Mes = @MesActual AND Anio = @AnioActual THEN 1 ELSE 0 END) as TotalMesActual,
                        SUM(CASE WHEN Mes = @MesAnterior AND Anio = @AnioActual THEN 1 ELSE 0 END) as TotalMesAnterior,
                        (SELECT COALESCE(SUM(Aprobado), 0) FROM presupuestocalendarizado) as TotalPresupuesto
                    FROM ministraciones
                ";

                var mesActual = DateTime.Now.Month;
                var anioActual = DateTime.Now.Year;
                var mesAnterior = mesActual == 1 ? 12 : mesActual - 1;

                try
                {
                    return con.QueryFirst<ModelEstadisticasMinistraciones>(sql, new
                    {
                        MesActual = mesActual,
                        AnioActual = anioActual,
                        MesAnterior = mesAnterior
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en GetEstadisticas: " + ex.Message);
                    return new ModelEstadisticasMinistraciones();
                }
            }
        }
    }
}