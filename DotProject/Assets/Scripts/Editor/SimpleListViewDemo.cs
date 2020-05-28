﻿using DotEditor.GUIExtension.ListView;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestListViewData
{
    public string name;
    public int id;

    public override string ToString()
    {
        return $"{id}:{name}";
    }
}


public class SimpleListViewDemo : EditorWindow
{
    [MenuItem("Test/SimpleListViewDemo")]
    static void ShowWin()
    {
        EditorWindow.GetWindow<SimpleListViewDemo>().Show();
    }

    private List<TestListViewData> datas = new List<TestListViewData>();
    private SimpleListView<TestListViewData> sListView = null;

    private List<string> strDatas = new List<string>()
    {
        "A","B","C","D","E"
    };

    private SimpleListView<string> strDataListView = null;

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
        sListView.GetItemHeight = (index) =>
        {
            return EditorGUIUtility.singleLineHeight * 2;
        };
        sListView.OnDrawItem = (rect, index) =>
        {
            Rect drawRect = rect;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            var data = sListView.GetItem(index);

            EditorGUI.IntField(drawRect, "id", data.id);
            drawRect.y += drawRect.height;

            EditorGUI.LabelField(drawRect, "Name", data.name);
        };
        sListView.OnSelectedChange = (index) =>
        {
            var data = sListView.GetItem(index);
            Debug.Log(data.name);
        };
        sListView.Reload();


        strDataListView = new SimpleListView<string>(strDatas);
        strDataListView.Header = "Str List";
        strDataListView.Reload();
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Reload"))
        {
            sListView.Reload();
        }
        if(GUILayout.Button("Add"))
        {
            datas.Add(new TestListViewData()
            {
                id = 6,
                name = "F",
            });
            sListView.Reload();
        }
        if (GUILayout.Button("Remove"))
        {
            datas.RemoveAt(1);
            sListView.Reload();
        }
        Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        //sListView.OnGUI(rect);
        strDataListView.OnGUI(rect);
    }
}