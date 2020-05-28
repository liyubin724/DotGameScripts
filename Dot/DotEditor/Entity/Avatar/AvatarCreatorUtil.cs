using Dot.Entity.Avatar;
using Dot.Entity.Node;
using DotEditor.Entity.Node;
using DotEditor.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Dot.Entity.Avatar.AvatarPartData;
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

        public static AvatarPartData CreatePart(string outputFolder,PartCreatorData data)
        {
            if(data == null)
            {
                Debug.LogError("AvatarCreatorUtil::CreatePart->the data is null");
                return null;
            }

            if(string.IsNullOrEmpty(outputFolder))
            {
                Debug.LogError("AvatarCreatorUtil::CreatePart->The outputFolder is empty");
                return null;
            }

            if(string.IsNullOrEmpty(data.name))
            {
                Debug.LogError("AvatarCreatorUtil::CreatePart->The name is empty");
                return null;
            }

            string partAssetPath = data.GetTargetPath(outputFolder);
            
            AvatarPartData partData = ScriptableObject.CreateInstance<AvatarPartData>();
            partData.name = data.name;
            partData.partType = data.partType;

            List<AvatarPrefabPartData> prefabPartDatas = new List<AvatarPrefabPartData>();
            foreach(var prefabCreatorData in data.prefabDatas)
            {
                if(string.IsNullOrEmpty(prefabCreatorData.bindNodeName))
                {
                    Debug.LogError("AvatarCreatorUtil::CreatePart->The bindNodeName is empty");
                    return null;
                }

                if(prefabCreatorData.bindPrefab == null)
                {
                    Debug.LogError("AvatarCreatorUtil::CreatePart->The bindPrefab is null");
                    return null;
                }

                PrefabAssetType assetType = UnityEditor.PrefabUtility.GetPrefabAssetType(prefabCreatorData.bindPrefab);
                if (assetType != PrefabAssetType.Regular)
                {
                    Debug.LogError($"AvatarCreatorUtil::CreatePart->The bindPrefab is not a Prefab.type = {assetType}");
                    return null;
                }

                AvatarPrefabPartData prefabPartData = new AvatarPrefabPartData();
                prefabPartData.bindNodeName = prefabCreatorData.bindNodeName;
                prefabPartData.prefabGO = prefabCreatorData.bindPrefab;
                prefabPartDatas.Add(prefabPartData);
            }
            partData.prefabParts = prefabPartDatas.ToArray();

            List<AvatarRendererPartData> rendererPartDatas = new List<AvatarRendererPartData>();
            foreach(var rendererCreatorData in data.smRendererDatas)
            {
                if(rendererCreatorData.partFbx == null)
                {
                    Debug.LogError("AvatarCreatorUtil::CreatePart->The fbx is null");
                    return null;
                }

                PrefabAssetType assetType = UnityEditor.PrefabUtility.GetPrefabAssetType(rendererCreatorData.partFbx);
                if (assetType != PrefabAssetType.Model)
                {
                    Debug.LogError($"AvatarCreatorUtil::CreatePart->The fbx is not a model.type = {assetType}");
                    return null;
                }

                SkinnedMeshRenderer[] renderers = rendererCreatorData.partFbx.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                foreach(var renderer in renderers)
                {
                    AvatarRendererPartData rendererPartData = new AvatarRendererPartData();
                    rendererPartData.rendererNodeName = renderer.name;
                    rendererPartData.rootBoneName = renderer.rootBone.name;
                    rendererPartData.boneNames = (from bone in renderer.bones select bone.name).ToArray();
                    rendererPartData.materials = renderer.sharedMaterials;

                    Mesh mesh = renderer.sharedMesh;
                    if(rendererCreatorData.IsCopyMesh)
                    {
                        mesh = Utilities.MeshUtility.CopyMeshTo(mesh, $"{outputFolder}/{mesh.name}_mesh.asset");
                    }
                    rendererPartData.mesh = mesh;

                    rendererPartDatas.Add(rendererPartData);
                }
            }
            partData.rendererParts = rendererPartDatas.ToArray();


            AssetDatabase.CreateAsset(partData, partAssetPath);
            AssetDatabase.ImportAsset(partAssetPath);

            return partData;
        }
    }
}
