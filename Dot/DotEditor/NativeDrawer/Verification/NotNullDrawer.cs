using Dot.NativeDrawer;
using Dot.NativeDrawer.Verification;
using UnityEditor;

namespace DotEditor.NativeDrawer.Verification
{
    [CustomAttributeDrawer(typeof(NotNullAttribute))]
    public class NotNullDrawer : VerificationDrawer
    {
        public NotNullDrawer(object target, VerificationCompareAttribute attr) : base(target, attr)
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
