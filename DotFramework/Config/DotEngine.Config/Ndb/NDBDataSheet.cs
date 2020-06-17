using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

//Native Data Buffer
namespace DotEngine.Config.Ndb
{
    public class NDBDataSheet
    {
        private string name;
        private byte[] dataBytes = null;

        private NDBHeader header;
        private Dictionary<string, int> fieldNameToIndexDic = new Dictionary<string, int>();


        public NDBDataSheet()
        {
        }
    }
}
