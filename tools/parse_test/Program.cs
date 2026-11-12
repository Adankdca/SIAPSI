using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace parse_test
{
    class Program
    {
        static void Main(string[] args)
        {
            string pdfPath = @"c:\Users\itzdx\OneDrive\Documentos\repos\SIAP\SISTEMA EXCEL\Adecuaciones\Adecuacion_21111206EA10009_2362026_1538.pdf";
            string outPath = @"c:\Users\itzdx\OneDrive\Documentos\repos\SIAP\SIAP\pdf_out.txt";
            
            using (PdfReader reader = new PdfReader(pdfPath))
            using (PdfDocument pdfDoc = new PdfDocument(reader))
            {
                using (StreamWriter writer = new StreamWriter(outPath))
                {
                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        var page = pdfDoc.GetPage(i);
                        string text = PdfTextExtractor.GetTextFromPage(page);
                        writer.WriteLine($"--- Page {i} ---");
                        writer.WriteLine(text);
                    }
                }
            }
        }
    }
}
