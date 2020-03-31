using Dot.Asset.Datas;
using DotEditor.Core.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Dot.Asset.Datas.AssetAddressConfig;

namespace DotEditor.Asset.AssetAddress
{
    public static class AssetAddressUtil
    {
        [MenuItem("Game/Asset/Create Address Group",priority =0)]
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

        [MenuItem("Game/Asset/Create Address Group", priority = 1)]
        public static void BuildAssetAddressConfig()
        {
            AssetAddressGroup[] groups = AssetDatabaseUtil.FindInstances<AssetAddressGroup>();
            if(groups!=null && groups.Length>0)
            {
                AssetAddressConfig config = GetOrCreateConfig();
                config.Clear();

                foreach(var group in groups)
                {
                    UpdateConfigByGroup(group);
                }
                config.Reload();
                AssetDatabase.SaveAssets();
            }
        }

        public static void UpdateConfigByGroup(AssetAddressGroup group)
        {
            if(!group.isMain ||!group.isEnable)
            {
                return;
            }
            AssetAddressConfig config = GetOrCreateConfig();
            Dictionary<string, AssetAddressData> dataDic = new Dictionary<string, AssetAddressData>();
            foreach(var d in config.addressDatas)
            {
                dataDic.Add(d.assetPath, d);
            }

            foreach (var filter in group.filters)
            {
                string[] assetPaths = filter.Filter();
                if (assetPaths != null && assetPaths.Length > 0)
                {
                    foreach (var assetPath in assetPaths)
                    {
                        if(!dataDic.TryGetValue(assetPath,out AssetAddressData addressData))
                        {
                            addressData = group.operation.GetAddressData(assetPath);
                            dataDic.Add(assetPath, addressData);
                        }else
                        {
                            group.operation.UpdateAddressData(addressData);
                        }
                        addressData.isPreload = group.isPreload;
                        addressData.isNeverDestroy = group.isNeverDestroy;
                    }
                }
            }

            config.addressDatas = dataDic.Values.ToArray();
            config.Reload();
        }

        private static readonly string ASSET_ADDRESS_CONFIG_PATH = "Assets/address_config.asset";
        public static AssetAddressConfig GetOrCreateConfig()
        {
            AssetAddressConfig config = AssetDatabase.LoadAssetAtPath<AssetAddressConfig>(ASSET_ADDRESS_CONFIG_PATH);
            if(config == null)
            {
                config = ScriptableObject.CreateInstance<AssetAddressConfig>();
                AssetDatabase.CreateAsset(config, ASSET_ADDRESS_CONFIG_PATH);
                AssetDatabase.ImportAsset(ASSET_ADDRESS_CONFIG_PATH);
            }
            return config;
        }
    }
}
