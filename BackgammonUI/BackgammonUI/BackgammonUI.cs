using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BackgammonLogic;

namespace BackgammonUI
{
    class GameUI
    {
        private readonly Game _game = new Game();
        private readonly StringBuilder _colsDisplay = new StringBuilder();
        private readonly StringBuilder _numberingDisplayDown = new StringBuilder();
        private readonly StringBuilder _numberingDisplayUp = new StringBuilder();
        private Player _player1 = new Player(Cell.X);
        private Player _player2;
        private int _nextToolInEatenZone = -2;
        private bool _endOfGame = false;

        public GameUI()
        {
            _game.Computer.Tool = Cell.O;
            _game.Computer.Name = "Watson";
            InitializeNumbering();
        }

        public void Menu()
        {
            int numberOfPlayers = 0;
            bool isRematch = false;
            bool currectInput, isComputerPlayer = false;
            string menu =
@"One player enter '1'.
Two players enter '2'.";

            string numOfPlayersStr = string.Empty;
            do
            {
                Console.Clear();
                Console.WriteLine(
@"-----Menu-----
{0}", menu);
                numOfPlayersStr = Console.ReadLine();
                currectInput = int.TryParse(numOfPlayersStr, out numberOfPlayers);
            }
            while (!currectInput || (numberOfPlayers > 2 || numberOfPlayers < 1));

            Console.Clear();
            Console.Write("Please enter first player name: ");
            _player1.Name = Console.ReadLine();

            if(numberOfPlayers == 2)
            {
                Console.Write("Please enter second player name: ");
                _player2 = new Player(Cell.X);
                _player2.Name = Console.ReadLine();
            }
            else
            {
                _player2 = _game.Computer;
                isComputerPlayer = true;
            }

            string rematchStr = string.Empty;
            int rematchAnswer = 0;
            do
            {
                _game.InitializeGame();
                Start(_player1, _player2, isComputerPlayer);
                do
                {
                    Console.WriteLine(
@"Would you like to rematch
1. Yes
2. No");
                    rematchStr = Console.ReadLine();
                    currectInput = int.TryParse(rematchStr, out rematchAnswer);
                    PrintAllBoard();
                }
                while (!currectInput || (rematchAnswer > 2 || rematchAnswer < 1));
                isRematch = false;

                if(rematchAnswer == 1)
                {
                    isRematch = true;
                    _endOfGame = false;
                }
            }
            while (isRematch);
        }

        private void Start(Player firstPlayer, Player secondPlayer, bool onePlayerOnly) 
        {
            Console.WriteLine($"{firstPlayer.Name} is playing with {firstPlayer.Tool}.");
            Thread.Sleep(3000);
            while (!_endOfGame)
            {
                Play(firstPlayer);
                if (_game.PlayerIsWin(firstPlayer))
                {
                    _endOfGame = true;
                    PrintScoringAndWinner(firstPlayer, secondPlayer);
                }

                if (!_endOfGame)
                {
                    if(onePlayerOnly)
                    {
                        ComputerPlay();
                    }
                    else
                    {
                        Play(secondPlayer);
                    }

                    if (_game.PlayerIsWin(secondPlayer))
                    {
                        _endOfGame = true;
                        PrintScoringAndWinner(secondPlayer, firstPlayer);
                    }
                }
            }
        }

        private void ComputerPlay()
        {
            bool endOfTurn = false;
            string dice = string.Empty;
            if (_game.PlayerRollTheDice(_game.Computer))
            {
                dice = _game.PlayerDice();
            }
            else
            {
                endOfTurn = true;
                PrintAllBoard();
                Console.WriteLine(_game.PlayerDice());
                Console.WriteLine($"{_game.Computer.Name} don't have a valid move, turn is pass... ");
                Thread.Sleep(1500);
            }

            while (!endOfTurn)
            {
                PrintAllBoard();
                Console.WriteLine(dice);
                Console.Write($"{_game.Computer.Name} Turn... ");
                endOfTurn = _game.ComputerNextMove();
                Thread.Sleep(1500);
            }
        }

        private void Play(Player player)
        {
            string input, dice = string.Empty;
            string[] data;
            bool endOfTurn = false, validInput = false;
            int firstArg = 0, secondArg = 0;
            if (_game.PlayerRollTheDice(player))
            {
                dice = _game.PlayerDice();
            }
            else
            {
                endOfTurn = true;
                validInput = true;
                PrintAllBoard();
                Console.WriteLine(_game.PlayerDice());
                Console.WriteLine($"{player.Name} don't have a valid move, turn is pass... ");
                Thread.Sleep(1500);
            }

            while (!endOfTurn || !validInput)
            {
                PrintAllBoard();
                Console.WriteLine(dice);
                Console.Write($"{player.Name} Turn: ");
                input = Console.ReadLine();
                validInput = endOfTurn = false;
                data = input.Split(',');
                if (data.Length == 2)
                {
                    validInput = int.TryParse(data[0], out firstArg) 
                        && int.TryParse(data[1], out secondArg);
                    if (validInput)
                    {
                        endOfTurn = _game.PlayerNextMove(player, firstArg, secondArg, false);
                    }
                }
                else if (data.Length == 1)
                {
                    if (data[0].ToLower() == "q")
                    {
                        endOfTurn = true;
                        validInput = true;
                        _endOfGame = true;
                    }
                    else
                    {
                        validInput = int.TryParse(data[0], out firstArg);
                        if (validInput)
                        {
                            endOfTurn = _game.PlayerNextMove(player, firstArg, false);
                        }
                    }
                }
            } 
        }

        private void InitializeNumbering()
        {
            _numberingDisplayDown.AppendFormat(" ");
            _numberingDisplayUp.AppendFormat(" ");
            for (int i = 0; i < _game.Board.GetLength(1) / 2; i++)
            {
                if(i == 6)
                {
                    _numberingDisplayDown.AppendFormat("   N");
                    _numberingDisplayUp.AppendFormat("   N");
                }
                if (i < 10)
                {
                    _numberingDisplayDown.AppendFormat("   {0}", i + 1);
                }
                else
                {
                    _numberingDisplayDown.AppendFormat("  {0}", i + 1);
                }

                _numberingDisplayUp.AppendFormat("  {0}", (_game.Board.GetLength(1) - i));
            }
        }

        private void PrintAllBoard()
        {
            const int visibleSize = 6;
            Console.Clear();
            PrintUpBoard(visibleSize);
            PrintDownBoard(visibleSize);
            Console.WriteLine(Environment.NewLine);
        }

        private void PrintScoringAndWinner(Player winnerPlayer, Player secondPlayer)
        {
            PrintAllBoard();
            Console.WriteLine($"{winnerPlayer.Name} win!!!");
            Console.WriteLine(
@"Scoring
{0} : {1}
{2} : {3}", winnerPlayer.Name, winnerPlayer.AmountOfWins, 
secondPlayer.Name, secondPlayer.AmountOfWins);
        }

        private void PrintDownBoard(int visibleSize)
        {
            PrintRowsWithColor();
            _colsDisplay.Clear();
            for (int i = _game.Board.GetLength(0) - visibleSize; i < _game.Board.GetLength(0); i++)
            {
                _colsDisplay.Append("|");
                for (int j = 0; j < (_game.Board.GetLength(1) / 2); j++)
                {
                    SetContentOfBoardUI(i, j);
                }

                if (i - 2 >= 10)
                {
                    Console.WriteLine("{0}{1}", i - 2, _colsDisplay);
                }
                else
                {
                    Console.WriteLine("{0} {1}", i - 2, _colsDisplay);
                }
                _colsDisplay.Clear();
                PrintRowsWithColor();
            }

            _nextToolInEatenZone = -2;
            Console.WriteLine(_numberingDisplayDown);
        }

        private void PrintUpBoard(int visibleSize)
        {
            Console.WriteLine(_numberingDisplayUp);
            PrintRowsWithColor();
            _colsDisplay.Clear();
            for (int i = _game.Board.GetLength(0) - 1; i >= _game.Board.GetLength(0) - visibleSize; i--)
            {
                _colsDisplay.Append("|");
                for (int j = _game.Board.GetLength(1) - 1; j > (_game.Board.GetLength(1) / 2) - 1; j--)
                {
                    SetContentOfBoardUI(i, j);
                }

                Console.WriteLine("{0} {1}", (_game.Board.GetLength(0) - i), _colsDisplay);
                _colsDisplay.Clear();
                PrintRowsWithColor();
            }
        }

        private void SetContentOfBoardUI(int i, int j)
        {
            const int firstPartition= 6;
            const int secondPartition= 17;
            if (j == firstPartition || j == secondPartition)
            {
                _colsDisplay.AppendFormat("   |");
            }

            if (_game.Board[i, j] != Cell.Empty)
            {
                _colsDisplay.AppendFormat(" {0} |", _game.Board[i, j]);
            }
            else
            {
                _colsDisplay.Append("   |");
            }
        }

        private void PrintRowsWithColor()
        {
            const int Partition = 6;
            Console.Write("  ");
            for (int i = 0; i < _game.Board.GetLength(1) / 2; i++)
            {
                if((i % 2) == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (i == Partition)
                {
                    PrintEatenZoneTools();
                }

                Console.Write(" ===");
            }

            Console.Write(Environment.NewLine);
            Console.ResetColor();
        }

        private void PrintEatenZoneTools()
        {
            if (_nextToolInEatenZone < _game.EatenTools.Count && _nextToolInEatenZone >= 0)
            {
                ConsoleColor previosColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"  {_game.EatenTools[_nextToolInEatenZone]} ");
                Console.ForegroundColor = previosColor;
            }
            else
            {
                Console.Write("    ");
            }

            _nextToolInEatenZone++;
        }
    }
}
