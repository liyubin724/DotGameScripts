using DotEditor.Utilities;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExtension
{
    public static class EGUIUtility
    {
        public static readonly float singleLineHeight = EditorGUIUtility.singleLineHeight;
        public static readonly float standSpacing = EditorGUIUtility.standardVerticalSpacing;
        public static readonly float boxFrameSize = 6.0f;
        public static readonly float padding = 5.0f;

        public static T CreateAsset<T>(bool deleteIfExist = true) where T:ScriptableObject
        {
            string filePath = EditorUtility.SaveFilePanel("Save Data To", Application.dataPath, "", "asset");
            if(string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            string fileAssetPath = PathUtility.GetAssetPath(filePath);
            if(File.Exists(filePath))
            {
                if (deleteIfExist)
                {
                    AssetDatabase.DeleteAsset(fileAssetPath);
                }
                else
                {
                    return AssetDatabase.LoadAssetAtPath<T>(fileAssetPath);
                }
            }

            T data = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(data, fileAssetPath);

            return data;
        }

    }
}
