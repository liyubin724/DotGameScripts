using DotEngine.Config.Ndb;
using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.IO;
using DotTool.ETD.IO.Ndb;
using DotTool.ETD.Log;
using System.Drawing;
using System.IO;

namespace ExcelToDataTool
{
    class Program
    {
        static void Main(string[] args)
        {
            NDBSheet ndbSheet = new NDBSheet("Test");
            ndbSheet.SetData(File.ReadAllBytes("D:/Test.ndb"));

            for (int i = 0; i < ndbSheet.DataCount(); ++i)
            {
                for(int j =0;j<ndbSheet.FieldCount();++j)
                {
                    object data = ndbSheet.GetDataByIndex(i, j);
                    Colorful.Console.Write(data +",   ", Color.Green);
                }
                Colorful.Console.WriteLine();
            }

            string v1 = ndbSheet.GetDataById<string>(1, "StringField");
            Colorful.Console.WriteLine(v1, Color.Red);

            System.Console.ReadKey();
        }

        static void Main1(string[] args)
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

                for(int i =0;i<workbook.SheetCount;++i)
                {
                    Sheet sheet = workbook.GetSheeetByIndex(i);
                    NdbWriter.WriteTo(sheet, "D:/");
                }

                
            }
            else
            {
                Colorful.Console.WriteLine("Failed");

                string c = workbook.ToString();
                File.WriteAllText("D:/test.txt", c);
            }

            System.Console.ReadKey();
        }
    }
}
