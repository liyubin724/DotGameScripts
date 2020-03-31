using Dot.Asset.Datas;
using Dot.Log;
using Newtonsoft.Json;
using System.IO;

namespace Dot.Asset
{
    public static class AssetConst
    {
        public const string LOGGER_NAME = "DotAsset";

        public static readonly string ASSET_ADDRESS_CONFIG_NAME = "asset_address_config.asset";
        public static readonly string ASSET_ADDRESS_BUNDLE_NAME = "asset_address_config_asset";

        public static readonly string ASSET_BUNDLE_CONFIG_NAME = "asset_bundle_config.json";
        


        public static readonly string ASSET_MANIFEST_NAME = "manifest_config";
        public static readonly string ASSET_MANIFEST_EXT = ".json";
        
        public static readonly string ASSET_ADDRESS_NAME = "address_config";
        public static readonly string ASSET_ADDRESS_EXT = ".json";

        public static AssetAddressConfig GetAddressConfig()
        {
            string configPath = AssetConst.AssetAddressConfigPath;
            if (!File.Exists(configPath))
            {
                LogUtil.LogError(LOGGER_NAME, $"File not found.path = {configPath}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(configPath);
                return JsonConvert.DeserializeObject<AssetAddressConfig>(json);
            }
            catch
            {
                LogUtil.LogError(LOGGER_NAME, $"Deserialize error.path = {configPath}");
                return null;
            }
        }

        public static AssetBundleConfig GetBundleConfig(string bundleDir)
        {
            string bundleConfigPath = $"{bundleDir}/{ASSET_MANIFEST_NAME}{ASSET_MANIFEST_EXT}";
            if(!File.Exists(bundleConfigPath))
            {
                LogUtil.LogError(LOGGER_NAME, $"File not found.bundleDir = {bundleDir},path = {bundleConfigPath}");
                return null;
            }
            try
            {
                string configContent = File.ReadAllText(bundleConfigPath);
                return JsonConvert.DeserializeObject<AssetBundleConfig>(configContent);
            }
            catch
            {
                LogUtil.LogError(LOGGER_NAME, $"Deserialize error.bundleDir = {bundleDir},path = {bundleConfigPath}");
                return null;
            }
        }

        public static string AssetConfigDir
        {
            get
            {
                return $"{Path.GetFullPath(".").Replace("\\","/")}/AssetConfig";
            }
        }

        public static string AssetAddressConfigPath
        {
            get
            {
                return $"{AssetConfigDir}/{ASSET_ADDRESS_NAME}{ASSET_ADDRESS_EXT}";
            }
        }
    }
}
