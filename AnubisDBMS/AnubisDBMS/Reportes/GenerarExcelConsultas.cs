using AnubisDBMS.Data;
using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace AnubisDBMS.Reportes
{
    public class GenerarExcelConsultas
    {
        private readonly AnubisDbContext db = new AnubisDbContext();
        public GenerarExcelConsultas()
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

        }

        public byte[] GenerarDocumentoLecturasEquipos(long IdEquipo, DateTime Desde, DateTime Hasta, int Row = 0, int Col = 0)
        {
            ExcelFile ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("DataSensores");
            ef.Styles[BuiltInCellStyleName.Normal].Font.Name = "Calibri";
            ef.Styles[BuiltInCellStyleName.Normal].Font.Size = 8 * 22;
            ws.PrintOptions.TopMargin = 0.9;
            ws.PrintOptions.HeaderMargin = 0.05;
            ws.PrintOptions.FooterMargin = 0.1;
            ws.PrintOptions.LeftMargin = 0.3;
            ws.PrintOptions.RightMargin = 0.2;
            ws.PrintOptions.HorizontalCentered = true;
            ws.PrintOptions.PaperType = PaperType.A4;
            ws.PrintOptions.FitWorksheetWidthToPages = 1;
            ws.PrintOptions.FitToPage = true;

            var equiposensor = db.EquipoSensor.Where(c => c.IdEquipo == IdEquipo).Select(x => x.Sensores.SerieSensor).GroupBy(a => a).ToList();
            var lecturas = db.DataSensores.Where(x => equiposensor.Select(z => z.Key).Contains(x.SerieSensor) 
            && (DbFunctions.TruncateTime(x.FechaRegistro) >= DbFunctions.TruncateTime(Desde) 
            && DbFunctions.TruncateTime(x.FechaRegistro) <= DbFunctions.TruncateTime(Hasta))).ToList();

            ws.Cells[Row, Col++].Value = "#Serie";
            ws.Cells[Row, Col++].Value = "Tipo Sensor";
            ws.Cells[Row, Col++].Value = "Fecha Ingreso";
            ws.Cells[Row, Col++].Value = "Medición";
            ws.Cells[Row, Col].Value = "Unidad de Medida";
            Col = 0;
            Row++;
            int numws = 1;
            foreach(var lectura in lecturas)
            {
                for (int i = Row;  i <= lecturas.Count; i++)
                {
                    if(i <= 150)
                    {
                        ws.Cells[Row, Col++].Value = lectura.SerieSensor ?? "---";
                        ws.Cells[Row, Col++].Value = lectura.TipoSensor ?? "---";
                        ws.Cells[Row, Col++].Value = lectura?.FechaRegistro.ToString() ?? "---";
                        ws.Cells[Row, Col++].Value = lectura.Medida;
                        ws.Cells[Row, Col].Value = lectura.UnidadMedida ?? "---";
                        Col = 0;
                        Row = i;
                    }
                    else
                    {
                        if(i % 150 <= i)
                        {
                            ws = ef.Worksheets.AddCopy("Lecturas " + numws, ws);
                            numws

                        }
                        
                    }
                
                    
                }
           
                
                

            }


            byte[] fileContents = null;
            for (int i = 0; i < ef.Worksheets.Count; i++)
            {
                ef.Worksheets.ActiveWorksheet = ef.Worksheets[i];
                using (var stream = new MemoryStream())
                {
                    ef.Save(stream,
                    //    SaveOptions.XlsxDefault);
                    new PdfSaveOptions { SelectionType = SelectionType.EntireFile });
                    fileContents = stream.ToArray();
                }
            }
            return fileContents;

        }
    }
}