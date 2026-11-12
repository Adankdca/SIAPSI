using System;
using System.Data;
using MySql.Data.MySqlClient;

class Program {
    static void Main() {
        string connStr = "Server=localhost;Database=SIAP_DB;Uid=root;Pwd=root;";
        using (var conn = new MySqlConnection(connStr)) {
            conn.Open();
            using (var cmd = new MySqlCommand("SELECT * FROM cat_tipoadecuacion", conn)) {
                using (var reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        Console.WriteLine(string.Format("{0} - {1} - {2}", reader["Id"], reader["Clave"], reader["Descripcion"]));
                    }
                }
            }
        }
    }
}
