﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using GrpArbFourInDeathRow_MessageLib;
using Newtonsoft.Json;

namespace GrpArbFourInDeathRow
{

    public class Client
    {
        public Client(Game game)
        {
            _game = game;
        }

        private TcpClient client;
        private Game _game;

        public void Start()
        {
            client = new TcpClient("192.168.25.73", 5000);

            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();

            //Thread senderThread = new Thread(Send);
            //senderThread.Start();

            //senderThread.Join();
            listenerThread.Join();
        }

        public void Listen()
        {
            MessageGame messageGame = new MessageGame { Version = 1 };

            string message = "";

            try
            {
                while (true)
                {
                    NetworkStream n = client.GetStream();
                    message = new BinaryReader(n).ReadString();
                    messageGame.FromJson(message);
                    if (messageGame.MessageType != "AuthRespone")
                    {
                        _game.ProcessInput(messageGame);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Send(MessageGame messageGame)
        {

            string message = "";
            messageGame.Version = 1;
            message = messageGame.ToJson();
            try
            {
                NetworkStream n = client.GetStream();
                BinaryWriter w = new BinaryWriter(n);
                w.Write(message);
                w.Flush();


                //client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //public void Send( )
        //{

        //    string message = "tst";
        //    try
        //    {
        //        NetworkStream n = client.GetStream();
        //        BinaryWriter w = new BinaryWriter(n);
        //        w.Write(message);
        //        w.Flush();


        //        //client.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
    }
}
