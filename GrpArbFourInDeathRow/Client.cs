using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace GrpArbFourInDeathRow
{
    partial class Program
    {
        public class Client
        {
            private TcpClient client;

            public void Start()
            {
                client = new TcpClient("192.168.25.129", 5000);

                Thread listenerThread = new Thread(Send);
                listenerThread.Start();

                Thread senderThread = new Thread(Listen);
                senderThread.Start();

                senderThread.Join();
                listenerThread.Join();
            }

            public void Listen()
            {
                string message = "";

                try
                {
                    while (true)
                    {
                        NetworkStream n = client.GetStream();
                        message = new BinaryReader(n).ReadString();
                        Console.WriteLine("Other: " + message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void Send()
            {
                string message = "";

                try
                {
                    while (!message.Equals("quit"))
                    {
                        NetworkStream n = client.GetStream();

                        message = Console.ReadLine();
                        BinaryWriter w = new BinaryWriter(n);
                        w.Write(message);
                        w.Flush();
                    }

                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
