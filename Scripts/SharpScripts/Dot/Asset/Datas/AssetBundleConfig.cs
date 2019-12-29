using System.Collections.Generic;

namespace Dot.Asset.Datas
{
    public class AssetBundleConfig
    {
        public int version = 1;
        public AssetBundleDetail[] details = new AssetBundleDetail[0];

        private Dictionary<string, AssetBundleDetail> bundleDetailDic = null;

        public void InitConfig()
        {
            bundleDetailDic = new Dictionary<string, AssetBundleDetail>();
            foreach (var detail in details)
            {
                bundleDetailDic.Add(detail.name, detail);
            }
        }
        
        public string[] GetDependencies(string bundlePath)
        {
            if(bundleDetailDic == null)
            {
                bundleDetailDic = new Dictionary<string, AssetBundleDetail>();
                foreach(var detail in details)
                {
                    bundleDetailDic.Add(detail.name, detail);
                }
            }
            if(bundleDetailDic.TryGetValue(bundlePath,out AssetBundleDetail bundleDetail))
            {
                return bundleDetail.dependencies;
            }
            return null;
        }
    }

    public class AssetBundleDetail
    {
        public string name;
        public string hash;
        public string crc;
        public string[] dependencies = new string[0];
    }
}
