using System;
using System.Data;
using MySql.Data.MySqlClient;

public class Program
{
    public static void Main()
    {
        string connStr = "Server=localhost;Database=SIAP_DB;Uid=root;Pwd=root;";
        using (var conn = new MySqlConnection(connStr))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            var tables = new[] { "cat_region", "cat_municipio", "cat_ramo", "cat_programafondo" };
            foreach (var t in tables)
            {
                Console.WriteLine(string.Format("\n--- {0} ---", t));
                cmd.CommandText = string.Format("SHOW COLUMNS FROM {0}", t);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(string.Format("{0} - {1}", reader.GetString(0), reader.GetString(1)));
                    }
                }
            }
        }
    }
}
