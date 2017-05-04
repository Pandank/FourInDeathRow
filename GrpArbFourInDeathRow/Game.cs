using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using GrpArbFourInDeathRow_MessageLib;

namespace GrpArbFourInDeathRow
{
    public class Game
    {
        public string UserName { get; set; }

        public int[,] GameBoard = new int[7, 6];
        private Client myClient;
        private MainWindow _mainWindow;

        public Game(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public enum Tile
        {
            Blank = 0,
            Player1 = 1,
            Player2 = 2
        }

        public void StartGame()
        {
            InitiateBoard();
            StartClient();
            GetUserName();


        }

        private void GetUserName()
        {
            var messageGame = new MessageGame();
            messageGame.MessageType = "Auth";
            messageGame.Text = "@UserName";
            myClient.Send(messageGame);
        }



        public void SendMoveToServer(int x, int y)
        {
            var messageGame = new MessageGame();
            messageGame.CoordX = x;
            messageGame.CoordY = y;
            messageGame.MessageType = "Move";
            messageGame.PlayerName = UserName;
            myClient.Send(messageGame);
            LockGameBoard();
        }



        private void InitiateBoard()
        {
            for (int x = 0; x < GameBoard.GetLength(0); x++)
            {
                for (int y = 0; y < GameBoard.GetLength(1); y++)
                {
                    GameBoard[x, y] = (int)Tile.Blank;
                }
            }
        }

        public void StartClient()
        {

            myClient = new Client(this);
            Thread clientThread = new Thread(myClient.Start);
            clientThread.Start();
            //clientThread.Join();

            //todo wait for server to tell us or player 2 to start

            //todo Our turn wait for input on board and then submit the move

            //todo wait for player 2

            //startgame()
            ////loop
            //Play

            //click the board
            //write the messege to send to server
            //server accepts message
            //player2
            //player 2 clicks the board
            //We server accepts the message
            //we get the move player 2 did

            ////Someone won
            //



        }


        public int[] CalculateMove(string buttonName)
        {
            //col2btn
            int column = Int32.Parse(buttonName[3].ToString()) - 1;
            int validYpos = 10;
            int[] coords = new int[2];
            for (int y = 5; y >= 0; y--)
            {
                if (GameBoard[column, y] == 0)
                {
                    if (validYpos > y)
                    {
                        validYpos = y;
                    }
                }

            }

            if (validYpos <= 5)
            {
                SendMoveToServer(column, validYpos);
                coords[0] = column;
                coords[1] = validYpos;
                return coords;
            }
            else
            {
                coords[0] = -1;
                coords[1] = -1;
                return coords;
            }
        }


        public void AuthResponse(MessageGame messageGame)
        {
            UserName = messageGame.PlayerName;
        }

        public void ProcessMove(MessageGame messageGame)
        {
            try
            {
                string CurrentPlayer = messageGame.PlayerName;
                int newCoordY = messageGame.CoordY;
                int newCoordX = messageGame.CoordX;
                if (CurrentPlayer == "Player1")
                {
                    GameBoard[newCoordX, newCoordY] = 1;

                }
                else if (CurrentPlayer == "Player2")
                {
                    GameBoard[newCoordX, newCoordY] = 2;
                }
                _mainWindow.Dispatcher.Invoke(_mainWindow.DrawBoard);
                CheckForWin();
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        private void CheckForWin()
        {
            //horizontal
            for (int y = 0; y <= 5; y++)
            {
                for (int x = 0; x <= 3; x++)
                {
                    if (GameBoard[x, y] != 0 && GameBoard[x, y] == GameBoard[x + 1, y] &&
                        GameBoard[x, y] == GameBoard[x + 2, y] &&
                        GameBoard[x, y] == GameBoard[x + 3, y])
                    {

                        MessageBox.Show("YOUWIN");
                    }
                }
            }

            //vertical
            for (int y = 0; y <= 2; y++)
            {
                for (int x = 0; x <= 6; x++)
                {
                    if (GameBoard[x, y] != 0 &&
                        GameBoard[x, y] == GameBoard[x, y + 1] &&
                        GameBoard[x, y] == GameBoard[x, y + 2] &&
                        GameBoard[x, y] == GameBoard[x, y + 3])
                    {

                        MessageBox.Show("YOUWIN");
                    }
                }
            }


            //diagonal falling to the left
            for (int y = 0; y <= 2; y++)
            {
                for (int x = 0; x <= 3; x++)
                {
                    if (GameBoard[x, y] != 0 &&
                        GameBoard[x, y] == GameBoard[x + 1, y + 1] &&
                        GameBoard[x, y] == GameBoard[x + 2, y + 2] &&
                        GameBoard[x, y] == GameBoard[x + 3, y + 3])
                    {

                        MessageBox.Show("YOUWIN");
                    }
                }
            }

            //diagonal falling to the right
            for (int y = 0; y <= 2; y++)
            {
                for (int x = 3; x <= 6; x++)
                {
                    if (GameBoard[x, y] != 0 &&
                        GameBoard[x, y] == GameBoard[x - 1, y + 1] &&
                        GameBoard[x, y] == GameBoard[x - 2, y + 2] &&
                        GameBoard[x, y] == GameBoard[x - 3, y + 3])
                    {

                        MessageBox.Show("YOUWIN");
                    }
                }
            }







        }

        public void Begin(MessageGame messageGame)
        {
            if (messageGame.PlayerName == UserName)
            {
                //our turn
                UnlockGameBoard();

            }
            else
            {
                //not our turn
            }
        }
        private void LockGameBoard()
        {
            //lock the buttons
        }

        private void UnlockGameBoard()
        {
            //unlock the buttons
        }
    }
}