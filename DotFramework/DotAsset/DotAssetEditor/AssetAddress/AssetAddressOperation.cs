using Dot.Core.Extension;
using System;
using System.IO;
using static Dot.Asset.Datas.AssetAddressConfig;

namespace DotEditor.Asset.AssetAddress
{
    //public enum AssetCompressionMode
    //{
    //    Uncompressed = 0,
    //    LZ4,
    //    LZMA,
    //}

    //public enum AssetBundleNameType
    //{
    //    Origin = 0,
    //    MD5,
    //}

    public enum AssetPackMode
    {
        Together,
        Separate,
    }

    public enum AssetAddressMode
    {
        FullPath,
        FileName,
        FileNameWithoutExtension,
    }

    [Serializable]
    public class AssetAddressOperation
    {
        public AssetPackMode packMode = AssetPackMode.Together;
        public AssetAddressMode addressMode = AssetAddressMode.FullPath;
        public string labels = string.Empty;

        public string GetAddressName(string assetPath)
        {
            if (addressMode == AssetAddressMode.FullPath)
                return assetPath;
            else if (addressMode == AssetAddressMode.FileName)
                return Path.GetFileName(assetPath);
            else if (addressMode == AssetAddressMode.FileNameWithoutExtension)
                return Path.GetFileNameWithoutExtension(assetPath);
            else
                return assetPath;
        }

        public string GetBundleName(string assetPath)
        {
            string bundleName = string.Empty;
            if (packMode == AssetPackMode.Separate)
            {
                bundleName = assetPath.ReplaceSpecialCharacter("_");
            }
            else if (packMode == AssetPackMode.Together)
            {
                string rootFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
                bundleName = rootFolder.ReplaceSpecialCharacter("_");
            }
            return bundleName.ToLower();
        }

        public string[] GetLabels()
        {
            if(string.IsNullOrEmpty(labels))
            {
                return new string[0];
            }else
            {
                return labels.SplitToNotEmptyArray('|');
            }
        }

        private bool IsScene(string assetPath)
        {
            if(Path.GetExtension(assetPath).ToLower() == ".unity")
            {
                return true;
            }
            return false;
        }

        public AssetAddressData GetAddressData(string assetPath)
        {
            AssetAddressData addressData = new AssetAddressData();
            addressData.assetAddress = GetAddressName(assetPath);
            addressData.assetPath = assetPath;
            addressData.bundlePath = GetBundleName(assetPath);
            addressData.labels = GetLabels();
            addressData.isScene = IsScene(assetPath);

            return addressData;
        }

        public void UpdateAddressData(AssetAddressData addressData)
        {
            string assetPath = addressData.assetPath;
            addressData.assetAddress = GetAddressName(assetPath);
            addressData.bundlePath = GetBundleName(assetPath);
            addressData.labels = GetLabels();
            addressData.isScene = IsScene(assetPath);
        }
    }
}
