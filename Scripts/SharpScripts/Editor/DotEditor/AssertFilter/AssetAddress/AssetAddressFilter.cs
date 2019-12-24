using Dot.Asset.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssertFilter.AssetAddress
{
    public static class AssetAddressFilter
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
    }
}
