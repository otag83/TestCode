using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerSum
{
    public class SocketResultSum
    {
        private readonly WebSocketServer m_server;

        public ClientConnection socket;

         public SocketResultSum(IPAddress address, int port)
            {
                m_server = new WebSocketServer(address, port);
                m_server.OnClientConnected += OnClientConnected;
            }

            public void Start()
            {
                m_server.Start();
            }

            public void Stop()
            {
                m_server.Stop();
            }

            private void OnClientConnected(ClientConnection client)
            {
                client.SendTextualData += OnSendTextualData;
                client.Disconnected += OnClientDisconnected;
                client.StartReceiving();
                socket = client;
                Console.WriteLine("Client {0} Connected...", client.Id);
            }
           
            private void OnClientDisconnected(ClientConnection client)
            {
                client.SendTextualData -= OnSendTextualData;
                client.Disconnected -= OnClientDisconnected;

                Console.WriteLine("Client {0} Disconnected...", client.Id);
            }

            private void OnSendTextualData(ClientConnection client, string data)
            {
                Console.WriteLine("Client {0} Received Message: {1}", client.Id, data);
                client.Send(data);
            }

    }
}
