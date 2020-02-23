using DotEditor.Core.EGUI;
using ExtractInject;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Gen.Tabs
{
    public class GenCSharpCallLuaTab : AGenTab
    {
        [EIField(EIFieldUsage.In)]
        public GenConfig genConfig;

        [EIField(EIFieldUsage.In)]
        public GenTabCallLuaAssemblies tabAssemblies;

        private GenGenericDrawer genericDrawer;

        public override void DoEnable()
        {
            base.DoEnable();
            genericDrawer = new GenGenericDrawer(genConfig.callLuaGenericTypeNames);
        }

        private Vector2 scrollPos = Vector2.zero;
        public override void DoGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                if (tabAssemblies == null || genConfig == null)
                {
                    EditorGUILayout.LabelField("Data Is Null", DotEditorStyles.BoldLabelStyle);
                }
                else
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        foreach (var aData in tabAssemblies.datas)
                        {
                            if (IsShowTabAssembly(aData))
                            {
                                aData.isFoldout = EditorGUILayout.Foldout(aData.isFoldout, aData.assemblyName, true);
                                if (aData.isFoldout)
                                {
                                    DrawTabAssembly(aData);
                                }
                            }
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Generic Type List", DotEditorStyles.BoldLabelStyle);
                        EditorGUILayout.LabelField("Example:List<int> == System.Collections.Generic.List`1@System.Int32");
                        genericDrawer.DoGUILayout();
                    }
                    EditorGUILayout.EndScrollView();
                }
            }
            GUILayout.EndArea();
        }

        public override void DoSearch(string searchText)
        {
            this.searchText = searchText.ToLower();
            foreach (var aData in tabAssemblies.datas)
            {
                aData.isFoldout = !string.IsNullOrEmpty(searchText);
            }
        }

        private bool IsShowTabAssembly(GenTabAssemblyData data)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }
            return data.typeDatas.Any((d) =>
            {
                return d.typeFullName.ToLower().IndexOf(searchText) >= 0;
            });
        }

        private void DrawTabAssembly(GenTabAssemblyData aData)
        {
            DotEditorGUI.BeginIndent();
            {
                foreach (var tData in aData.typeDatas)
                {
                    if (string.IsNullOrEmpty(searchText) || tData.typeFullName.ToLower().IndexOf(searchText) >= 0)
                    {
                        DrawTabTypeData(tData);
                    }
                }
            }
            DotEditorGUI.EndIndent();
        }

        private void DrawTabTypeData( GenTabTypeData tData)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                bool isSelected = EditorGUILayout.ToggleLeft(tData.typeFullName, tData.isSelected);
                if (isSelected != tData.isSelected)
                {
                    tData.isSelected = isSelected;

                    UpdateGenConfig(tData);
                }
                if (GUILayout.Button("Copy Type Name", GUILayout.Width(160)))
                {
                    GUIUtility.systemCopyBuffer = tData.typeFullName;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void UpdateGenConfig(GenTabTypeData tData)
        {
            if (tData.isSelected)
            {
                if (genConfig.callLuaTypeNames.IndexOf(tData.typeFullName) < 0)
                {
                    genConfig.callLuaTypeNames.Add(tData.typeFullName);
                }
            }
            else
            {
                if (genConfig.callLuaTypeNames.IndexOf(tData.typeFullName) >= 0)
                {
                    genConfig.callLuaTypeNames.Remove(tData.typeFullName);
                }
            }
        }
    }
}
