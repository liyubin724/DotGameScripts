using DotEditor.EGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DemoWin : EditorWindow
{
    [MenuItem("Test/Demo Win")]
    static void ShowWin()
    {
        EditorWindow.GetWindow<DemoWin>().Show();
    }

    private void OnGUI()
    {
        //EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        {
            DEGUILayout.DrawBoxHeader("Just for Test",GUILayout.ExpandWidth(true));
        }
        //EditorGUILayout.EndVertical();
    }
}
