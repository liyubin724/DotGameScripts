using DotEngine.Timeline.Data;
using System.Collections.Generic;

namespace DotEngine.Timeline.Item
{
    public sealed class ActionItemFactory : IActionItemFactory
    {
        private Dictionary<int, IActionItemPool> itemPoolDic = new Dictionary<int, IActionItemPool>();

        private static ActionItemFactory itemFactory = null;

        private ActionItemFactory() { }

        public static ActionItemFactory GetInstance()
        {
            if(itemFactory == null)
            {
                itemFactory = new ActionItemFactory();
            }
            return itemFactory;
        }

        public void DoClear()
        {
            foreach(var kvp in itemPoolDic)
            {
                kvp.Value.DoClear();
            }
            itemPoolDic.Clear();
        }

        public void DoDestroy()
        {
            DoClear();

            itemFactory = null;
        }

        public void RegisterPool(int id, IActionItemPool pool)
        {
            if(itemPoolDic.ContainsKey(id))
            {
                throw new TimelineException(TimelineConst.FACTORY_POOL_HAS_BEEN_ADDED_ERROR, pool.GetType().FullName);
            }

            itemPoolDic.Add(id, pool);
        }

        public void ReleaseItem(ActionItem item)
        {
            if(itemPoolDic.TryGetValue(item.Data.Id,out IActionItemPool pool))
            {
                pool.ReleaseItem(item);
            }else
            {
                throw new TimelineException(TimelineConst.FACTORY_POOL_NOT_FOUND_ERROR,item.Data.Id);
            }
        }

        public ActionItem RetainItem(ActionData actionData)
        {
            if (itemPoolDic.TryGetValue(actionData.Id, out IActionItemPool pool))
            {
                return pool.RetainItem();
            }else
            {
                throw new TimelineException(TimelineConst.FACTORY_POOL_NOT_FOUND_ERROR, actionData.Id);
            }
        }
    }
}
