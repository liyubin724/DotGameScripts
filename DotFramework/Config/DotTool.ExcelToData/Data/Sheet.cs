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

        public void AddField(Field field)
        {
            fields.Add(field);
        }

        public Field GetFieldByCol(int col)
        {
            foreach(var field in fields)
            {
                if(field.Col == col)
                {
                    return field;
                }
            }
            return null;
        }

        public Field GetFieldByIndex(int index)
        {
            if(index>=0 && index < fields.Count)
            {
                return fields[index];
            }
            return null;
        }

        public void AddLine(Line line)
        {
            lines.Add(line);
        }

        public Line GetLineByRow(int row)
        {
            foreach(var line in lines)
            {
                if(line.Row == row)
                {
                    return line;
                }
            }
            return null;
        }

        public Line GetLineByIndex(int index)
        {
            if(index>=0&&index<lines.Count)
            {
                return lines[index];
            }
            return null;
        }

        public string GetLineIDByRow(int row)
        {
            Line line = GetLineByRow(row);
            for(int i =0;i<FieldCount;++i)
            {
                Field field = GetFieldByIndex(i);
                if(field.Type == FieldType.Id)
                {
                    return line.GetCellByIndex(i).GetValue(field);
                }
            }
            return null;
        }

        public string GetLineIDByIndex(int index)
        {
            Line line = GetLineByIndex(index);
            for (int i = 0; i < FieldCount; ++i)
            {
                Field field = GetFieldByIndex(i);
                if (field.Type == FieldType.Id)
                {
                    return line.GetCellByIndex(i).GetValue(field);
                }
            }
            return null;
        }

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
