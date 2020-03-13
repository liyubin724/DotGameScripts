using Dot.Entity.Avatar;
using Dot.XNodeEx;
using UnityEngine;
using XNode;

namespace DotEditor.Entity.Avatar.Preview
{
    public class AvatarPreviewNode : DotNode {
        [Input]
        public GameObject skeletonPrefab = null;
        [Input]
        public AvatarPartData headPart = null;
        [Input]
        public AvatarPartData bodyPart = null;
        [Input]
        public AvatarPartData handPart = null;
        [Input]
        public AvatarPartData feetPart = null;
        [Input]
        public AvatarPartData weaponPart = null;

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            AvatarPreviewGraph previewGraph = GetGraph<AvatarPreviewGraph>();
            string fieldName = to.fieldName;
            if(fieldName == "skeletonPrefab")
            {
                previewGraph.CreatePreview(
                        GetInputValue<GameObject>("skeletonPrefab"),
                        GetInputValue<AvatarPartData>("headPart"),
                        GetInputValue<AvatarPartData>("bodyPart"),
                        GetInputValue<AvatarPartData>("handPart"),
                        GetInputValue<AvatarPartData>("feetPart"),
                        GetInputValue<AvatarPartData>("weaponPart"));
            }
            else if(fieldName.EndsWith("Part"))
            {
                AvatarPartData partData = to.GetInputValue<AvatarPartData>();
                if (previewGraph.isPreviewing)
                {
                    previewGraph.AssemblePart(partData);
                }
            }
        }

        public override void OnRemoveConnection(NodePort from, NodePort to)
        {
            AvatarPreviewGraph previewGraph = GetGraph<AvatarPreviewGraph>();
            string fieldName = to.fieldName;
            if (fieldName == "skeletonPrefab")
            {
                previewGraph.DestroyPreview();
            }
            else if (fieldName.EndsWith("Part"))
            {
                AvatarPartData partData = from.GetOutputValue<AvatarPartData>();
                if (previewGraph.isPreviewing)
                {
                    previewGraph.DisassemblePart(partData);
                }
            }
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port) {
		    return null; // Replace this
	    }
    }

}