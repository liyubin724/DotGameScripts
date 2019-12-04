using ExtractInject;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class GenConfig : ScriptableObject,IExtractInjectObject
    {
        public List<GenTypeData> callCSharpDatas = new List<GenTypeData>();
        public List<GenTypeData> callLuaDatas = new List<GenTypeData>();
        public List<GenTypeData> optimizeDatas = new List<GenTypeData>();

        public List<GenBlackData> blackDatas = new List<GenBlackData>();

        [Serializable]
        public class GenTypeData
        {
            public string assemblyName;
            public List<string> typeFullNames = new List<string>();
        }

        [Serializable]
        public class GenBlackData
        {
            public string assemblyName;
            public string typeFullName;
            public string methodName;
            public List<string> paramList = new List<string>();
        }
    }
}
