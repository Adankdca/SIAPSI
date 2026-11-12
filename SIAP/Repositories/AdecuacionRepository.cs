using System;
using System.Data;
using Dapper;
using SIAP.Models;

namespace SIAP.Repositories
{
    public class AdecuacionRepository
    {
        /// <summary>
        /// Guarda una adecuación presupuestal y todos sus movimientos (ampliaciones/reducciones) asociados dentro de una transacción de base de datos.
        /// </summary>
        /// <remarks>
        /// Este método es fundamental para mantener la integridad referencial y de datos del módulo de Adecuaciones.
        /// 
        /// Flujo de la operación:
        /// 1. Abre la conexión a la base de datos y comienza una transacción (BeginTransaction).
        /// 2. Inserta el registro cabecera en la tabla <c>Adecuaciones</c>, incluyendo el número de oficio, fecha, tipo y monto total.
        /// 3. Obtiene el ID autogenerado de la cabecera (LAST_INSERT_ID).
        /// 4. Itera sobre la colección de <c>Movimientos</c> de la adecuación. Para cada movimiento (que representa un aumento o disminución mensual en una clave específica), asigna el <c>AdecuacionId</c> e inserta el registro en la tabla <c>AdecuacionMovimientos</c>.
        /// 5. Si todas las inserciones son exitosas, confirma la transacción (Commit) haciendo los cambios permanentes.
        /// 6. Si se produce alguna excepción durante el proceso (ej. error de clave, fallo de red), se revierte la transacción (Rollback), deshaciendo todas las inserciones previas y garantizando que no queden datos huérfanos.
        /// </remarks>
        /// <param name="adecuacion">Objeto de tipo <see cref="Adecuacion"/> que contiene los datos principales y la lista de movimientos a guardar.</param>
        /// <returns>Devuelve el ID entero de la adecuación generada en la base de datos.</returns>
        /// <exception cref="Exception">Se lanza si hay un problema al ejecutar los queries SQL. La excepción original es capturada para hacer Rollback y luego se relanza con un mensaje contextual.</exception>
        public int GuardarAdecuacionConTransaccion(Adecuacion adecuacion)
        {
            using (IDbConnection db = DbConnectionFactory.GetConnection())
            {
                db.Open();
                // Iniciamos la transacción para asegurar que si algo falla, no se guarde nada a medias.
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar la cabecera
                        string sqlCabecera = @"
                            INSERT INTO Adecuaciones 
                            (NumOficio, NumSolicitud, Asunto, TipoAdecuacion, FechaSolicitud, MontoTotal, Estatus, FechaCreacion) 
                            VALUES 
                            (@NumOficio, @NumSolicitud, @Asunto, @TipoAdecuacion, @FechaSolicitud, @MontoTotal, @Estatus, @FechaCreacion);
                            SELECT LAST_INSERT_ID();";

                        adecuacion.FechaCreacion = DateTime.Now;
                        int adecuacionId = db.QuerySingle<int>(sqlCabecera, adecuacion, transaction: transaction);

                        // 2. Insertar el detalle (los movimientos de Aumento y Disminución)
                        string sqlMovimiento = @"
                            INSERT INTO AdecuacionMovimientos 
                            (AdecuacionId, TipoOperacion, ClavePresupuestal, 
                             Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre) 
                            VALUES 
                            (@AdecuacionId, @TipoOperacion, @ClavePresupuestal, 
                             @Enero, @Febrero, @Marzo, @Abril, @Mayo, @Junio, @Julio, @Agosto, @Septiembre, @Octubre, @Noviembre, @Diciembre);";

                        foreach (var mov in adecuacion.Movimientos)
                        {
                            mov.AdecuacionId = adecuacionId;
                            db.Execute(sqlMovimiento, mov, transaction: transaction);
                        }

                        // Si todo sale bien, confirmamos la transacción (Guardamos en base de datos)
                        transaction.Commit();
                        return adecuacionId;
                    }
                    catch (Exception ex)
                    {
                        // Si ocurre cualquier error, revertimos todo
                        transaction.Rollback();
                        throw new Exception("Error al guardar la adecuación: " + ex.Message);
                    }
                }
            }
        }
    }
}
