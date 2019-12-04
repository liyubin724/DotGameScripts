using ExtractInject;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class GenConfig : ScriptableObject,IExtractInjectObject
    {
        public List<string> callCSharpTypeNames = new List<string>();
        public List<string> callLuaTypeNames = new List<string>();
        public List<string> optimizeTypeNames = new List<string>();

        public List<GenBlackData> blackDatas = new List<GenBlackData>();

        [Serializable]
        public class GenBlackData
        {
            public string typeFullName;
            public string methodName;
            public List<string> paramList = new List<string>();
        }
    }
}
