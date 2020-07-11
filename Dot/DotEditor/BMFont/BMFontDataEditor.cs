using DotEngine.BMFont;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BMFont
{
    [CustomEditor(typeof(BMFontData))]
    public class BMFontDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("");
            if(GUILayout.Button("Editor"))
            {

            }
            EditorGUI.BeginDisabledGroup(true);
            {
                base.OnInspectorGUI();
            }
            EditorGUI.EndDisabledGroup();
        }

    }
}
