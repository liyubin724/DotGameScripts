using ExtractInject;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class GenConfig : ScriptableObject,IExtractInjectObject
    {
        public List<string> callCSharpTypeNames = new List<string>();
        public List<string> callLuaTypeNames = new List<string>();
        public List<string> optimizeTypeNames = new List<string>();

        public List<string> blackDatas = new List<string>();
    }
}
