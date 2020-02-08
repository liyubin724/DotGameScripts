using Dot.Entity.Avatar;
using DotEditor.Util;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarPartCreator
    {
        [MenuItem("Game/Entity/Avatar/Create Part",priority =0)]
        public static void CreatePartData()
        {
            AvatarPartData partData = ScriptableObject.CreateInstance<AvatarPartData>();
            string filePath = EditorUtility.SaveFilePanel("Save", Application.dataPath, "avatar_part", "asset");
            if(!string.IsNullOrEmpty(filePath))
            {
                string assetPath = PathUtil.GetAssetPath(filePath);
                AssetDatabase.CreateAsset(partData, assetPath);
                AssetDatabase.ImportAsset(assetPath);
            }
        }

        [MenuItem("Game/Entity/Avatar/Create Prefab Part", priority = 1)]
        public static void CreatePrefabPartData()
        {
            AvatarPrefabPartData partData = ScriptableObject.CreateInstance<AvatarPrefabPartData>();
            string filePath = EditorUtility.SaveFilePanel("Save", Application.dataPath, "avatar_prefab_part", "asset");
            if (!string.IsNullOrEmpty(filePath))
            {
                string assetPath = PathUtil.GetAssetPath(filePath);
                AssetDatabase.CreateAsset(partData, assetPath);
                AssetDatabase.ImportAsset(assetPath);
            }
        }
        [MenuItem("Game/Entity/Avatar/Create Skeleton", priority = 2)]
        public static void CreateSkeleton()
        {
            AvatarSkeletonCreatorWindow.ShowWin();
        }

        [MenuItem("Game/Entity/Avatar/Create Renderer Part", priority = 3)]
        public static void CreateRendererPartData()
        {

        }
    }
}
