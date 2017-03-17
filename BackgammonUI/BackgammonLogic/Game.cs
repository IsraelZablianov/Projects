using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLogic
{
    public class Game
    {
        private readonly GameLogic _gameLogic = new GameLogic();
        private readonly ComputerAI _computerAI;

        public Game()
        {
            _computerAI = new ComputerAI(_gameLogic);
            InitializeGame();
        }

        public Cell[,] Board
        {
            get
            {
                return _gameLogic.Board;
            }
        }

        public IList<Cell> EatenTools
        {
            get
            {
                return _gameLogic.EatenTools.AsReadOnly();
            }
        }

        public Player Computer
        {
            get
            {
                return _computerAI;
            }
        }

        public void InitializeGame()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    Board[i, j] = Cell.Empty;
                }
            }

            Board[14, 0] = Cell.O;
            Board[13, 0] = Cell.O;
            Board[13, 23] = Cell.X;
            Board[14, 23] = Cell.X;

            for (int i = 0; i < 3; i++)
            {
                Board[12 + i, 7] = Cell.X;
                Board[12 + i, 16] = Cell.O;
            }

            for (int i = 0; i < 5; i++)
            {
                Board[10 + i, 11] = Cell.O;
                Board[10 + i, 18] = Cell.O;
                Board[10 + i, 12] = Cell.X;
                Board[10 + i, 5] = Cell.X;
            }

            _gameLogic.EatenTools.Clear();
        }

        public bool PlayerNextMove(Player player, int startingPoint, int targetPoint, bool fixedToBoardLength)
        {
            bool moveIsDone = false;
            if (!fixedToBoardLength)
            {
                startingPoint--;
                targetPoint--;
            }

            moveIsDone = MoveFromPointToPoint(player.Tool, startingPoint, targetPoint);
            if (moveIsDone)
            {
                _gameLogic.UpdateAmountOfMovesAndDice(Math.Abs(targetPoint - startingPoint));
            }

            return _gameLogic.IsEndOfTurn(player.Tool);
        }

        public bool PlayerNextMove(Player player, int targetPoint, bool fixedToBoardLength)
        {
            bool moveIsDone = false;
            bool isPullOut = false;
            int usedDie = 0;
            if (!fixedToBoardLength)
            {
                targetPoint--;
            }

            moveIsDone = MoveInsertFromEatenZone(player.Tool, targetPoint);
            if (!moveIsDone)
            {
                moveIsDone = MovePullOutTool(player.Tool, targetPoint);
                isPullOut = true;
            }

            if (moveIsDone)
            {
                if ((Cell.O == player.Tool && !isPullOut)
                    || Cell.X == player.Tool && isPullOut)
                {
                    usedDie = targetPoint + 1;
                }
                else
                {
                    usedDie = Board.GetLength(1) - targetPoint;
                }

                _gameLogic.UpdateAmountOfMovesAndDice(usedDie);
            }

            return _gameLogic.IsEndOfTurn(player.Tool);
        }

        public bool ComputerNextMove()
        {
            bool isEndOfTurn = _computerAI.ComputerNextMove();
            return isEndOfTurn;
        }

        public bool PlayerRollTheDice(Player player)
        {
            return _gameLogic.RollTheDice(player.Tool);
        }

        public bool PlayerIsWin(Player player)
        {
            bool playerIsWin = _gameLogic.IsAllToolsAreOut(player.Tool);
            if (playerIsWin)
            {
                player.AmountOfWins++;
            }

            return playerIsWin;
        }

        public string PlayerDice()
        {
            return _gameLogic.Dice.ToString();
        }

        private bool MovePullOutTool(Cell tool, int pullOutToolIndex)
        {
            bool pullOutToolLegal;
            bool isPullOutToolExist = false;
            pullOutToolLegal = _gameLogic.IsLegalToPullOut(tool, pullOutToolIndex);
            if (pullOutToolLegal)
            {
                for (int i = 0; i < Board.GetLength(0) && !isPullOutToolExist; i++)
                {
                    if (Board[i, pullOutToolIndex] == tool)
                    {
                        Board[i, pullOutToolIndex] = Cell.Empty;
                        isPullOutToolExist = true;
                    }
                }
            }

            return pullOutToolLegal && isPullOutToolExist;
        }

        private bool MoveInsertFromEatenZone(Cell tool, int targetPoint)
        {
            bool makeMove;
            int targetPointInSizeOfHomeZone;
            if (Cell.X == tool)
            {
                targetPointInSizeOfHomeZone = Board.GetLength(1) - targetPoint;
            }
            else
            {
                targetPointInSizeOfHomeZone = targetPoint + 1;
            }

            makeMove = _gameLogic.IsPlayerHasEatenTool(tool)
                && _gameLogic.IsInRangeOfStartZone(tool, targetPoint)
                && _gameLogic.IsAccordingToDice(tool, 0, targetPointInSizeOfHomeZone)
                && _gameLogic.IsTargetPointLegal(tool, targetPoint);

            if (makeMove)
            {
                _gameLogic.SetToolOnTopOrEat(tool, targetPoint);
                _gameLogic.EatenTools.Remove(tool);
            }

            return makeMove;
        }

        private bool MoveFromPointToPoint(Cell tool, int startingPoint, int targetPoint)
        {
            bool makeMove;
            makeMove = _gameLogic.IsLegalMove(tool, startingPoint, targetPoint);
            if (makeMove)
            {
                bool findTheTopTool = false;
                for (int i = 0; i < Board.GetLength(0) && !findTheTopTool; i++)
                {
                    if (Board[i, startingPoint] == tool)
                    {
                        findTheTopTool = true;
                        Board[i, startingPoint] = Cell.Empty;
                    }
                }

                _gameLogic.SetToolOnTopOrEat(tool, targetPoint);
            }

            return makeMove;
        }
    }
}
