using System.Messaging;

namespace Data
{
    public interface ITransmitter<T>
    {
        void SendMessage(MessageQueue messageQueue);
    }
}