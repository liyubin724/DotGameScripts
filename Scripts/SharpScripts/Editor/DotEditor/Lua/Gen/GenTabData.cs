﻿using ExtractInject;
using System.Collections.Generic;
using System.Linq;

namespace DotEditor.Lua.Gen
{
    public class GenTabTypeData
    {
        public bool isSelected = false;
        public string typeFullName;
    }

    public class GenTabAssemblyData
    {
        public string assemblyName;
        public bool isFoldout = false;
        public List<GenTabTypeData> typeDatas = new List<GenTabTypeData>();

        public void Sort()
        {
            typeDatas.Sort((item1, item2) =>
            {
                return item1.typeFullName.CompareTo(item2.typeFullName);
            });
        }
    }

    public class GenTabAssemblies
    {
        public List<GenTabAssemblyData> datas = new List<GenTabAssemblyData>();

        public void Sort()
        {
            datas.Sort((item1, item2) =>
            {
                return item1.assemblyName.CompareTo(item2.assemblyName);
            });
            foreach(var d in datas)
            {
                d.Sort();
            }
        }
    }

    public class GenTabCallCSharpAssemblies : GenTabAssemblies ,IExtractInjectObject
    {
    }

    public class GenTabCallLuaAssemblies : GenTabAssemblies,IExtractInjectObject
    {

    }

    public class GenTabOptimize
    {
        public List<GenTabOptimizeData> datas = new List<GenTabOptimizeData>();
    }

    public class GenTabOptimizeData
    {
        public string typeFullName;
        public bool isSelected = false;
    }

    public class GenTabBlacks
    {
        public List<GenTabBlackData> datas = new List<GenTabBlackData>();
    }

    public class GenTabBlackData
    {
        public string typeFullName;
        public bool isFoldout = false;
        public List<GenTabMemberData> datas = new List<GenTabMemberData>();
    }

    public enum GenTabMemberType
    {
        None = 0,
        Field = 1,
        Property = 2,
        Method = 3,
    }

    public class GenTabMemberData
    {
        public bool isSelected = false;
        public string memberName;
        public GenTabMemberType memberType = GenTabMemberType.None;
        public List<string> paramList = new List<string>();
    }
}