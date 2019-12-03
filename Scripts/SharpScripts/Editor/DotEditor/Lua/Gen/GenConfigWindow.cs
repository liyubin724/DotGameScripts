using Dot.EGUI;
using DotEditor.Lua.Gen.Tabs;
using ExtractInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public enum GenTabType
    {
        GenAssembly = 0,
        GenLuaCallCSharp,
        GenCSharpCallLua,
        GenGCOptimize,
        GenBlackList,
    }

    public class GenConfigWindow : EditorWindow
    {
        [MenuItem("Game/XLua/Gen Window")]
        public static void ShowWin()
        {
            var win = EditorWindow.GetWindow<GenConfigWindow>();
            win.titleContent = new GUIContent("Gen Config Window");
            win.wantsMouseMove = true;
            win.Show();
        }
        private static int ToolbarBtnHeight = 40;

        private ExtractInjectContext context = new ExtractInjectContext();

        private GUIContent[] toolbarContents = new GUIContent[]
        {
            new GUIContent("Assembly"),
            new GUIContent("LuaCallCSharp"),
            new GUIContent("CSharpCallLua"),
            new GUIContent("GCOptimize"),
            new GUIContent("BlackList"),
        };
        private AGenTab[] tabs = null;

        private AutocompleteSearchField searchField = null;
        private void OnEnable()
        {
            if (searchField == null)
            {
                searchField = new AutocompleteSearchField();
                searchField.onConfirm = (searchText) =>
                {
                    tabs[toolbarSelectIndex].DoSearch(searchText);
                };
                searchField.onInputChanged = (searchText) =>
                {
                    var results = tabs[toolbarSelectIndex].GetSearchResult(searchText);
                    searchField.ClearResults();
                    foreach(var result in results)
                    {
                        searchField.AddResult(result);
                    }
                };
            }

            if (tabs==null)
            {
                tabs = new AGenTab[]
                {
                    new GenAssemblyTab(),
                    new GenLuaCallCSharpTab(),
                    new GenCSharpCallLuaTab(),
                    new GenGCOptimizeTab(),
                    new GenBlackListTab(),
                };
            }

            toolbarSelectIndex = 0;
            tabs[toolbarSelectIndex].Inject(context);
        }

        private int toolbarSelectIndex = -1;
        private void OnGUI()
        {
            int newIndex = GUILayout.Toolbar(toolbarSelectIndex, toolbarContents, GUILayout.Height(ToolbarBtnHeight),GUILayout.ExpandWidth(true));
            if(toolbarSelectIndex !=newIndex)
            {
                toolbarSelectIndex = newIndex;
                tabs[toolbarSelectIndex].Inject(context);
            }
            
            searchField.OnGUI();
            
        }
    }
}
