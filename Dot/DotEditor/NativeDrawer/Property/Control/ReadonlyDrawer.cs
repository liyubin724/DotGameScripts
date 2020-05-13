using Dot.NativeDrawer;
using Dot.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttDrawerLink(typeof(ReadonlyAttribute))]
    public class ReadonlyDrawer : PropertyControlDrawer
    {
        public ReadonlyDrawer(PropertyControlAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUIStart()
        {
            EditorGUI.BeginDisabledGroup(true);
        }

        public override void OnLayoutGUIEnd()
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}
