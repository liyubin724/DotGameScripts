using Dot.NativeDrawer.Decorator;
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
    public TEnumType enumType = TEnumType.A;
    public TFlagEnumType flagEnumType = TFlagEnumType.E;

    [Help("Test for it")]
    [Indent(2)]
    [BeginGroup("Test")]
    public int intValue;
    [Help("Test for it ", HelpMessageType.Error)]
    [FloatSlider(0, 10)]
    public float floatValue;
    public string nullStrValue;
    public bool boolValue = false;
    [ShowIf("boolValue")]
    private string strValue = "sss";
    [EndGroup]
    [HideIf("boolValue")]
    public string strValue2 = "ddd";

    [Help("Object Data")]
    public InnerData innerData = new InnerData();

    public List<InnerData> innerDataList = new List<InnerData>();
    public InnerData[] innerDataArr = new InnerData[0];

    public List<int> intList = new List<int>();
    public int[] intArray = new int[0];
}

public class InnerData
{
    [Help("InnerData int Value")]
    public int iValue;
}

public enum TEnumType
{
    A,
    B,
    C,
    D,
}

[Flags]
public enum TFlagEnumType
{
    E = 1<<0,
    F = 1<<1,
    G = 1<<2,
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
            drawerObject.IsShowScroll = true;
        }

        drawerObject.OnGUILayout();
    }
}
