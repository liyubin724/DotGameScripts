using DotEditor.GUIExtension.ListView;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class TestListViewData : ISimpleListViewItem
{
    public string name;
    public int id;

    public string GetDisplayName()
    {
        return name;    
    }

    public int GetID()
    {
        return id;
    }
}


public class LearnTreeView : EditorWindow
{
    [MenuItem("Test/Open TreeView")]
    static void ShowWin()
    {
        EditorWindow.GetWindow<LearnTreeView>().Show();
    }

    private SimpleListView<TestListViewData> sListView = null;

    private List<TestListViewData> datas = new List<TestListViewData>();
    private void Awake()
    {
        datas.Add(new TestListViewData()
        {
            id = 1,
            name = "A",
        });
        datas.Add(new TestListViewData()
        {
            id = 2,
            name = "B",
        });
        datas.Add(new TestListViewData()
        {
            id = 3,
            name = "C",
        });
        datas.Add(new TestListViewData()
        {
            id = 4,
            name = "D",
        });
        datas.Add(new TestListViewData()
        {
            id = 5,
            name = "E",
        });

        sListView = new SimpleListView<TestListViewData>(datas);
        sListView.Header = "Test Data List";
        sListView.GetHeight = (TestListViewData data) =>
        {
            return EditorGUIUtility.singleLineHeight * 2;
        };
        sListView.OnDrawItem = (rect, data) =>
        {
            Rect drawRect = rect;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.IntField(drawRect, "id",data.id);
            drawRect.y += drawRect.height;

            EditorGUI.LabelField(drawRect,"Name", data.name);
        };
        sListView.OnItemSelected = (data) =>
        {
            Debug.Log(data.name);
        };
        sListView.Reload();
    }

    private void OnGUI()
    {
        sListView.OnGUI(new Rect(0, 0, position.width, position.height));
    }
}
