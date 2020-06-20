using DotTool.ETD.Data;
using DotTool.ETD.IO;
using DotTool.ETD.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToDataTool
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkbookReader reader = new WorkbookReader(new LogHandler((type, id, msg) =>
            {
                Console.WriteLine(msg);
            }));

            string excelPath = @"D:\WorkSpace\DotGameProject\DotGameScripts\DotFramework\Config\test.xlsx";
            Workbook workbook = reader.ReadExcelFromFile(excelPath);

            Console.ReadKey();
        }
    }
}
