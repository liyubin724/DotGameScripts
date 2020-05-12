using Dot.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [CustomAttDrawerLink(typeof(HideIfAttribute))]
    public class HideIfDrawer : ConditionVisibleDrawer
    {
        public HideIfDrawer(object target, VisiableCompareAttribute attr) : base(target, attr)
        {
        }

        public override bool IsVisible()
        {
            return IsValid();
        }
    }
}
