using DotEditor.Utilities;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExtension
{
    public static class EGUIUtility
    {
        public static readonly float singleLineHeight = EditorGUIUtility.singleLineHeight;
        public static readonly float standSpacing = EditorGUIUtility.standardVerticalSpacing;
        public static readonly float boxFrameSize = 6.0f;
        public static readonly float padding = 5.0f;

        public static T CreateAsset<T>() where T:ScriptableObject
        {
            string filePath = EditorUtility.SaveFilePanel("Save Asset", Application.dataPath, "", "asset");
            if(string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            string fileAssetPath = PathUtility.GetAssetPath(filePath);
            if(AssetDatabase.LoadAssetAtPath<UnityObject>(fileAssetPath) !=null)
            {
                Debug.LogError($"EGUIUtility::CreateAsset->The file is exist in \"{fileAssetPath}\"");
                return null;
            }else
            {
                T data = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(data, fileAssetPath);

                AssetDatabase.ImportAsset(fileAssetPath);

                return data;
            }
        }

    }
}
