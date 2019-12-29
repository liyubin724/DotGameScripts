using Dot.Log;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Asset.Datas
{
    [Serializable]
    public class AssetAddressConfig
    {
        public AssetAddressData[] addressDatas = new AssetAddressData[0];

        private Dictionary<string, AssetAddressData> addressToDataDic = new Dictionary<string, AssetAddressData>();
        private Dictionary<string, AssetAddressData> pathToDataDic = new Dictionary<string, AssetAddressData>();

        private Dictionary<string, List<string>> labelToAddressDic = new Dictionary<string, List<string>>();

        public void InitConfig()
        {
            foreach (var data in addressDatas)
            {
                if (addressToDataDic.ContainsKey(data.assetAddress))
                {
                    LogUtil.LogError(GetType(), $"The address is repeated. address = {data.assetAddress}");
                    continue;
                }
                else
                {
                    addressToDataDic.Add(data.assetAddress, data);
                }
                
                if (pathToDataDic.ContainsKey(data.assetPath))
                {
                    LogUtil.LogError(GetType(), $"The path is repeated. path = {data.assetPath}");
                    continue;
                }
                else
                {
                    pathToDataDic.Add(data.assetPath, data);
                }

                if (data.labels != null && data.labels.Length > 0)
                {
                    foreach (var label in data.labels)
                    {
                        if (!labelToAddressDic.TryGetValue(label, out List<string> addressList))
                        {
                            addressList = new List<string>();
                            labelToAddressDic.Add(label, addressList);
                        }

                        addressList.Add(data.assetAddress);
                    }
                }
            }
        }

        public string GetPathByAddress(string address)
        {
            if(addressToDataDic.TryGetValue(address,out AssetAddressData data))
            {
                return data.assetPath;
            }
            return null;
        }

        public string[] GetPathsByLabel(string label)
        {
            if(labelToAddressDic.TryGetValue(label,out List<string> addressList))
            {
                string[] paths = new string[addressList.Count];
                for(int i =0;i<addressList.Count;i++)
                {
                    paths[i] = GetPathByAddress(addressList[i]);
                }
                return paths;
            }
            return null;
        }

        public string GetBundleByPath(string path)
        {
            if(pathToDataDic.TryGetValue(path,out AssetAddressData data))
            {
                return data.bundlePath;
            }
            return null;
        }

        public string[] GetBundlesByPathes(string[] pathes)
        {
            string[] result = new string[pathes.Length];
            for(int i =0;i<pathes.Length;++i)
            {
                result[i] = GetBundleByPath(pathes[i]);
            }
            return result;
        }

        public void Clear()
        {
            addressDatas = new AssetAddressData[0];
            addressToDataDic.Clear();
            pathToDataDic.Clear();
            labelToAddressDic.Clear();
        }

        [Serializable]
        public class AssetAddressData
        {
            public string assetAddress;
            public string assetPath;
            public string bundlePath;
            public string[] labels = new string[0];

            public bool isPreload = false;
            public bool isNeverDestroy = false;
        }
    }
}
