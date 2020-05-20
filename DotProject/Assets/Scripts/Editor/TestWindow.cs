using Dot.NativeDrawer;
using Dot.NativeDrawer.Decorator;
using Dot.NativeDrawer.Layout;
using Dot.NativeDrawer.Listener;
using Dot.NativeDrawer.Property;
using Dot.NativeDrawer.Visible;
using DotEditor.GUIExtension;
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
    [StringPopup(IsSearchable = true,Options = new string[] { "A", "B", "C", "D", "E", })]
    public string strValue = "A";
    
    
    
    //[OpenFilePath(IsAbsolute =false)]
    //public string filePath1;
    //[OpenFilePath(IsAbsolute = true)]
    //public string filePath2;
    
    //[SpaceLine]

    //[SeparatorLine]
    //[OpenFilePath(IsAbsolute = false,Extension ="txt")]
    //public string filePath3;
    //[OpenFilePath(IsAbsolute = true,Extension ="txt")]
    //public string filePath4;
    
    //[SeparatorLine]
    //[OpenFilePath(IsAbsolute = false,Filters =new string[] { "CSharp", "cs", "All Files", "*" })]
    //public string filePath5;
    //[OpenFilePath(IsAbsolute = true, Filters = new string[] { "CSharp", "cs", "All Files", "*" })]
    //public string filePath6;


    
    //[EnumButton]
    //[OnValueChanged("OnEnumTypeChanged")]
    //public TEnumType enumType = TEnumType.A;
    //[Readonly]
    //[EnumButton]
    //public TFlagEnumType flagEnumType = TFlagEnumType.E;

    //[Help("Test for it")]
    //[Indent(2)]
    //[BeginGroup("Test")]
    //public int intValue;
    //[Help("Test for it ", HelpMessageType.Error)]
    //[FloatSlider(0, 10)]
    //public float floatValue;
    //public string nullStrValue;
    //public bool boolValue = false;
    //[ShowIf("boolValue",true)]
    //private string strValue = "sss";
    //[EndGroup]
    //[HideIf("boolValue",true)]
    //[MultilineText]
    //public string strValue2 = "dddsssssssssssssssssssssssssssssssssss";

    //[Help("Object Data")]
    //public InnerData innerData = new InnerData();

    //public List<InnerData> innerDataList = new List<InnerData>();
    //public InnerData[] innerDataArr = new InnerData[0];

    //public List<int> intList = new List<int>();
    //public int[] intArray = new int[0];

    //public void OnEnumTypeChanged()
    //{
    //    if(enumType == TEnumType.A)
    //    {
    //        flagEnumType = TFlagEnumType.E;
    //    }else if(enumType == TEnumType.B)
    //    {
    //        flagEnumType = TFlagEnumType.F;
    //    }else if(enumType == TEnumType.C)
    //    {
    //        flagEnumType = TFlagEnumType.E | TFlagEnumType.F;
    //    }else if(enumType == TEnumType.D)
    //    {
    //        flagEnumType = TFlagEnumType.G;
    //    }
    //}
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
        var win =EditorWindow.GetWindow<TestWindow>();
        win.wantsMouseMove = true;
        win.Show();

    }

    private TestData data = new TestData();

    private NativeDrawerObject drawerObject = null;

    private void Awake()
    {
        NativeDrawerSetting.IsShowDecorator = true;
        drawerObject = new NativeDrawerObject(data);
        drawerObject.IsShowScroll = true;
    }

    public string strValue = "A";
    private string[] options = new string[] { "A", "B", "C", "D", "E", };
    private Rect ddbRect = Rect.zero;
    private void OnGUI()
    {
        drawerObject.OnGUILayout();



        //Vector2 mousePosition = EditorGUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        
        //EditorGUILayout.LabelField("guiPosition" + Event.current.mousePosition + " screenPosition = " + mousePosition+"  dbb = "+ddbRect);
        //string value = strValue;
        //EditorGUI.BeginChangeCheck();
        //{
        //    EditorGUILayout.BeginHorizontal();
        //    {
        //        EditorGUILayout.PrefixLabel("StrValue" + "  " + mousePosition);
        //        Rect lastRect = GUILayoutUtility.GetRect(new GUIContent(value), "dropdownbutton");
        //        if(Event.current.type == EventType.Repaint)
        //        {
        //            ddbRect = lastRect;
        //        }
        //        if (EditorGUI.DropdownButton(lastRect,new GUIContent(value), FocusType.Keyboard))
        //        {
        //            try
        //            {
        //                SearchablePopup.Show(ddbRect, Array.IndexOf(options, value), options, (selected) =>
        //                {
        //                    value = options[selected];
        //                    strValue = value;
        //                    Repaint();
        //                });
        //            }
        //            catch (ExitGUIException)
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //    EditorGUILayout.EndHorizontal();
        //}
        //if (EditorGUI.EndChangeCheck())
        //{
        //    strValue = value;
        //}


    }
}
