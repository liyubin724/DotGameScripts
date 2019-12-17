using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Lua.Loader
{
    public static class ProtobufFileLoader
    {
        public static byte[] LoadProtobuf(string name)
        {
            return File.ReadAllBytes(GetProtobufPath(name));
        }

        private static string GetProtobufPath(string name)
        {
            return $"{LuaConfig.LuaDiskDirPath}/Dot/Net/{name}.pb";
        }
    }
}
