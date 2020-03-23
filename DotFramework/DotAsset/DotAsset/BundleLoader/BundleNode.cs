using Dot.Asset.Datas;
using Dot.Log;
using Dot.Core.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Asset
{
    public class BundleNode : IObjectPoolItem
    {
        internal string BundlePath { get; private set; }
        private int refCount = 0;
        internal int RefCount { get => refCount; }
        private bool isDone = false;
        private AssetBundle assetBundle = null;
        private List<BundleNode> dependNodes = new List<BundleNode>();

        internal void InitNode(string bundlePath)
        {
            BundlePath = bundlePath;
        }

        internal void SetBundle(AssetBundle bundle)
        {
            assetBundle = bundle;
            isDone = true;
        }

        internal void AddDepend(BundleNode node)
        {
            if(!dependNodes.Contains(node))
            {
                dependNodes.Add(node);
                node.RetainRef();
            }
        }

        internal void RetainRef()
        {
            ++refCount;
        }

        internal void ReleaseRef()
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

        internal bool IsDone
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

        internal bool IsUsedByScene { get; set; } = false;

        internal bool IsScene
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

        internal UnityEngine.Object GetAsset(string assetPath)
        {
            return IsScene ? assetBundle : assetBundle?.LoadAsset(assetPath);
        }

        internal void Unload()
        {
            if(assetBundle!=null)
            {
                assetBundle.Unload(!IsUsedByScene);
            }
            assetBundle = null;
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            Unload();
            IsUsedByScene = false;
            BundlePath = null;
            isDone = false;
            dependNodes.Clear();
            refCount = 0;
        }
    }
}
