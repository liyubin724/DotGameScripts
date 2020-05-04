using Dot.GUI.Drawer;
using Dot.GUI.Drawer.Decorator;
using System.Reflection;
using UnityEditor;

namespace DotEditor.EGUI.Drawer.Decorator
{
    [CustomDrawer(typeof(HelpAttribute))]
    public class HelpDrawer : DecoratorDrawer
    {
        public HelpDrawer(object data, FieldInfo field, DrawerAttribute attr) : base(data, field, attr)
        {
        }

        public override void DoLayoutGUI()
        {
            HelpAttribute attr = GetAttr<HelpAttribute>();
            if(attr!=null && !string.IsNullOrEmpty(attr.Text))
            {
                MessageType mType = MessageType.None;
                if(attr.MessageType == HelpMessageType.Info)
                {
                    mType = MessageType.Info;
                }else if(attr.MessageType == HelpMessageType.Warning)
                {
                    mType = MessageType.Warning;
                }else if(attr.MessageType == HelpMessageType.Error)
                {
                    mType = MessageType.Error;
                }
                EditorGUILayout.HelpBox(attr.Text, mType);
            }
        }
    }
}
