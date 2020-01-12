using Dot.Dispose;
using Dot.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Asset
{
    internal class AssetBridgeData : IObjectPoolItem
    {
        internal AssetHandler assetHandler = null;
        internal List<OnAssetLoadComplete> completeList = new List<OnAssetLoadComplete>();

        public void OnGet()
        {
        }

        public void OnRelease()
        {
        }
    }

    public class AssetBridge : ABaseDispose
    {
        private AssetLoaderPriority loaderPriority = AssetLoaderPriority.Default;

        public AssetBridge() { }
        public AssetBridge(AssetLoaderPriority priority)
        {
            loaderPriority = priority;
        }

        protected override void DisposeManagedResource()
        {
            
        }

        protected override void DisposeUnmanagedResource()
        {
            
        }
    }
}
