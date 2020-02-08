using Dot.Entity.Node;
using DotEditor.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public static class AvatarSkeletonCreatorUtil
    {
        private static readonly string SKELETON_CONFIG_DIR = "Assets/Tools/Entity/Avatar";
        private static readonly string SKELETON_NAME_FORMAT = "{0}_skeleton.prefab";
        public static List<AvatarSkeletonCreatorData> FindAllData()
        {
            List<AvatarSkeletonCreatorData> datas = new List<AvatarSkeletonCreatorData>();
            string[] dataPaths = AssetDatabaseUtil.FindAssetInFolder<AvatarSkeletonCreatorData>(SKELETON_CONFIG_DIR);
            if (dataPaths != null && dataPaths.Length > 0)
            {
                foreach (var dataPath in dataPaths)
                {
                    AvatarSkeletonCreatorData data = AssetDatabase.LoadAssetAtPath<AvatarSkeletonCreatorData>(dataPath);
                    datas.Add(data);
                }
            }
            return datas;
        }

        public static void CreateAllSkeleton(bool isShowProgress = false)
        {
            List<AvatarSkeletonCreatorData> datas = FindAllData();
            if(isShowProgress)
            {
                EditorUtility.DisplayProgressBar("Create Skeleton", "", 0.0f);
            }
            for(int i =0;i<datas.Count;++i)
            {
                var data = datas[i];
                if (isShowProgress)
                {
                    EditorUtility.DisplayProgressBar("Create Skeleton", $"Create:{data.creatorName}", i/(float)datas.Count);
                }
            }
            if (isShowProgress)
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public static void CreateSkeleton(AvatarSkeletonCreatorData creatorData)
        {
            if(creatorData == null)
            {
                Debug.LogError("AvatarSkeletonCreatorUtil::CreateSkeleton->data is null");
                return;
            }
            if(string.IsNullOrEmpty(creatorData.savedAssetDir))
            {
                Debug.LogError("AvatarSkeletonCreatorUtil::CreateSkeleton->dir is null");
                return;
            }
            if(!AssetDatabase.IsValidFolder(creatorData.savedAssetDir))
            {
                Debug.LogError($"AvatarSkeletonCreatorUtil::CreateSkeleton->the folder({creatorData.savedAssetDir}) not found");
                return;
            }

            foreach(var d in creatorData.datas)
            {
                if(d.isEnable)
                {
                    if(IsFBX(d.fbxPrefab))
                    {
                        CreateSkeleton(creatorData.savedAssetDir, d.fbxPrefab);
                    }else
                    {
                        Debug.LogError($"AvatarSkeletonCreatorUtil::CreateSkeleton->the prefab is not a FBX");
                    }
                }
            }
        }

        private static void CreateSkeleton(string savedAssetDir,GameObject fbxGO)
        {
            string skeletonName = string.Format(SKELETON_NAME_FORMAT, fbxGO.name);

            GameObject instanceGO = GameObject.Instantiate<GameObject>(fbxGO);
            instanceGO.name = Path.GetFileNameWithoutExtension(skeletonName);

            NodeBehaviour nodeBehaviour = instanceGO.AddComponent<NodeBehaviour>();

        }

        public static bool IsFBX(GameObject fbxGO)
        {
            if(fbxGO == null)
            {
                return false;
            }

            return PrefabUtility.GetPrefabAssetType(fbxGO) == PrefabAssetType.Model;
        }

    }
}
