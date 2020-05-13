using Dot.NativeDrawer.Property;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttDrawerLink(typeof(HideLabelAttribute))]
    public class HideLabelDrawer : PropertyLabelDrawer
    {
        public HideLabelDrawer(PropertyLabelAttribute attr) : base(attr)
        {
        }

        public override string GetLabel()
        {
            return null;
        }
    }
}
