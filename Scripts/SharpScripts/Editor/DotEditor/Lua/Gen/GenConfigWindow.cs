using DotEditor.Core.Util;
using DotEditor.Lua.Gen.Tabs;
using ExtractInject;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static DotEditor.Lua.Gen.GenConfig;

namespace DotEditor.Lua.Gen
{
    public class GenConfigWindow : EditorWindow
    {
        [MenuItem("Game/XLua/Gen Window")]
        public static void ShowWin()
        {
            var win = EditorWindow.GetWindow<GenConfigWindow>();
            win.titleContent = new GUIContent("Gen Config Window");
            win.Show();
        }

        private static int TOOLBAR_HEIGHT = 18;
        private static int SPACE_HEIGHT = 10;
        private static int TOOLBAR_BTN_HEIGHT = 40;
        private static string GEN_CONFIG_ASSET_PATH = "Assets/Tools/XLua/gen_config.asset";

        private string[] excludeAssemblyNames = new string[]
        {
            "UnityEditor",
            "nunit.framework",
            "UnityEngine.TestRunner",
        };

        private ExtractInjectContext context = new ExtractInjectContext();

        private GUIContent[] toolbarContents = new GUIContent[]
        {
            new GUIContent("LuaCallCSharp"),
            new GUIContent("CSharpCallLua"),
            new GUIContent("GCOptimize"),
            new GUIContent("BlackList"),
        };
        private AGenTab[] tabs = null;

        private SearchField searchField = null;
        private string searchText = string.Empty;
        private void OnEnable()
        {
            LoadGenConfig();
            CreateTabData();

            if (searchField == null)
            {
                searchField = new SearchField();
                searchField.autoSetFocusOnFindCommand = true;
            }

            if (tabs==null)
            {
                tabs = new AGenTab[]
                {
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
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar,GUILayout.Height(TOOLBAR_HEIGHT),GUILayout.ExpandWidth(true));
            {
                GUILayout.FlexibleSpace();
                string newSearchText = searchField.OnToolbarGUI(searchText, GUILayout.Width(160));
                if(newSearchText != searchText)
                {
                    searchText = newSearchText;
                    tabs[toolbarSelectIndex].DoSearch(searchText);
                }
            }
            EditorGUILayout.EndHorizontal();

            int newIndex = GUILayout.Toolbar(toolbarSelectIndex, toolbarContents, GUILayout.Height(TOOLBAR_BTN_HEIGHT),GUILayout.ExpandWidth(true));
            if(toolbarSelectIndex !=newIndex)
            {
                tabs[toolbarSelectIndex].Extract(context);
                toolbarSelectIndex = newIndex;
                tabs[toolbarSelectIndex].Inject(context);
                tabs[toolbarSelectIndex].DoEnable();

                searchText = "";
            }
            int y = TOOLBAR_HEIGHT + TOOLBAR_BTN_HEIGHT + SPACE_HEIGHT;
            tabs[toolbarSelectIndex].DoGUI(new Rect(0, y, position.width, position.height - y));
        }

        private GenConfig genConfig;

        private void LoadGenConfig()
        {
            genConfig = AssetDatabase.LoadAssetAtPath<GenConfig>(GEN_CONFIG_ASSET_PATH);
            if(genConfig == null)
            {
                genConfig = ScriptableObject.CreateInstance<GenConfig>();
                AssetDatabase.CreateAsset(genConfig, GEN_CONFIG_ASSET_PATH);
                AssetDatabase.ImportAsset(GEN_CONFIG_ASSET_PATH);
            }
            context.AddObject<GenConfig>(genConfig);
        }

        private void SaveGenConfig()
        {
            EditorUtility.SetDirty(genConfig);
            AssetDatabase.SaveAssets();
        }

        private void OnDestroy()
        {
            SaveGenConfig();
        }

        private void CreateAssemblies(GenTabAssemblies genAssemblies, List<GenTypeData> typeDatas)
        {
            var assemlies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemlies)
            {
                string assemblyName = assembly.GetName().Name;
                if (Array.IndexOf(excludeAssemblyNames, assemblyName) >= 0)
                {
                    continue;
                }
                if (assemblyName.IndexOf("Editor") >= 0)
                {
                    continue;
                }

                GenTypeData typeData = null;
                foreach(var td in typeDatas)
                {
                    if(td.assemblyName == assemblyName)
                    {
                        typeData = td;
                        break;
                    }
                }

                GenTabAssemblyData tabAssemblyData = new GenTabAssemblyData();
                tabAssemblyData.assemblyName = assemblyName;
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if(type.IsGenericType || type.IsNotPublic || type.IsInterface || type.IsAbstract)
                    {
                        continue;
                    }
                    if(type.IsNested && !type.IsNestedPublic)
                    {
                        continue;
                    }

                    GenTabTypeData tabTypeData = new GenTabTypeData();
                    tabTypeData.typeFullName = type.FullName;
                    tabTypeData.isSelected = typeData==null?false:typeData.typeFullNames.Any((data) => { return data == type.FullName; });

                    tabAssemblyData.typeDatas.Add(tabTypeData);
                }
                if(tabAssemblyData.typeDatas.Count>0)
                {
                    genAssemblies.datas.Add(tabAssemblyData);
                }
            }

            genAssemblies.Sort();
        }

        private void CreateTabData()
        {
            GenTabCallCSharpAssemblies callCSharpAssemblies = new GenTabCallCSharpAssemblies();
            context.AddObject<GenTabCallCSharpAssemblies>(callCSharpAssemblies);

            CreateAssemblies(callCSharpAssemblies, genConfig.callCSharpDatas);

            GenTabCallLuaAssemblies callLuaAssemblies = new GenTabCallLuaAssemblies();
            context.AddObject<GenTabCallLuaAssemblies>(callLuaAssemblies);

            CreateAssemblies(callLuaAssemblies, genConfig.callLuaDatas);
        }
    }
}
