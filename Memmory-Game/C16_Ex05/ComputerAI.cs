namespace C16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    public class ComputerAI
    {
        private readonly BoardManager r_OriginalBoard;
        private readonly BoardManager r_MemoryBoard = new BoardManager();
        private readonly Random r_RandomProducer = new Random();
        private readonly PlayerState r_ComputerState = new PlayerState();

        public ComputerAI(BoardManager i_BoardManager)
        {
            r_OriginalBoard = i_BoardManager;
            r_MemoryBoard.Board = new Cell[r_OriginalBoard.Board.GetLength(0), r_OriginalBoard.Board.GetLength(1)];
            InitialMemory();
            r_ComputerState.Name = "Watson";
        }

        public event PerformAction OnMoveDone;

        public event PerformAction OnScoreChange;

        public PlayerState ComputerState
        {
            get { return r_ComputerState; }
        } 

        private Cell[,] MemoryBoard
        {
            get
            {
                return r_MemoryBoard.Board;
            }
        }

        private Cell[,] OriginalBoard
        {
            get
            {
                return r_OriginalBoard.Board;
            }
        }

        public void InitialMemory()
        {
            for (int i = 0; i < MemoryBoard.GetLength(0); i++)
            {
                for (int j = 0; j < MemoryBoard.GetLength(1); j++)
                {
                    MemoryBoard[i, j] = new Cell();
                }
            }
        }

        public void DoMove()
        {
            bool foundPair = false;
            copyUncoveredCells();

            do
            {
                foundPair = tryUncoverPairFromMemory();
                
                if (!foundPair)
                {
                    foundPair = tryFindingPairUsingRandomSelection();
                }

                if (foundPair)
                {
                    r_ComputerState.Score++;
                    onScoreChanges();
                }
            } 
            while (foundPair && !r_OriginalBoard.IsAllCellsUncovered());
        }

        private bool tryFindingPairUsingRandomSelection()
        {
            bool foundPair = false;
            Cell firstCell = getRandomCell();
            firstCell.Covered = false;
            firstCell.WaitingForPartner = true;
            onMoveDone();
            bool foundPartnerFromMemory = tryUncoverPartnerFromMemory(firstCell);

            if (foundPartnerFromMemory)
            {
                firstCell.UpdateFoundPartner();
                foundPair = true;
            }
            else
            {
                Cell secondCell = getRandomCell();
                secondCell.WaitingForPartner = true;
                secondCell.Covered = false;
                onMoveDone();

                if (secondCell.Content == firstCell.Content)
                {
                    secondCell.UpdateFoundPartner();
                    firstCell.UpdateFoundPartner();
                    foundPair = true;
                }
                else
                {
                    copyUncoveredCells();
                    firstCell.Covered = true;
                    firstCell.WaitingForPartner = false;
                    secondCell.WaitingForPartner = false;
                    secondCell.Covered = true;
                }
            }

            return foundPair;
        }

        private Cell getRandomCell()
        {
            List<Cell> cellsWithOutPartner = r_OriginalBoard.GetCellsWithOutPartner();
            return cellsWithOutPartner[r_RandomProducer.Next(0, cellsWithOutPartner.Count)];
        }

        private bool tryUncoverPairFromMemory()
        {
            bool uncoveredPartnerFromMemory = false;

            for (int i = 0; i < MemoryBoard.GetLength(0) && !uncoveredPartnerFromMemory; i++)
            {
                for (int j = 0; j < MemoryBoard.GetLength(1) && !uncoveredPartnerFromMemory; j++)
                {
                    if (OriginalBoard[i, j].Covered)
                    {
                        uncoveredPartnerFromMemory = tryUncoverPartnerFromMemory(MemoryBoard[i, j]);
                        
                        if (uncoveredPartnerFromMemory)
                        {
                            OriginalBoard[i, j].WaitingForPartner = true;
                            OriginalBoard[i, j].Covered = false;
                            onMoveDone();
                            OriginalBoard[i, j].UpdateFoundPartner();
                        }
                    }
                }
            }

            return uncoveredPartnerFromMemory;
        }

        private bool tryUncoverPartnerFromMemory(Cell i_Cell)
        {
            bool foundCellInMemory = false;

            for (int i = 0; i < MemoryBoard.GetLength(0) && !foundCellInMemory; i++)
            {
                for (int j = 0; j < MemoryBoard.GetLength(1) && !foundCellInMemory; j++)
                {
                    if (MemoryBoard[i, j].Content != " " && MemoryBoard[i, j].Content == i_Cell.Content
                        && MemoryBoard[i, j] != i_Cell && OriginalBoard[i, j] != i_Cell && !OriginalBoard[i, j].FoundPartner)
                    {
                        foundCellInMemory = true;
                        OriginalBoard[i, j].WaitingForPartner = true;
                        OriginalBoard[i, j].Covered = false;
                        onMoveDone();
                        OriginalBoard[i, j].UpdateFoundPartner();
                    }
                }
            }

            return foundCellInMemory;
        }

        private void copyUncoveredCells()
        {
            for (int i = 0; i < MemoryBoard.GetLength(0); i++)
            {
                for (int j = 0; j < MemoryBoard.GetLength(1); j++)
                {
                    if (!OriginalBoard[i, j].Covered)
                    {
                        MemoryBoard[i, j].Content = OriginalBoard[i, j].Content;
                    }
                }
            }
        }

        private void onScoreChanges()
        {
            if (OnScoreChange != null)
            {
                OnScoreChange.Invoke();
            }
        }

        private void onMoveDone()
        {
            if (OnMoveDone != null)
            {
                OnMoveDone.Invoke();
            }
        }
    }
}
