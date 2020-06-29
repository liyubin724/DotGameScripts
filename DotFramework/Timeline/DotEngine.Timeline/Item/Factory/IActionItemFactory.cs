using DotEngine.Timeline.Data;

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
