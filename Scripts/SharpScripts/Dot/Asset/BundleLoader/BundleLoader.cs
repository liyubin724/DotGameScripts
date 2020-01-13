using Dot.Asset.Datas;
using Dot.Log;
using Dot.Pool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public class BundleLoader : AAssetLoader
    {
        private ObjectPool<BundleNode> bundleNodePool = new ObjectPool<BundleNode>();
        private Dictionary<string, BundleNode> bundleNodeDic = new Dictionary<string, BundleNode>();

        private ObjectPool<BundleAssetNode> assetNodePool = new ObjectPool<BundleAssetNode>();
        private AssetBundleConfig bundleConfig = null;

        protected override void DoInitUpdate()
        {
            addressConfig = AssetConst.GetAddressConfig();
            if (addressConfig == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "Address config is null");
                State = AssetLoaderState.Error;
            }
            bundleConfig = AssetConst.GetBundleConfig(assetRootDir);
            if(bundleConfig == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "Bundle Config is Null");
                State = AssetLoaderState.Error;
            }else
            {
                State = AssetLoaderState.Running;
            }
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
                if(operations.Contains(bundlePath))
                {
                    return operations.GetByKey(bundlePath).GetProgress();
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

        protected override void OnUnloadUnusedAsset()
        {
            string[] assetPaths = assetNodeDic.Keys.ToArray();
            foreach (var assetPath in assetPaths)
            {
                if (assetNodeDic.TryGetValue(assetPath, out AAssetNode assetNode) && !assetNode.IsAlive())
                {
                    assetNode.Unload(true);
                    assetNodeDic.Remove(assetPath);

                    assetNodePool.Release(assetNode as BundleAssetNode);
                }
            }

            string[] bundlePaths = bundleNodeDic.Keys.ToArray();
            foreach(var bundlePath in bundlePaths)
            {
                if(bundleNodeDic.TryGetValue(bundlePath,out BundleNode bundleNode) && bundleNode.RefCount == 0)
                {
                    bundleNodeDic.Remove(bundlePath);

                    bundleNode.Unload(true);
                    bundleNodePool.Release(bundleNode);
                }
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
            BundleAssetNode assetNode = assetNodePool.Get();

            string mainBundlePath = addressConfig.GetBundleByPath(assetPath);
            BundleNode bundleNode = GetOrCreateMainBundleNode(mainBundlePath);
            assetNode.InitNode(assetPath, bundleNode);
            assetNodeDic.Add(assetPath, assetNode);
            return assetNode;
        }

        private BundleNode GetOrCreateMainBundleNode(string mainBundlePath)
        {
            if(bundleNodeDic.TryGetValue(mainBundlePath,out BundleNode bundleNode))
            {
                return bundleNode;
            }

            bundleNode = CreateBundleNode(mainBundlePath);
            string[] depends = bundleConfig.GetDependencies(mainBundlePath);
            if(depends!=null && depends.Length>0)
            {
                foreach(var depend in depends)
                {
                    if(!bundleNodeDic.TryGetValue(depend,out BundleNode dependBundleNode))
                    {
                        dependBundleNode = CreateBundleNode(depend);
                    }

                    bundleNode.AddDepend(dependBundleNode);
                }
            }
            return bundleNode;
        }

        private BundleNode CreateBundleNode(string bundlePath)
        {
            BundleNode bundleNode = new BundleNode();
            bundleNode.InitNode(bundlePath);

            bundleNodeDic.Add(bundlePath, bundleNode);

            BundleAsyncOperation operation = new BundleAsyncOperation(bundlePath, GetBundleFilePath);
            operations.AddOrUpdate(bundlePath, operation);

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
