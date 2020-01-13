using Dot.Pool;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public abstract class AAssetNode : IObjectPoolItem
    {
        public string AssetPath { get; private set; }
        public bool IsNeverDestroy { get; set; }

        protected int refCount = 0;
        protected internal void Retain() => ++refCount;
        protected internal void Release() => --refCount;

        protected internal void InitNode(string path)
        {
            AssetPath = path;
        }

        protected internal abstract UnityObject GetAsset();
        protected internal abstract UnityObject GetInstance();
        protected internal abstract UnityObject GetInstance(UnityObject uObj);
        protected internal abstract bool IsAlive();
        protected internal abstract bool IsDone();
        protected internal abstract void Unload();

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
