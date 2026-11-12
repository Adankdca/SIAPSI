using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using MySql.Data.MySqlClient;

class Program {
    static void Main() {
        string csvPath = @"C:\Users\itzdx\OneDrive\Documentos\repos\SIAP\SISTEMA EXCEL\CONCENTRADO DE ADECUACIONES 2025 VERTICAL - BASE GENERAL VERTICAL.csv";
        string connStr = "Server=localhost;Database=SIAP_DB;Uid=root;Pwd=root;";

        var records = new List<CsvRow>();

        using (TextFieldParser parser = new TextFieldParser(csvPath, System.Text.Encoding.Default)) {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            
            string[] headers = parser.ReadFields();
            int idxNoSec = Array.IndexOf(headers, "No. Sec.");
            int idxOficio = Array.IndexOf(headers, "OFICIO");
            int idxFecha = Array.IndexOf(headers, "FECHA DE OFICIO");
            int idxTO = Array.IndexOf(headers, "T/O");
            int idxTipoOpString = Array.FindIndex(headers, h => h.StartsWith("TIPO DE OPERACI"));
            int idxClaves = Array.IndexOf(headers, "CLAVES");
            int idxMes = Array.IndexOf(headers, "MES");
            int idxImpAdec = Array.IndexOf(headers, "IMP ADECUACIONES");
            int idxTipoOper = Array.IndexOf(headers, "T/OPER");

            while (!parser.EndOfData) {
                string[] fields = parser.ReadFields();
                if(fields.Length > Math.Max(idxNoSec, idxImpAdec) && !string.IsNullOrWhiteSpace(fields[idxNoSec])) {
                    decimal d = 0m;
                    string amtStr = fields[idxImpAdec].Replace(",", "").Replace("$", "").Trim();
                    decimal.TryParse(amtStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out d);
                    
                    int monthTest;
                    if (int.TryParse(fields[idxMes], out monthTest) && monthTest >= 1 && monthTest <= 12) {
                        records.Add(new CsvRow {
                            NoSec = fields[idxNoSec],
                            Oficio = fields[idxOficio],
                            Fecha = fields[idxFecha],
                            TO = fields[idxTO],
                            TipoOpString = fields[idxTipoOpString],
                            Claves = fields[idxClaves],
                            Mes = fields[idxMes],
                            ImpAdecuaciones = d,
                            TipoOper = fields[idxTipoOper]
                        });
                    }
                }
            }
        }

        Console.WriteLine("Parsed " + records.Count + " valid records from CSV.");

        var groupedBySec = records.GroupBy(r => r.NoSec);

        using (MySqlConnection conn = new MySqlConnection(connStr)) {
            conn.Open();
            using (var trans = conn.BeginTransaction()) {
                try {
                    // Cache TipoAdecuacion mapping
                    var tipoMap = new Dictionary<string, int>();
                    using (var cmd = new MySqlCommand("SELECT Id, Clave FROM cat_tipoadecuacion", conn, trans)) {
                        using (var reader = cmd.ExecuteReader()) {
                            while(reader.Read()) {
                                tipoMap[reader["Clave"].ToString()] = Convert.ToInt32(reader["Id"]);
                            }
                        }
                    }

                    using (var cmd = new MySqlCommand("SET FOREIGN_KEY_CHECKS=0; TRUNCATE TABLE adecuacionmovimientos; TRUNCATE TABLE adecuaciones; SET FOREIGN_KEY_CHECKS=1;", conn, trans)) {
                        cmd.ExecuteNonQuery();
                    }

                    foreach(var secGroup in groupedBySec) {
                        var first = secGroup.First();
                        int tipoAdId = tipoMap.ContainsKey(first.TO) ? tipoMap[first.TO] : 0;
                        
                        DateTime fechaSol;
                        if(!DateTime.TryParse(first.Fecha, out fechaSol)) {
                            fechaSol = DateTime.Now;
                        }

                        // Insert Adecuacion
                        string insAdec = @"INSERT INTO adecuaciones (NumOficio, FechaSolicitud, TipoAdecuacionId, TipoAdecuacion, Estatus, MontoTotal) 
                                           VALUES (@nof, @fs, @tid, @ta, 'BORRADOR', @monto); SELECT LAST_INSERT_ID();";
                        int adecuacionId = 0;
                        using (var cmd = new MySqlCommand(insAdec, conn, trans)) {
                            cmd.Parameters.AddWithValue("@nof", first.Oficio);
                            cmd.Parameters.AddWithValue("@fs", fechaSol);
                            cmd.Parameters.AddWithValue("@tid", tipoAdId > 0 ? (object)tipoAdId : DBNull.Value);
                            cmd.Parameters.AddWithValue("@ta", first.TipoOpString);
                            // Sum absolute amounts for total
                            decimal montoTotal = secGroup.Sum(r => Math.Abs(r.ImpAdecuaciones));
                            cmd.Parameters.AddWithValue("@monto", montoTotal);
                            adecuacionId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Group by Clave + TipoOper
                        var claveGroups = secGroup.GroupBy(r => new { r.Claves, r.TipoOper });
                        foreach(var cg in claveGroups) {
                            string insMov = @"INSERT INTO adecuacionmovimientos 
                                              (AdecuacionId, TipoOperacion, ClavePresupuestal, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre)
                                              VALUES (@aid, @to, @cp, @ene, @feb, @mar, @abr, @may, @jun, @jul, @ago, @sep, @oct, @nov, @dic)";
                            using(var cmd = new MySqlCommand(insMov, conn, trans)) {
                                cmd.Parameters.AddWithValue("@aid", adecuacionId);
                                cmd.Parameters.AddWithValue("@to", string.IsNullOrWhiteSpace(cg.Key.TipoOper) ? "MOVIMIENTO" : cg.Key.TipoOper);
                                cmd.Parameters.AddWithValue("@cp", cg.Key.Claves);

                                string[] meses = {"Enero","Febrero","Marzo","Abril","Mayo","Junio","Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"};
                                for(int i=0; i<12; i++) {
                                    int mNum = i + 1;
                                    decimal amt = cg.Where(r => {
                                        int parsedMes;
                                        if (int.TryParse(r.Mes, out parsedMes)) {
                                            return parsedMes == mNum;
                                        }
                                        return false;
                                    }).Sum(r => r.ImpAdecuaciones);
                                    cmd.Parameters.AddWithValue("@" + meses[i].Substring(0,3).ToLower(), amt);
                                }
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    trans.Commit();
                    Console.WriteLine("Import successful!");
                }
                catch(Exception ex) {
                    trans.Rollback();
                    Console.WriteLine("Error during import: " + ex.Message);
                }
            }
        }
    }

    class CsvRow {
        public string NoSec { get; set; }
        public string Oficio { get; set; }
        public string Fecha { get; set; }
        public string TO { get; set; }
        public string TipoOpString { get; set; }
        public string Claves { get; set; }
        public string Mes { get; set; }
        public decimal ImpAdecuaciones { get; set; }
        public string TipoOper { get; set; }
    }
}
