using Dot.Asset.Datas;
using DotEditor.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using static Dot.Asset.Datas.AssetAddressConfig;

namespace DotEditor.AssetFilter.AssetAddress
{
    public static class AssetAddressUtil
    {
        public static AssetAddressConfig GetAddressConfig(bool isCreateIfNot = true)
        {
            string configPath = AssetConst.AssetAddressConfigPath;
            AssetAddressConfig config = null;        
            if(File.Exists(configPath))
            {
                config = JsonConvert.DeserializeObject<AssetAddressConfig>(configPath);
            }

            if(config == null && isCreateIfNot)
            {
                config = new AssetAddressConfig();
            }
            return config;
        }

        public static void SaveAddressConfig(AssetAddressConfig config)
        {
            if (config == null)
            {
                return;
            }
            string configPath = AssetConst.AssetAddressConfigPath;
            string dir = Path.GetDirectoryName(configPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var json = JsonConvert.SerializeObject(config,Formatting.Indented);
            File.WriteAllText(configPath, json);
        }

        public static void UpdateAddressConfig()
        {
            AssetAddressConfig addressConfig = new AssetAddressConfig();

            string[] groupPaths = AssetDatabaseUtil.FindAssets<AssetAddressGroup>();
            List<AssetAddressData> addressDatas = new List<AssetAddressData>();

            foreach(var groupPath in groupPaths)
            {
                AssetAddressGroup addressGroup = AssetDatabase.LoadAssetAtPath<AssetAddressGroup>(groupPath);
                if(addressGroup.isMain)
                {
                    UpdateAddressConfig(addressDatas, addressGroup);
                }
            }
            addressConfig.addressDatas = addressDatas.ToArray();

            AssetAddressUtil.SaveAddressConfig(addressConfig);
        }

        private static void UpdateAddressConfig(List<AssetAddressData> addressDatas, AssetAddressGroup addressGroup)
        {
            foreach(var finder in addressGroup.finders)
            {
                string[] assetPaths = finder.Find();
                if(assetPaths!=null && assetPaths.Length>0)
                {
                    foreach(var assetPath in assetPaths)
                    {
                        AssetAddressData addressData = addressGroup.operation.GetAddressData(assetPath);

                        addressData.isPreload = addressGroup.isPreload;
                        addressData.isNeverDestroy = addressGroup.isNeverDestroy;

                        addressDatas.Add(addressData);
                    }
                }
            }
        }

    }
}
