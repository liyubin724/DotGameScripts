using System.Collections.Generic;

namespace Dot.Asset.Datas
{
    /// <summary>
    /// 存储打包后的所有AssetBundle的信息
    /// </summary>
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
                InitConfig();
            }

            if(bundleDetailDic.TryGetValue(bundlePath,out AssetBundleDetail bundleDetail))
            {
                return bundleDetail.dependencies;
            }
            return null;
        }
    }

    /// <summary>
    /// AssetBundle的详细信息
    /// </summary>
    public class AssetBundleDetail
    {
        public string name;
        public string hash;
        public string crc;
        public string md5;
        public string[] dependencies = new string[0];
    }
}
