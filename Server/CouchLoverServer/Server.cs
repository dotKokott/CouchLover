using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;


namespace CouchLoverServer
{
    class Server
    {
        private Socket socket;
        private List<WorkerSocket> workerSockets;

        public void Start(int port)
        {
            workerSockets = new List<WorkerSocket>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);
            socket.Bind(ipLocal);
            socket.Listen(int.MaxValue);

            socket.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }

        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                Socket workerSocket = socket.EndAccept(asyn);                
                workerSockets.Add(new WorkerSocket(workerSocket, this));
                socket.BeginAccept(new AsyncCallback(OnClientConnect), null);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debug.WriteLine("Socket has been closed");
            }
            catch (SocketException se)
            {
                System.Diagnostics.Debug.WriteLine(se.Message);
            }
        }
    }           
}
