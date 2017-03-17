using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLogic
{
    public enum Cell
    {
        Empty = 0,
        X,
        O
    }

    class GameLogic
    {
        private readonly Cell[,] _board = new Cell[15, 24];
        private readonly Dice _dice = new Dice();
        private readonly List<Cell> _eatenTools = new List<Cell>();

        public GameLogic()
        {
            BlockedDieValue = -100;
        }

        public Cell[,] Board
        {
            get
            {
                return _board;
            }
        }

        public List<Cell> EatenTools
        {
            get
            {
                return _eatenTools;
            }
        }

        public Dice Dice
        {
            get
            {
                return _dice;
            }
        }

        public int AmountOfMovesToPlay
        {
            get;
            set;
        }

        public int BlockedDieValue
        {
            get;
        }

        public void UpdateAmountOfMovesAndDice(int usedDie)
        {
            AmountOfMovesToPlay--;
            if (AmountOfMovesToPlay == 1)
            {
                if (_dice.FirstDie == usedDie)
                {
                    _dice.FirstDie = BlockedDieValue;
                }
                else if (_dice.SecondDie == usedDie)
                {
                    _dice.SecondDie = BlockedDieValue;
                }
                else if (usedDie < _dice.FirstDie)
                {
                    _dice.FirstDie = BlockedDieValue;
                }
                else
                {
                    _dice.SecondDie = BlockedDieValue;
                }
            }
        }

        public bool RollTheDice(Cell tool)
        {
            _dice.RollTheDice();
            bool isPossibleToMakeMove = true;
            if (!IsPossibleToMakeNextMove(tool))
            {
                isPossibleToMakeMove = false;
            }
            else
            {
                if (IsDoubleTurn())
                {
                    AmountOfMovesToPlay = 4;
                }
                else
                {
                    AmountOfMovesToPlay = 2;
                }
            }

            return isPossibleToMakeMove;
        }

        public void SetToolOnTopOrEat(Cell tool, int targetPoint)
        {
            bool isFindTheTopTool = false;
            if (Board[Board.GetLength(0) - 1, targetPoint] == Cell.Empty)
            {
                isFindTheTopTool = true;
                Board[Board.GetLength(0) - 1, targetPoint] = tool;
            }
            else
            {
                for (int i = 0; i < Board.GetLength(0) && !isFindTheTopTool; i++)
                {
                    if (Board[i, targetPoint] == tool)
                    {
                        isFindTheTopTool = true;
                        Board[i - 1, targetPoint] = tool;
                    }
                    else if (Board[i, targetPoint] != tool && Board[i, targetPoint] != Cell.Empty)
                    {
                        isFindTheTopTool = true;
                        _eatenTools.Add(Board[i, targetPoint]);
                        Board[i, targetPoint] = tool;
                    }
                }
            }
        }

        public bool IsDoubleTurn()
        {
            return _dice.FirstDie == _dice.SecondDie;
        }

        public bool IsEndOfTurn(Cell tool)
        {
            bool isEndOfTurn = false;
            if (AmountOfMovesToPlay == 0 || !IsPossibleToMakeNextMove(tool))
            {
                isEndOfTurn = true;
            }

            return isEndOfTurn;
        }

        public bool IsLegalMove(Cell tool, int startingPoint, int targetPoint)
        {
            return !IsPlayerHasEatenTool(tool)
                && IsInRangeOfBoard(startingPoint, targetPoint)
                && IsCorrectDirection(tool, startingPoint, targetPoint)
                && IsAccordingToDice(tool, startingPoint, targetPoint)
                && IsTargetPointLegal(tool, targetPoint)
                && _board[_board.GetLength(0) - 1, startingPoint] == tool;
        }

        public bool IsLegalToPullOut(Cell tool, int pullOutToolIndex)
        {
            return !IsPlayerHasEatenTool(tool)
                && IsInRangeOfBoard(0, pullOutToolIndex)
                && IsAllToolsOfPlayerInHomeZone(tool)
                && (_board[_board.GetLength(0) - 1, pullOutToolIndex] == tool)
                && IsAccordingToDicePullOut(tool, pullOutToolIndex)
                && !IsAllToolsAreOut(tool);
        }

        public bool IsInRangeOfStartZone(Cell tool, int targetPoint)
        {
            bool isInRangeOfStartZone = false;
            if(tool == Cell.O)
            {
                isInRangeOfStartZone = (targetPoint >= 0 && targetPoint < 6);
            }
            else if(tool == Cell.X)
            {
                isInRangeOfStartZone = (targetPoint >= 18 && targetPoint < 24);
            }

            return isInRangeOfStartZone;
        }

        public bool IsAllToolsOfPlayerInHomeZone(Cell tool)
        {
            int homeZoneSize = 6, numberOfTools = 0;
            int endIndex = 0, startIndex = 0;
            if(tool == Cell.O)
            {
                startIndex = 0;
                endIndex = _board.GetLength(1) - homeZoneSize;
            }
            else
            {
                startIndex = homeZoneSize;
                endIndex = _board.GetLength(1);
            }

            for (int j = startIndex; j < endIndex; j++)
            {
                if (_board[_board.GetLength(0) - 1, j] == tool)
                {
                    numberOfTools++;
                }
            }

            return numberOfTools == 0 && !IsPlayerHasEatenTool(tool);
        }

        public bool IsAllToolsAreOut(Cell tool)
        {
            bool isAllToolsOut = true;
            if (IsPlayerHasEatenTool(tool))
            {
                isAllToolsOut = false;
            }

            for (int j = 0; j < _board.GetLength(1) && isAllToolsOut; j++)
            {
                if (_board[_board.GetLength(0) - 1, j] == tool)
                {
                    isAllToolsOut = false;
                }
            }

            return isAllToolsOut;
        }

        public bool IsTargetPointLegal(Cell tool, int targetPoint)
        {
            int oneBeforeEndOfRow = _board.GetLength(0) - 2;
            return IsInRangeOfBoard(0, targetPoint) &&
                (_board[oneBeforeEndOfRow, targetPoint] == tool 
                || _board[oneBeforeEndOfRow, targetPoint] == Cell.Empty);
        }

        public bool IsAccordingToDice(Cell tool, int startingPoint, int targetPoint)
        {
            return (Math.Abs(targetPoint - startingPoint) == _dice.FirstDie 
                || Math.Abs(targetPoint - startingPoint) == _dice.SecondDie);
        }

        public bool IsPlayerHasEatenTool(Cell tool)
        {
            bool isPlayerHasEatenTool = false;
            foreach (var eaten in _eatenTools)
            {
                if (tool == eaten)
                {
                    isPlayerHasEatenTool = true;
                }
            }

            return isPlayerHasEatenTool;
        }

        public bool IsPossibleToMakeNextMove(Cell tool)
        {
            bool isPossibleToMakeMove = false;
            bool isPossibleToGetOutFromEatenZone = IsPossibleToGetOutFromEatenZone(tool);
            int firstAdjustedOption = 0, secondAdjustedOption = 0;
            if (Cell.X == tool)
            {
                firstAdjustedOption = -_dice.FirstDie;
                secondAdjustedOption = -_dice.SecondDie;

            }
            else
            {
                firstAdjustedOption = _dice.FirstDie;
                secondAdjustedOption = _dice.SecondDie;
            }

            if (isPossibleToGetOutFromEatenZone)
            {
                for (int i = 0; i < _board.GetLength(1) && !isPossibleToMakeMove; i++)
                {
                    if (_board[_board.GetLength(0) - 1, i] == tool)
                    {
                        isPossibleToMakeMove = (IsInRangeOfBoard(i, i + firstAdjustedOption)
                            && IsTargetPointLegal(tool, i + firstAdjustedOption))
                            || (IsInRangeOfBoard(i, i + secondAdjustedOption)
                            && IsTargetPointLegal(tool, i + secondAdjustedOption));
                    }
                }
            }

            return isPossibleToMakeMove 
                || IsPossibleToPullOut(tool)
                || (isPossibleToGetOutFromEatenZone && IsPlayerHasEatenTool(tool));
        }

        public bool IsPossibleToGetOutFromEatenZone(Cell tool)
        {
            bool isPossibleGetOutFromEatenZone = true;
            int firstAdjustedOption = 0, secondAdjustedOption = 0;
            if (Cell.X == tool)
            {
                firstAdjustedOption = _board.GetLength(1) - _dice.FirstDie;
                secondAdjustedOption = _board.GetLength(1) - _dice.SecondDie;
            }
            else
            {
                firstAdjustedOption = _dice.FirstDie - 1;
                secondAdjustedOption = _dice.SecondDie - 1;
            }

            if (IsPlayerHasEatenTool(tool))
            {
                isPossibleGetOutFromEatenZone = false;
                if (IsTargetPointLegal(tool, firstAdjustedOption) 
                    || IsTargetPointLegal(tool, secondAdjustedOption))
                {
                    isPossibleGetOutFromEatenZone = true;
                }
            }

            return isPossibleGetOutFromEatenZone;
        }

        public bool IsPossibleToPullOut(Cell tool)
        {
            bool isPossibleToPullOut = !IsPlayerHasEatenTool(tool)
                && IsAllToolsOfPlayerInHomeZone(tool)
                && !IsAllToolsAreOut(tool);

            int homeZoneSize = 6;
            bool notExistBigger = true;
            bool isExistToolToPullOut = false;
            int maxOfDie = _dice.FirstDie > _dice.SecondDie ? _dice.FirstDie : _dice.SecondDie;
            int firstAdjustedOption, secondAdjustedOption, startIndex, endIndex; 
            if (Cell.X == tool)
            {
                firstAdjustedOption = _dice.FirstDie - 1;
                secondAdjustedOption = _dice.SecondDie - 1;
                startIndex = maxOfDie;
                endIndex = homeZoneSize;
            }
            else
            {
                firstAdjustedOption = _board.GetLength(1) - _dice.FirstDie;
                secondAdjustedOption = _board.GetLength(1) - _dice.SecondDie;
                startIndex = _board.GetLength(1) - homeZoneSize;
                endIndex = _board.GetLength(1) - maxOfDie;
            }

            if(IsInRangeOfBoard(_dice.FirstDie, firstAdjustedOption)
                && _board[_board.GetLength(0) - 1, firstAdjustedOption] == tool
                || (IsInRangeOfBoard(_dice.SecondDie, secondAdjustedOption)
                && _board[_board.GetLength(0) - 1, secondAdjustedOption] == tool))
            {
                isExistToolToPullOut = true;
            }

            for (int j = startIndex; j < endIndex; j++)
            {
                if (_board[_board.GetLength(0) - 1, j] == tool)
                {
                    notExistBigger = false;
                }
            }

            return isPossibleToPullOut && (notExistBigger || isExistToolToPullOut);
        }

        public bool IsInRangeOfBoard(int startingPoint, int targetPoint)
        {
            return startingPoint >= 0
                && startingPoint < _board.GetLength(1)
                && targetPoint >= 0 && targetPoint < _board.GetLength(1);
        }

        private bool IsCorrectDirection(Cell tool, int startingPoint, int targetPoint)
        {
            bool isCorrectDirection = true;

            if (tool == Cell.O)
            {
                if (targetPoint <= startingPoint)
                {
                    isCorrectDirection = false;
                }
            }
            else
            {
                if (targetPoint >= startingPoint)
                {
                    isCorrectDirection = false;
                }
            }

            return isCorrectDirection;
        }

        private bool IsAccordingToDicePullOut(Cell tool, int pullOutToolIndex)
        {
            bool isAccordingToDicePullOut = false;
            int sizeOfHomeZone = 6, startIndex, endIndex, usedDie;
            bool isPullOutToolIndexLowerMaxDie = false;
            if (Cell.X == tool)
            {
                startIndex = pullOutToolIndex + 1;
                endIndex = sizeOfHomeZone;
                isPullOutToolIndexLowerMaxDie = startIndex < _dice.FirstDie || startIndex < _dice.SecondDie;
                usedDie = pullOutToolIndex + 1;
            }
            else
            {
                startIndex = _board.GetLength(1) - sizeOfHomeZone;
                endIndex = pullOutToolIndex;
                isPullOutToolIndexLowerMaxDie = (_board.GetLength(1) - pullOutToolIndex) < _dice.FirstDie 
                    || (_board.GetLength(1) - pullOutToolIndex) < _dice.SecondDie;
                usedDie = _board.GetLength(1) - pullOutToolIndex;
            }

            if (isPullOutToolIndexLowerMaxDie)
            {
                isAccordingToDicePullOut = true;
                for (int j = startIndex; j < endIndex; j++)
                {
                    if (_board[_board.GetLength(0) - 1, j] == tool)
                    {
                        isAccordingToDicePullOut = false;
                    }
                }
            }

            return isAccordingToDicePullOut || IsAccordingToDice(tool, 0, usedDie);
        }
    }
}
