using DotEngine.Pool;
using System;
using System.Collections.Generic;

namespace DotEngine.Timeline.Item
{
    public class ActionItemFactory
    {
        private Dictionary<Type, ObjectItemPool<ActionItem>> itemPoolDic = new Dictionary<Type, ObjectItemPool<ActionItem>>();

        private static ActionItemFactory itemFactory = null;

        private ActionItemFactory() { }

        public static ActionItemFactory GetInstance()
        {
            if (itemFactory == null)
            {
                itemFactory = new ActionItemFactory();
            }
            return itemFactory;
        }

        public void RegisterItemPool(Type dataType,ObjectItemPool<ActionItem> itemPool)
        {
            if(!itemPoolDic.ContainsKey(dataType))
            {
                itemPoolDic.Add(dataType, itemPool);
            }
        }

        public ActionItem RetainItem(Type dataType)
        {
            if(itemPoolDic.TryGetValue(dataType,out ObjectItemPool<ActionItem> itemPool))
            {
                return itemPool.GetItem();
            }
            return null;
        }

        public void ReleaseItem(ActionItem item)
        {
            Type dataType = item.Data.GetType();
            if (itemPoolDic.TryGetValue(dataType, out ObjectItemPool<ActionItem> itemPool))
            {
                itemPool.ReleaseItem(item);
            }
        }

        public void DoClear()
        {
            foreach(var kvp in itemPoolDic)
            {
                kvp.Value.Clear();
            }
            itemPoolDic.Clear();
        }

        public void DoDestroy()
        {
            DoClear();

            itemFactory = null;
        }
    }
}
