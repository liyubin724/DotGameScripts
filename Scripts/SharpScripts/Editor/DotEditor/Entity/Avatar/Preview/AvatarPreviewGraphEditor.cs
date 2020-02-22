using DotEditor.XNodeEx;
using System;

namespace DotEditor.Entity.Avatar.Preview
{
    [CustomNodeGraphEditor(typeof(AvatarPreviewGraph))]
    public class AvatarPreviewGraphEditor : DotNodeGraphEditor
    {
        private AvatarPreviewGraph previewGraph = null;
        public override void OnOpen()
        {
            base.OnOpen();
            previewGraph = GetGraph<AvatarPreviewGraph>();
        }

        public override void OnClose()
        {
            if(previewGraph.isPreviewing)
            {
                previewGraph.DestroyPreview();
            }
            base.OnClose();
        }

        public override string GetNodeMenuName(Type type)
        {
            string menuName = base.GetNodeMenuName(type);
            if (type.Namespace == "DotEditor.Entity.Avatar.Preview")
            {
                menuName = menuName.Replace("Dot Editor/Entity/Avatar/Preview/", "");
                if (menuName.StartsWith("Avatar"))
                {
                    menuName = menuName.Replace("Avatar", "").Trim();
                }
            }
            return menuName;
        }
    }
}
