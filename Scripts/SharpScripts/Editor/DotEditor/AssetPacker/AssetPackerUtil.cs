using Dot.Asset.Datas;
using DotEditor.AssetFilter.AssetAddress;
using DotEditor.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Build.Pipeline;
using UnityEngine.U2D;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AssetPacker
{
    public static class AssetPackerUtil
    {
        public static AssetPackerConfig GetAssetPackerConfig()
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

        public static BundlePackConfig GetBundlePackConfig()
        {
            BundlePackConfig bundlePackConfig = null;

            string configPath = AssetConst.BundlePackConfigPath;
            if(File.Exists(configPath))
            {
                string configContent = File.ReadAllText(configPath);
                bundlePackConfig = JsonConvert.DeserializeObject<BundlePackConfig>(configContent);
            }
            if(bundlePackConfig == null)
            {
                bundlePackConfig = new BundlePackConfig();
            }

            return bundlePackConfig;
        }

        public static void SaveBundlePackConfig(BundlePackConfig config)
        {
            if(config == null)
            {
                return;
            }
            string configPath = AssetConst.BundlePackConfigPath;
            string dir = Path.GetDirectoryName(configPath);
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }

        public static void ClearBundleNames(bool isShowProgressBar = false)
        {
            string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();
            if (isShowProgressBar)
            {
                EditorUtility.DisplayProgressBar("Clear Bundle Names", "", 0.0f);
            }
            for (int i = 0; i < bundleNames.Length; i++)
            {
                if (isShowProgressBar)
                {
                    EditorUtility.DisplayProgressBar("Clear Bundle Names", bundleNames[i], i / (float)bundleNames.Length);
                }
                AssetDatabase.RemoveAssetBundleName(bundleNames[i], true);
            }
            if (isShowProgressBar)
            {
                EditorUtility.ClearProgressBar();
            }

            AssetDatabase.SaveAssets();
        }

        public static void SetAssetBundleNames(bool isShowProgressBar = false)
        {
            AssetPackerConfig config = GetAssetPackerConfig();
            SetAssetBundleNames(config, isShowProgressBar);
        }

        public static void SetAssetBundleNames(AssetPackerConfig assetPackerConfig, bool isShowProgressBar = false)
        {
            if (isShowProgressBar)
            {
                EditorUtility.DisplayProgressBar("Set Bundle Names", "", 0f);
            }

            List<AssetPackerAddressData> addressDatas = new List<AssetPackerAddressData>();
            assetPackerConfig.groupDatas.ForEach((groupData) =>
            {
                addressDatas.AddRange(groupData.assetFiles);
            });

            for(int i = 0;i<addressDatas.Count;++i)
            {
                if (isShowProgressBar)
                {
                    EditorUtility.DisplayProgressBar("Set Bundle Names", addressDatas[i].assetPath, i / (float)addressDatas.Count);
                }

                string assetPath = addressDatas[i].assetPath;
                string bundlePath = addressDatas[i].bundlePath;

                AssetImporter ai = AssetImporter.GetAtPath(assetPath);
                ai.assetBundleName = bundlePath;

                if (Path.GetExtension(assetPath).ToLower() == ".spriteatlas")
                {
                    SetSpriteBundleNameByAtlas(assetPath, bundlePath);
                }
            }

            if (isShowProgressBar)
            {
                EditorUtility.ClearProgressBar();
            }

            AssetDatabase.SaveAssets();
        }

        private static void SetSpriteBundleNameByAtlas(string atlasAssetPath, string bundlePath)
        {
            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasAssetPath);
            if (atlas != null)
            {
                List<string> spriteAssetPathList = new List<string>();
                UnityObject[] objs = atlas.GetPackables();
                foreach (var obj in objs)
                {
                    if (obj.GetType() == typeof(Sprite))
                    {
                        spriteAssetPathList.Add(AssetDatabase.GetAssetPath(obj));
                    }
                    else if (obj.GetType() == typeof(DefaultAsset))
                    {
                        string folderPath = AssetDatabase.GetAssetPath(obj);
                        string[] assets = AssetDatabaseUtil.FindAssetInFolder<Sprite>(folderPath);
                        spriteAssetPathList.AddRange(assets);
                    }
                }
                spriteAssetPathList.Distinct();
                foreach (var path in spriteAssetPathList)
                {
                    AssetImporter ai = AssetImporter.GetAtPath(path);
                    ai.assetBundleName = bundlePath;
                }
            }
        }

        public static void PackAssetBundle(bool isShowProgressBar = false)
        {
            AssetPackerConfig assetPackerConfig = GetAssetPackerConfig();
            BundlePackConfig bundlePackConfig = GetBundlePackConfig();
            PackAssetBundle(assetPackerConfig, bundlePackConfig, isShowProgressBar);
        }

        public static void PackAssetBundle(AssetPackerConfig assetPackerConfig, BundlePackConfig bundlePackConfig, bool isShowProgressBar = false)
        {
            string outputDir = $"{bundlePackConfig.bundleOutputDir}/{bundlePackConfig.buildTarget.ToString()}/{AssetConst.ASSET_BUNDLE_DIR_NAME}";
            if(bundlePackConfig.cleanupBeforeBuild && Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            if(!Directory.CreateDirectory(outputDir).Exists)
            {
                Debug.LogError("AssetPackUitl::PackAssetBundle->Folder is not found. dir = "+outputDir);
                return;
            }

            var manifest = CompatibilityBuildPipeline.BuildAssetBundles(outputDir, bundlePackConfig.GetBundleOptions(), bundlePackConfig.GetBuildTarget());
            if(manifest!=null)
            {
                SaveManifestAsJson(assetPackerConfig, manifest,$"{outputDir}/{AssetConst.ASSET_MANIFEST_NAME}{AssetConst.ASSET_MANIFEST_EXT}");
            }else
            {
                Debug.LogError("AssetPackerUtil::PackAssetBundle->Build Failed");
            }
        }

        private static void SaveManifestAsJson(AssetPackerConfig assetPackerConfig, CompatibilityAssetBundleManifest manifest,string filePath)
        {
            AssetBundleConfig assetBundleConfig = new AssetBundleConfig();

            List<AssetBundleDetail> bundleDetails = new List<AssetBundleDetail>();

            string[] bundles = manifest.GetAllAssetBundles();
            foreach(var bundlePath in bundles)
            {
                AssetBundleDetail detail = new AssetBundleDetail();
                detail.name = bundlePath;
                detail.hash = manifest.GetAssetBundleHash(bundlePath).ToString();
                detail.crc = manifest.GetAssetBundleCrc(bundlePath).ToString();
                detail.dependencies = manifest.GetAllDependencies(bundlePath);

                bundleDetails.Add(detail);
            }
            assetBundleConfig.details = bundleDetails.ToArray();

            var json = JsonConvert.SerializeObject(assetBundleConfig, Formatting.Indented);
            File.WriteAllText(filePath,json);
        }
    }
}
