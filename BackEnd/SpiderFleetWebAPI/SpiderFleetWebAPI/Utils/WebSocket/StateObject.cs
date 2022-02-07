using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace SpiderFleetWebAPI.Utils.WebSocket
{
    public class StateObject
    {
        //https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example

        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }
}