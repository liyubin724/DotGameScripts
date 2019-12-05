using DotEditor.Lua.Gen;
using DotEditor.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TestAssemblyWin : EditorWindow
{
    [MenuItem("Test/Assembly Win")]
    public static void ShowWin()
    {
        EditorWindow.GetWindow<TestAssemblyWin>().Show();
    }

    [MenuItem("Test/Test Gen")]
    public static void TestGen()
    {
        List<Type> types1 = XLuaGenConfig.GetCSharpCallLuaTypeList;
        List<Type> types2 = XLuaGenConfig.GetLuaCallCSharpTypeList;
        List<Type> types3 = XLuaGenConfig.GetGCOptimizeTypeList;

        List<List<string>> list4 = XLuaGenConfig.GetBlackList;

        Debug.Log(types1.Count);
        Debug.Log(types2.Count);
        Debug.Log(types3.Count);
        Debug.Log(list4.Count);
    }

    private Assembly[] assemblies = new Assembly[0];
    private void OnEnable()
    {
        assemblies = AppDomain.CurrentDomain.GetAssemblies();
    }

    private Vector2 scrollPos = Vector2.zero;
    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            foreach(var assembly in assemblies)
            {
                EditorGUILayout.LabelField(assembly.GetName().Name);
            }
        }
        EditorGUILayout.EndScrollView();
    }
}
