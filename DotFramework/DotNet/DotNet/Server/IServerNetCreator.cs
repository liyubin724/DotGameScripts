using Dot.Net.Message;

namespace Dot.Net.Server
{
    public interface IServerNetCreator
    {
        IMessageReader GetMessageReader();
        IMessageWriter GetMessageWriter();
    }
}
