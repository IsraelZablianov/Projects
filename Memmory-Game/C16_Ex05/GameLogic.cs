namespace C16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    public delegate void PerformAction();
    
    public class GameLogic
    {
        private readonly BoardManager r_BoardManager = new BoardManager(); 
        private readonly PlayerState r_FirstPlayer = new PlayerState();
        private PlayerState m_SecondPlayer = new PlayerState();
        private bool m_PlayWithComputer;
        private ComputerAI m_ComputerAI;
        private PlayerState m_CurrentPlayer;

        public GameLogic()
        {
            r_BoardManager.MaxSize = 6;
            r_BoardManager.MinSize = 4;
        }

        public event PerformAction OnCellSelected;

        public event PerformAction OnScoreChange;

        public event PerformAction OnGameOver;

        public event PerformAction OnPlayerChange;

        public Cell[,] Board
        {
            get { return r_BoardManager.Board; }
        }

        public bool PlayWithComputer
        {
            get { return m_PlayWithComputer; }
            set { m_PlayWithComputer = value; }
        }

        public string CurrentPlayerName
        {
            get
            {
                return m_CurrentPlayer.Name;
            }
        }

        public int FirstPlayerScore
        {
            get { return r_FirstPlayer.Score; }
        }

        public int SecondPlayerScore
        {
            get { return m_SecondPlayer.Score; }
        }

        public string FirstPlayerName
        {
            get { return r_FirstPlayer.Name; }
            set { r_FirstPlayer.Name = value; }
        }

        public string SecondPlayerName
        {
            get { return m_SecondPlayer.Name; }
            set { m_SecondPlayer.Name = value; }
        }

        public int MaxBoardSize
        {
            get
            {
                return r_BoardManager.MaxSize;
            }
        }

        public int MinBoardSize
        {
            get
            {
                return r_BoardManager.MinSize;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    Board[i, j] = new Cell();
                }
            }

            r_BoardManager.SetRandomBoardContent();
            r_FirstPlayer.Score = 0;
            m_SecondPlayer.Score = 0;
            onScoreChanges();
            m_CurrentPlayer = r_FirstPlayer;
            m_ComputerAI.InitialMemory();
        }

        public void InitialGame(int i_Rows, int i_Cols)
        {
            bool isValidBoardSize = r_BoardManager.IsValidBoardSize(i_Rows, i_Cols);

            if (isValidBoardSize)
            {
                r_BoardManager.Board = new Cell[i_Rows, i_Cols];
                m_ComputerAI = new ComputerAI(r_BoardManager);
                m_ComputerAI.OnMoveDone += onMoveDone;
                m_ComputerAI.OnScoreChange += onScoreChanges;
                Reset();

                if (PlayWithComputer)
                {
                    m_SecondPlayer = m_ComputerAI.ComputerState;
                }
            }
        }

        public void DoMove(int i_Row, int i_Col)
        {
            r_BoardManager.CoverCellsNotWaitingForPartner();
            bool legalMove = IsCoveredCell(i_Row, i_Col) && IsInBorderOfBoard(i_Row, i_Col);

            if (legalMove)
            {
                Board[i_Row, i_Col].Covered = false;
                onMoveDone();
                bool foundWaitingCell = false;
                foundWaitingCell = searchForWaitingCell(i_Row, i_Col);

                if (!foundWaitingCell)
                {
                    Board[i_Row, i_Col].WaitingForPartner = true;
                }
            }

            r_BoardManager.CoverCellsNotWaitingForPartner();
            onGameOverExecuteLogic();
        }

        public void DoMove()
        {
            m_ComputerAI.DoMove();
            r_BoardManager.CoverCellsNotWaitingForPartner();
            switchCurrentPlayer();
            onPlayerChanges();
            onGameOverExecuteLogic();
        }

        public bool IsCoveredCell(int i_Row, int i_Col)
        {
            return r_BoardManager.GetCoveredCells().Contains(Board[i_Row, i_Col]) || 
                (!Board[i_Row, i_Col].WaitingForPartner && !Board[i_Row, i_Col].FoundPartner);
        }

        public bool IsInBorderOfBoard(int i_Row, int i_Col)
        {
            return (i_Row < Board.GetLength(0)) && i_Row >= 0 && i_Col >= 0 && (i_Col < Board.GetLength(1));
        }

        public bool IsValidBoardSize(int i_Rows, int i_Cols)
        {
            return r_BoardManager.IsValidBoardSize(i_Rows, i_Cols);
        }

        public bool IsComputerTurn()
        {
            return PlayWithComputer && m_CurrentPlayer == m_SecondPlayer;
        }

        public void Quit()
        {
            Environment.Exit(0);
        }

        private void onGameOverExecuteLogic()
        {
            if (r_BoardManager.IsAllCellsUncovered() && OnGameOver != null)
            {
                OnGameOver.Invoke();
            }
        }

        private bool searchForWaitingCell(int i_Row, int i_Col)
        {
            bool foundWaitingCell = false;

            for (int i = 0; i < Board.GetLength(0) && !foundWaitingCell; i++)
            {
                for (int j = 0; j < Board.GetLength(1) && !foundWaitingCell; j++)
                {
                    if (Board[i, j].WaitingForPartner)
                    {
                        foundWaitingCell = true;

                        if (Board[i, j].Content == Board[i_Row, i_Col].Content)
                        {
                            Board[i, j].UpdateFoundPartner();
                            Board[i_Row, i_Col].UpdateFoundPartner();
                            m_CurrentPlayer.Score++;
                            onScoreChanges();
                        }
                        else
                        {
                            Board[i, j].WaitingForPartner = false;
                            switchCurrentPlayer();
                            onPlayerChanges();
                        }
                    }
                }
            }

            return foundWaitingCell;
        }

        private void onPlayerChanges()
        {
            if (OnPlayerChange != null)
            {
                OnPlayerChange.Invoke();
            }
        }

        private void switchCurrentPlayer()
        {
            m_CurrentPlayer = m_CurrentPlayer == r_FirstPlayer ? m_SecondPlayer : r_FirstPlayer;
        }

        private void onMoveDone()
        {
            if (OnCellSelected != null)
            {
                OnCellSelected.Invoke();
            }
        }

        private void onScoreChanges()
        {
            if (OnScoreChange != null)
            {
                OnScoreChange.Invoke();
            }
        }
    }
}
