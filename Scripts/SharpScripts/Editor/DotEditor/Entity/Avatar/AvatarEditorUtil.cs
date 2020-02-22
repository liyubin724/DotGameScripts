using Dot.Entity.Avatar;
using Dot.Entity.Node;
using DotEditor.Entity.Avatar.Preview;
using DotEditor.Entity.Node;
using DotEditor.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;

namespace DotEditor.Entity.Avatar
{
    public static class AvatarEditorUtil
    {
        internal static readonly string CREATOR_DATA_DIR = "Assets/Tools/Entity/Avatar/Creator";
        internal static readonly string PREVIEW_DATA_DIR = "Assets/Tools/Entity/Avatar/Preview";
        internal static readonly string TEMPLATE_PREVIEW_DATA_PAT = "Assets/Tools/Entity/Avatar/Preview/preview_graph.asset";

        internal static readonly string CREATOR_DATA_DEFAULT_NAME = "avatar_creator";

        private static readonly string SKELETON_NAME_FORMAT = "{0}_skeleton.prefab";
        private static readonly string PART_NAME_FORMAT = "{0}_part.asset";
        private static readonly string PREFAB_NAME_FORMAT = "{0}_prefab.asset";
        private static readonly string RENDERER_NAME_FORMAT = "{0}_renderer.asset";
        private static readonly string RENDERER_MESH_NAME_FORMAT = "{0}_mesh.asset";

        public static List<AvatarCreatorData> FindCreatorDatas()
        {
            List<AvatarCreatorData> datas = new List<AvatarCreatorData>();
            string[] dataPaths = AssetDatabaseUtil.FindAssetInFolder<AvatarCreatorData>(CREATOR_DATA_DIR);
            if (dataPaths != null && dataPaths.Length > 0)
            {
                foreach (var dataPath in dataPaths)
                {
                    AvatarCreatorData data = AssetDatabase.LoadAssetAtPath<AvatarCreatorData>(dataPath);
                    datas.Add(data);
                }
            }
            return datas;
        }

        public static void CreateAvatar(AvatarCreatorData data)
        {
            foreach(var d in data.skeletonCreatorDatas)
            {
                CreateSkeleton(d);
            }

            foreach(var d in data.partCreatorDatas)
            {
                CreatePart(d);
            }
        }

        public static GameObject CreateSkeleton(AvatarSkeletonCreatorData data)
        {
            if(data == null || !data.isEnable || data.fbxPrefab == null)
            {
                return null;
            }

            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(data.fbxPrefab);
            if(assetType != PrefabAssetType.Model)
            {
                return null;
            }
            return CreateSkeleton(data.savedDir, data.fbxPrefab);
        }

        private static GameObject CreateSkeleton(string savedAssetDir, GameObject fbxGO)
        {
            string skeletonName = string.Format(SKELETON_NAME_FORMAT, fbxGO.name);
            string skeletonPath = $"{savedAssetDir}/{skeletonName}";

            NodeBehaviour prefabNodeBehaviour = AssetDatabase.LoadAssetAtPath<NodeBehaviour>(skeletonPath);

            GameObject instanceGO = GameObject.Instantiate<GameObject>(fbxGO);
            instanceGO.name = Path.GetFileNameWithoutExtension(skeletonName);

            NodeBehaviour nodeBehaviour = instanceGO.AddComponent<NodeBehaviour>();
            NodeBehaviourEditorUtil.AutoFindBoneNode(nodeBehaviour);
            NodeBehaviourEditorUtil.AutoFindRendererNode(nodeBehaviour);
            if (prefabNodeBehaviour != null)
            {
                NodeBehaviourEditorUtil.CopyBindNodeFrom(prefabNodeBehaviour, nodeBehaviour);
            }
            SkinnedMeshRenderer[] renderers = instanceGO.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            if (renderers != null && renderers.Length > 0)
            {
                for (int i = 0; i < renderers.Length; ++i)
                {
                    SkinnedMeshRenderer smr = renderers[i];
                    smr.sharedMaterials = new Material[0];
                    smr.rootBone = null;
                    smr.sharedMesh = null;
                    smr.bones = new Transform[0];
                }
            }
            PrefabUtility.SaveAsPrefabAsset(instanceGO, skeletonPath);

            GameObject.DestroyImmediate(instanceGO);

            return AssetDatabase.LoadAssetAtPath<GameObject>(skeletonPath);
        }

        public static AvatarPartData CreatePart(AvatarPartCreatorData data)
        {
            if(!data.isEnable || string.IsNullOrEmpty(data.dataName) 
                || data.partType<= AvatarPartType.None || data.partType>= AvatarPartType.Max
                ||string.IsNullOrEmpty(data.savedDir) || !AssetDatabase.IsValidFolder(data.savedDir))
            {
                return null;
            }

            string assetPath = $"{data.savedDir}/{string.Format(PART_NAME_FORMAT, data.dataName)}";
            AvatarPartData partData = AssetDatabase.LoadAssetAtPath<AvatarPartData>(assetPath);
            if(partData == null)
            {
                partData = ScriptableObject.CreateInstance<AvatarPartData>();
                AssetDatabase.CreateAsset(partData, assetPath);
            }
            partData.partType = data.partType;

            List<AvatarPrefabPartData> prefabPartDatas = new List<AvatarPrefabPartData>();
            foreach(var d in data.prefabPartDatas)
            {
                AvatarPrefabPartData ppData = CreatePrefabPart(d);
                if(ppData!=null)
                {
                    prefabPartDatas.Add(ppData);
                }
            }
            partData.prefabParts = prefabPartDatas.ToArray();

            List<AvatarRendererPartData> rendererPartDatas = new List<AvatarRendererPartData>();
            foreach(var d in data.rendererPartDatas)
            {
                AvatarRendererPartData[] rpDatas = CreateRendererPart(d);
                if(rpDatas!=null)
                {
                    rendererPartDatas.AddRange(rpDatas);
                }
            }
            partData.rendererParts = rendererPartDatas.ToArray();

            EditorUtility.SetDirty(partData);

            return partData;
        }

        public static AvatarPrefabPartData CreatePrefabPart(AvatarPrefabCreatorData data)
        {
            if(!data.isEnable || data.bindPrefab == null || string.IsNullOrEmpty(data.bindNodeName) || string.IsNullOrEmpty(data.savedDir) || !AssetDatabase.IsValidFolder(data.savedDir))
            {
                return null;
            }

            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(data.bindPrefab);
            if (assetType != PrefabAssetType.Regular)
            {
                return null;
            }
            string assetPath = $"{data.savedDir}/{string.Format(PREFAB_NAME_FORMAT, data.dataName)}";
            AvatarPrefabPartData partData = AssetDatabase.LoadAssetAtPath<AvatarPrefabPartData>(assetPath);
            if(partData == null)
            {
                partData = ScriptableObject.CreateInstance<AvatarPrefabPartData>();
                AssetDatabase.CreateAsset(partData, assetPath);
            }

            partData.bindNodeName = data.bindNodeName;
            partData.prefabGO = data.bindPrefab;

            EditorUtility.SetDirty(partData);

            return partData;
        }

        public static AvatarRendererPartData[] CreateRendererPart(AvatarRendererCreatorData data)
        {
            if(!data.isEnable || data.fbxPrefab == null || string.IsNullOrEmpty(data.savedDir) || !AssetDatabase.IsValidFolder(data.savedDir))
            {
                return null;
            }

            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(data.fbxPrefab);
            if(assetType != PrefabAssetType.Model)
            {
                return null;
            }

            List<AvatarRendererPartData> partDatas = new List<AvatarRendererPartData>();

            ModelImporter modelImporter = (ModelImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(data.fbxPrefab));

            SkinnedMeshRenderer[] renderers = data.fbxPrefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach(var renderer in renderers)
            {
                string assetPath = $"{data.savedDir}/{string.Format(RENDERER_NAME_FORMAT,renderer.name.ToLower())}";
                AvatarRendererPartData partData = AssetDatabase.LoadAssetAtPath<AvatarRendererPartData>(assetPath);
                if(partData == null)
                {
                    partData = ScriptableObject.CreateInstance<AvatarRendererPartData>();
                    AssetDatabase.CreateAsset(partData, assetPath);
                }

                partData.rendererNodeName = renderer.name;
                partData.rootBoneName = renderer.rootBone.name;
                partData.materials = renderer.sharedMaterials;
                partData.boneNames = (from bone in renderer.bones select bone.name).ToArray();

                string meshAssetPath = $"{data.savedDir}/{string.Format(RENDERER_MESH_NAME_FORMAT, renderer.name.ToLower())}";

                partData.mesh = CreateRendererMesh(meshAssetPath, renderer.sharedMesh, modelImporter.meshCompression);

                EditorUtility.SetDirty(partData);

                partDatas.Add(partData);
            }
            return partDatas.ToArray();
        }

        private static Mesh CreateRendererMesh(string assetPath,Mesh mesh, ModelImporterMeshCompression compression)
        {
            Mesh savedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
            if(savedMesh != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
            savedMesh = UnityEngine.Object.Instantiate<Mesh>(mesh);
            if(compression!= ModelImporterMeshCompression.Off)
            {
                MeshUtility.SetMeshCompression(savedMesh, compression);
            }
            MeshUtility.Optimize(savedMesh);
            AssetDatabase.CreateAsset(savedMesh, assetPath);
            return savedMesh;
        }

        public static AvatarPreviewGraph CreatePreview(AvatarCreatorData data)
        {
            string newPreviewGraphPath = $"{PREVIEW_DATA_DIR}/{data.dataName}_preview_graph.asset";
            AvatarPreviewGraph newPreviewGraph = AssetDatabase.LoadAssetAtPath<AvatarPreviewGraph>(newPreviewGraphPath);
            if(newPreviewGraph ==null)
            {
                AvatarPreviewGraph previewGraph = AssetDatabase.LoadAssetAtPath<AvatarPreviewGraph>(TEMPLATE_PREVIEW_DATA_PAT);
                if (previewGraph == null)
                {
                    return null;
                } 
                newPreviewGraph = (AvatarPreviewGraph)previewGraph.Copy();
                AssetDatabase.CreateAsset(newPreviewGraph, newPreviewGraphPath);

                foreach (var node in newPreviewGraph.nodes)
                {
                    node.name = node.name.Replace("(Clone)", "");
                    AssetDatabase.AddObjectToAsset(node,newPreviewGraph);
                }

                AssetDatabase.ImportAsset(newPreviewGraphPath);
            }
            newPreviewGraph.skeletonList.Clear();
            newPreviewGraph.partList.Clear();

            foreach (var d in data.skeletonCreatorDatas)
            {
                string skeletonPath = $"{d.savedDir}/{string.Format(SKELETON_NAME_FORMAT, d.fbxPrefab.name)}";

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(skeletonPath);
                if(prefab!=null)
                {
                    newPreviewGraph.skeletonList.Add(prefab);
                }
            }

            foreach (var d in data.partCreatorDatas)
            {
                string assetPath = $"{d.savedDir}/{string.Format(PART_NAME_FORMAT, d.dataName)}";

                AvatarPartData partData = AssetDatabase.LoadAssetAtPath<AvatarPartData>(assetPath);
                if(partData!=null)
                {
                    newPreviewGraph.partList.Add(partData);
                }
            }

            EditorUtility.SetDirty(newPreviewGraph);
            return newPreviewGraph;
        }
    }
}
