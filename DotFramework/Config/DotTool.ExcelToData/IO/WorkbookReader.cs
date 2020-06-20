using DotTool.ETD.Data;
using DotTool.ETD.Fields;
using DotTool.ETD.Log;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using ETDField = DotTool.ETD.Data.Field;
using ETDLine = DotTool.ETD.Data.Line;
using ETDSheet = DotTool.ETD.Data.Sheet;
using ETDWorkbook = DotTool.ETD.Data.Workbook;

namespace DotTool.ETD.IO
{
    public class WorkbookReader
    {
        private static int MIN_ROW_COUNT = 6;
        private static int MIN_COLUMN_COUNT = 2;
        private static string SHEET_NAME_REGEX = @"^[A-Z]\w{3,10}";
        private static string FIELD_NAME_REGEX = @"^[A-Za-z]{2,15}$";
        private static string ID_FIELD_NAME = "ID";
        private static string TEXT_BOOK_NAME = "Text";
        private static string ROW_START_FLAG = "start";
        private static string ROW_END_FLAG = "end";
        private static string CELL_NULL_FLAG = "nil";

        private LogHandler logHandler = null;
        public WorkbookReader(LogHandler handler)
        {
            logHandler = handler;
        }

        public void ReadExcel(ETDWorkbook etdWorkbook)
        {
            string excelPath = etdWorkbook.FilePath;
            string ext = Path.GetExtension(excelPath).ToLower();

            using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                IWorkbook workbook = null;
                if (ext == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else
                {
                    workbook = new HSSFWorkbook(fs);
                }

                if (workbook == null || workbook.NumberOfSheets == 0)
                {
                    logHandler.Log(LogType.Warning, LogMessage.LOG_WORKBOOK_EMPTY, excelPath);
                    return;
                }

                logHandler.Log(LogType.Info, LogMessage.LOG_WORKBOOK_START, excelPath);

                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    logHandler.Log(LogType.Info, LogMessage.LOG_WARP_LINE);

                    ISheet sheet = workbook.GetSheetAt(i);

                    string sheetName = sheet.SheetName;
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        logHandler.Log(LogType.Warning, LogMessage.LOG_SHEET_NAME_NULL, i);

                        continue;
                    }
                    if (sheetName.StartsWith("#"))
                    {
                        logHandler.Log(LogType.Info, LogMessage.LOG_IGNORE_SHEET, sheetName);
                        continue;
                    }
                    if (!Regex.IsMatch(sheetName, SHEET_NAME_REGEX))
                    {
                        logHandler.Log(LogType.Error, LogMessage.LOG_SHEET_NAME_NOT_MATCH, sheetName, SHEET_NAME_REGEX);
                        continue;
                    }

                    Sheet dataSheet = ReadFromSheet(sheet);
                    etdWorkbook.AddSheet(dataSheet);

                    logHandler.Log(LogType.Info, LogMessage.LOG_WARP_LINE);
                }

                logHandler.Log(LogType.Info, LogMessage.LOG_WORKBOOK_END, excelPath);
            }
        }

        private ETDSheet ReadFromSheet(ISheet sheet)
        {
            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_START, sheet.SheetName);

            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;

            int firstCol = sheet.GetRow(firstRow).FirstCellNum;
            int lastCol = sheet.GetRow(firstRow).LastCellNum;

            int rowCount = lastRow - firstRow + 1;
            int colCount = lastCol - firstCol + 1;
            if (rowCount < MIN_ROW_COUNT)
            {
                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_ROW_LESS, rowCount, MIN_ROW_COUNT);
                return null;
            }
            if (colCount < MIN_COLUMN_COUNT)
            {
                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_COL_LESS, colCount, MIN_COLUMN_COUNT);
                return null;
            }

            ETDSheet sheetData = new ETDSheet(sheet.SheetName);
            ReadFieldFromSheet(sheetData, sheet);
            ReadLineFromSheet(sheetData, sheet);

            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_END, sheet.SheetName);

            return sheetData;
        }

        private void ReadFieldFromSheet(ETDSheet sheetData, ISheet sheet)
        {
            MethodInfo getFieldMI = typeof(FieldFactory).GetMethod("GetField", BindingFlags.Public | BindingFlags.Static);

            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_START);

            int firstRow = sheet.FirstRowNum;
            int firstCol = sheet.GetRow(firstRow).FirstCellNum;
            int lastCol = sheet.GetRow(firstRow).LastCellNum;

            for (int i = firstCol; i <= lastCol; ++i)
            {
                IRow nameRow = sheet.GetRow(firstRow);
                string nameContent = GetCellStringValue(nameRow.GetCell(i));
                if (string.IsNullOrEmpty(nameContent))
                {
                    logHandler.Log(LogType.Warning, LogMessage.LOG_SHEET_FIELD_NAME_NULL);
                    continue;
                }
                if (nameContent.StartsWith("#") || nameContent.StartsWith("_"))
                {
                    logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_IGNORE, nameContent);
                    continue;
                }

                object[] datas = new object[MIN_ROW_COUNT + 1];
                datas[0] = i;
                for (int j = 0; j < MIN_ROW_COUNT; j++)
                {
                    IRow row = sheet.GetRow(firstRow + j);
                    datas[j+1]  = GetCellStringValue(row.GetCell(i));
                }

                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_CREATE, i);

                Field field = (Field)getFieldMI.Invoke(null, datas);
                sheetData.AddField(field);

                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_DETAIL, field.ToString());
            }
            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_END);
        }

        private void ReadLineFromSheet(ETDSheet sheetData, ISheet sheet)
        {
            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_LINE_START);

            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;

            int firstCol = sheet.GetRow(firstRow).FirstCellNum;

            bool isStart = false;
            for (int i = MIN_ROW_COUNT; i < lastRow; i++)
            {
                IRow row = sheet.GetRow(i);
                ICell cell = row.GetCell(firstCol);
                if (!isStart)
                {
                    if (cell == null)
                    {
                        continue;
                    }
                    else
                    {
                        string cellContent = GetCellStringValue(cell);
                        if (cellContent == ROW_START_FLAG)
                        {
                            isStart = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    if (cell != null)
                    {
                        string cellContent = GetCellStringValue(cell);
                        if (cellContent == ROW_END_FLAG)
                        {
                            break;
                        }
                    }
                }

                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_LINE_CREATE, i);

                ETDLine line = new ETDLine(i);
                int fieldCount = sheetData.FieldCount;
                for (int j = 0; j < fieldCount; ++j)
                {
                    ETDField fieldData = sheetData.GetFieldByIndex(j);
                    ICell valueCell = row.GetCell(fieldData.Col);
                    line.AddCell(fieldData.Col, GetCellStringValue(valueCell));
                }
                sheetData.AddLine(line);

                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_LINE_DETAIL, line.ToString());
            }

            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_LINE_END);
        }

        private string GetCellStringValue(ICell cell)
        {
            if (cell == null)
                return null;

            CellType cType = cell.CellType;
            if (cType == CellType.Formula)
            {
                cType = cell.CachedFormulaResultType;
            }

            if (cType == CellType.Unknown || cType == CellType.Blank || cType == CellType.Error)
            {
                return null;
            }
            else if (cType == CellType.String)
            {
                return cell.StringCellValue;
            }
            else if (cType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }
            else if (cType == CellType.Boolean)
            {
                return cell.BooleanCellValue.ToString();
            }

            return null;
        }
    }
}
