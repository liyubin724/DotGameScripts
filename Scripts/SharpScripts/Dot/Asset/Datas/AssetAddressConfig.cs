using Dot.Log;
using System;
using System.Collections.Generic;

namespace Dot.Asset.Datas
{
    [Serializable]
    public class AssetAddressConfig
    {
        public AssetAddressData[] addressDatas = new AssetAddressData[0];

        private bool isInit = false;
        private Dictionary<string, AssetAddressData> addressToDataDic = new Dictionary<string, AssetAddressData>();
        private Dictionary<string, AssetAddressData> pathToDataDic = new Dictionary<string, AssetAddressData>();
        private Dictionary<string, List<string>> labelToAddressDic = new Dictionary<string, List<string>>();

        private void InitConfig()
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
            isInit = true;
        }

        public bool CheckIsSceneByPath(string path)
        {
            if (!isInit)
            {
                InitConfig();
            }

            if (pathToDataDic.TryGetValue(path, out AssetAddressData data))
            {
                return data.isScene;
            }
            LogUtil.LogError(GetType(), $"data is not found.path={path}!");
            return false;
        }

        public string GetPathByAddress(string address)
        {
            if(!isInit)
            {
                InitConfig();
            }

            if(addressToDataDic.TryGetValue(address,out AssetAddressData data))
            {
                return data.assetPath;
            }

            LogUtil.LogError(GetType(), $"Path is not found.address={address}!");
            return null;
        }

        private List<string> tempStrList = new List<string>();
        public string[] GetPathsByAddresses(string[] addresses)
        {
            if (!isInit)
            {
                InitConfig();
            }

            tempStrList.Clear();
            foreach(var address in addresses)
            {
                string path = GetPathByAddress(address);
                if(string.IsNullOrEmpty(path))
                {
                    return null;
                }else
                {
                    tempStrList.Add(path);
                }
            }
            return tempStrList.ToArray();
        }

        public string[] GetAddressesByLabel(string label)
        {
            if (!isInit)
            {
                InitConfig();
            }

            if (labelToAddressDic.TryGetValue(label, out List<string> addressList))
            {
                return addressList.ToArray();
            }
            LogUtil.LogError(GetType(), $"address is not found.label={label}!");
            return null;
        }

        public string GetBundleByPath(string path)
        {
            if (!isInit)
            {
                InitConfig();
            }

            if (pathToDataDic.TryGetValue(path,out AssetAddressData data))
            {
                return data.bundlePath;
            }
            LogUtil.LogError(GetType(), $"bundle is not found.path={path}!");
            return null;
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

            public bool isScene = false;
            public bool isPreload = false;
            public bool isNeverDestroy = false;
        }
    }
}
