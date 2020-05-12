using Dot.NativeDrawer;
using Dot.NativeDrawer.Layout;

namespace DotEditor.NativeDrawer.Layout
{
    public abstract class LayoutDrawer : AttrNativeDrawer
    {
        protected LayoutDrawer(LayoutAttribute attr) : base(attr)
        {
        }

        public abstract void OnLayoutGUI();
    }
}
