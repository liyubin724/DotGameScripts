using Dot.NativeDrawer;
using Dot.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [CustomAttDrawerLink(typeof(ShowAttribute))]
    public class ShowDrawer : VisibleDrawer
    {
        public ShowDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override bool IsVisible()
        {
            return true;
        }
    }
}
