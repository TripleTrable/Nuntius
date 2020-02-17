using System;
using System.Collections.Generic;

namespace nuntiusModel
{
    [Serializable]
    public class User
    {
        private string alias;       
        private string password;
        private LinkedList<User> contacts;
        private LinkedList<Message> messages;
        public User()
        {
            
        }

        public User(string alias, string password)
        {
            this.alias = alias;
            this.password = password;
            contacts = new LinkedList<User>();
            messages = new LinkedList<Message>();
        }

        #region Properties

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public LinkedList<User> Contacts
        {
            get { return contacts; }
            set { contacts = value; }
        }

        public LinkedList<Message> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        #endregion
    }
}
