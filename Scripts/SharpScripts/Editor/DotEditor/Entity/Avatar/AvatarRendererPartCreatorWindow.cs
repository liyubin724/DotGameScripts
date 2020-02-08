using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarRendererPartCreatorWindow :EditorWindow
    {
        internal static void ShowWin()
        {
            var win = EditorWindow.GetWindow<AvatarRendererPartCreatorWindow>();
            win.titleContent = Contents.WinTitleContent;
            win.Show();
        }


        class Contents
        {
            internal static GUIContent WinTitleContent = new GUIContent("Renderer Part Creator");
            internal static GUIContent FBXListTitleContent = new GUIContent("FBX List");

            internal static GUIContent RefreshBtnContent = new GUIContent("Refresh");
            internal static GUIContent NewBtnContent = new GUIContent("New");

            internal static GUIContent CreateSkeletonContent = new GUIContent("Create Skeleton");

            internal static GUIContent CreatorNameContent = new GUIContent("Creator Name");
            internal static string SavedAssetDirContent = "Saved Asset Dir";

            internal static GUIContent IsEnableContent = new GUIContent("Is Enable");
            internal static GUIContent FBXContent = new GUIContent("FBX");
        }
    }
}
