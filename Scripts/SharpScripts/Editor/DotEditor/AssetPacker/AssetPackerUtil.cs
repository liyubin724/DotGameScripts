using DotEditor.AssetFilter.AssetAddress;
using DotEditor.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace DotEditor.AssetPacker
{
    public static class AssetPackerUtil
    {
        public static AssetPackerConfig GetPackerConfig()
        {
            AssetPackerConfig packerConfig = new AssetPackerConfig();

            string[] groupPaths = AssetDatabaseUtil.FindAssets<AssetAddressGroup>();
            foreach (var groupPath in groupPaths)
            {
                AssetAddressGroup addressGroup = AssetDatabase.LoadAssetAtPath<AssetAddressGroup>(groupPath);

                AssetPackerGroupData groupData = new AssetPackerGroupData();
                groupData.groupName = addressGroup.groupName;
                groupData.isMain = addressGroup.isMain;
                groupData.isPreload = addressGroup.isPreload;
                groupData.isNeverDestroy = addressGroup.isNeverDestroy;

                groupData.assetFiles = GetAssetsInGroup(addressGroup);

                packerConfig.groupDatas.Add(groupData);
            }

            return packerConfig;
        }

        private static List<AssetPackerAddressData> GetAssetsInGroup(AssetAddressGroup groupData)
        {
            List<string> assets = new List<string>();
            foreach(var finder in groupData.finders)
            {
                string[] finderAssets = finder.Find();
                if(finderAssets!=null && finderAssets.Length>0)
                {
                    assets.AddRange(finderAssets);
                }
            }
            assets = assets.Distinct().ToList();

            List<AssetPackerAddressData> addressDatas = new List<AssetPackerAddressData>();
            foreach(var asset in assets)
            {
                AssetPackerAddressData data = new AssetPackerAddressData();
                data.assetAddress = groupData.operation.GetAddressName(asset);
                data.assetPath = asset;
                data.bundlePath = groupData.operation.GetBundleName(asset);
                data.labels = groupData.operation.GetLabels();
                data.compressionType = groupData.operation.compressionType;

                addressDatas.Add(data);
            }
            return addressDatas;
        }
    }
}
