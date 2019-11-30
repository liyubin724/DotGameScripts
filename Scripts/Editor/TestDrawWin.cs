using Dot.FieldDrawer.Attributes;
using DotEditor.EGUI.FieldDrawer;
using UnityEditor;

public class TestDrawWin : EditorWindow
{
    [MenuItem("Test/TestWin")]
    public static void ShowWin()
    {
        EditorWindow.GetWindow<TestDrawWin>().Show();
    }
    [MemberDesc("Test Class For It")]
    public class TestClass
    {
        [FieldHide]
        public TestEnum enumValue;
        public bool boolValue;
        [FieldOrder(1)]
        public int intValue;
        public float floatValue;
        [FieldMultilineText]
        public string textValue;
        [FieldShow]
        private string stringValue;
        [MemberDesc("Unity Object")]
        public UnityEngine.Object uObj;

        public TestOClass testClass = new TestOClass();
        //[FieldOrder(1)]
        //public int index;
        //[FieldHide]
        //public float floatValue;
        //[FieldMultilineText]
        //[MemberDesc("String Value")]
        //public string strValue;
        //public TestOClass tClass;
        //public int[] intArr;
    }

    public class TestOClass
    {
        public float fValue;
        public int iValue;
        public TestTClass tClass;
    }

    public class TestTClass
    {
        public string strValue;
    }

    public enum TestEnum
    {
        A,
        B,
    }

    private EGUIObjectDrawer drawer;
    private void OnGUI()
    {
        if(drawer == null)
        {
            drawer = new EGUIObjectDrawer(new TestClass(),true);
        }
        drawer.OnGUILayout();
    }
}
