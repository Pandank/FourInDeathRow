using System;
using GrpArbFourInDeathRow_MessageLib;

namespace GrpArbFourInDeathRow_Server
{
    public class Game
    {

        //gameboard
        public int[,] GameBoard = new int[7, 6];

        private Server _server;

        public Game(Server server)
        {
            _server = server;
        }

        public void StartGame()
        {
            InitiateBoard();
        }
        private void InitiateBoard()
        {
            for (int x = 0; x < GameBoard.GetLength(0); x++)
            {
                for (int y = 0; y < GameBoard.GetLength(1); y++)
                {
                    GameBoard[x, y] = 0;
                }
            }

            //todo who goes first?
            MessageGame messageStartGame = new MessageGame
            {
                BoardState = GameBoard,
                GameOver = false,
                IsFromServer = true,
                MessageType = "StartGame",
                PlayerName = "Player1",
                Text = "Game Starts now Player 1 goes first",
                Version = 1
            };
            _server.Broadcast(messageStartGame);

        }

        public void Movehandler(MessageGame messageGame)
        {
            //!!! from server
            if (messageGame.GameOver && messageGame.IsFromServer)
            {
                //if we are here someone won
                Console.WriteLine($"{messageGame.PlayerName} won the game");
                messageGame.MessageType = "GameOver";
                _server.Broadcast(messageGame);
            }
            else if (!messageGame.IsFromServer)
            {
                CalculateMove(messageGame);
            }



        }

        public void CalculateMove(MessageGame messageGame)
        {
            int x = messageGame.CoordX;
            int y = messageGame.CoordY;
            if (GameBoard[x, y] != 0)
            {
                //client sent invalid pos
                throw new NotImplementedException();
            }
            else
            {
                if (messageGame.PlayerName == "Player1")
                {
                    GameBoard[x, y] = 1;
                }
                else if (messageGame.PlayerName == "Player2")
                {
                    GameBoard[x, y] = 2;

                }
                //Skicka gameboard till p1 p2
                SendMoveToClients(messageGame);
            }

        }

        public void SendMoveToClients(MessageGame messageGame)
        {

            MessageGame messageGameToClients = new MessageGame
            {
                BoardState = GameBoard,
                MessageType = "GameBoardUpdate",
                IsFromServer = true,
                CoordX = -1,
                CoordY = -1,
                GameOver = false,
                Version = 1,
                Text = "@UpdateGameboard",
                PlayerName = messageGame.PlayerName
            };

            _server.Broadcast(messageGameToClients);
            CheckWin(messageGame);
        }


        public void CheckWin(MessageGame messageGame)
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
                        //won
                        Console.WriteLine("won");
                        messageGame.GameOver = true;
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

                        //won
                        Console.WriteLine("won");
                        messageGame.GameOver = true;
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

                        //won
                        Console.WriteLine("won");
                        messageGame.GameOver = true;
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

                        //won
                        Console.WriteLine("won");
                        messageGame.GameOver = true;
                    }
                }
            }
            messageGame.IsFromServer = true;
            if (messageGame.IsFromServer && messageGame.GameOver)
            {
                Movehandler(messageGame);
            }
        }
    }
}