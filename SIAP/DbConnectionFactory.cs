using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace SIAP
{
    public static class DbConnectionFactory
    {
        // Obtiene la cadena de conexión desde el Web.config
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString;

        /// <summary>
        /// Devuelve una nueva conexión a la base de datos MySQL (SIAP_DB).
        /// Usar siempre dentro de un bloque 'using' con Dapper.
        /// </summary>
        /// <returns>IDbConnection abierta o lista para abrirse</returns>
        public static IDbConnection GetConnection()
        {
            var connection = new MySqlConnection(ConnectionString);
            return connection;
        }
    }
}
