using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuntiusServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketListener listener = new SocketListener();
            listener.StartListening();
            Console.Read();
        }
    }
}
