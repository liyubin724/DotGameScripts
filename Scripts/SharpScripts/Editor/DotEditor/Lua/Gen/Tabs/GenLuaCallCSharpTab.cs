using DotEditor.Core.EGUI;
using ExtractInject;
using UnityEditor;
using UnityEngine;
using static DotEditor.Lua.Gen.GenConfig;

namespace DotEditor.Lua.Gen.Tabs
{
    public class GenLuaCallCSharpTab : AGenTab
    {
        [ExtractInjectField(ExtractInjectUsage.In)]
        public GenConfig genConfig;

        [ExtractInjectField(ExtractInjectUsage.In)]
        public GenTabCallCSharpAssemblies tabAssemblies;

        private Vector2 scrollPos = Vector2.zero;
        public override void DoGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                if(tabAssemblies == null || genConfig == null)
                {
                    EditorGUILayout.LabelField("Data Is Null",EditorGUIStyle.BoldLabelStyle);
                }else
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        foreach(var aData in tabAssemblies.datas)
                        {
                            aData.isFoldout = EditorGUILayout.Foldout(aData.isFoldout, aData.assemblyName, true);
                            if(aData.isFoldout)
                            {
                                EditorGUIUtil.BeginIndent();
                                {
                                    foreach (var tData in aData.typeDatas)
                                    {
                                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                        {
                                            bool isSelected = EditorGUILayout.ToggleLeft(tData.typeFullName, tData.isSelected);
                                            if (isSelected != tData.isSelected)
                                            {
                                                tData.isSelected = isSelected;

                                                GenTypeData gtData = null;
                                                foreach (var d in genConfig.callCSharpDatas)
                                                {
                                                    if (d.assemblyName == aData.assemblyName)
                                                    {
                                                        gtData = d;
                                                        break;
                                                    }
                                                }
                                                if (tData.isSelected)
                                                {
                                                    if (gtData == null)
                                                    {
                                                        gtData = new GenTypeData();
                                                        gtData.assemblyName = aData.assemblyName;
                                                        genConfig.callCSharpDatas.Add(gtData);
                                                    }
                                                    gtData.typeFullNames.Add(tData.typeFullName);
                                                }
                                                else
                                                {
                                                    if (gtData != null)
                                                    {
                                                        gtData.typeFullNames.Remove(tData.typeFullName);
                                                        if (gtData.typeFullNames.Count == 0)
                                                        {
                                                            genConfig.callCSharpDatas.Remove(gtData);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        EditorGUILayout.EndHorizontal();
                                    }
                                }
                                EditorGUIUtil.EndIndent();
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
            Debug.Log(searchText);
        }
    }
}
