namespace DotEngine.Framework
{
    public interface IService
    {
        string Name { get; }

        void DoRegister();
        void DoRemove();
    }
}
