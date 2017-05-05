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

            //StartGame när 2 är connected till servern
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
            }

            public void Movehandler(MessageGame messageGame)
            {
                //!!! from server
                if (messageGame.GameOver && messageGame.IsFromServer)
                {
                    Console.WriteLine($"{messageGame.PlayerName} won the game");
                }
                else if (!messageGame.IsFromServer)
                {
                    CalculateMove(messageGame, messageGame.CoordX, messageGame.CoordY);
                }



            }

            public void CalculateMove(MessageGame messageGame, int x, int y)
            {
                if (GameBoard[x, y] != 0)
                {
                    //client sent invalid pos
                }
                else
                {
                    GameBoard[x, y] = Convert.ToInt32(messageGame.PlayerName.Substring(7));
                    //Skicka gameboard till p1 p2
                    MessageGame messageGameToClients = new MessageGame
                    {
                        BoardState = GameBoard,
                        MessageType = "GameBoardUpdate",
                        IsFromServer = true,
                        CoordX = -1,
                        CoordY = -1,
                        GameOver = false,
                        Version = 1,
                        Text = "@UpdateGameboard"

                    };
                    _server.Broadcast(messageGameToClients);

                }


                CheckWin(messageGame);
            }

            //todo actually break the gameloop
            public void CheckWin(MessageGame messageGame)
            {
                //if we win abort the session



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
                Movehandler(messageGame);
            }






        }
    }