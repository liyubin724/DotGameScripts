using Dot.Asset.Datas;
using Dot.Log;
using Dot.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Asset
{
    public class BundleNode : IObjectPoolItem
    {
        public string BundlePath { get; private set; }
        private int refCount = 0;
        public int RefCount { get => refCount; }
        private bool isDone = false;
        private AssetBundle assetBundle = null;
        private List<BundleNode> dependNodes = new List<BundleNode>();

        public BundleNode() { }

        public void InitNode(string bundlePath)
        {
            BundlePath = bundlePath;
        }

        public void SetBundle(AssetBundle bundle)
        {
            assetBundle = bundle;
            isDone = true;
        }

        public void AddDepend(BundleNode node)
        {
            if(!dependNodes.Contains(node))
            {
                dependNodes.Add(node);
                node.RetainRef();
            }
        }

        public void RetainRef()
        {
            ++refCount;
        }

        public void ReleaseRef()
        {
            --refCount;
            if(refCount == 0)
            {
                foreach(var node in dependNodes)
                {
                    node.ReleaseRef();
                }
            }
        }

        public bool IsDone
        {
            get
            {
                if(!isDone)
                {
                    return false;
                }

                foreach(var node in dependNodes)
                {
                    if(!node.isDone)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool IsScene
        {
            get
            {
                if (assetBundle != null)
                {
                    return assetBundle.isStreamedSceneAssetBundle;
                }
                else if (!isDone)
                {
                    LogUtil.LogError(AssetConst.LOGGER_NAME,"BundleNode::IsScene->AssetBundle has not been loaded,you should call IsDone at first");
                }
                else
                {
                    LogUtil.LogError(AssetConst.LOGGER_NAME, "BundleNode::IsScene->AssetBundle Load failed");
                }
                return false;
            }
        }

        public UnityEngine.Object GetAsset(string assetPath)
        {
            return IsScene ? assetBundle : assetBundle?.LoadAsset(assetPath);
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            isDone = false;
            assetBundle?.Unload(true);
            assetBundle = null;
            dependNodes.Clear();
            refCount = 0;
        }
    }
}
