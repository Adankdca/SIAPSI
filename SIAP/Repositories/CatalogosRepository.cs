using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;
using System.Data.SqlClient;

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
    /// Utiliza las vistas de sistema de SQL Server para descubrir columnas dinámicamente,
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
            return new SqlConnection(_connectionString);
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
                // [_] escapa el guion bajo, que en LIKE es comodín
                return db.Query<string>(@"
                    SELECT TABLE_NAME
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = 'dbo' AND TABLE_NAME LIKE 'cat[_]%'
                    ORDER BY TABLE_NAME").ToList();
            }
        }

        /// <summary>
        /// Obtiene el esquema dinámico de la tabla (Nombres de columnas, Tipos de datos y Llaves).
        /// Consulta las vistas de sistema de SQL Server (sys.columns, sys.indexes, sys.foreign_key_columns).
        /// Fundamental para que el Front-end sepa qué inputs (texto, número) dibujar en el modal.
        /// </summary>
        public List<ColumnSchema> GetSchema(string tableName)
        {
            if (!IsValidCatalog(tableName)) throw new ArgumentException("Invalid table name");

            using (var db = GetConnection())
            {
                // DataType se devuelve con los nombres que usaba MySQL (varchar, tinyint, datetime, text)
                // para no cambiar el contrato con el front-end.
                var query = @"
                    SELECT 
                        c.name AS ColumnName,
                        CASE 
                            WHEN t.name IN ('nvarchar','varchar') AND c.max_length = -1 THEN 'text'
                            WHEN t.name = 'nvarchar' THEN 'varchar'
                            WHEN t.name = 'nchar' THEN 'char'
                            WHEN t.name = 'bit' THEN 'tinyint'
                            WHEN t.name IN ('datetime2','smalldatetime') THEN 'datetime'
                            ELSE t.name
                        END AS DataType,
                        CASE WHEN pk.column_id IS NOT NULL THEN 1 ELSE 0 END AS IsPrimaryKey,
                        CASE WHEN fk.parent_column_id IS NOT NULL THEN 1 ELSE 0 END AS IsForeignKey,
                        OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
                        COL_NAME(fk.referenced_object_id, fk.referenced_column_id) AS ReferencedColumn
                    FROM sys.columns c
                    INNER JOIN sys.types t ON t.user_type_id = c.user_type_id
                    OUTER APPLY (
                        SELECT TOP 1 ic.column_id
                        FROM sys.indexes i
                        INNER JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
                        WHERE i.object_id = c.object_id AND i.is_primary_key = 1 AND ic.column_id = c.column_id
                    ) pk
                    OUTER APPLY (
                        SELECT TOP 1 fkc.parent_column_id, fkc.referenced_object_id, fkc.referenced_column_id
                        FROM sys.foreign_key_columns fkc
                        WHERE fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
                    ) fk
                    WHERE c.object_id = OBJECT_ID(N'dbo.' + QUOTENAME(@TableName))
                    ORDER BY c.column_id;
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
                return db.Query($"SELECT * FROM [{tableName}]");
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
                // Un campo vacío del formulario se guarda como NULL (en SQL Server '' a INT da 0 y rompe las llaves foráneas)
                var valor = data[col];
                if (valor is string texto && string.IsNullOrWhiteSpace(texto)) valor = null;
                parameters.Add("@" + col, valor);
            }

            using (var db = GetConnection())
            {
                if (isUpdate)
                {
                    parameters.Add("@PK", pkValue);
                    var setClauses = string.Join(", ", columnsToSave.Select(c => $"[{c}] = @{c}"));
                    var query = $"UPDATE [{tableName}] SET {setClauses} WHERE [{pkColumn.ColumnName}] = @PK";
                    db.Execute(query, parameters);
                }
                else
                {
                    var cols = string.Join(", ", columnsToSave.Select(c => $"[{c}]"));
                    var vals = string.Join(", ", columnsToSave.Select(c => "@" + c));
                    var query = $"INSERT INTO [{tableName}] ({cols}) VALUES ({vals})";
                    db.Execute(query, parameters);
                }
            }
        }
    }
}
