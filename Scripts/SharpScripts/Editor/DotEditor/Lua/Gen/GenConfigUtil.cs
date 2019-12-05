using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public static class GenConfigUtil
    {
        private static string GEN_CONFIG_ASSET_PATH = "Assets/Tools/XLua/gen_config.asset";
        public static GenConfig LoadGenConfig(bool createIfNotExist = true)
        {
            GenConfig genConfig = AssetDatabase.LoadAssetAtPath<GenConfig>(GEN_CONFIG_ASSET_PATH);
            if (genConfig == null && createIfNotExist)
            {
                genConfig = ScriptableObject.CreateInstance<GenConfig>();
                AssetDatabase.CreateAsset(genConfig, GEN_CONFIG_ASSET_PATH);
                AssetDatabase.ImportAsset(GEN_CONFIG_ASSET_PATH);
            }
            return genConfig;
        }
    }
}
