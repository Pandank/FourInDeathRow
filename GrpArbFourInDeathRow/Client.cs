using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using GrpArbFourInDeathRow_MessageLib;

namespace GrpArbFourInDeathRow
{

    public class Client
    {
        public Client(Game game, MainWindow mainWindow)
        {
            _game = game;
            _mainWindow = mainWindow;
        }
        private TcpClient client;
        private Game _game;
        private MainWindow _mainWindow;

        public void Start()
        {
            client = new TcpClient("192.168.25.129", 5000);

            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();
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
                    messageGame = messageGame.FromJson(message);
                    switch (messageGame.MessageType)
                    {
                        case "AuthResponse":
                            _game.AuthResponse(messageGame);
                            break;
                        //case "GameReset":
                        //    _mainWindow.Dispatcher.Invoke(_mainWindow.ResetGame);
                            //break;
                        case "StartGame":
                            _game.StartGame(messageGame);
                            break;
                        case "GameBoardUpdate":
                            _game.UpdateGameBoard(messageGame);
                            break;
                        case "GameOver":
                            _game.GameOver(messageGame);
                            break;
                            default:
                            break;
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
    }
}
