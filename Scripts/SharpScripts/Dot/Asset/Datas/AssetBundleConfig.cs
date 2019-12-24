namespace Dot.Asset.Datas
{
    public class AssetBundleConfig
    {
        public int version = 1;
        public AssetBundleDetail[] details = new AssetBundleDetail[0];
    }

    public class AssetBundleDetail
    {
        public string name;
        public string hash;
        public string crc;
        public string[] Dependencies = new string[0];

        public bool isPreload = false;
        public bool isNeverDestroy = false;
    }
}
