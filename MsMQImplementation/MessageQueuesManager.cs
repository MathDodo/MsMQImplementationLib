using System.Messaging;

namespace Data
{
    public sealed class MessageQueuesManager : SingletonBase<MessageQueuesManager>
    {
        private const string PRIVATE_PATH = @".\Private$\";

        public readonly string MulticastPath = @"formatname:MULTICAST=234.1.1.1:8001";

        private MessageQueuesManager()
        {
        }

        public MessageQueue CreateQueue(string distinctivePath)
        {
            MessageQueue msMq = MessageQueue.Exists(PRIVATE_PATH + distinctivePath) ? new MessageQueue(PRIVATE_PATH + distinctivePath) :
                MessageQueue.Create(PRIVATE_PATH + distinctivePath);

            msMq.Formatter = JsonFormatter.Instance;

            return msMq;
        }

        public MessageQueue CreateMulticastQueue<T>(IReceiver<T> receiver)
        {
            MessageQueue msMq = null;

            msMq = new MessageQueue(MulticastPath);

            msMq.Formatter = JsonFormatter.Instance;
            receiver.ReceivingQueue(typeof(T), msMq);

            return msMq;
        }

        public MessageQueue CreateQueue<T>(IReceiver<T> receiver, string distinctivePath)
        {
            MessageQueue msMq = MessageQueue.Exists(PRIVATE_PATH + receiver.Name + distinctivePath) ? new MessageQueue(PRIVATE_PATH + receiver.Name + distinctivePath) :
                MessageQueue.Create(PRIVATE_PATH + receiver.Name + distinctivePath);

            msMq.Formatter = JsonFormatter.Instance;
            msMq.ReceiveCompleted += receiver.OnReceive;
            receiver.ReceivingQueue(typeof(T), msMq);
            msMq.BeginReceive();

            return msMq;
        }

        public MessageQueue CreateQueue<T>(IReceiver<T> receiver, string distinctivePath, string multicastPath)
        {
            MessageQueue msMq = MessageQueue.Exists(PRIVATE_PATH + receiver.Name + distinctivePath) ? new MessageQueue(PRIVATE_PATH + receiver.Name + distinctivePath) :
                            MessageQueue.Create(PRIVATE_PATH + receiver.Name + distinctivePath, false);

            msMq.Formatter = JsonFormatter.Instance;
            msMq.MulticastAddress = multicastPath;
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

        public void SendMessage(object body, string label, MessageQueue targetQueue, MessageQueue responseQueue, string correlationID)
        {
            var msg = new Message(body, JsonFormatter.Instance);
            msg.CorrelationId = correlationID;
            msg.Label = label;
            msg.ResponseQueue = responseQueue;

            targetQueue.Send(msg);
        }
    }
}