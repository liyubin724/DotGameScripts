using DotEditor.Core.EGUI;
using DotEditor.Util;
using ExtractInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
                
                FieldInfo[] fInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                foreach(var fInfo in fInfos)
                {
                    GenTabMethodData mData = new GenTabMethodData();
                    mData.methodName = fInfo.Name;
                    bData.datas.Add(mData);
                }
                PropertyInfo[] pInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                foreach(var pInfo in pInfos)
                {
                    GenTabMethodData mData = new GenTabMethodData();
                    mData.methodName = pInfo.Name;
                    bData.datas.Add(mData);
                }
                MethodInfo[] mInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                foreach(var mInfo in mInfos)
                {
                    GenTabMethodData mData = new GenTabMethodData();
                    mData.methodName = mInfo.Name;

                    ParameterInfo[] paramInfos = mInfo.GetParameters();
                    foreach(var pi in paramInfos)
                    {
                        mData.paramList.Add(pi.ParameterType.FullName);
                    }
                    bData.datas.Add(mData);
                }

                tabBlacks.datas.Add(bData);
            }
        }

        public override void DoGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                foreach(var bData in tabBlacks.datas)
                {
                    bData.isFoldout = EditorGUILayout.Foldout(bData.isFoldout, bData.typeFullName, true);
                    if(bData.isFoldout)
                    {
                        EditorGUIUtil.BeginIndent();
                        {
                            foreach(var mData in bData.datas)
                            {
                                bool isSelected = EditorGUILayout.ToggleLeft($"{mData.methodName}:[{string.Join(",", mData.paramList.ToArray())}]",mData.isSelected);
                                if(isSelected != mData.isSelected)
                                {
                                    mData.isSelected = isSelected;
                                }
                            }
                        }
                        EditorGUIUtil.EndIndent();
                    }
                }
            }
            GUILayout.EndArea();
        }

        public override void DoSearch(string searchText)
        {

        }
    }
}
