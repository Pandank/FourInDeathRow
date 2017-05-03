﻿using System;
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
                    Console.WriteLine("hejhej");
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
            if (messageGame.MessageType == "Auth")
            {
                messageGame.MessageType = "AuthResponse";

                if (client == clients[0])
                {
                    messageGame.PlayerName = "Player1";
                }
                else if (client == clients[1])
                {
                    messageGame.PlayerName = "Player2";

                }
                else
                {
                    messageGame.PlayerName = "SOMETHING WENT WRONG";

                }
                NetworkStream n = client.tcpclient.GetStream();
                BinaryWriter w = new BinaryWriter(n);
                messageJson = messageGame.ToJson();
                w.Write(messageJson);
                w.Flush();
                Console.WriteLine("AUTHoutput:"+messageJson);
            }




            foreach (ClientHandler tmpClient in clients)
            {
                if (tmpClient != client)
                {
                    NetworkStream n = tmpClient.tcpclient.GetStream();
                    BinaryWriter w = new BinaryWriter(n);
                    w.Write(messageJson);
                    w.Flush();
                }
                else if (clients.Count() == 1)
                {
                    NetworkStream n = tmpClient.tcpclient.GetStream();
                    BinaryWriter w = new BinaryWriter(n);
                    w.Write("Sorry, you are alone...");
                    w.Flush();
                }
            }
        }

        public void DisconnectClient(ClientHandler client)
        {
            clients.Remove(client);
            Console.WriteLine("Client X has left the building...");
            Broadcast(client, "Client X has left the building...");
        }
    }
}