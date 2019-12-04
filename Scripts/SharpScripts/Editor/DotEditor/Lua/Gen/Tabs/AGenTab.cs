using ExtractInject;
using UnityEngine;

namespace DotEditor.Lua.Gen.Tabs
{
    public abstract class AGenTab : UsedExtractInject
    {
        protected string searchText = string.Empty;
        public virtual void DoEnable()
        {
            searchText = string.Empty;
        }

        public abstract void DoGUI(Rect rect);
        public abstract void DoSearch(string searchText);
    }
}
