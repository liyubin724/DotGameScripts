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
    [EnumButton]
    public TEnumType enumType = TEnumType.A;
    [EnumButton]
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
    [MultilineText]
    public string strValue2 = "dddsssssssssssssssssssssssssssssssssss";

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

    private void Awake()
    {
        NativeDrawerSetting.IsShowDecorator = true;
    }
    private void OnGUI()
    {
        //string[] enumNames = Enum.GetNames(typeof(TEnumType));
        //Array arr = Enum.GetValues(typeof(TEnumType));
        //EditorGUILayout.BeginHorizontal();
        //{
        //    foreach(var a in arr)
        //    {
        //        EditorGUILayout.LabelField(a.ToString()+"   "+a.GetType().ToString());
        //    }
        //    foreach(var en in enumNames)
        //    {
        //        GUILayout.Toggle(true, en,EditorStyles.toolbarButton);
        //    }
        //}
        //EditorGUILayout.EndHorizontal();


        if (drawerObject == null)
        {
            drawerObject = new NativeDrawerObject(data);
            drawerObject.IsShowScroll = true;
        }

        drawerObject.OnGUILayout();
    }
}
