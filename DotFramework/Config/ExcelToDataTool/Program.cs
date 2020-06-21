using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.IO;
using DotTool.ETD.Log;
using System.Drawing;
using System.IO;

namespace ExcelToDataTool
{
    class Program
    {
        static void Main(string[] args)
        {
            LogHandler logHandler = new LogHandler((type, id, msg) =>
            {
                string msgType = "Info";
                Color color = Color.White;

                if (type == LogType.Error)
                {
                    msgType = "Error";
                    color = Color.Red;
                }else if(type == LogType.Warning)
                {
                    msgType = "Warning";
                    color = Color.Yellow;
                }

                Colorful.Console.WriteLine($"[{msgType}]    [{id}]  {msg}", color);
            });

            WorkbookReader reader = new WorkbookReader(logHandler);

            TypeContext context = new TypeContext();
            context.Add(typeof(LogHandler), logHandler);

            string excelPath = @"D:\WorkSpace\DotGameProject\DotGameScripts\DotFramework\Config\test.xlsx";
            Workbook workbook = reader.ReadExcelFromFile(excelPath);

            bool result = workbook.Verify(context);

            if(result)
            {
                Colorful.Console.WriteLine("Verify success");
            }else
            {
                Colorful.Console.WriteLine("Failed");

                string c = workbook.ToString();
                File.WriteAllText("D:/test.txt", c);
            }

            System.Console.ReadKey();
        }
    }
}
