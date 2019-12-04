using ExtractInject;
using UnityEngine;

namespace DotEditor.Lua.Gen.Tabs
{
    public abstract class AGenTab : UsedExtractInject
    {
        public abstract void DoGUI(Rect rect);
        public abstract void DoSearch(string searchText);
    }
}
