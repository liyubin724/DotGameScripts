using Dot.NativeDrawer;
using Dot.NativeDrawer.Property;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttDrawerLink(typeof(NewLabelAttribute))]
    public class NewLabelDrawer : PropertyLabelDrawer
    {
        public NewLabelDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override string GetLabel()
        {
            NewLabelAttribute attr = GetAttr<NewLabelAttribute>();
            return attr.Label;
        }
    }
}
