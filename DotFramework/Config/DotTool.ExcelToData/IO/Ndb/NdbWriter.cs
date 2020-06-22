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

        public void WriteTo(Sheet sheet,string targetDir)
        {
            string outputDir = $"{targetDir}/{sheet.Name}";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
        }

    }
}
