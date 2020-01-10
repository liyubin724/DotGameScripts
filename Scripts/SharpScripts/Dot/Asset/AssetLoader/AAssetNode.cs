using Dot.Pool;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public abstract class AAssetNode : IObjectPoolItem
    {
        public string AssetPath { get; private set; }
        public bool IsNeverDestroy { get; set; }

        protected int refCount = 0;
        public void Retain() => ++refCount;
        public void Release() => --refCount;

        public void InitNode(string path)
        {
            AssetPath = path;
        }

        public abstract UnityObject GetAsset();
        public abstract UnityObject GetInstance();
        public abstract UnityObject GetInstance(UnityObject uObj);
        public abstract bool IsAlive();
        public abstract bool IsDone();
        public abstract void Unload(bool isForce);

        public void OnGet()
        {
        }

        public virtual void OnRelease()
        {
            AssetPath = null;
            IsNeverDestroy = false;
            refCount = 0;
        }
    }
}
