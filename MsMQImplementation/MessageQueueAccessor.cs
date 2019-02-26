using System;
using System.Messaging;
using System.Collections.Generic;

namespace Data
{
    public abstract class MessageQueueAccessor
    {
        protected Dictionary<Type, MessageQueue> _msgReceivingQueues = new Dictionary<Type, MessageQueue>();

        public MessageQueue this[Type queueType]
        {
            get
            {
                return _msgReceivingQueues[queueType];
            }
        }

        public void ReceiveQueue(Type queueReceiveType, MessageQueue messageQueue)
        {
            _msgReceivingQueues.Add(queueReceiveType, messageQueue);
        }
    }
}