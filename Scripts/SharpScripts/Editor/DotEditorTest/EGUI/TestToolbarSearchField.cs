using Dot.FieldDrawer;
using DotEditor.EGUI;
using DotEditor.EGUI.FieldDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditorTest.EGUI
{
    public enum TestEnum
    {
        None,
        Int,
    }
    public class TestData
    {
        [FieldDesc("Int value")]
        public int IntValue;
        [FieldDesc("str value")]
        [FieldReadonly]
        public string StrValue = "Test";
        public float floatValue;
        public bool boolValue;
        public List<int> intList;
        public List<float> floatList;
        [FieldMultilineText(60)]
        public List<string> strList;
        public TestEnum testEnum;

        public TestData2 data2;
    }

    public class TestData2
    {
        public int IntValue;
        public string StrValue;
    }

    public class TestToolbarSearchField : EditorWindow
    {
        [MenuItem("Test/toolbarsearchfield")]
        public static void ShowWin()
        {
            EditorWindow.GetWindow<TestToolbarSearchField>().Show();
        }

        private EGUIToolbarSearchField searchField = null;

        private string text = "";
        private string category = "";

        public TestData testData = new TestData();
        ObjectDrawer drawer = null;
        private void OnGUI()
        {
            if(drawer == null)
            {
                drawer = new ObjectDrawer("TestData",testData);
            }
            drawer.OnGUI(true);
        }
    }
}
