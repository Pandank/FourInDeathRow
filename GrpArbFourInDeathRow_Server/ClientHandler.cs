using System;
using System.IO;
using System.Net.Sockets;
using GrpArbFourInDeathRow_MessageLib;

namespace GrpArbFourInDeathRow_Server
{
    public class ClientHandler
    {
        public TcpClient tcpclient;
        private Server myServer;
        public ClientHandler(TcpClient c, Server server)
        {
            tcpclient = c;
            this.myServer = server;
        }

        public void Run()
        {
            var messageGame = new MessageGame();

            try
            {

                while (messageGame.Text != "Quit")
                {
                    NetworkStream n = tcpclient.GetStream();
                    var message = new BinaryReader(n).ReadString();
                    messageGame = messageGame.FromJson(message);

                    Console.WriteLine("INPUT: " + message);
                    myServer.Broadcast(this, message);



                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myServer.DisconnectClient(this);
                tcpclient.Close();
            }
        }
    }
}