namespace DotEngine.Timeline.Item
{
    public interface IActionItemPool
    {
        ActionItem RetainItem();
        void ReleaseItem(ActionItem item);

        void DoClear();
        void DoDestroy();
    }
}
