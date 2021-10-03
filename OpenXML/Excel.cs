using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.IO;

namespace OpenXML
{
    public class Excel : MyInterfaces.DokumanInterfaces.IDokumanReadAndEditTool
    {

        public bool Kontrol(string FilePath)
        {
            bool s = false;
            if (File.Exists(FilePath))
                try
                {
                    using (var document = SpreadsheetDocument.Open(FilePath, true))
                    {
                        var workbookPart = document.WorkbookPart;
                        var workbook = workbookPart.Workbook;


                        var sheets = workbook.Descendants<Sheet>();


                        s = true;
                    }
                }
                catch (Exception)
                {
                    s = false;
                }

            return s;

        }

        private string GetValue(Cell cell, SharedStringTablePart stringTablePart)
        {
            if (cell.ChildElements.Count == 0)
                return null;
            //get cell value
            string value = cell.CellValue.InnerText;
            //Look up real value from shared string table
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                value = stringTablePart.SharedStringTable
                .ChildElements[Int32.Parse(value)]
                .InnerText;
            return value;
        }

        public List<string> ParametreAra(string FilePath, string baslangicText, string bitisText, out string docText)
        {
            List<string> parametreList = new List<string>();
            docText = "";

            using (var document = SpreadsheetDocument.Open(FilePath, true))
            {                
                var workbookPart = document.WorkbookPart;
                var workbook = workbookPart.Workbook;


                var sheets = workbook.Descendants<Sheet>();
                foreach (var sheet in sheets)
                {
                    var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                    var sharedStringPart = workbookPart.SharedStringTablePart;
                    var values = sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                    var cells = worksheetPart.Worksheet.Descendants<Cell>();
                    foreach (var cell in cells)
                    {
                        string cellText = GetValue(cell, workbookPart.SharedStringTablePart);
                        if (cellText == null)
                            continue;

                        docText += "\t" + cellText;

                        int birOncekiBaslangicIndex = 0;
                        do
                        {
                            if (birOncekiBaslangicIndex >= cellText.Length)
                                break;

                            int baslangicIndex = cellText.IndexOf(baslangicText, birOncekiBaslangicIndex) + baslangicText.Length;
                            if (baslangicIndex < 1)
                            {
                                break;
                            }
                            
                            int bitisIndex = cellText.IndexOf(bitisText, baslangicIndex);

                            if (bitisIndex < 1)
                            {
                                break;
                            }
                            else
                            {
                                string parametre = cellText.Substring(baslangicIndex, bitisIndex - baslangicIndex);
                                if (parametre.Length < 51)
                                {
                                    parametreList.Add(baslangicText + parametre + bitisText);
                                }

                                birOncekiBaslangicIndex = bitisIndex + bitisText.Length;
                            }
                        } while (true);                        
                    }

                }
            }

            return parametreList.Distinct().ToList();
        }

        public void FindAndReplace(string FilePath, Dictionary<string, string> findReplaceList)
        {
            using (var document = SpreadsheetDocument.Open(FilePath, true))
            {
                var workbookPart = document.WorkbookPart;
                var workbook = workbookPart.Workbook;


                var sheets = workbook.Descendants<Sheet>();
                foreach (var sheet in sheets)
                {
                    var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                    var sharedStringPart = workbookPart.SharedStringTablePart;
                    var values = sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                    var cells = worksheetPart.Worksheet.Descendants<Cell>();
                    foreach (var cell in cells)
                    {
                        foreach (var item in findReplaceList)
                        {
                            if (GetValue(cell, workbookPart.SharedStringTablePart) == item.Key)
                            {
                                cell.DataType = new EnumValue<CellValues>(CellValues.String);
                                cell.CellValue = new CellValue(item.Value);
                            }
                        }
                    }

                }
            }
        }

    }
}