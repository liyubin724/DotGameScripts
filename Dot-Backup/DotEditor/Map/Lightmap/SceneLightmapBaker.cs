using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Map.Lightmap
{
    public class SceneLightmapBaker : EditorWindow
    {
        public static void ShowWin()
        {
            var win = GetWindow<SceneLightmapBaker>();
            win.titleContent = Content.WinTitleContent;
            win.Show();
        }

        void OnGUI()
        {

        }

        class Content
        {
            internal static GUIContent WinTitleContent = new GUIContent("Scene Lightmap Baker");
        }
    }
}
