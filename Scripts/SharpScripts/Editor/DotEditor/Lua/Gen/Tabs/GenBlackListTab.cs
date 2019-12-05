using DotEditor.Core.EGUI;
using DotEditor.Util;
using ExtractInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Gen.Tabs
{
    public class GenBlackListTab : AGenTab
    {
        [ExtractInjectField(ExtractInjectUsage.In)]
        public GenConfig genConfig;

        [ExtractInjectField(ExtractInjectUsage.In)]
        public GenTabCallLuaAssemblies tabCallLuaAssemblies;

        [ExtractInjectField(ExtractInjectUsage.In)]
        public GenTabCallCSharpAssemblies tabCallCSharpAssemblies;

        public GenTabBlacks tabBlacks;

        public override void DoEnable()
        {
            base.DoEnable();

            tabBlacks = new GenTabBlacks();

            List<string> allTypeNames = new List<string>();
            allTypeNames.AddRange(genConfig.callCSharpTypeNames);
            allTypeNames.AddRange(genConfig.callLuaTypeNames);
            allTypeNames = allTypeNames.Distinct().ToList();

            foreach(var name in allTypeNames)
            {
                GenTabBlackData bData = new GenTabBlackData();
                bData.typeFullName = name;

                Type type = AssemblyUtil.GetTypeByFullName(name);
                if(type.IsClass)
                {
                    FieldInfo[] fInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                    foreach (var fInfo in fInfos)
                    {
                        GenTabMemberData mData = new GenTabMemberData();
                        mData.memberName = fInfo.Name;
                        mData.memberType = GenTabMemberType.Field;

                        mData.isSelected = genConfig.blackDatas.IndexOf(GetBlackMemberStr(bData, mData)) >= 0;

                        bData.datas.Add(mData);
                    }
                    PropertyInfo[] pInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                    foreach (var pInfo in pInfos)
                    {
                        GenTabMemberData mData = new GenTabMemberData();
                        mData.memberName = pInfo.Name;
                        mData.memberType = GenTabMemberType.Property;

                        mData.isSelected = genConfig.blackDatas.IndexOf(GetBlackMemberStr(bData, mData)) >= 0;

                        bData.datas.Add(mData);
                    }
                }
                
                MethodInfo[] mInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                foreach(var mInfo in mInfos)
                {
                    if(mInfo.Name.StartsWith("set_")||mInfo.Name.StartsWith("get_")||mInfo.Name.StartsWith("op_"))
                    {
                        continue;
                    }
                    GenTabMemberData mData = new GenTabMemberData();
                    mData.memberName = mInfo.Name;
                    mData.memberType = GenTabMemberType.Method;

                    ParameterInfo[] paramInfos = mInfo.GetParameters();
                    foreach(var pi in paramInfos)
                    {
                        mData.paramList.Add(pi.ParameterType.FullName);
                    }

                    mData.isSelected = genConfig.blackDatas.IndexOf(GetBlackMemberStr(bData, mData)) >= 0;

                    bData.datas.Add(mData);
                }

                tabBlacks.datas.Add(bData);
            }
        }

        private Vector2 scrollPos = Vector2.zero;
        public override void DoGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    foreach (var bData in tabBlacks.datas)
                    {
                        if(IsShowBlack(bData))
                        {
                            DrawBlackData(bData);
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void DrawBlackData(GenTabBlackData bData)
        {
            bData.isFoldout = EditorGUILayout.Foldout(bData.isFoldout, bData.typeFullName, true);
            if (bData.isFoldout)
            {
                EditorGUIUtil.BeginIndent();
                {
                    foreach (var mData in bData.datas)
                    {
                        if(string.IsNullOrEmpty(searchText) || (mData.memberName.ToLower().IndexOf(searchText)>=0))
                        {
                            DrawMemberData(bData,mData);
                        }
                    }
                }
                EditorGUIUtil.EndIndent();
            }
        }

        private void DrawMemberData(GenTabBlackData bData,GenTabMemberData mData)
        {
            string label = $"{mData.memberType.ToString()}  {mData.memberName}";
            if(mData.memberType == GenTabMemberType.Method)
            {
                label += $":[{string.Join(",", mData.paramList.ToArray())}]";
            }

            bool isSelected = EditorGUILayout.ToggleLeft(label, mData.isSelected);
            if (isSelected != mData.isSelected)
            {
                mData.isSelected = isSelected;

                UpdateGenConfig(bData, mData);
            }
        }

        private void UpdateGenConfig(GenTabBlackData bData,GenTabMemberData mData)
        {
            string blackStr = GetBlackMemberStr(bData, mData);
            if(mData.isSelected)
            {
                genConfig.blackDatas.Add(blackStr);
            }else
            {
                genConfig.blackDatas.Remove(blackStr);
            }
        }
        
        private bool IsShowBlack(GenTabBlackData bData)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }
            return bData.datas.Any((d) =>
            {
                return d.memberName.ToLower().IndexOf(searchText) >= 0;
            });
        }

        private string GetBlackMemberStr(GenTabBlackData bData, GenTabMemberData mData)
        {
            string backStr = $"{bData.typeFullName}@{mData.memberName}";
            if (mData.paramList.Count > 0)
            {
                backStr += $"@{string.Join("$", mData.paramList.ToArray())}";
            }
            return backStr;
        }

        public override void DoSearch(string searchText)
        {
            this.searchText = searchText.ToLower();
            foreach (var bData in tabBlacks.datas)
            {
                bData.isFoldout = !string.IsNullOrEmpty(searchText);
            }
        }
    }
}
