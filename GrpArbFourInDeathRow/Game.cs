using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using GrpArbFourInDeathRow_MessageLib;

namespace GrpArbFourInDeathRow
{
    public class Game
    {
        public string UserName { get; set; }
        public string Info { get; set; }
        public int[,] GameBoard = new int[7, 6];
        private Client myClient;
        private MainWindow _mainWindow;

        public Game(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        public void StartGame()
        {
            StartClient();
            GetUserName();
            _mainWindow.Dispatcher.Invoke(_mainWindow.LockGameBoard);
        }
        private void GetUserName()
        {
            var messageGame = new MessageGame
            {
                MessageType = "Auth",
                Text = "@UserName"
            };
            myClient.Send(messageGame);
        }
        public void SendMoveToServer(int[] coords)
        {
            var messageGame = new MessageGame
            {
                CoordX = coords[0],
                CoordY = coords[1],
                MessageType = "PlayerMove",
                PlayerName = UserName,
                IsFromServer = false
            };
            myClient.Send(messageGame);
            _mainWindow.Dispatcher.Invoke(_mainWindow.LockGameBoard);
        }
        public void StartClient()
        {
            myClient = new Client(this,_mainWindow);
            Thread clientThread = new Thread(myClient.Start);
            clientThread.Start();
        }
        public int[] IsValidMove(Button button)
        {
            int column = Int32.Parse(button.Name[3].ToString()) - 1;
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
            _mainWindow.Dispatcher.Invoke(_mainWindow.UpdateInfoBox);
        }
        public void StartGame(MessageGame messageGame)
        {
            GameBoard = messageGame.BoardState;
            _mainWindow.Dispatcher.Invoke(_mainWindow.LockGameBoard);
            if (messageGame.PlayerName == UserName)
            {
                //our turn
                _mainWindow.Dispatcher.Invoke(_mainWindow.UnlockGameBoard);
                Info = "Your turn";
            }
            else
            {
                //not our turn
                Info = "Opponents turn";
            }
            _mainWindow.Dispatcher.Invoke(_mainWindow.DrawBoard);
            _mainWindow.Dispatcher.Invoke(_mainWindow.UpdateInfoBox);
        }

        public void UpdateGameBoard(MessageGame messageGame)
        {
            if (messageGame.IsFromServer)
            {
                GameBoard = messageGame.BoardState;
                _mainWindow.Dispatcher.Invoke(_mainWindow.DrawBoard);
            }
            if (messageGame.PlayerName != UserName)
            {
                _mainWindow.Dispatcher.Invoke(_mainWindow.UnlockGameBoard);
                Info = "Your turn";
            }
            else
            {
                Info = "Opponents turn";
            }
            _mainWindow.Dispatcher.Invoke(_mainWindow.UpdateInfoBox);
        }

        public void GameOver(MessageGame messageGame)
        {
            _mainWindow.Dispatcher.Invoke(_mainWindow.LockGameBoard);

            Info = messageGame.PlayerName + " Won the game!";
            _mainWindow.Dispatcher.Invoke(_mainWindow.UpdateInfoBox);
        }

        public void ResetRequest()
        {
            
            var messageReset = new MessageGame
            {
                MessageType = "ResetRequest"
            };
             
            myClient.Send(messageReset);
        }
    }
}