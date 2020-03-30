using Dot.Asset.Datas;
using DotEditor.Core.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static Dot.Asset.Datas.AssetAddressConfig;

namespace DotEditor.Asset.AssetAddress
{
    public static class AssetAddressUtil
    {
        [MenuItem("Game/Asset/Create Address Group")]
        public static void CreateGroupAsset()
        {
            string[] dirs = SelectionUtil.GetSelectionDirs();
            if(dirs!=null && dirs.Length>0)
            {
                string filePath = $"{dirs[0]}/asset_address_group.asset";
                filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
                var config = ScriptableObject.CreateInstance<AssetAddressGroup>();
                AssetDatabase.CreateAsset(config, filePath);
                AssetDatabase.ImportAsset(filePath);
            }
        }

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
            foreach(var finder in addressGroup.filters)
            {
                string[] assetPaths = finder.Filter();
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
