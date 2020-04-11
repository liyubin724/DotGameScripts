using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using System.IO;

namespace DotEditor.EGUI
{
    public static class DEUtility
    {
        private static string EGUI_ROOT_FOLDER_NAME = "Dot EGUI";
        private static string EGUI_ASSET_FOLDER_NAME = "Editor Resources";

        internal static string RootPath { get; private set; }
        internal static string AssetPath { get; private set; }
        internal static string RootRelativePath { get; private set; }
        internal static string AssetRelativePath { get; private set; }
        [InitializeOnLoadMethod]
        private static void Init()
        {
            var dirs = Directory.GetDirectories(Application.dataPath, EGUI_ROOT_FOLDER_NAME, SearchOption.AllDirectories);
            if(dirs == null || dirs.Length == 0)
            {
                return;
            }

            RootPath = dirs[0].Replace('\\', '/');
            RootRelativePath = RootPath.Replace(Application.dataPath, "Assets");

            AssetPath = RootPath + "/" + EGUI_ASSET_FOLDER_NAME;
            AssetRelativePath = AssetPath.Replace(Application.dataPath, "Assets");
        }
    }
}
