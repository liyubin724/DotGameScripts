using Dot.Log;
using Dot.Lua.Register;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace Dot.UI
{
    [RequireComponent(typeof(LoopListView2),typeof(ScrollRect))]
    public class LuaLoopListViewController : LuaScriptBindBehaviour
    {
        private static readonly string FUNC_GET_ITEM_NAME = "GetItemName";
        private static readonly string ACTION_SET_ITEM_DATA = "SetItemData";

        public LoopListView2 listView = null;

        protected override void OnInitFinished()
        {
            if(listView == null)
            {
                listView = GetComponent<LoopListView2>();
            }
            if(listView != null)
            {
                ObjTable.Set("listViewController", this);
            }else
            {
                LogUtil.LogError("LoopListView", "LuaLoopListViewController::Awake->listview is Null");
            }
        }

        public void InitListView(int tototalCount)
        {
            if(listView!=null)
            {
                listView.InitListView(tototalCount, OnGetItemByIndex);
            }
        }

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            string itemName = CallFunc(FUNC_GET_ITEM_NAME,index);
            if(string.IsNullOrEmpty(itemName))
            {
                return null;
            }

            LoopListViewItem2 item = listView.NewListViewItem(itemName);
            if(item.LuaListViewItem ==null)
            {
                item.LuaListViewItem = item.GetComponent<LuaLoopListViewItem>();
                item.LuaListViewItem.InitLua();
            }

            if(item.LuaListViewItem != null)
            {
                CallAction(ACTION_SET_ITEM_DATA, item.LuaListViewItem.ObjTable, index);
            }
            else
            {
                LogUtil.LogError("LoopListView", "LuaLoopListViewController::OnGetItemByIndex->the behaviour of ObjectBind is Null");
            }
            
            return item;
        }

        public void SetListItemCount(int itemCount,bool resetPos = true)
        {
            listView?.SetListItemCount(itemCount, resetPos);
        }

        public LuaTable GetShownItemByItemIndex(int itemIndex)
        {
            LoopListViewItem2 item = listView?.GetShownItemByItemIndex(itemIndex);
            if (item != null && item.LuaListViewItem != null)
            {
                return item.LuaListViewItem.ObjTable;
            }
            return null;
        }

        public LuaTable GetShownItemByIndex(int index)
        {
            LoopListViewItem2 item = listView?.GetShownItemByIndex(index);
            if(item != null && item.LuaListViewItem != null)
            {
                return item.LuaListViewItem.ObjTable;
            }
            return null;
        }

        public void RefreshItemByItemIndex(int itemIndex)
        {
            listView?.RefreshItemByItemIndex(itemIndex);
        }

        public void RefreshAllShownItem()
        {
            listView?.RefreshAllShownItem();
        }

        public void MovePanelToItemIndex(int itemIndex,float offset)
        {
            listView?.MovePanelToItemIndex(itemIndex, offset);
        }

        public void OnItemSizeChanged(int itemIndex)
        {
            listView?.OnItemSizeChanged(itemIndex);
        }
    }
}
