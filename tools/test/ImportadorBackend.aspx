<%@ Page Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="ExcelDataReader" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";
        try
        {
            string filePath = @"C:\Users\itzdx\OneDrive\Documentos\repos\SIAP\SISTEMA EXCEL\CONCENTRADO DE ADECUACIONES 2025 VERTICAL.xls";
            
            var resultList = new System.Collections.Generic.List<object>();
            
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var conf = new ExcelDataSetConfiguration() {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    };
                    var dataSet = reader.AsDataSet(conf);
                    
                    var table = dataSet.Tables["BASE GENERAL VERTICAL$"];
                    if (table == null) table = dataSet.Tables[3]; // Fallback to 4th sheet
                    
                    if (table == null) {
                        Response.Write("{\"error\": \"Sheet not found\"}");
                        return;
                    }
                    
                    int count = 0;
                    foreach(DataRow row in table.Rows)
                    {
                        if(row["No# Sec#"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["No# Sec#"].ToString())) {
                            resultList.Add(new {
                                Sec = row["No# Sec#"].ToString(),
                                Oficio = row["OFICIO"].ToString(),
                                Fecha = row["FECHA DE OFICIO"].ToString(),
                                TipoO = row["T/O"].ToString(),
                                TipoOpString = row["TIPO DE OPERACIÓN"].ToString(),
                                TipoOpCol = row.Table.Columns.Contains("T/OPER") ? row["T/OPER"].ToString() : "N/A",
                                Clave = row["CLAVES"].ToString(),
                                Mes = row["MES"].ToString(),
                                Importe = row["IMP ADECUACIONES"].ToString(),
                                TipoAdecuacion = row["TIPO DE ADECUACION"].ToString()
                            });
                            count++;
                        }
                        if(count >= 5) break;
                    }
                }
            }
            Response.Write(JsonConvert.SerializeObject(resultList));
        }
        catch (Exception ex)
        {
            Response.Write("{\"error\": \"" + ex.Message.Replace("\"", "\\\"").Replace("\n", " ") + "\"}");
        }
    }
</script>
