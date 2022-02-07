using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace SpiderFleetWebAPI.Utils.WebSocket
{
    public class AsyncSocketListener
    {
        //https://radu-matei.com/blog/aspnet-core-websockets-middleware/
        //https://www.youtube.com/watch?v=c8tUf0-pzwg
        //https://www.youtube.com/watch?v=JOnbtwraU8g
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public AsyncSocketListener()
        {

        }

        public static void StartListening()
        {
            byte[] bytes = new Byte[4096];
            IPAddress ip = IPAddress.Parse("192.168.0.9");
            IPEndPoint point = new IPEndPoint(ip, 11000);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(point);
                listener.Listen(100);

                while(true)
                {
                    allDone.Reset();

                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    allDone.WaitOne();

                }
            }
            catch(Exception ex)
            {

            }
            
        }
        

        public static void AcceptCallback(IAsyncResult result)
        {
            allDone.Set();

            Socket listener = (Socket)result.AsyncState;
            Socket handler = listener.EndAccept(result);

            StateObject state = new StateObject();
            state.workSocket = handler;

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public static void ReadCallback(IAsyncResult result)
        {
            string content = string.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)result.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(result);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the
                    // client. Display it on the console. 
                    


                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }


        private static void SendCallback(IAsyncResult result)
        {
            try
            {
                Socket socket = (Socket)result.AsyncState;
                int byteSent = socket.EndSend(result);

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch(Exception ex)
            {

            }
        }
    }
}