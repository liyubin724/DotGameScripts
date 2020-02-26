using Dot.Ini;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class PrefabTest : EditorWindow
{
    [MenuItem("Test/TestPrefab")]
    static void ShowWin()
    {
        var win = GetWindow<PrefabTest>();
        win.Show();

        string v = "A[A,B,C,]";
        string pattern = @"(?<key>\w+)\[(?<option>[(\w+,?)]+)\]$";
        Match match = new Regex(pattern).Match(v);
        if (match.Success)
        {
            Debug.LogError($"Key = {match.Groups["key"].Value}  Vlaues = {match.Groups["option"].Value}");
        }
    }

    GameObject gObj = null;
    StringBuilder sb = new StringBuilder();

    private TextAsset ta = null;
    private void OnGUI()
    {
        ta = (TextAsset)EditorGUILayout.ObjectField("Text:", ta, typeof(TextAsset), false);
        if(GUILayout.Button("Init"))
        {
            IniConfig config = new IniConfig(ta.text, false);
            Debug.Log("Ini===>" + config.GetValueInGroup("Default", "email"));
            Debug.Log("Ini===>" + config.GetValue("defaultLanguage"));
        }


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
