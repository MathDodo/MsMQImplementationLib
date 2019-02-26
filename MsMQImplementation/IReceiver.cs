using System;
using System.Messaging;

namespace Data
{
    public interface IReceiver<T>
    {
        string Name { get; }

        void ReceivingQueue(Type queueReceiveType, MessageQueue messageQueue);

        void OnReceive(object messageQueue, ReceiveCompletedEventArgs asyncResult);
    }
}