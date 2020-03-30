using Dot.Core.Util;
using Dot.Util;
using System;
using System.IO;
using static Dot.Asset.Datas.AssetAddressConfig;

namespace DotEditor.AssetFilter.AssetAddress
{
    public enum AssetCompressionMode
    {
        Uncompressed = 0,
        LZ4,
        LZMA,
    }

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

    public enum AssetBundleNameType
    {
        Origin = 0,
        MD5,
    }

    [Serializable]
    public class AssetAddressOperation
    {
        public AssetPackMode packMode = AssetPackMode.Together;
        public AssetAddressMode addressMode = AssetAddressMode.FullPath;
        public AssetBundleNameType bundleNameType = AssetBundleNameType.Origin;
        public string labels = string.Empty;
        public AssetCompressionMode compressionType = AssetCompressionMode.LZ4;

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
                bundleName = StringUtil.RemoveSpecialCharacter(assetPath, "_");
            }
            else if (packMode == AssetPackMode.Together)
            {
                string rootFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
                bundleName = StringUtil.RemoveSpecialCharacter(rootFolder, "_");
            }

            if(bundleNameType == AssetBundleNameType.MD5)
            {
                bundleName = MD5Util.GetMD5(bundleName);
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
                return labels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
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
    }
}
