using Dot.Core.Logger;
using System.IO;
using XLua;

namespace Dot.Lua.Loader
{
    public static class LuaScriptFileLoader
    {
        private static string[] ms_FilePathFormats = new string[0];
        public static void ScriptLoadFromFile(LuaEnv luaEnv,string[] filePathFormats)
        {
            ms_FilePathFormats = filePathFormats;

            luaEnv.AddLoader(LoadFromFile);
        }

        private static byte[] LoadFromFile(ref string filePath)
        {
            if(ms_FilePathFormats!=null && ms_FilePathFormats.Length > 0)
            {
                foreach(var f in ms_FilePathFormats)
                {
                    string fp = string.Format(f, filePath);
                    if(File.Exists(fp))
                    {
                        filePath = fp;
                        return File.ReadAllBytes(fp);
                    }
                }
            }

            DebugLogger.LogError($"LuaScriptFileLoader::LoadFromFile->Script not found.filePath = {filePath}");

            return null;
        }
    }
}
