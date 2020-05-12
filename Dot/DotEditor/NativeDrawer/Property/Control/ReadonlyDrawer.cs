using Dot.NativeDrawer;
using Dot.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttDrawerLink(typeof(ReadonlyAttribute))]
    public class ReadonlyDrawer : PropertyControlDrawer
    {
        public ReadonlyDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override void BeginDoLayoutGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
        }

        public override void EndDoLayoutGUI()
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}
