using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;

namespace Dot.Asset
{
    public partial class AssetManager
    {
        private AssetHandler LoadBatchAssetAsync(string[] addresses,
            bool isInstance,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress,
            OnBatchAssetsLoadProgress batchProgress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            AssetHandler handler = new AssetHandler();

            return handler;
        }
    }
}
