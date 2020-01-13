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

        internal void InitNode(string assetPath,BundleNode node)
        {
            InitNode(assetPath);
            bundleNode = node;
            bundleNode.RetainRef();
        }

        protected internal override UnityObject GetAsset()
        {
            UnityObject asset = bundleNode.GetAsset(AssetPath);
            if (!bundleNode.IsScene)
            {
                AddAsset(asset);
            }
            return asset;
        }

        protected internal override UnityObject GetInstance()
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

        protected internal override UnityObject GetInstance(UnityObject uObj)
        {
            if(uObj!=null)
            {
                UnityObject instance = UnityObject.Instantiate(uObj);
                AddAsset(instance);

                return instance;
            }
            return null;
        }

        protected internal override bool IsAlive()
        {
            if(IsNeverDestroy)
            {
                return true;
            }
            if(refCount>0)
            {
                return true;
            }
            if(IsScene)
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

        protected internal override bool IsDone() => bundleNode.IsDone;

        internal bool IsScene { get; set; } = false;

        protected internal override void Unload()
        {
            if (bundleNode != null)
            {
                bundleNode.ReleaseRef();
                bundleNode = null;
            }

            foreach (var asset in weakAssets)
            {
                asset.Target = null;
            }
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

        public override void OnRelease()
        {
            Unload();
            IsScene = false;
            base.OnRelease();
        }
    }
}
