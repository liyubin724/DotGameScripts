using DotEditor.XNodeEx;
using System;
using UnityEngine;
using XNodeEditor;

namespace DotEditor.Entity.Avatar.Preview
{
    [CustomNodeGraphEditor(typeof(AvatarPreviewGraph))]
    public class AvatarPreviewGraphEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(Type type)
        {
            if (type.Namespace == "DotEditor.Entity.Avatar.Preview")
            {
                string menuName = base.GetNodeMenuName(type).Replace("Dot Editor/Entity/Avatar/Preview/", "");
                if (menuName.StartsWith("Avatar"))
                {
                    menuName = menuName.Replace("Avatar", "").Trim();
                }
                return menuName;
            }
            else
                return XNodeEditorUtil.GetNodeMenuName(type);
        }
    }
}
