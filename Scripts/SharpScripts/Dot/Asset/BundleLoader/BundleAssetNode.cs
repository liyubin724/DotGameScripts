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
        private WeakReference assetWeakRef = null;
        private List<WeakReference> instanceWeakRefs = new List<WeakReference>();

        internal void InitNode(string assetPath,BundleNode node)
        {
            InitNode(assetPath);
            bundleNode = node;
            bundleNode.RetainRef();
        }

        protected internal override UnityObject GetAsset()
        {
            UnityObject asset = bundleNode.GetAsset(AssetPath);
            if(assetWeakRef == null)
            {
                assetWeakRef = new WeakReference(asset);
            }else
            {
                assetWeakRef.Target = asset;
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
            AddInstance(instance);

            return instance;
        }

        protected internal override UnityObject GetInstance(UnityObject uObj)
        {
            if(uObj!=null)
            {
                UnityObject instance = UnityObject.Instantiate(uObj);
                AddInstance(instance);

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
            if(assetWeakRef!=null && !IsNull(assetWeakRef.Target))
            {
                return true;
            }

            foreach (var instance in instanceWeakRefs)
            {
                if (!IsNull(instance.Target))
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

            assetWeakRef = null;

            foreach (var asset in instanceWeakRefs)
            {
                asset.Target = null;
            }
        }

        private void AddInstance(UnityObject uObj)
        {
            bool isSet = false;
            for (int i = 0; i < instanceWeakRefs.Count; ++i)
            {
                if (IsNull(instanceWeakRefs[i].Target))
                {
                    instanceWeakRefs[i].Target = uObj;
                    isSet = true;
                    break;
                }
            }

            if (!isSet)
            {
                instanceWeakRefs.Add(new WeakReference(uObj, false));
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
