using DotEditor.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FileUtil = DotEditor.Core.Util.FileUtil;

namespace DotEditor.AssetAddress
{
    [Serializable]
    public class AssetFilterFinder
    {
        public string assetFolder = "Assets";
        public bool isIncludeSubfolder = true;

        public string fileNameRegex = string.Empty;
        public string inAnyFolderNames = string.Empty;
        public string inParentFolderNames = string.Empty;

        public string[] Find()
        {
            List<string> assetList = new List<string>();
            string[] assets = DirectoryUtil.GetAssetsByFileNameFilter(assetFolder, isIncludeSubfolder, fileNameRegex, new string[] { ".meta" });

            if(assets != null && assets.Length >= 0)
            {
                foreach(var asset in assets)
                {
                    if(IsMatchAnyFolderName(asset) && IsMatchParentFolderName(asset))
                    {
                        assetList.Add(asset);
                    }
                }
            }
            return assetList.ToArray();
        }

        private bool IsMatchFileName(string assetPath)
        {
            if(!string.IsNullOrEmpty(fileNameRegex) && Regex.IsMatch(assetPath, fileNameRegex))
            {
                return true;
            }

            return false;
        }

        private bool IsMatchAnyFolderName(string assetpath)
        {
            if(string.IsNullOrEmpty(inAnyFolderNames))
            {
                return true;
            }

            string[] inAnyFolders = inAnyFolderNames.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (inAnyFolders.Length>0)
            {
                string[] folderNames = FileUtil.GetFolderNames(assetpath);
                if (folderNames == null || folderNames.Length == 0)
                {
                    return false;
                }

                var intersectResult = folderNames.Intersect(inAnyFolders).ToArray();
                if (intersectResult == null || intersectResult.Length == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsMatchParentFolderName(string assetPath)
        {
            if (string.IsNullOrEmpty(inParentFolderNames))
            {
                return true;
            }

            string[] inParentFolder = inParentFolderNames.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);


            if (inParentFolder.Length>0)
            {
                string parentFolder = FileUtil.GetParentFolderName(assetPath);
                return Array.IndexOf(inParentFolder, parentFolder) >=0;
            }
            return true;
        }
    }


}
