using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.Ndb
{
    public class NDBSheetLine
    {
        internal object[] cells;

        public object GetData(int fieldIndex)
        {
            if(fieldIndex>=0&& fieldIndex<cells.Length)
            {
                return cells[fieldIndex];
            }
            return null;
        }

        public T GetData<T>(int fieldIndex)
        {
            if (fieldIndex >= 0 && fieldIndex < cells.Length)
            {
                return (T)cells[fieldIndex];
            }
            return default;
        }
    }
}
