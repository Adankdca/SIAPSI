using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using ExcelDataReader;

namespace SIAP.Services
{
    public class OficioMovimientoDto
    {
        public string ClavePresupuestal { get; set; }
        public decimal MontoAmpliacion { get; set; }
        public decimal MontoReduccion { get; set; }
        public int TipoOperacionClave { get; set; }
        public decimal Enero { get; set; }
        public decimal Febrero { get; set; }
        public decimal Marzo { get; set; }
        public decimal Abril { get; set; }
        public decimal Mayo { get; set; }
        public decimal Junio { get; set; }
        public decimal Julio { get; set; }
        public decimal Agosto { get; set; }
        public decimal Septiembre { get; set; }
        public decimal Octubre { get; set; }
        public decimal Noviembre { get; set; }
        public decimal Diciembre { get; set; }
        public decimal PorLiberar { get; set; }
        public bool EsNuevaClave { get; set; }
        public bool RequiresReview { get; set; }
        public string ReviewMessage { get; set; }
        public decimal ImporteOriginal { get; set; }
    }

    public class OficioLecturaResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Oficio { get; set; }
        public DateTime? Fecha { get; set; }
        public string Solicita { get; set; }
        public string Autoriza { get; set; }
        public int MovimientosCount { get; set; }
        public decimal TotalReduccion { get; set; }
        public decimal TotalAmpliacion { get; set; }
        public List<OficioMovimientoDto> Movimientos { get; set; }
    }

    public class LectorOficiosService
    {
        public OficioLecturaResult ProcesarArchivo(Stream fileStream, string fileName)
        {
            try
            {
                if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    return ProcesarPdf(fileStream);
                }
                else
                {
                    return new OficioLecturaResult { Success = false, Message = "Formato no soportado." };
                }
            }
            catch (Exception ex)
            {
                return new OficioLecturaResult { Success = false, Message = "Error al leer archivo: " + ex.Message };
            }
        }

        private OficioLecturaResult ProcesarPdf(Stream stream)
        {
            var result = new OficioLecturaResult { Movimientos = new List<OficioMovimientoDto>(), Success = true };
            
            using (PdfDocument document = PdfDocument.Open(stream))
            {
                string fullText = "";
                foreach (var page in document.GetPages())
                {
                    // Obtener todas las palabras y agruparlas por línea (eje Y) usando una tolerancia de 3 puntos
                    // Luego ordenarlas de izquierda a derecha (eje X) para reconstruir la fila visualmente
                    var words = page.GetWords();
                    var lines = words.GroupBy(w => Math.Round(w.BoundingBox.Bottom / 3.0) * 3.0)
                                     .OrderByDescending(g => g.Key)
                                     .Select(g => string.Join(" ", g.OrderBy(w => w.BoundingBox.Left).Select(w => w.Text)));
                    fullText += string.Join("\n", lines) + "\n";
                }
                
                // File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdf_debug.txt"), fullText);
                
                string numOficio = "DESCONOCIDO";
                var oficioMatch = Regex.Match(fullText, @"No\.\s*de\s*(?:Oficio|Solicitud):\s*([^\s]+)", RegexOptions.IgnoreCase);
                if (oficioMatch.Success) numOficio = oficioMatch.Groups[1].Value;

                DateTime? fechaOficio = null;
                var fechaMatch = Regex.Match(fullText, @"Fecha:\s*(\d{2}/\d{2}/\d{4})", RegexOptions.IgnoreCase);
                if (fechaMatch.Success) {
                    if (DateTime.TryParseExact(fechaMatch.Groups[1].Value, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate)) {
                        fechaOficio = parsedDate;
                    }
                }

                string solicita = null;
                string autoriza = null;
                
                foreach (var page in document.GetPages())
                {
                    var words = page.GetWords().ToList();
                    var wordSolicita = words.FirstOrDefault(w => w.Text == "SOLICITA");
                    var wordAutoriza = words.FirstOrDefault(w => w.Text == "AUTORIZA");
                    
                    if (wordSolicita != null) {
                        float rightBound = wordAutoriza != null ? (float)wordAutoriza.BoundingBox.Left - 5f : (float)wordSolicita.BoundingBox.Right + 150f;
                        var solWords = words.Where(w => w.BoundingBox.Bottom < wordSolicita.BoundingBox.Bottom - 5 && 
                                                        w.BoundingBox.Left >= wordSolicita.BoundingBox.Left - 100 &&
                                                        w.BoundingBox.Right <= rightBound &&
                                                        w.BoundingBox.Bottom > wordSolicita.BoundingBox.Bottom - 30)
                                            .OrderByDescending(w => Math.Round(w.BoundingBox.Bottom / 3.0) * 3.0)
                                            .ThenBy(w => w.BoundingBox.Left);
                        if (solWords.Any()) solicita = string.Join(" ", solWords.Select(w => w.Text));
                    }
                    if (wordAutoriza != null) {
                        var autWords = words.Where(w => w.BoundingBox.Bottom < wordAutoriza.BoundingBox.Bottom - 5 && 
                                                        w.BoundingBox.Left >= wordAutoriza.BoundingBox.Left - 50 &&
                                                        w.BoundingBox.Right <= wordAutoriza.BoundingBox.Right + 200 &&
                                                        w.BoundingBox.Bottom > wordAutoriza.BoundingBox.Bottom - 30)
                                            .OrderByDescending(w => Math.Round(w.BoundingBox.Bottom / 3.0) * 3.0)
                                            .ThenBy(w => w.BoundingBox.Left);
                        if (autWords.Any()) autoriza = string.Join(" ", autWords.Select(w => w.Text));
                    }
                    if (solicita != null || autoriza != null) break;
                }

                bool isTraspasoDoc = fullText.Contains("TRASPASO") || fullText.Contains("Traspaso");
                bool isReduccionDoc = fullText.Contains("REDUCCIÓN") || fullText.Contains("Reducción");
                bool isAmpliacionDoc = fullText.Contains("AMPLIACIÓN") || fullText.Contains("Ampliación");
                
                decimal totalReduccion = 0;
                decimal totalAmpliacion = 0;
                
                decimal totalAdecuacion = 0;
                var totalMatch = Regex.Match(fullText, @"TOTAL DE ADECUACIÓN\s*:\s*\$?\s*([\d,]+\.\d{2})", RegexOptions.IgnoreCase);
                if (totalMatch.Success)
                {
                    totalAdecuacion = decimal.Parse(totalMatch.Groups[1].Value.Replace(",", ""));
                }

                decimal runningSum = 0;
                bool isFaseIncremento = false;

                // Expresión regular para encontrar filas.
                // Se permite cruzar saltos de línea con [^\d,] por si el importe se movió al renglón de abajo
                var pattern = @"([A-Z0-9][A-Z0-9 \t\-]{35,200}?)[^\d,]*?([\d,]+\.\d{2})";
                var matches = Regex.Matches(fullText, pattern, RegexOptions.IgnoreCase);

                foreach (Match match in matches)
                {
                    // Clean up the clave (remove extra spaces)
                    string currentClave = Regex.Replace(match.Groups[1].Value, @"\s+", "");
                    // The clave presupuestal is EXACTLY 56 characters long. 
                    // Any extra characters at the beginning are likely the row number (e.g. '01', '02').
                    // Any extra character at the end is the 'T/O' (Tipo de Operación).
                    int tipoOp = 3; // Default Ampliacion
                    bool wasTipoOpExtracted = false;
                    
                    if (currentClave.Length >= 57)
                    {
                        char toChar = currentClave[currentClave.Length - 1];
                        currentClave = currentClave.Substring(currentClave.Length - 57, 56);
                        
                        // Parse TipoOp directly from the Clave
                        if (int.TryParse(toChar.ToString(), out int parsedTo)) {
                            tipoOp = parsedTo;
                            wasTipoOpExtracted = true;
                        }
                    }
                    else if (currentClave.Length > 56)
                    {
                        currentClave = currentClave.Substring(currentClave.Length - 56);
                    }
                    
                    string importeStr = match.Groups[2].Value.Replace(",", "");
                    if (decimal.TryParse(importeStr, out decimal importe))
                    {
                        // Fallback logic if tipoOp wasn't explicitly set by a 57-char clave
                        if (!wasTipoOpExtracted)
                        {
                            if (isTraspasoDoc)
                            {
                                if (!isFaseIncremento) {
                                    tipoOp = 2; // Decrementos primero
                                    runningSum += importe;
                                    if (runningSum >= totalAdecuacion - 0.1m) {
                                        isFaseIncremento = true; 
                                    }
                                } else {
                                    tipoOp = 1; // Incrementos despues
                                }
                            }
                            else if (isReduccionDoc) tipoOp = 4;
                            else if (isAmpliacionDoc) tipoOp = 3;
                        }

                        bool isReduccion = (tipoOp == 2 || tipoOp == 4);
                    
                    var mov = new OficioMovimientoDto
                    {
                        ClavePresupuestal = currentClave,
                        MontoAmpliacion = isReduccion ? 0 : importe,
                        MontoReduccion = isReduccion ? importe : 0,
                        TipoOperacionClave = tipoOp
                    };

                    // Extract Calendario Pagos for this Clave
                    int startIdx = match.Index + match.Length;
                    int endIdx = (match.NextMatch().Success) ? match.NextMatch().Index : fullText.Length;
                    string gapText = fullText.Substring(startIdx, endIdx - startIdx);
                    
                    int conceptoIdx = gapText.IndexOf("CONCEPTO:");
                    if (conceptoIdx >= 0) gapText = gapText.Substring(0, conceptoIdx);
                    
                    // Regex to match "DE AL [IMPORTE]" (e.g. "5 5 18,084.58") OR "[IMPORTE] DE AL" (e.g. "101,200.00 5 5")
                    // Uses [^\d]+ to flexibly skip any non-digits before the amount, preventing misses
                    var calMatches = Regex.Matches(gapText, @"(?:([\d,]+\.\d{2})\s+)?\b([0-9]|1[0-2])\s+([0-9]|1[0-2])\b(?:[^\d]+([\d,]+\.\d{2}))?");
                    decimal sumaCalendario = 0;
                    foreach (Match calMatch in calMatches)
                    {
                        if (int.TryParse(calMatch.Groups[2].Value, out int mesDe) && int.TryParse(calMatch.Groups[3].Value, out int mesAl))
                        {
                            decimal importeMes = importe; // Fallback al importe de la operación
                            if (calMatch.Groups[1].Success)
                            {
                                decimal.TryParse(calMatch.Groups[1].Value.Replace(",", ""), out importeMes);
                            }
                            else if (calMatch.Groups[4].Success)
                            {
                                decimal.TryParse(calMatch.Groups[4].Value.Replace(",", ""), out importeMes);
                            }
                            
                            int numMeses = (mesDe == 0 && mesAl == 0) ? 1 : (mesAl - mesDe + 1);
                            if (numMeses < 1) numMeses = 1;

                            sumaCalendario += (importeMes * numMeses);

                            if (mesDe == 0 && mesAl == 0)
                            {
                                mov.PorLiberar += importeMes;
                            }
                            else
                            {
                                for (int i = mesDe; i <= mesAl; i++)
                                {
                                    switch (i)
                                    {
                                        case 1: mov.Enero += importeMes; break;
                                        case 2: mov.Febrero += importeMes; break;
                                        case 3: mov.Marzo += importeMes; break;
                                        case 4: mov.Abril += importeMes; break;
                                        case 5: mov.Mayo += importeMes; break;
                                        case 6: mov.Junio += importeMes; break;
                                        case 7: mov.Julio += importeMes; break;
                                        case 8: mov.Agosto += importeMes; break;
                                        case 9: mov.Septiembre += importeMes; break;
                                        case 10: mov.Octubre += importeMes; break;
                                        case 11: mov.Noviembre += importeMes; break;
                                        case 12: mov.Diciembre += importeMes; break;
                                    }
                                }
                            }
                        }
                    } // Cierra el foreach (Match calMatch in calMatches)

                        // Override total amounts with calendar sum if calendars are present
                        if (sumaCalendario > 0) {
                            mov.ImporteOriginal = importe; // Guardar el original antes de sobrescribirlo
                            if (Math.Abs(sumaCalendario - importe) > 1m) {
                                mov.RequiresReview = true;
                                mov.ReviewMessage = $"El total del calendario (${sumaCalendario:N2}) no coincide con el importe de operación (${importe:N2}). Se usó el calendario pero requiere revisión.";
                            }

                            if (isReduccion) {
                                mov.MontoReduccion = sumaCalendario;
                                mov.MontoAmpliacion = 0;
                            } else {
                                mov.MontoAmpliacion = sumaCalendario;
                                mov.MontoReduccion = 0;
                            }
                            importe = sumaCalendario;
                        }

                        result.Movimientos.Add(mov);
                        if (isReduccion) totalReduccion += importe; else totalAmpliacion += importe;
                    }
                }

                result.Success = true;
                result.Message = "Oficio procesado correctamente.";
                result.Oficio = numOficio;
                result.Fecha = fechaOficio;
                result.Solicita = solicita;
                result.Autoriza = autoriza;
                result.MovimientosCount = result.Movimientos.Count;
                result.TotalReduccion = totalReduccion;
                result.TotalAmpliacion = totalAmpliacion;

                if (result.MovimientosCount == 0)
                {
                    result.Success = false;
                    result.Message = "No se encontraron claves presupuestales en el documento.";
                }
            }

            return result;
        }

    }
}
