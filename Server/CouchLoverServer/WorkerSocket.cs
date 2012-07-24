using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;


namespace CouchLoverServer
{
    class WorkerSocket
    {
        private Server server;
        private Socket socket;
        private NetworkStream stream;
        private BinaryWriter writer;
        private AsyncCallback workerCallback;

        public WorkerSocket(Socket workerSocket, Server server)
        {
            this.server = server;
            this.socket = workerSocket;
            this.stream = new NetworkStream(socket);
            this.writer = new BinaryWriter(stream, Encoding.UTF8);

            WaitForData(); 
        }

        private void WaitForData()
        {
            try
            {
                workerCallback = new AsyncCallback(OnDataReceived);

                byte[] dataBuffer = new byte[1024];
                socket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, workerCallback, dataBuffer);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        private void OnDataReceived(IAsyncResult asyn)
        {
            byte[] dataBuffer = (byte[])asyn.AsyncState;
            try
            {
                int CharCount = socket.EndReceive(asyn);
                if (CharCount > 0)
                {
                    char[] chars = new char[CharCount];
                    
                    String text = Encoding.UTF8.GetString(dataBuffer);

                    StringReader reader = new StringReader(text);
                    while(reader.Peek() != -1)
                    {
                        string line = reader.ReadLine();
                        if(line.IndexOf(":") != -1) 
                            ExecuteCommand(line);
                        
                    }
                    //AddIncomingPacket(packet);
                    //Debug.WriteLine(String.Format("Packet from {0} UserId: {1}! CharLength: {2} Data: {3}", this.socket.RemoteEndPoint.ToString(), UserID.ToString(), charLen.ToString(), data));
                    WaitForData();
                }
                else
                {
                    throw new SocketException(10054);
                }
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("OnDataReceived: Socket has been closed");
                //server.RemoveWorkerSocket(this);
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10054) // Connection reset by peer
                {
                    string msg = "Client Disconnected";
                    Console.WriteLine(msg);
                    //server.RemoveWorkerSocket(this);
                }
                else
                {
                    Console.WriteLine(se.Message);
                    //server.RemoveWorkerSocket(this);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //server.RemoveWorkerSocket(this);
            }
        }

        public void ExecuteCommand(String data)
        {
            //TODO: Per RegEx groups
            string command = data.Substring(0, data.IndexOf(":"));
            string[] parameters = data.Replace(command + ":", "").Split(';');
            
            switch(command)
            {
                case "mouseUp":
                    //CursorUtils.MouseClick(MouseButtons.Left, parameters[0], parameters[1]);
                    break;
                case "mouseMove":
                    InputUtils.MoveBy(int.Parse(parameters[0]), int.Parse(parameters[1]));
                    break;
                case "leftClick":
                    InputUtils.MouseClick(MouseButtons.Left, Cursor.Position.X, Cursor.Position.Y);
                    break;
                case "rightClick":
                    InputUtils.MouseClick(MouseButtons.Right, Cursor.Position.X, Cursor.Position.Y);
                    break;
                case "sendKey":
                    InputUtils.SendKey(parameters[0]);
                    break;
            }
        }
    }
}
