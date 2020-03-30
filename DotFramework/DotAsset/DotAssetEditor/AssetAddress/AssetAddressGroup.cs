using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Asset.AssetAddress
{
    [CreateAssetMenu(fileName = "asset_address_group", menuName = "Asset/Asset Address Group", order = 0)]
    public class AssetAddressGroup : ScriptableObject
    {
        public string groupName = "Asset Address Group";
        public bool isEnable = true;

        public bool isMain = false;
        
        public bool isPreload = false;
        public bool isNeverDestroy = false;

        public List<AssetFilter> finders = new List<AssetFilter>();
        public AssetAddressOperation operation = new AssetAddressOperation();
    }
}
