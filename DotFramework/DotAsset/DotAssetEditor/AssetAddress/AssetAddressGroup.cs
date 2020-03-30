using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Asset.AssetAddress
{
    public class AssetAddressGroup : ScriptableObject
    {
        public string groupName = "Asset Address Group";
        public bool isEnable = true;

        public bool isMain = false;
        
        public bool isPreload = false;
        public bool isNeverDestroy = false;

        public AssetAddressOperation operation = new AssetAddressOperation();
        public List<AssetFilter> filters = new List<AssetFilter>();
    }
}
