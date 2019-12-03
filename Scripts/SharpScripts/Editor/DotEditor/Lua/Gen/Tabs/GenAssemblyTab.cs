using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEditor.Lua.Gen.Tabs
{
    public class GenAssemblyTab : AGenTab
    {


        public override void DoGUI(Rect rect)
        {
            
        }

        public override void DoSearch(string searchText)
        {
            
        }

        public override string[] GetSearchResult(string searchText)
        {
            return new string[] { "A", "B", "C" };
        }
    }
}
