using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotTool.ETD.Data
{
    public class Workbook
    {
        private string bookFilePath;
        public string Name { get => Path.GetFileNameWithoutExtension(bookFilePath); }

        private List<Sheet> sheets = new List<Sheet>();
        public int SheetCount { get => sheets.Count; }
        public string[] SheetNames
        {
            get
            {
                return (from sheet in sheets select sheet.Name).ToArray();
            }
        }

        public Workbook(string path)
        {
            bookFilePath = path;
        }
        
        public Sheet GetSheetByName(string name)
        {
            foreach(var sheet in sheets)
            {
                if(sheet.Name == name)
                {
                    return sheet;
                }
            }
            return null;
        }

        public Sheet GetSheeetByIndex(int index)
        {
            if(index>=0 && index<sheets.Count)
            {
                return sheets[index];
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(bookFilePath);
            sb.AppendLine("===========================");
            foreach(var sheet in sheets)
            {
                sb.AppendLine(sheet.ToString());
            }
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
