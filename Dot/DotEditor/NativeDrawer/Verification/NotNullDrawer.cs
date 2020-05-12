using Dot.NativeDrawer;
using Dot.NativeDrawer.Verification;
using UnityEditor;

namespace DotEditor.NativeDrawer.Verification
{
    [CustomAttDrawerLink(typeof(NotNullAttribute))]
    public class NotNullDrawer : VerificationDrawer
    {
        public NotNullDrawer(object target, CompareDrawerAttribute attr) : base(target, attr)
        {
        }

        public override void OnLayoutGUI()
        {
            NotNullAttribute attr = GetAttr<NotNullAttribute>();

            bool isNotNull = IsValid();
            if(!isNotNull)
            {
                EditorGUILayout.HelpBox(string.IsNullOrEmpty(attr.UnvalidMsg) ? "" : attr.UnvalidMsg, MessageType.Error);
            }
        }
    }
}
