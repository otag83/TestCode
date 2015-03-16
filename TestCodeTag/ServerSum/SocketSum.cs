using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerSum
{
    public class SocketSum
    {
        
            private readonly WebSocketServer m_server;
            private static SocketResultSum _resultSocketSum;
            private static int SumValues = 0;

        
            public SocketSum(IPAddress address, int port, SocketResultSum resultSocketSum)
            {
                m_server = new WebSocketServer(address, port);
                m_server.OnClientConnected += OnClientConnected;
                _resultSocketSum = resultSocketSum;
                
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
                client.ReceivedTextualData += OnReceivedTextualData;
                client.Disconnected += OnClientDisconnected;
                client.StartReceiving();

                Console.WriteLine("Client {0} Connected...", client.Id);
            }

            private void OnClientDisconnected(ClientConnection client)
            {
                client.ReceivedTextualData -= OnReceivedTextualData;
                client.Disconnected -= OnClientDisconnected;

                Console.WriteLine("Client {0} Disconnected...", client.Id);
            }

            private void OnReceivedTextualData(ClientConnection client, string data)
            {
                Console.WriteLine("Client {0} Received Message: {1}", client.Id, data);
                int value = 0;
                if (int.TryParse(data, out value)) SumValues += value;
                else client.Send(string.Format("Input invalido {0}",data));

                
            }

            public static void SendResultSum(int iteracion)
            {
                _resultSocketSum.socket.Send(string.Format("{0},{1}", SumValues.ToString(),iteracion.ToString()));
                SumValues = 0;
           }
        }
  }
