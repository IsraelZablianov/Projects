using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLogic
{
    class ComputerAI : Player
    {
        private readonly GameLogic _gameLogic;
        private int _firstOption = 0;
        private int _secondOption = 0;

        public ComputerAI(GameLogic gameLogic)
            : base(Cell.X)
        {
            _gameLogic = gameLogic;
            Name = "Computer";
        }

        public bool ComputerNextMove()
        {
            bool makedMove = false;
            makedMove = InsertFromEatenZoneMove();
            if (!makedMove)
            {
                makedMove = PullOutMove();
            }
            if (!makedMove)
            {
                makedMove = BuildHouseOnEmptyPlaceMove();
            }
            if (!makedMove)
            {
                makedMove = BuildHouseWithOpenToolsMove();
            }
            if (!makedMove)
            {
                makedMove = EatOpponentOpenToolMove();
            }
            if (!makedMove)
            {
                DoAnyPossibleMove();
            }

            return _gameLogic.IsEndOfTurn(Tool);
        }

        private bool BuildHouseOnEmptyPlaceMove()
        {
            bool succeed = false;
            if(Tool == Cell.O)
            {
                _firstOption = _gameLogic.Dice.FirstDie > _gameLogic.Dice.SecondDie
                    ? _gameLogic.Dice.FirstDie : _gameLogic.Dice.SecondDie;
                _secondOption = _gameLogic.Dice.FirstDie + _gameLogic.Dice.SecondDie - _firstOption;
            }
            else
            {
                _firstOption = _gameLogic.Dice.FirstDie < _gameLogic.Dice.SecondDie
                    ? _gameLogic.Dice.FirstDie : _gameLogic.Dice.SecondDie;
                _secondOption = _gameLogic.Dice.FirstDie + _gameLogic.Dice.SecondDie - _firstOption;
                _firstOption *= -1;
                _secondOption *= -1;
            }

            for (int i = 0; i < _gameLogic.Board.GetLength(1) && !succeed; i++)
            {
                if (_gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, i] == Tool)
                {
                    for(int j = i + 1; j < _gameLogic.Board.GetLength(1) && !succeed; j++)
                    {
                        if (_gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, j] == Tool)
                        {
                            if(_gameLogic.AmountOfMovesToPlay == 2)
                            {
                                if((_gameLogic.Board[_gameLogic.Board.GetLength(0) - 3, j] == Tool
                                    && _gameLogic.Board[_gameLogic.Board.GetLength(0) - 3, i] == Tool)
                                    && _gameLogic.IsLegalMove(Tool, i, i + _firstOption)
                                    && _gameLogic.IsLegalMove(Tool, j, j + _secondOption)
                                    && (i + _firstOption == j + _secondOption))
                                {
                                    ExcecuteMove(i, i + _firstOption);
                                    succeed = true;
                                }
                            }
                        }
                    }

                    if (!succeed && _gameLogic.AmountOfMovesToPlay == 1
                        && _gameLogic.Board[_gameLogic.Board.GetLength(0) - 3, i] == Tool
                        && (_gameLogic.IsLegalMove(Tool, i, i + _firstOption) 
                        && _gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, i + _firstOption] == Tool
                        || (_gameLogic.IsLegalMove(Tool, i, i + _secondOption)
                        && _gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, i + _secondOption] == Tool)))
                    {
                        if(_gameLogic.IsLegalMove(Tool, i, i + _firstOption))
                        {
                            ExcecuteMove(i, i + _firstOption);
                        }
                        else
                        {
                            ExcecuteMove(i, i + _secondOption);
                        }

                        succeed = true;
                    }
                }
            }

            return succeed;
        } 

        private bool BuildHouseWithOpenToolsMove()
        {
            bool succeed = false;

            GenerateOptionsAccordingToDiceAndTool();
            for (int i = 0; i < _gameLogic.Board.GetLength(1) && !succeed; i++)
            {
                if(_gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, i] == Tool
                    && _gameLogic.Board[_gameLogic.Board.GetLength(0) - 2, i] != Tool)
                {
                    if(!IsBlockDie(_gameLogic.Dice.FirstDie)
                        && _gameLogic.IsLegalMove(Tool, i, i +_firstOption)
                        && _gameLogic.Board[0, i + _firstOption] == Tool)
                    {
                        ExcecuteMove(i, i + _firstOption);
                        succeed = true;
                    }
                    else if(!IsBlockDie(_gameLogic.Dice.SecondDie)
                        && _gameLogic.IsLegalMove(Tool, i, i + _secondOption)
                        && _gameLogic.Board[0, i + _secondOption] == Tool)
                    {
                        ExcecuteMove(i, i + _secondOption);
                        succeed = true;
                    }
                    else if(_gameLogic.AmountOfMovesToPlay > 2
                        && (_gameLogic.IsLegalMove(Tool, i, i + _secondOption)
                        || _gameLogic.IsLegalMove(Tool, i, i + _firstOption))
                        && _gameLogic.IsLegalMove(Tool, i, i + _secondOption + _firstOption)
                        && _gameLogic.Board[0, i + _secondOption + _firstOption] == Tool)
                    {
                        if(_gameLogic.IsLegalMove(Tool, i, i + _firstOption))
                        {
                            ExcecuteMove(i, i + _firstOption);
                        }
                        else
                        {
                            ExcecuteMove(i, i + _secondOption);
                        }

                        succeed = true;
                    }
                    else if(_gameLogic.IsDoubleTurn()
                        && _gameLogic.AmountOfMovesToPlay > 2
                        && _gameLogic.IsLegalMove(Tool, i, i + _firstOption)
                        && _gameLogic.IsLegalMove(Tool, i, i + _firstOption + _firstOption + _firstOption)
                        &&  _gameLogic.Board[0, i + _firstOption + _firstOption + _firstOption] == Tool)
                    {
                        ExcecuteMove(i, i + _firstOption);
                        succeed = true;
                    }
                }
            }

            return succeed;
        }

        private bool EatOpponentOpenToolMove()
        {
            bool succeed = false;

            GenerateOptionsAccordingToDiceAndTool();
            for (int i = 0; i < _gameLogic.Board.GetLength(1) && !succeed; i++)
            {
                if (!IsBlockDie(_gameLogic.Dice.FirstDie)
                    && IsPossibleToEat(i, i + _firstOption))
                {
                    ExcecuteMove(i, i + _firstOption);
                    succeed = true;
                }
                else if (!IsBlockDie(_gameLogic.Dice.SecondDie)
                    && IsPossibleToEat(i, i + _secondOption))
                {
                    ExcecuteMove(i, i + _secondOption);
                    succeed = true;
                }
                else if (_gameLogic.AmountOfMovesToPlay > 1
                    && (_gameLogic.IsLegalMove(Tool, i, i + _firstOption)
                    || _gameLogic.IsLegalMove(Tool, i, i + _secondOption))
                    && IsPossibleToEat(i, i + _secondOption + _firstOption))
                {
                    if (_gameLogic.IsLegalMove(Tool, i, i + _firstOption))
                    {
                        ExcecuteMove(i, i + _firstOption);

                    }
                    else
                    {
                        ExcecuteMove(i, i + _secondOption);
                    }

                    succeed = true;
                }
                else if (_gameLogic.IsDoubleTurn()
                    && _gameLogic.IsLegalMove(Tool, i, i + _firstOption)
                    && (_gameLogic.AmountOfMovesToPlay > 2)
                    && IsPossibleToEat(i, i + _firstOption + _firstOption + _firstOption))
                {
                    ExcecuteMove(i, i + _firstOption);
                    succeed = true;
                }
            }

            return succeed;
        }

        private void GenerateOptionsAccordingToDiceAndTool()
        {
            if (Tool == Cell.O)
            {
                _firstOption = _gameLogic.Dice.FirstDie;
                _secondOption = _gameLogic.Dice.SecondDie;
            }
            else
            {
                _firstOption = -_gameLogic.Dice.FirstDie;
                _secondOption = -_gameLogic.Dice.SecondDie;
            }
        }

        private bool IsPossibleToEat(int startingPoint, int targetPoint)
        {
            return _gameLogic.IsLegalMove(Tool, startingPoint, targetPoint)
                    && _gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, targetPoint] != Tool
                    && _gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, targetPoint] != Cell.Empty
                    && _gameLogic.Board[_gameLogic.Board.GetLength(0) - 2, targetPoint] == Cell.Empty;
        }

        private bool PullOutMove()
        {
            bool firstOptionLegal = false;
            bool secondOptionLegal = false;
            bool makedMove = false;
            if (Tool == Cell.O)
            {
                _firstOption = _gameLogic.Board.GetLength(1) - _gameLogic.Dice.FirstDie;
                _secondOption = _gameLogic.Board.GetLength(1) - _gameLogic.Dice.SecondDie;
            }
            else
            {
                _firstOption = _gameLogic.Dice.FirstDie - 1;
                _secondOption = _gameLogic.Dice.SecondDie - 1;
            }

            firstOptionLegal = _gameLogic.IsLegalToPullOut(Tool, _firstOption);
            secondOptionLegal = _gameLogic.IsLegalToPullOut(Tool, _secondOption);
            if (firstOptionLegal)
            {
                PullOutFromTop(_gameLogic.Dice.FirstDie, _firstOption);
                makedMove = true;
            }
            else if (secondOptionLegal)
            {
                PullOutFromTop(_gameLogic.Dice.SecondDie, _secondOption);
                makedMove = true;
            }
            else if(_gameLogic.IsPossibleToPullOut(Tool)
                && !IsBlockDie(_gameLogic.Dice.FirstDie)
                && PullOutSmallerIfLegalMove(_firstOption))
            {
                makedMove = true;
            }
            else if(_gameLogic.IsPossibleToPullOut(Tool)
                && !IsBlockDie(_gameLogic.Dice.SecondDie)
                && PullOutSmallerIfLegalMove(_secondOption))
            {
                makedMove = true;
            }

            return makedMove;
        }

        private bool PullOutSmallerIfLegalMove(int dieOption)
        {
            bool notExistBiger = true;
            bool makedMove = false;
            int sizeOfHomeZone = 6;
            int startIndex = 0, endIndex = 0;
            int die = 0;
            if (Cell.X == Tool)
            {
                startIndex = dieOption;
                endIndex = sizeOfHomeZone;
                die = dieOption + 1;
            }
            else
            {
                startIndex = _gameLogic.Board.GetLength(1) - sizeOfHomeZone;
                endIndex = dieOption;
                die = _gameLogic.Board.GetLength(1) - dieOption;
            }

            for (int j = startIndex; j <= endIndex; j++)
            {
                if (_gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, j] == Tool)
                {
                    notExistBiger = false;
                }
            }

            if(notExistBiger)
            {
                if(Cell.X == Tool)
                {
                    for (int j = endIndex; j >= 0 && !makedMove; j--)
                    {
                        if (_gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, j] == Tool)
                        {
                            PullOutFromTop(die, j);
                            makedMove = true;
                        }
                    }
                }
                else
                {
                    for (int j = startIndex; j < _gameLogic.Board.GetLength(1) && !makedMove; j++)
                    {
                        if (_gameLogic.Board[_gameLogic.Board.GetLength(0) - 1, j] == Tool)
                        {
                            PullOutFromTop(die, j);
                            makedMove = true;
                        }
                    }
                }
            }

            return makedMove;
        }

        private void PullOutFromTop(int die, int targetPoint)
        {
            bool pulled = false;
            for (int i = 0; i < _gameLogic.Board.GetLength(0) && !pulled; i++)
            {
                if (_gameLogic.Board[i, targetPoint] == Tool)
                {
                    _gameLogic.UpdateAmountOfMovesAndDice(die);
                    _gameLogic.Board[i, targetPoint] = Cell.Empty;
                    pulled = true;
                }
            }
        }

        private bool InsertFromEatenZoneMove()
        {
            bool succeed = false;
            if(Tool == Cell.O)
            {
                _firstOption = _gameLogic.Dice.FirstDie - 1;
                _secondOption = _gameLogic.Dice.SecondDie - 1;
            }
            else
            {
                _firstOption = _gameLogic.Board.GetLength(1) - _gameLogic.Dice.FirstDie;
                _secondOption = _gameLogic.Board.GetLength(1) - _gameLogic.Dice.SecondDie;
            }

            if(_gameLogic.IsPlayerHasEatenTool(Tool))
            {
                if(!IsBlockDie(_gameLogic.Dice.FirstDie) 
                    && _gameLogic.IsTargetPointLegal(Tool, _firstOption))
                {
                    _gameLogic.UpdateAmountOfMovesAndDice(_gameLogic.Dice.FirstDie);
                    _gameLogic.SetToolOnTopOrEat(Tool, _firstOption);
                    _gameLogic.EatenTools.Remove(Tool);
                    succeed = true;
                }

                else if (!IsBlockDie(_gameLogic.Dice.SecondDie)
                    && _gameLogic.IsTargetPointLegal(Tool, _secondOption))
                {
                    _gameLogic.UpdateAmountOfMovesAndDice(_gameLogic.Dice.SecondDie);
                    _gameLogic.SetToolOnTopOrEat(Tool, _secondOption);
                    _gameLogic.EatenTools.Remove(Tool);
                    succeed = true;
                }
            }

            return succeed;
        }

        private void DoAnyPossibleMove()
        {
            bool succeed = false;

            GenerateOptionsAccordingToDiceAndTool();
            for(int i = 0; i < _gameLogic.Board.GetLength(1) && !succeed; i++)
            {
                if(!IsBlockDie(_gameLogic.Dice.FirstDie)
                    && _gameLogic.IsLegalMove(Tool, i, i + _firstOption))
                {
                    succeed = true;
                    ExcecuteMove(i, i + _firstOption);
                }
                else if(!IsBlockDie(_gameLogic.Dice.SecondDie)
                    && _gameLogic.IsLegalMove(Tool, i, i + _secondOption))
                {
                    succeed = true;
                    ExcecuteMove(i, i + _secondOption);
                }
            }
        }

        private void ExcecuteMove(int startingPoint, int targetPoint)
        {
            bool findTheTopTool = false;
            for (int i = 0; i < _gameLogic.Board.GetLength(0) && !findTheTopTool; i++)
            {
                if (_gameLogic.Board[i, startingPoint] == Tool)
                {
                    findTheTopTool = true;
                    _gameLogic.Board[i, startingPoint] = Cell.Empty;
                    _gameLogic.UpdateAmountOfMovesAndDice(Math.Abs(targetPoint - startingPoint));
                }
            }

            _gameLogic.SetToolOnTopOrEat(Tool, targetPoint);
        }

        private bool IsBlockDie(int die)
        {
            return die == _gameLogic.BlockedDieValue;
        }
    }
}
