using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Pool;
using Priority_Queue;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public enum DataState
    {
        None = 0,
        Waiting,
        Loading,
        Finished,
        Canceled,
        Error,
    }

    public class AssetLoaderData : StablePriorityQueueNode, IObjectPoolItem
    {
        internal string[] addresses = new string[0];
        internal string[] paths = new string[0];
        internal bool isInstance = false;
        internal OnAssetLoadComplete complete;
        internal OnAssetLoadProgress progress;
        internal OnBatchAssetLoadComplete batchComplete;
        internal OnBatchAssetsLoadProgress batchProgress;
        internal SystemObject userData = null;
                                                                                                       
        internal AssetHandler handler = null;

        internal DataState State { get; set; }

        private AAssetNode[] assetNodes = null;
        
        public void AddAssetNode(int index, AAssetNode node)
        {
            if(assetNodes == null)
            {
                assetNodes = new AAssetNode[addresses.Length];
            }
            assetNodes[index] = node;
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            
        }

        internal void UpdateDataState()
        {

        }
    }
}
