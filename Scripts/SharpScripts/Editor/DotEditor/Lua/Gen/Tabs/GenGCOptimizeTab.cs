using DotEditor.Util;
using ExtractInject;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Gen.Tabs
{
    public class GenGCOptimizeTab : AGenTab
    {
        [EIField(EIFieldUsage.In)]
        public GenConfig genConfig;

        [EIField(EIFieldUsage.In)]
        public GenTabCallLuaAssemblies tabCallLuaAssemblies;

        [EIField(EIFieldUsage.In)]
        public GenTabCallCSharpAssemblies tabCallCSharpAssemblies;

        public GenTabOptimize tabOptimize;

        public override void DoEnable()
        {
            base.DoEnable();
            tabOptimize = new GenTabOptimize();

            List<string> allTypeNames = new List<string>();
            allTypeNames.AddRange(genConfig.callCSharpTypeNames);
            allTypeNames.AddRange(genConfig.callLuaTypeNames);
            allTypeNames = allTypeNames.Distinct().ToList();

            for(int i =genConfig.optimizeTypeNames.Count-1;i>=0;i--)
            {
                if(allTypeNames.IndexOf(genConfig.optimizeTypeNames[i])<0)
                {
                    genConfig.optimizeTypeNames.RemoveAt(i);
                }
            }

            foreach(var name in allTypeNames)
            {
                Type type = AssemblyUtil.GetTypeByFullName(name);
                if(type.IsEnum || (type.IsValueType && !type.IsPrimitive))
                {
                    GenTabOptimizeData oData = new GenTabOptimizeData();
                    oData.typeFullName = name;
                    oData.isSelected = genConfig.optimizeTypeNames.IndexOf(name) >= 0;

                    tabOptimize.datas.Add(oData);
                }
            }
        }

        private Vector2 scrollPos = Vector2.zero;
        public override void DoGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    foreach (var oData in tabOptimize.datas)
                    {
                        if(!string.IsNullOrEmpty(searchText) && oData.typeFullName.IndexOf(searchText)<0)
                        {
                            continue;
                        }

                        bool isSelected = EditorGUILayout.ToggleLeft(oData.typeFullName, oData.isSelected);
                        if(isSelected!=oData.isSelected)
                        {
                            oData.isSelected = isSelected;
                            if(oData.isSelected)
                            {
                                genConfig.optimizeTypeNames.Add(oData.typeFullName);
                            }else
                            {
                                genConfig.optimizeTypeNames.Remove(oData.typeFullName);
                            }
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        public override void DoSearch(string searchText)
        {
            this.searchText = searchText.ToLower();
        }
    }
}
