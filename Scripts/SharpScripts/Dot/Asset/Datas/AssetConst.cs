﻿using Newtonsoft.Json;
using System.IO;

namespace Dot.Asset.Datas
{
    public static class AssetConst
    {
        public const string LOGGER_NAME = "AssetLoader";

        public static readonly string ASSET_MANIFEST_NAME = "manifest_config";
        public static readonly string ASSET_MANIFEST_EXT = ".json";
        public static readonly string ASSET_ADDRESS_NAME = "address_config";
        public static readonly string ASSET_ADDRESS_EXT = ".json";
        public static readonly string ASSET_BUNDLE_PACK_NAME = "bundle_config";
        public static readonly string ASSET_BUNDLE_PACK_EXT = ".json";

        public static readonly string ASSET_BUNDLE_DIR_NAME = "assetbundles";

        public static AssetAddressConfig GetAddressConfig()
        {
            string configPath = AssetConst.AssetAddressConfigPath;

            AssetAddressConfig config = null;
            if (File.Exists(configPath))
            {
                config = JsonConvert.DeserializeObject<AssetAddressConfig>(configPath);
            }

            return config;
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

        public static string BundlePackConfigPath
        {
            get
            {
                return $"{AssetConfigDir}/{ASSET_BUNDLE_PACK_NAME}{ASSET_BUNDLE_PACK_EXT}";
            }
        }
    }
}
