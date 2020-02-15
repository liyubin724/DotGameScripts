using Dot.Entity.Node;
using UnityEngine;

namespace Dot.Entity.Avatar
{
    public static class AvatarUtil
    {
        public static AvatarPartInstance AssembleAvatarPart(NodeBehaviour nodeBehaviour, AvatarPartData partData)
        {
            AvatarPartInstance partInstance = new AvatarPartInstance();
            partInstance.partType = partData.partType;
            partInstance.gameObjects = new GameObject[partData.prefabParts.Length];
            for (int i = 0; i < partData.prefabParts.Length; ++i)
            {
                var prefabData = partData.prefabParts[i];

                GameObject bindGameObject = GameObject.Instantiate(prefabData.prefabGO);
                NodeData bindNodeData = nodeBehaviour.GetNode(NodeType.BindNode, prefabData.bindNodeName);
                bindGameObject.transform.SetParent(bindNodeData.transform, false);

                partInstance.gameObjects[i] = bindGameObject;
            }

            partInstance.renderers = new SkinnedMeshRenderer[partData.rendererParts.Length];
            for (int i = 0; i < partData.rendererParts.Length; ++i)
            {
                var rendererData = partData.rendererParts[i];

                SkinnedMeshRenderer smRenderer = nodeBehaviour.GetNode(NodeType.SMRendererNode, rendererData.rendererNodeName).renderer;
                smRenderer.rootBone = nodeBehaviour.GetNode(NodeType.BoneNode, rendererData.rootBoneName).transform;
                smRenderer.bones = nodeBehaviour.GetBoneByNames(rendererData.boneNames);
                smRenderer.sharedMesh = rendererData.mesh;
                smRenderer.sharedMaterials = rendererData.materials;

                partInstance.renderers[i] = smRenderer;
            }

            return partInstance;
        }

        public static void DisassembleAvatarPart(AvatarPartInstance partInstance, bool isInEditorMode = false)
        {
            foreach (var go in partInstance.gameObjects)
            {
                if(isInEditorMode)
                {
                    GameObject.DestroyImmediate(go);
                }else
                {
                    GameObject.Destroy(go);
                }
            }
            foreach (var smr in partInstance.renderers)
            {
                smr.sharedMaterials = new Material[0];
                smr.rootBone = null;
                smr.sharedMesh = null;
                smr.bones = new Transform[0];
            }
        }
    }
}
