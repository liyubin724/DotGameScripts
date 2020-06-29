using DotEngine.Timeline.Data;
using DotEngine.Timeline.Item.Factory;

namespace DotEngine.Timeline.Item
{
    public interface IActionItemFactory
    {
        void RegisterPool(int id, IActionItemPool pool);
        ActionItem RetainItem(ActionData actionData);
        void ReleaseItem(ActionItem item);
        void DoClear();
        void DoDestroy();
    }
}
