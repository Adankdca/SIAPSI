using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SIAP
{
    public static class DbConnectionFactory
    {
        // Obtiene la cadena de conexión desde el Web.config
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString;

        /// <summary>
        /// Devuelve una nueva conexión a la base de datos SQL Server (SIAP_DB).
        /// Usar siempre dentro de un bloque 'using' con Dapper.
        /// </summary>
        /// <returns>IDbConnection lista para abrirse</returns>
        public static IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Igual que GetConnection pero con el tipo concreto, para código que usa SqlTransaction o SqlCommand.
        /// </summary>
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
