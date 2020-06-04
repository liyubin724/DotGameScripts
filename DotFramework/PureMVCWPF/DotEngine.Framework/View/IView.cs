namespace DotEngine.Framework
{
    public interface IView
    {
        void RegisterMediator(IViewController mediator);
        IViewController RetrieveMediator(string mediatorName);
        IViewController RemoveMediator(string mediatorName);
        bool HasMediator(string mediatorName);
    }
}
