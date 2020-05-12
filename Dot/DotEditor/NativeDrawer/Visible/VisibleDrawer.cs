using Dot.NativeDrawer;
using Dot.NativeDrawer.Visible;
using System.Reflection;

namespace DotEditor.NativeDrawer.Visible
{
    public abstract class VisibleDrawer : AttrNativeDrawer
    {
        public VisibleDrawer(VisiableAtrribute attr) : base(attr)
        {
            
        }

        public abstract bool IsVisible();
    }

    public abstract class ConditionVisibleDrawer : CompareAttrNativeDrawer
    {
        protected ConditionVisibleDrawer(object target, VisiableCompareAttribute attr) : base(target, attr)
        {
        }

        public abstract bool IsVisible();
    }
}
