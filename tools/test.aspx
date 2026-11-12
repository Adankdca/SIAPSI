<%@ Page Language="C#" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="MySql.Data.MySqlClient" %>
<%
    string connStr = "Server=localhost;Database=siap;Uid=root;Pwd=;";
    using (var conn = new MySqlConnection(connStr))
    {
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT ClavePresupuestal, Aprobado, Aumentos, Disminuciones, Pagado FROM presupuestocalendarizado WHERE ClavePresupuestal LIKE '%010012211H04501T11A001000A1B113110025115S28C001081120252%' LIMIT 1";
        using (var reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                Response.Write("Aprobado: " + reader.GetDecimal(1) + ", Aumentos: " + reader.GetDecimal(2) + ", Disminuciones: " + reader.GetDecimal(3));
            }
            else
            {
                Response.Write("No found");
            }
        }
    }
%>
