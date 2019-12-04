﻿using DotEditor.Core.EGUI;
using ExtractInject;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Gen.Tabs
{
    public class GenCSharpCallLuaTab : AGenTab
    {
        [ExtractInjectField(ExtractInjectUsage.In)]
        public GenConfig genConfig;

        [ExtractInjectField(ExtractInjectUsage.In)]
        public GenTabCallLuaAssemblies tabAssemblies;

        private Vector2 scrollPos = Vector2.zero;
        public override void DoGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                if (tabAssemblies == null || genConfig == null)
                {
                    EditorGUILayout.LabelField("Data Is Null", EditorGUIStyle.BoldLabelStyle);
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
            EditorGUIUtil.BeginIndent();
            {
                foreach (var tData in aData.typeDatas)
                {
                    if (string.IsNullOrEmpty(searchText) || tData.typeFullName.ToLower().IndexOf(searchText) >= 0)
                    {
                        DrawTabTypeData(tData);
                    }
                }
            }
            EditorGUIUtil.EndIndent();
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
