using Dot.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public enum NodeState
    {
        None = 0,
        Loading,
        Finished,
    }
    public abstract class AssetNode : IObjectPoolItem
    {
        public string AssetPath { get; private set; }
        public bool IsNeverDestroy { get; set; }
        public NodeState State { get; set; } = NodeState.None;

        protected int refCount = 0;
        public void Retain() => ++refCount;
        public void Release() => --refCount;

        public void InitNode(string path)
        {
            AssetPath = path;
            State = NodeState.Loading;
        }

        public abstract UnityObject GetAsset();
        public abstract UnityObject GetInstance();
        public abstract bool IsAlive();
        public abstract bool IsDone();

        public void OnGet()
        {
        }

        public virtual void OnRelease()
        {
        }
    }
}
