namespace DotEngine.Framework
{
    public interface IView
    {
        void RegisterMediator(IMediator mediator);
        IMediator RetrieveMediator(string mediatorName);
        IMediator RemoveMediator(string mediatorName);
        bool HasMediator(string mediatorName);
    }
}
