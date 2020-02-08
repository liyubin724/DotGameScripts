using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class PrefabTest : EditorWindow
{
    [MenuItem("Test/TestPrefab")]
    static void ShowWin()
    {
        var win = GetWindow<PrefabTest>();
        win.Show();
    }

    GameObject gObj = null;
    StringBuilder sb = new StringBuilder();
    private void OnGUI()
    {
        gObj = (GameObject)EditorGUILayout.ObjectField("Obj:", gObj, typeof(GameObject), true);
        if(GUILayout.Button("Check"))
        {
            sb.Clear();
            sb.AppendLine($"AssetPath = {AssetDatabase.GetAssetPath(gObj)}");
            sb.AppendLine($"GetPrefabAssetPathOfNearestInstanceRoot = {PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gObj)}");
            
            sb.AppendLine($"IsPartOfPrefabAsset = {PrefabUtility.IsPartOfPrefabAsset(gObj)}");
            sb.AppendLine($"IsPartOfPrefabInstance = {PrefabUtility.IsPartOfPrefabInstance(gObj)}");
            sb.AppendLine($"IsPartOfAnyPrefab = {PrefabUtility.IsPartOfAnyPrefab(gObj)}");
            sb.AppendLine($"IsPartOfRegularPrefab = {PrefabUtility.IsPartOfRegularPrefab(gObj)}");
        }

        if(sb.Length>0)
        {
            EditorGUILayout.LabelField(sb.ToString(),GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
        }
    }
}
