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
