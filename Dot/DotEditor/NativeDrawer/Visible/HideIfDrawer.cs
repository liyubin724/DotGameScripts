using Dot.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [CustomAttDrawerLink(typeof(HideIfAttribute))]
    public class HideIfDrawer : VisibleCompareDrawer
    {
        public HideIfDrawer(object target, VisibleCompareAttribute attr) : base(target, attr)
        {
        }

        public override bool IsVisible()
        {
            return !IsValid();
        }
    }
}
