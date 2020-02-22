using Dot.Entity.Avatar;
using DotEditor.XNodeEx;
using UnityEngine;

namespace DotEditor.Entity.Avatar.Preview
{
    [CustomNodeEditor(typeof(AvatarPreviewNode))]
    public class AvatarPreviewNodeEditor : DotNodeEditor
    {
        AvatarPreviewNode previewNode = null;
        AvatarPreviewGraph previewGraph = null;

        public override void OnEnable()
        {
            previewNode = GetNode<AvatarPreviewNode>();
            previewGraph = previewNode.GetGraph<AvatarPreviewGraph>();
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            bool isPreview = previewGraph.isPreviewing;
            if(GUILayout.Button(isPreview?"Destroy Preview":"Show Preview",GUILayout.Height(25)))
            {
                if(isPreview)
                {
                    previewGraph.DestroyPreview();
                }else
                {
                    previewGraph.CreatePreview(
                        previewNode.GetInputValue<GameObject>("skeletonPrefab"),
                        previewNode.GetInputValue<AvatarPartData>("headPart"),
                        previewNode.GetInputValue<AvatarPartData>("bodyPart"),
                        previewNode.GetInputValue<AvatarPartData>("handPart"),
                        previewNode.GetInputValue<AvatarPartData>("feetPart"),
                        previewNode.GetInputValue<AvatarPartData>("weaponPart"));
                }
            }
        }

        public override int GetWidth()
        {
            return 300;
        }

        public override Color GetTint()
        {
            if(previewGraph.isPreviewing)
            {
                return Color.blue;
            }
            else
            {
                return base.GetTint();
            }
        }
    }
}
