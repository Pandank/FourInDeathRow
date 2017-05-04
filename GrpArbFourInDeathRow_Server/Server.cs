using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GrpArbFourInDeathRow_MessageLib;

namespace GrpArbFourInDeathRow_Server
{
    public class Server
    {
        List<ClientHandler> clients = new List<ClientHandler>();
        public void Run()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            Console.WriteLine("Server up and running, waiting for messages...");

            try
            {
                listener.Start();

                while (true)
                {
                    TcpClient c = listener.AcceptTcpClient();
                    ClientHandler newClient = new ClientHandler(c, this);
                    clients.Add(newClient);

                    Thread clientThread = new Thread(newClient.Run);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

        public void Broadcast(ClientHandler client, string messageJson)
        {
            MessageGame messageGame = new MessageGame();
            messageGame = messageGame.FromJson(messageJson);
            NetworkStream n = client.tcpclient.GetStream();
            BinaryWriter w = new BinaryWriter(n);
            switch (messageGame.MessageType)
            {

                case "Auth":
                    messageGame.MessageType = "AuthResponse";
                    if (client == clients[0])
                    {
                        messageGame.PlayerName = "Player1";

                        messageJson = messageGame.ToJson();
                        w.Write(messageJson);
                        w.Flush();
                    }
                    else if (client == clients[1])
                    {
                        messageGame.PlayerName = "Player2";
                        messageJson = messageGame.ToJson();
                        w.Write(messageJson);
                        w.Flush();
                        StartGameSeassion(client);


                    }
                    else
                    {
                        messageGame.PlayerName = "SOMETHING WENT WRONG";
                    }
                    Console.WriteLine("AUTHoutput:" + messageJson);
                    break;
                case "Move":
                    foreach (var clientHandler in clients)
                    {
                        NetworkStream nTemp = clientHandler.tcpclient.GetStream();
                        BinaryWriter wTemp = new BinaryWriter(nTemp);
                        messageGame.MessageType = "MoveResponse";
                        messageJson = messageGame.ToJson();
                        wTemp.Write(messageJson);
                        wTemp.Flush();
                    }
                    Console.WriteLine("MOVEoutput:" + messageJson);
                    break;
                case "Begin":
                    w.Write(messageJson);
                    w.Flush();
                    Console.WriteLine("BEGINoutput: " + messageJson);
                    break;
                case "Error":
                    break;
                default:
                    break;
            }

        }

        private void StartGameSeassion(ClientHandler client)
        {
            Random random = new Random();
            MessageGame messageGame = new MessageGame
            {
                Version = 1,
                MessageType = "Begin",
                PlayerName = "Player"+random.Next(1,3)
                
            };
            var messageJson = messageGame.ToJson();
            Broadcast(client, messageJson);

        }

        public void DisconnectClient(ClientHandler client)
        {
            clients.Remove(client);
            Console.WriteLine("Client X has left the building...");
            Broadcast(client, "Client X has left the building...");
        }
    }
}