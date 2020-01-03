using Dot.Asset.Datas;
using Dot.Log;
using System;
using System.Collections.Generic;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public class BundleAssetNode : AAssetNode
    {
        private BundleNode bundleNode = null;
        private List<WeakReference> weakAssets = new List<WeakReference>();

        public void InitNode(string assetPath,BundleNode node)
        {
            InitNode(assetPath);
            bundleNode = node;
            bundleNode.RetainRef();
        }

        public override UnityObject GetAsset()
        {
            UnityObject asset = bundleNode.GetAsset(AssetPath);
            if (!bundleNode.IsScene)
            {
                AddAsset(asset);
            }
            return asset;
        }

        public override UnityObject GetInstance()
        {
            UnityObject asset = bundleNode.GetAsset(AssetPath);
            if (asset == null)
            {
                return null;
            }
            if (bundleNode.IsScene)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME,"AssetNode::GetInstance->bundle is scene.can't Instance it");
                return null;
            }

            UnityObject instance = UnityObject.Instantiate(asset);
            AddAsset(instance);

            return instance;
        }

        public override bool IsAlive()
        {
            if(IsNeverDestroy)
            {
                return true;
            }
            if(refCount>0)
            {
                return true;
            }

            foreach (var weakAsset in weakAssets)
            {
                if (!IsNull(weakAsset.Target))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool IsDone() => bundleNode.IsDone;

        public override void OnRelease()
        {
            bundleNode?.ReleaseRef();
            bundleNode = null;
            foreach(var asset in weakAssets)
            {
                asset.Target = null;
            }

            base.OnRelease();
        }

        private void AddAsset(UnityObject uObj)
        {
            bool isSet = false;
            for (int i = 0; i < weakAssets.Count; ++i)
            {
                if (IsNull(weakAssets[i].Target))
                {
                    weakAssets[i].Target = uObj;
                    isSet = true;
                    break;
                }
            }

            if (!isSet)
            {
                weakAssets.Add(new WeakReference(uObj, false));
            }
        }

        private bool IsNull(SystemObject sysObj)
        {
            if (sysObj == null || sysObj.Equals(null))
            {
                return true;
            }

            return false;
        }
    }
}
