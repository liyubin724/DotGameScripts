using Dot.Entity.Node;
using DotEditor.Entity.Node;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;

namespace DotEditor.Entity.Avatar
{
    public static class AvatarCreatorUtil
    {
        public static GameObject CreateSkeleton(SkeletonCreatorData data)
        {
            if(data == null)
            {
                Debug.LogError("AvatarCreatorUtil::CreateSkeleton->the data is null");
                return null;
            }

            if(data.fbx == null)
            {
                Debug.LogError("AvatarCreatorUtil::CreateSkeleton->The fbx is null");
                return null;
            }

            PrefabAssetType assetType = UnityEditor.PrefabUtility.GetPrefabAssetType(data.fbx);
            if (assetType != PrefabAssetType.Model)
            {
                Debug.LogError($"AvatarCreatorUtil::CreateSkeleton->The fbx is not a model.type = {assetType}");
                return null;
            }

            if(string.IsNullOrEmpty(data.outputFolder))
            {
                Debug.LogError("AvatarCreatorUtil::CreateSkeleton->The outputFolder is empty");
                return null;
            }

            string outputDiskFolder = PathUtility.GetDiskPath(data.outputFolder);
            if(!Directory.Exists(outputDiskFolder))
            {
                Directory.CreateDirectory(outputDiskFolder);
            }

            string skeletonPrefabAssetPath = data.GetTargetPrefabPath();
            GameObject cachedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(skeletonPrefabAssetPath);
            NodeBehaviour cachedNodeBehaviour = cachedPrefab?.GetComponent<NodeBehaviour>();

            GameObject instanceGameObject = GameObject.Instantiate<GameObject>(data.fbx);
            NodeBehaviour instanceNodeBehaviour = instanceGameObject.AddComponent<NodeBehaviour>();

            instanceNodeBehaviour.FindBoneNodes();
            instanceNodeBehaviour.FindSMRendererNodes();
            if(cachedNodeBehaviour!=null)
            {
                NodeBehaviourUtil.CopyBindNodes(cachedNodeBehaviour, instanceNodeBehaviour);
            }

            if(instanceNodeBehaviour.smRendererNodes!=null && instanceNodeBehaviour.smRendererNodes.Length>0)
            {
                foreach(var nodeData in instanceNodeBehaviour.smRendererNodes)
                {
                    SkinnedMeshRenderer smr = nodeData.renderer;
                    if(smr != null)
                    {
                        smr.sharedMaterials = new Material[0];
                        smr.rootBone = null;
                        smr.sharedMesh = null;
                        smr.bones = new Transform[0];
                    }
                }
            }

            UnityEditor.PrefabUtility.SaveAsPrefabAsset(instanceGameObject, skeletonPrefabAssetPath);
            GameObject.DestroyImmediate(instanceGameObject);

            AssetDatabase.ImportAsset(skeletonPrefabAssetPath);

            return AssetDatabase.LoadAssetAtPath<GameObject>(skeletonPrefabAssetPath);
        }
    }
}
