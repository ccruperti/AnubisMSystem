using AnubisDBMS.Data;
using AnubisDBMS.Data.Entities;
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
            ExcelWorksheet ws2 = ef.Worksheets.Add("DataSensores 2");
            ExcelWorksheet ws3 = ef.Worksheets.Add("DataSensores 3");
            ExcelWorksheet ws4 = ef.Worksheets.Add("DataSensores 4");
            ExcelWorksheet ws5 = ef.Worksheets.Add("DataSensores 5");
            ef.Styles[BuiltInCellStyleName.Normal].Font.Name = "Calibri";
            ef.Styles[BuiltInCellStyleName.Normal].Font.Size = 8 * 22;
            SetOptions(ws);
            SetOptions(ws2);
            SetOptions(ws3);
            SetOptions(ws4);
            SetOptions(ws5);

            var equiposensor = db.EquipoSensor.Where(c => c.IdEquipo == IdEquipo && c.Activo).ToList();
            List<DataSensores> lecturas = new List<DataSensores>();
            foreach(var eqs in equiposensor)
            {
                lecturas.AddRange(db.DataSensores.Where(x => x.SerieSensor == eqs.Sensores.SerieSensor
            && DbFunctions.TruncateTime(x.FechaRegistro) <= DbFunctions.TruncateTime(Hasta)).ToList());
            }
            

            SetCells(ws, Row, Col);
            SetCells(ws2, Row, Col);
            SetCells(ws3, Row, Col);
            SetCells(ws4, Row, Col);
            SetCells(ws5, Row, Col);

            var cont = 1;
            foreach (var lectura in lecturas.Take(600))
            {
                cont++;
                if (cont >=1 && cont <= 140)
                {
                    for (int i = Row; i <= 149; i++)
                    {
                        Row = 1;
                        Col = 1;
                        setContent(ws, Row, Col, cont, lectura);
                        Row = i;
                    }
                }
                
                if (cont >= 149 && cont <=299)
                {
                    Row =1;
                    Col = 0;
                    for (int i = Row; i <= 149; i++)
                    {
                        setContent(ws2, Row, Col, cont, lectura);
                        Row = i;
                    }
                }

                if (cont >= 299 && cont <=449)
                {

                    Row =1;
                    Col = 0;
                    for (int i = Row; i <= 149; i++)
                    {
                        setContent(ws3, Row, Col, cont, lectura);
                        Row = i;
                    }
                }


                if (cont >= 449 && cont <= 549)
                {

                    Row = 1;
                    Col = 0;
                    for (int i = Row; i <= 149; i++)
                    {
                        setContent(ws4, Row, Col, cont, lectura);
                        Row = i;
                    }
                }

                if (cont >=549 && cont <= 500)
                {

                    Row = 1;
                    Col = 0;
                    for (int i = Row; i <= 149; i++)
                    {
                        setContent(ws5, Row, Col, cont, lectura);
                        Row = i;
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
                       SaveOptions.XlsxDefault);
                   // new PdfSaveOptions { SelectionType = SelectionType.EntireFile });
                    fileContents = stream.ToArray();
                }
            }
            return fileContents;

        }

        public ExcelWorksheet SetOptions(ExcelWorksheet ws)
        {
            ws.PrintOptions.TopMargin = 0.9;
            ws.PrintOptions.HeaderMargin = 0.05;
            ws.PrintOptions.FooterMargin = 0.1;
            ws.PrintOptions.LeftMargin = 0.3;
            ws.PrintOptions.RightMargin = 0.2;
            ws.PrintOptions.HorizontalCentered = true;
            ws.PrintOptions.PaperType = PaperType.A4;
            ws.PrintOptions.FitWorksheetWidthToPages = 1;
            ws.PrintOptions.FitToPage = true;
            return ws;
        }


        public ExcelWorksheet SetCells(ExcelWorksheet ws, int Row, int Col)
        {
            ws.Cells[Row, Col++].Value = "#Serie";
            ws.Cells[Row, Col++].Value = "Tipo Sensor";
            ws.Cells[Row, Col++].Value = "Fecha Ingreso";
            ws.Cells[Row, Col++].Value = "Medición";
            ws.Cells[Row, Col].Value = "Unidad de Medida";
            Col = 0;
            return ws;
        }

        public void setContent(ExcelWorksheet ws, int Row, int Col, int cont, DataSensores lectura)
            {
             cont++;
                    ws.Cells[Row, Col++].Value = lectura.SerieSensor ?? "---";
                    ws.Cells[Row, Col++].Value = lectura.TipoSensor ?? "---";
                    ws.Cells[Row, Col++].Value = lectura?.FechaRegistro.ToString() ?? "---";
                    ws.Cells[Row, Col++].Value = lectura.Medida;
                    ws.Cells[Row, Col].Value = lectura.UnidadMedida ?? "---";
                    Col = 0;
             }


    }
}