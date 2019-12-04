using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.Util
{
    public static class AssemblyUtil
    {
        public static Type GetTypeByFullName(string typeFullName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach(var type in types)
                {
                    if(type.FullName == typeFullName)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

    }
}
