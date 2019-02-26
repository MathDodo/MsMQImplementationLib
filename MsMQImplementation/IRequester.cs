using System.Messaging;

namespace Data
{
    public interface IRequester
    {
        void Request(MessageQueue queueToSendRequest);
    }
}