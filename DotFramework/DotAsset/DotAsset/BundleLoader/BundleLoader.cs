﻿using Dot.Asset.Datas;
using Dot.Log;
using Dot.Core.Pool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using Newtonsoft.Json;
using System.IO;

namespace Dot.Asset
{
    /// <summary>
    /// 在AssetBundle模式下对资源的加载器
    /// </summary>
    public class BundleLoader : AAssetLoader
    {
        private ObjectPool<BundleNode> bundleNodePool = new ObjectPool<BundleNode>();
        private Dictionary<string, BundleNode> bundleNodeDic = new Dictionary<string, BundleNode>();

        private ObjectPool<BundleAssetNode> assetNodePool = new ObjectPool<BundleAssetNode>();
        private AssetBundleConfig bundleConfig = null;

        protected override void DoInitUpdate()
        {
            //以同步的方式读取以JSON格式存储的Bundle的配置文件
            string bundleConfigPath = $"{assetRootDir}/{AssetConst.ASSET_BUNDLE_CONFIG_NAME}";
            string bundleConfigContent = File.ReadAllText(bundleConfigPath);
            bundleConfig = JsonConvert.DeserializeObject<AssetBundleConfig>(bundleConfigContent);

            //以同步的方式加载AssetBundle，并读取资源地址的配置文件
            AssetBundle assetAddressConfigAB;
            if (GetBundleFilePath(AssetConst.ASSET_ADDRESS_BUNDLE_NAME,out string configPath,out ulong offset))
            {
                assetAddressConfigAB = AssetBundle.LoadFromFile(configPath, 0, offset);
            }else
            {
                assetAddressConfigAB = AssetBundle.LoadFromFile(configPath);
            }
            if(assetAddressConfigAB!=null)
            {
                AssetAddressConfig[] addressConfigs = assetAddressConfigAB.LoadAllAssets<AssetAddressConfig>();
                if(addressConfigs!=null && addressConfigs.Length>0)
                {
                    addressConfig = addressConfigs[0];
                }
                assetAddressConfigAB.Unload(false);
            }

            if(addressConfig!=null && bundleConfig!=null)
            {
                State = AssetLoaderState.Running;
                return;
            }

            LogUtil.LogError(AssetConst.LOGGER_NAME, "config is Null");
            State = AssetLoaderState.Error;
        }

        protected override void OnDataUpdate(AssetLoaderData data)
        {
            if(data.State == AssetLoaderDataState.Canceled || data.State == AssetLoaderDataState.Error)
            {
                foreach(var path in data.Paths)
                {
                    if(!string.IsNullOrEmpty(path))
                    {
                        assetNodeDic[path].Release();
                    }
                }
            }else if(data.State == AssetLoaderDataState.Loading)
            {
                bool isComplete = true;
                bool isProgressChanged = false;
                for(int i = 0;i<data.Paths.Length;++i)
                {
                    string path = data.Paths[i];
                    if (string.IsNullOrEmpty(path))
                    {
                        continue;
                    }

                    AAssetNode assetNode = assetNodeDic[path];
                    if (assetNode.IsDone())
                    {
                        data.DoComplete(i, assetNode);
                        assetNode.Release();
                    }
                    else
                    {
                        float progress = GetAssetProgress(path);
                        data.DoProgress(i, progress);

                        isProgressChanged = true;
                        isComplete = false;
                    }
                }
                if(isProgressChanged)
                {
                    data.DoBatchProgress();
                }
                if(isComplete)
                {
                    data.DoBatchComplete();
                }
            }
        }

        private float GetAssetProgress(string assetPath)
        {
            string mainBundlePath = addressConfig.GetBundleByPath(assetPath);
            BundleNode bundleNode = bundleNodeDic[mainBundlePath];
            if (bundleNode.IsDone)
            {
                return 1.0f;
            }
            string[] depends = bundleConfig.GetDependencies(mainBundlePath);
            float progress = GetBundleProgress(mainBundlePath);
            float count = 1;
            if(depends!=null && depends.Length>0)
            {
                count += depends.Length;
                foreach(var depend in depends)
                {
                    progress += GetBundleProgress(depend);
                }
            }
            return progress / count;
        }

        private float GetBundleProgress(string bundlePath)
        {
            BundleNode bundleNode = bundleNodeDic[bundlePath];
            if(bundleNode.IsDone)
            {
                return 1.0f;
            }
            else
            {
                if(operations.ContainsKey(bundlePath))
                {
                    return operations[bundlePath].GetProgress();
                }
            }
            LogUtil.LogError(AssetConst.LOGGER_NAME, "The Bundle not loading");
            return 1.0f;
        }

        protected override void OnOperationFinished(AAsyncOperation operation)
        {
            AssetBundle assetbundle = operation.GetAsset() as AssetBundle;
            if(bundleNodeDic.TryGetValue(operation.AssetPath,out BundleNode bundleNode))
            {
                bundleNode.SetBundle(assetbundle);
            }else
            {
                if(assetbundle!=null)
                {
                    assetbundle.Unload(true);
                }
            }
        }

        protected internal override void UnloadUnusedAsset()
        {
            UnloadAssetNode();
            UnloadBundleNode();
        }

        private void UnloadAssetNode()
        {
            string[] assetPaths = assetNodeDic.Keys.ToArray();
            foreach (var assetPath in assetPaths)
            {
                if (assetNodeDic.TryGetValue(assetPath, out AAssetNode assetNode) && !assetNode.IsAlive())
                {
                    assetNode.Unload();
                    assetNodeDic.Remove(assetPath);
                    assetNodePool.Release(assetNode as BundleAssetNode);
                }
            }
        }

        private void UnloadBundleNode()
        {
            string[] bundlePaths = bundleNodeDic.Keys.ToArray();
            foreach (var bundlePath in bundlePaths)
            {
                if (bundleNodeDic.TryGetValue(bundlePath, out BundleNode bundleNode) && bundleNode.RefCount == 0)
                {
                    bundleNodeDic.Remove(bundlePath);

                    bundleNode.Unload();
                    bundleNodePool.Release(bundleNode);
                }
            }
        }

        protected override void UnloadAsset(string assetPath)
        {
            if(!string.IsNullOrEmpty(assetPath) && assetNodeDic.TryGetValue(assetPath, out AAssetNode assetNode))
            {
                BundleAssetNode bundleAssetNode = assetNode as BundleAssetNode;
                if (!assetNode.IsAlive() || bundleAssetNode.IsScene)
                {
                    assetNode.Unload();
                    assetNodeDic.Remove(assetPath);
                    assetNodePool.Release(assetNode as BundleAssetNode);

                    if(bundleAssetNode.IsScene)
                    {
                        UnloadBundleNode();
                    }
                }
            }
            else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "BundleLoader::UnloadAsset->asset not found by address,assetPath = " + assetPath);
            }
        }

        protected override void StartLoadingData(AssetLoaderData data)
        {
            for(int i =0;i<data.Paths.Length;++i)
            {
                string assetPath = data.Paths[i];
                if(!assetNodeDic.TryGetValue(assetPath,out AAssetNode assetNode))
                {
                    assetNode = CreateAssetNode(assetPath);
                }
                assetNode.Retain();
            }
        }

        private BundleAssetNode CreateAssetNode(string assetPath)
        {
            string mainBundlePath = addressConfig.GetBundleByPath(assetPath);
            bool isScene = addressConfig.CheckIsSceneByPath(assetPath);

            BundleAssetNode assetNode = assetNodePool.Get();
            BundleNode bundleNode = GetOrCreateMainBundleNode(mainBundlePath,isScene);

            assetNode.InitNode(assetPath, bundleNode);
            assetNode.IsScene = isScene;

            assetNodeDic.Add(assetPath, assetNode);

            return assetNode;
        }

        private BundleNode GetOrCreateMainBundleNode(string mainBundlePath,bool isScene)
        {
            string[] depends = bundleConfig.GetDependencies(mainBundlePath);
            if (!bundleNodeDic.TryGetValue(mainBundlePath,out BundleNode bundleNode))
            {
                bundleNode = CreateBundleNode(mainBundlePath);
                if (depends != null && depends.Length > 0)
                {
                    foreach (var depend in depends)
                    {
                        if (!bundleNodeDic.TryGetValue(depend, out BundleNode dependBundleNode))
                        {
                            dependBundleNode = CreateBundleNode(depend);
                        }

                        bundleNode.AddDepend(dependBundleNode);
                    }
                }
            }

            if(isScene)
            {
                bundleNode.IsUsedByScene = true;
                if (depends != null && depends.Length > 0)
                {
                    foreach (var depend in depends)
                    {
                        if (bundleNodeDic.TryGetValue(depend, out BundleNode dependBundleNode))
                        {
                            dependBundleNode.IsUsedByScene = true;
                        }
                    }
                }
            }
            
            return bundleNode;
        }

        private BundleNode CreateBundleNode(string bundlePath)
        {
            BundleNode bundleNode = new BundleNode();
            bundleNodeDic.Add(bundlePath, bundleNode);

            BundleAsyncOperation operation = new BundleAsyncOperation(bundlePath, GetBundleFilePath);
            operations.Add(bundlePath, operation);

            return bundleNode;
        }

        private bool GetBundleFilePath(string assetPath,out string filePath,out ulong offset)
        {
            offset = 0u;
            filePath = $"{assetRootDir}/{assetPath}";

            return false;
        }

        protected internal override UnityObject InstantiateAsset(string address, UnityObject asset)
        {
            string assetPath = addressConfig.GetPathByAddress(address);
            if(assetNodeDic.TryGetValue(assetPath,out AAssetNode assetNode))
            {
                return assetNode.GetInstance();
            }
            return null;
        }
    }
}
