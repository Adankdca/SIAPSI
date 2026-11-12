using System;
using System.IO;
using System.Data;
using ExcelDataReader;

class Program {
    static void Main() {
        string filePath = @"C:\Users\itzdx\OneDrive\Documentos\repos\SIAP\SISTEMA EXCEL\CONCENTRADO DE ADECUACIONES 2025 VERTICAL.xls";
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read)) {
            using (var reader = ExcelReaderFactory.CreateReader(stream)) {
                var conf = new ExcelDataSetConfiguration() {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                };
                var dataSet = reader.AsDataSet(conf);
                var table = dataSet.Tables["BASE GENERAL VERTICAL$"];
                if (table == null && dataSet.Tables.Count > 3) table = dataSet.Tables[3];
                if (table == null) {
                    Console.WriteLine("Table not found. Available tables:");
                    foreach(DataTable t in dataSet.Tables) Console.WriteLine(t.TableName);
                    return;
                }
                
                int count = 0;
                foreach(DataRow row in table.Rows) {
                    if(row.Table.Columns.Contains("No# Sec#") && row["No# Sec#"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["No# Sec#"].ToString())) {
                        Console.WriteLine($"Sec: {row["No# Sec#"]}, Oficio: {row["OFICIO"]}, T/O: {row["T/O"]}, TipoOper: {(row.Table.Columns.Contains("T/OPER") ? row["T/OPER"] : "")}, Mes: {row["MES"]}, Claves: {row["CLAVES"]}, Importe: {row["IMP ADECUACIONES"]}");
                        count++;
                    }
                    if(count >= 5) break;
                }
            }
        }
    }
}
