namespace Dot.Pool
{
    public interface IObjectPoolItem
    {
        void OnGet();
        void OnRelease();
    }
}
