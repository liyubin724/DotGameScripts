using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEditor.Lua.Gen.Tabs
{
    public class GenGCOptimizeTab : AGenTab
    {
        public override void DoGUI(Rect rect)
        {
            throw new NotImplementedException();
        }

        public override void DoSearch(string searchText)
        {
            throw new NotImplementedException();
        }

        public override string[] GetSearchResult(string searchText)
        {
            return new string[] { "A", "B", "C" };
        }
    }
}
