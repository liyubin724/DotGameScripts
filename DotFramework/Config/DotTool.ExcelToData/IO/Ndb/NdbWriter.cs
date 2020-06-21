using DotTool.ETD.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.IO.Ndb
{
    public class NdbWriter
    {

        public static void WriteTo(Workbook workbook,string targetDir)
        {
            string outputDir = $"{targetDir}/{workbook.Name}";
            if(!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            for(int i =0;i<workbook.SheetCount;++i)
            {
                Sheet sheet = workbook.GetSheeetByIndex(i);
                string outputFilePath = $"{outputDir}/{sheet.Name}.ndb";
                using(FileStream fs = new FileStream(outputFilePath,FileMode.Create,FileAccess.Write,FileShare.None))
                {

                }
            }

        }

        private static byte[] GetHeader(Sheet sheet)
        {

        }
    }
}
