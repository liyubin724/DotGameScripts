using DotTool.ETD.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETDField = DotTool.ETD.Data.Field;
using ETDLine = DotTool.ETD.Data.Line;
using ETDSheet = DotTool.ETD.Data.Sheet;
using ETDWorkbook = DotTool.ETD.Data.Workbook;

namespace DotTool.ETD.IO
{
    public class WorkbookWriter
    {
        private LogHandler logHandler = null;
        public WorkbookWriter(LogHandler handler)
        {
            logHandler = handler;
        }

        public void WriteToExcelFile(ETDWorkbook etdWorkbook)
        {
            WriteToExcelFile(etdWorkbook.FilePath, etdWorkbook);
        }

        public void WriteToExcelFile(string excelPath,ETDWorkbook etdWorkbook)
        {
            if(string.IsNullOrEmpty(excelPath))
            {
                return;
            }
        }
    }
}
