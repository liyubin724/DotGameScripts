using ExtractInject;
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

    public class GenTabOptimizeAssemblies : GenTabAssemblies,IExtractInjectObject
    {

    }

    public class GenTabBlacks : IExtractInjectObject
    {
        public string assemblyName;
        public List<GenTabBlackData> datas = new List<GenTabBlackData>();
    }

    public class GenTabBlackData
    {
        public bool isSelected = false;
        public string typeFullName;
        public string methodName;
        public List<string> paramList = new List<string>();
    }
}
