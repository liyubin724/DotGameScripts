using Dot.NativeDrawer;
using Dot.NativeDrawer.Visible;
using System.Reflection;

namespace DotEditor.NativeDrawer.Visible
{
    [CustomAttDrawerLink(typeof(HideAttribute))]
    public class HideDrawer : VisibleDrawer
    {
        public HideDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override bool IsVisible()
        {
            return false;
        }
    }
}
