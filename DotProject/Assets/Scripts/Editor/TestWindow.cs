﻿using Dot.NativeDrawer.Decorator;
using Dot.NativeDrawer.Layout;
using Dot.NativeDrawer.Property;
using Dot.NativeDrawer.Visible;
using DotEditor.NativeDrawer;
using DotEditor.NativeDrawer.Property;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class TestData
{
    //[Help("Test for it")]
    //[Indent(2)]
    //[BeginGroup("Test")]
    //public int intValue;
    //[Help("Test for it ",HelpMessageType.Error)]
    //[FloatSlider(0,10)]
    //public float floatValue;
    public bool boolValue = false;
    [ShowIf("boolValue")]
    private string strValue = "sss";
    [HideIf("boolValue")]
    public string strValue2 = "ddd";

    //[Help("Object Data")]
    //public InnerData innerData = new InnerData();

    public List<int> intList = new List<int>();
    public int[] intArray = new int[0];
}

public class InnerData
{
    [Help("InnerData int Value")]
    public int iValue;
}


public class TestWindow : EditorWindow
{
    [MenuItem("Test/Test")]
   static void ShowWin()
    {
        EditorWindow.GetWindow<TestWindow>().Show();
    }

    private TestData data = new TestData();

    private NativeDrawerObject drawerObject = null;
    private void OnGUI()
    {
        if (drawerObject == null)
        {
            drawerObject = new NativeDrawerObject(data);
        }

        drawerObject.OnGUILayout();
    }
}
