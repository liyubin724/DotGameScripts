using Dot.Asset.Datas;
using DotEditor.Util;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Dot.Asset.Datas.AssetAddressConfig;

namespace DotEditor.AssetFilter.AssetAddress
{
    public static class AssetAddressUtil
    {
        public static AssetAddressConfig GetAddressConfig(bool isCreateIfNot = true)
        {
            AssetAddressConfig addressConfig = AssetDatabase.LoadAssetAtPath<AssetAddressConfig>(AssetAddressConfig.CONFIG_PATH);

            if(addressConfig == null && isCreateIfNot)
            {
                addressConfig = ScriptableObject.CreateInstance<AssetAddressConfig>();
                AssetDatabase.CreateAsset(addressConfig, AssetAddressConfig.CONFIG_PATH);
                AssetDatabase.ImportAsset(AssetAddressConfig.CONFIG_PATH);
            }

            return addressConfig;
        }

        public static void UpdateAddressConfig()
        {
            AssetAddressConfig addressConfig = GetAddressConfig();
            addressConfig.Clear();

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

            EditorUtility.SetDirty(addressConfig);

            AssetDatabase.SaveAssets();
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
                        AssetAddressData addressData = new AssetAddressData();
                        addressData.assetAddress = addressGroup.operation.GetAddressName(assetPath);
                        addressData.assetPath = assetPath;
                        addressData.bundlePath = addressGroup.operation.GetBundleName(assetPath);
                        addressData.labels = addressGroup.operation.GetLabels();

                        addressDatas.Add(addressData);
                    }
                }
            }
        }

    }
}
