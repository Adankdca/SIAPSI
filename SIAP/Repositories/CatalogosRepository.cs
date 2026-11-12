using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace SIAP.Repositories
{
    public class ColumnSchema
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public int IsPrimaryKey { get; set; }
        public int IsForeignKey { get; set; }
        public string ReferencedTable { get; set; }
        public string ReferencedColumn { get; set; }
    }

    /// <summary>
    /// Repositorio genérico para administrar todas las tablas maestras de catálogos.
    /// Utiliza reflexión de base de datos (`information_schema`) para descubrir columnas dinámicamente,
    /// lo que permite leer, insertar y actualizar cualquier tabla que comience con 'cat_'.
    /// </summary>
    public class CatalogosRepository
    {
        private readonly string _connectionString;

        public CatalogosRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["BDLOCALConnectionString"].ConnectionString;
        }

        private IDbConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        /// <summary>
        /// Filtro de seguridad crítico. Valida que el nombre de la tabla inicie con 'cat_'
        /// y contenga solo caracteres alfanuméricos. 
        /// Esto previene ataques de inyección SQL como 'DROP TABLE usuarios'.
        /// </summary>
        private bool IsValidCatalog(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName)) return false;
            if (!tableName.StartsWith("cat_")) return false;
            // Prevent SQL injection by strictly allowing only alphanumeric and underscore
            if (!tableName.All(c => char.IsLetterOrDigit(c) || c == '_')) return false;
            return true;
        }

        public List<string> GetCatalogs()
        {
            using (var db = GetConnection())
            {
                return db.Query<string>("SHOW TABLES LIKE 'cat_%'").ToList();
            }
        }

        /// <summary>
        /// Obtiene el esquema dinámico de la tabla (Nombres de columnas, Tipos de datos y Llaves).
        /// Consulta directamente el information_schema de MySQL. 
        /// Fundamental para que el Front-end sepa qué inputs (texto, número) dibujar en el modal.
        /// </summary>
        public List<ColumnSchema> GetSchema(string tableName)
        {
            if (!IsValidCatalog(tableName)) throw new ArgumentException("Invalid table name");

            using (var db = GetConnection())
            {
                var query = @"
                    SELECT 
                        c.COLUMN_NAME AS ColumnName,
                        c.DATA_TYPE AS DataType,
                        CASE WHEN c.COLUMN_KEY = 'PRI' THEN 1 ELSE 0 END AS IsPrimaryKey,
                        CASE WHEN k.REFERENCED_TABLE_NAME IS NOT NULL THEN 1 ELSE 0 END AS IsForeignKey,
                        k.REFERENCED_TABLE_NAME AS ReferencedTable,
                        k.REFERENCED_COLUMN_NAME AS ReferencedColumn
                    FROM information_schema.COLUMNS c
                    LEFT JOIN information_schema.KEY_COLUMN_USAGE k 
                        ON c.TABLE_NAME = k.TABLE_NAME 
                        AND c.COLUMN_NAME = k.COLUMN_NAME 
                        AND c.TABLE_SCHEMA = k.TABLE_SCHEMA 
                        AND k.REFERENCED_TABLE_NAME IS NOT NULL
                    WHERE c.TABLE_NAME = @TableName 
                    AND c.TABLE_SCHEMA = DATABASE()
                    ORDER BY c.ORDINAL_POSITION;
                ";
                return db.Query<ColumnSchema>(query, new { TableName = tableName }).ToList();
            }
        }

        public IEnumerable<dynamic> GetRecords(string tableName)
        {
            // En lugar de lanzar excepcion y crashear el JS, devolvemos vacio si intenta buscar llaves foraneas fuera de los catálogos
            if (!IsValidCatalog(tableName)) return new List<dynamic>();

            using (var db = GetConnection())
            {
                return db.Query($"SELECT * FROM {tableName}");
            }
        }

        public void SaveRecord(string tableName, Dictionary<string, object> data)
        {
            if (!IsValidCatalog(tableName)) throw new ArgumentException("Invalid table name");

            var schema = GetSchema(tableName);
            var pkColumn = schema.FirstOrDefault(c => c.IsPrimaryKey == 1);
            
            if (pkColumn == null) throw new Exception("Table does not have a primary key.");

            bool isUpdate = false;
            object pkValue = null;

            var parameters = new DynamicParameters();

            if (data.ContainsKey(pkColumn.ColumnName) && data[pkColumn.ColumnName] != null)
            {
                if (int.TryParse(data[pkColumn.ColumnName].ToString(), out int id) && id > 0)
                {
                    isUpdate = true;
                    pkValue = id;
                }
            }

            var columnsToSave = data.Keys.Where(k => k != pkColumn.ColumnName && schema.Any(s => s.ColumnName == k)).ToList();

            foreach(var col in columnsToSave)
            {
                parameters.Add("@" + col, data[col]);
            }

            using (var db = GetConnection())
            {
                if (isUpdate)
                {
                    parameters.Add("@PK", pkValue);
                    var setClauses = string.Join(", ", columnsToSave.Select(c => $"{c} = @{c}"));
                    var query = $"UPDATE {tableName} SET {setClauses} WHERE {pkColumn.ColumnName} = @PK";
                    db.Execute(query, parameters);
                }
                else
                {
                    var cols = string.Join(", ", columnsToSave);
                    var vals = string.Join(", ", columnsToSave.Select(c => "@" + c));
                    var query = $"INSERT INTO {tableName} ({cols}) VALUES ({vals})";
                    db.Execute(query, parameters);
                }
            }
        }
    }
}
