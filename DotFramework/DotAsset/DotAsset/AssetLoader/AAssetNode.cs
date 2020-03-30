using Dot.Core.Pool;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    /// <summary>
    /// 加载中及缓存到的资源结点
    /// </summary>
    public abstract class AAssetNode : IObjectPoolItem
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath { get; private set; }
        /// <summary>
        /// 是否设定此资源永不销毁
        /// </summary>
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

        public void OnNew()
        {
        }
    }
}
