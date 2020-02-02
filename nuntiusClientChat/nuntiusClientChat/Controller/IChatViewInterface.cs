using System;
using System.Collections.Generic;
using System.Text;
using nuntiusModel;
namespace nuntiusClientChat.Controller
{
    interface IChatViewInterface
    {
        void AddMesages(List<Message> messages);

        void RemoveMessages();

        void EditMessages();
    }
}
