namespace B16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public struct SizeOfMatrix
    {
        private int m_Cols;
        private int m_Rows;

        public int Cols
        {
            get
            {
                return m_Cols;
            }

            set
            {
                m_Cols = value;
            }
        }

        public int Rows
        {
            get
            {
                return m_Rows;
            }

            set
            {
                m_Rows = value;
            }
        }
    }

    public class Game
    {
        private Player m_Player1;
        private Player m_Player2;
        private ComputerPlayer m_ComputerPlayer;
        private int[,] m_Matrix = null;
        private SizeOfMatrix m_RowsAndCols;

        public Player Player1
        {
            get
            {
                return m_Player1;
            }
        }

        public int[,] Matrix
        {
            get
            {
                return m_Matrix;
            }
        }

        public Player Player2
        {
            get
            {
                return m_Player2;
            }
        }

        public ComputerPlayer ComputerPlayer
        {
            get
            {
                return m_ComputerPlayer;
            }
        }

        public Game()
        {
        }

        public void NewGame()
        {
            for (int i = 0; i < m_RowsAndCols.Rows; i++)
            {
                for (int j = 0; j < m_RowsAndCols.Cols; j++)
                {
                    m_Matrix[i, j] = 0;
                }
            }

            m_Player1.EndOfGame = false;
            if (m_Player2 != null)
            {
                m_Player2.EndOfGame = false;
            }
            else
            {
                m_ComputerPlayer.Player.EndOfGame = false;
            }
        }

        public void BuildMatrix(int i_Rows, int i_Cols)
        {
            m_RowsAndCols.Rows = i_Rows;
            m_RowsAndCols.Cols = i_Cols;
            m_Matrix = new int[m_RowsAndCols.Rows, m_RowsAndCols.Cols];
        }

        public void PlayWithComputer()
        {
            m_Player1 = new Player(1, m_Matrix);
            m_ComputerPlayer = new ComputerPlayer(2, m_Matrix);
        }

        public void PlayWithFriend()
        {
            m_Player1 = new Player(1, m_Matrix);
            m_Player2 = new Player(2, m_Matrix);
        }
    }
}
