using Dot.NativeDrawer.Decorator;
using DotEditor.GUIExtension;

namespace DotEditor.NativeDrawer.Decorator
{
    [CustomAttributeDrawer(typeof(SeparatorLineAttribute))]
    public class SeparatorLineDrawer : DecoratorDrawer
    {
        public SeparatorLineDrawer(DecoratorAttribute attr) : base(attr)
        {
        }

        public override void OnGUILayout()
        {
            EGUILayout.DrawHorizontalLine();
        }
    }
}
