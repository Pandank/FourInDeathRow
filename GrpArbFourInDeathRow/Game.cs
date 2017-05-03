﻿using System;
using System.Threading;
using GrpArbFourInDeathRow_MessageLib;

namespace GrpArbFourInDeathRow
{
    public class Game
    {
        public string PlayerName { get; set; }

        public int[,] GameBoard = new int[7, 6];
        private Client myClient;

        public enum Tile
        {
            Blank = 0,
            Player1 = 1,
            Player2 = 2
        }

        public void StartGame()
        {
            InitiateBoard();
            StartClient();//todo get playerID to set PlayerName


        }

        public void Play()
        {
            SendMoveToServer(1, 1); //todo get move from Click event on buttons


        }

        private void SendMoveToServer(int x, int y)
        {
            var messageGame = new MessageGame();
            messageGame.CoordX = x;
            messageGame.CoordY = y;
            messageGame.PlayerName = PlayerName;
            myClient.Send(messageGame);
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
            clientThread.Join();

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

        public void ProcessInput(MessageGame messageGame)
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
            //RefreshUI
            Play();

        }
    }
}