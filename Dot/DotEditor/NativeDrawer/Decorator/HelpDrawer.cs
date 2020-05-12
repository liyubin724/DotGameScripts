using Dot.NativeDrawer;
using Dot.NativeDrawer.Decorator;
using UnityEditor;

namespace DotEditor.NativeDrawer.Decorator
{
    [CustomAttDrawerLink(typeof(HelpAttribute))]
    public class HelpDrawer : DecoratorDrawer
    {
        public HelpDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUI()
        {
            HelpAttribute attr = GetAttr<HelpAttribute>();
            MessageType messageType = MessageType.None;
            if(attr.MessageType == HelpMessageType.Warning)
            {
                messageType = MessageType.Warning;
            }else if(attr.MessageType == HelpMessageType.Error)
            {
                messageType = MessageType.Error;
            }else if(attr.MessageType == HelpMessageType.Info)
            {
                messageType = MessageType.Info;
            }

            EditorGUILayout.HelpBox(attr.Text, messageType);
        }
    }
}
