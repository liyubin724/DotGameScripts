using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.AssetFilter.AssetAddress
{
    [CreateAssetMenu(fileName = "asset_address_group", menuName = "Asset/Asset Address Group", order = 0)]
    public class AssetAddressGroup : ScriptableObject
    {
        public string groupName = "Asset Address Group";
        public bool isEnable = true;

        public bool isMain = false;
        
        public bool isPreload = false;
        public bool isNeverDestroy = false;

        public List<AssetFilterFinder> finders = new List<AssetFilterFinder>();
        public AssetAddressOperation operation = new AssetAddressOperation();
    }
}
