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
        private GameServer game;
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

        public void Broadcast(MessageGame messageGameToClients)
        {
            var messageToClientsJson = "";
            foreach (var clientHandler in clients)
            {
                NetworkStream nTemp = clientHandler.tcpclient.GetStream();
                BinaryWriter wTemp = new BinaryWriter(nTemp);
                messageToClientsJson = messageGameToClients.ToJson();
                wTemp.Write(messageToClientsJson);
                wTemp.Flush();
            }
            Console.WriteLine("GAMEoutput:" + messageToClientsJson);
        }
        public void Broadcast(ClientHandler client, string messageJson)
        {
            MessageGame messageGame = new MessageGame();
            messageGame = messageGame.FromJson(messageJson);
            NetworkStream n = client.tcpclient.GetStream();
            BinaryWriter w = new BinaryWriter(n);
            switch (messageGame.MessageType)
            {
                case "PlayerMove":
                    game.Movehandler(messageGame);
                    break;
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
                        game = new GameServer(this);
                        game.StartGame(); 
                    }

                    Console.WriteLine("AUTHoutput:" + messageJson);
                    break;
                case "ResetRequest":
                    //game.GameBoard = new int[7, 6];
                    //MessageGame messageReset = new MessageGame
                    //{
                    //    BoardState = game.GameBoard
                    //};
                    //Broadcast(messageReset);
                    game.InitiateBoard();
                    break;

                default:
                    throw new ArgumentException();
                    
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