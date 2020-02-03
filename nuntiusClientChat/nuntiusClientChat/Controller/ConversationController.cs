using System;
using System.Collections.Generic;
using System.Text;
using nuntiusModel;

namespace nuntiusClientChat.Controller
{
    public class ConversationController
    {
        public delegate void MessageAddedEventHandler(object source, MessageEventArgs args);

        public event MessageAddedEventHandler MessageAdded;

        private List<Message> messages;

        public ConversationController()
        {
            messages = new List<Message>();
        }
        public ConversationController(List<Message> messages)
        {
            messages = new List<Message>();

            foreach (Message message in messages)
            {
                AddMessage(message);
            }
        }

        protected virtual void OnMessageAdded(Message message)
        {
            if (MessageAdded != null)
            {
                MessageAdded(this, new MessageEventArgs { Message = message });
            }
        }

        public void AddMessage(Message message)
        {
            messages.Add(message);
            OnMessageAdded(message);
        }

    }

    public class MessageEventArgs
    {
        public Message Message { get; set; }

    }
}
