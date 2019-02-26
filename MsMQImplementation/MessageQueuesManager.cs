using System.Messaging;

namespace Data
{
    public sealed class MessageQueuesManager : SingletonBase<MessageQueuesManager>
    {
        private const string PRIVATE_PATH = @".\Private$\";

        private MessageQueuesManager()
        {
        }

        public MessageQueue CreateQueue<T>(IReceiver<T> receiver, string distinctivePath)
        {
            MessageQueue msMq = null;

            // check if queue exists, if not create it
            if (!MessageQueue.Exists(PRIVATE_PATH + receiver.Name + distinctivePath))
            {
                msMq = MessageQueue.Create(PRIVATE_PATH + receiver.Name + distinctivePath);
            }
            else

            {
                msMq = new MessageQueue(PRIVATE_PATH + receiver.Name + distinctivePath);
            }

            msMq.Formatter = JsonFormatter.Instance;
            msMq.ReceiveCompleted += receiver.OnReceive;
            receiver.ReceivingQueue(typeof(T), msMq);
            msMq.BeginReceive();

            return msMq;
        }

        public void SendMessage(object body, MessageQueue targetQueue)
        {
            var msg = new Message(body, JsonFormatter.Instance);
            targetQueue.Send(msg);
        }

        public void SendMessage(object body, string label, MessageQueue targetQueue)
        {
            var msg = new Message(body, JsonFormatter.Instance);
            msg.Label = label;
            targetQueue.Send(msg);
        }

        public void SendMessage(object body, string label, MessageQueue targetQueue, MessageQueue responseQueue)
        {
            var msg = new Message(body, JsonFormatter.Instance);
            msg.Label = label;
            msg.ResponseQueue = responseQueue;

            targetQueue.Send(msg);
        }
    }
}