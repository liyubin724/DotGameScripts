using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.Data
{
    public class Sheet
    {
        private string name;

        private List<Field> fields = new List<Field>();
        private List<Line> lines = new List<Line>();

        public string Name { get => name; }

        public Sheet(string n)
        {
            name = n;
        }

        public int LineCount { get => lines.Count; }
        public int FieldCount { get => fields.Count; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(name);
            sb.AppendLine("-------------------------------------------");
            foreach(var field in fields)
            {
                sb.Append(field.ToString());
            }
            sb.AppendLine();
            sb.AppendLine("------------------------------------------");
            foreach(var line in lines)
            {
                sb.AppendLine(line.ToString());
            }
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
