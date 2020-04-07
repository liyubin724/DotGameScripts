﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Core.Utilities
{
    public static class SelectionUtility
    {
        public static string[] GetSelectionDirs()
        {
            List<string> dirs = new List<string>();
            string[] guids = Selection.assetGUIDs;
            if (guids != null && guids.Length > 0)
            {
                foreach (var guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                    string diskPath = PathUtility.GetDiskPath(assetPath);
                    if(File.Exists(diskPath))
                    {
                        assetPath = Path.GetDirectoryName(assetPath).Replace("\\","/");
                    }
                    dirs.Add(assetPath);
                }
            }

            return dirs.ToArray();
        }

        public static void PingObject(UnityObject uObj)
        {
            EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(uObj);
        }

        public static void PingObject(string assetPath)
        {
            var uObj = AssetDatabase.LoadAssetAtPath<UnityObject>(assetPath);
            PingObject(uObj);
        }

        /// <summary>
        /// 设置在Project中选中的资源
        /// </summary>
        /// <param name="uObj"></param>
        public static void ActiveObject(UnityObject uObj)
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = uObj;
        }

        public static void ActiveObject(string assetPath)
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityObject>(assetPath);
        }

        /// <summary>
        /// 设置在Project中选中的资源
        /// </summary>
        /// <param name="uObjs"></param>
        public static void ActiveObjects(UnityObject[] uObjs)
        {
            EditorUtility.FocusProjectWindow();
            Selection.objects = uObjs;
        }
    }
}