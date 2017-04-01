namespace B16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Player
    {
        private int m_SpecialNumberInMatrix;
        private SizeOfMatrix m_RowsAndCols;
        private int[,] m_Matrix;
        private bool m_EndOfGame;
        private int m_NumOfWins;
        private string m_NamePlayer;

        public Player(int i_HisNumberInMatrix, int[,] i_Matrix)
        {
            m_SpecialNumberInMatrix = i_HisNumberInMatrix;
            m_RowsAndCols.Rows = i_Matrix.GetLength(0);
            m_RowsAndCols.Cols = i_Matrix.GetLength(1);
            m_Matrix = i_Matrix;
        }

        public int NumOfWins
        {
            get
            {
                return m_NumOfWins;
            }

            set
            {
                m_NumOfWins = value;
            }
        }

        public bool EndOfGame
        {
            get
            {
                return m_EndOfGame;
            }

            set
            {
                m_EndOfGame = value;
            }
        }

        public int SpecialNumberInMatrix
        {
            get
            {
                return m_SpecialNumberInMatrix;
            }

            set
            {
                m_SpecialNumberInMatrix = value;
            }
        }

        public string NamePlayer
        {
            get
            {
                return m_NamePlayer;
            }

            set
            {
                m_NamePlayer = value;
            }
        }

        public SizeOfMatrix GetRowsAndColsOfMatrix()
        {
            return m_RowsAndCols;
        }

        public bool ColIsFull(int i_Select)
        {
            i_Select--;
            return m_Matrix[0, i_Select] != 0;
        }

        public bool PlayerMove(int i_Select)
        {
            bool corectInput = true;
            bool foundPlace = false;
            i_Select--;

            if (i_Select < 0 || i_Select >= m_RowsAndCols.Cols || m_Matrix[0, i_Select] != 0)
            {
                corectInput = false;
            }

            for (int i = m_RowsAndCols.Rows - 1; !foundPlace && i >= 0 && corectInput && !m_EndOfGame; i--)
            {
                if (m_Matrix[i, i_Select] == 0)
                {
                    m_Matrix[i, i_Select] = m_SpecialNumberInMatrix;
                    foundPlace = true;
                }
            }

            return corectInput;
        }

        public bool IfEndOfGame()
        {
            bool thereIsPlaceInMatrix = ThereIsPlaceInMatrix();
            if (!thereIsPlaceInMatrix)
            {
                m_EndOfGame = true;
            }

            return m_EndOfGame;
        }

        public bool ThereIsPlaceInMatrix()
        {
            bool thereIsPlaceInMatrix = false;
            for (int i = 0; i < m_RowsAndCols.Cols; i++)
            {
                if (m_Matrix[0, i] == 0)
                {
                    thereIsPlaceInMatrix = true;
                }
            }

            return thereIsPlaceInMatrix;
        }

        public bool IfPlayerWin()
        {
            bool IfPlayerWins = false;
            if (WinByCols() || WinByDiagonal() || WinByRows())
            {
                IfPlayerWins = true;
                m_NumOfWins++;
            }

            return IfPlayerWins;
        }

        private bool WinByCols()
        {
            const int k_NumberOfColToWin = 4;
            int countWinCol = 0;
            bool win = false;
            for (int i = 0; i < m_RowsAndCols.Cols && !win; i++)
            {
                for (int j = 0; j < m_RowsAndCols.Rows && !win; j++)
                {
                    if (m_Matrix[j, i] == m_SpecialNumberInMatrix)
                    {
                        countWinCol++;
                    }
                    else
                    {
                        countWinCol = 0;
                    }

                    if (countWinCol == k_NumberOfColToWin)
                    {
                        win = true;
                    }
                }

                if (!win)
                {
                    countWinCol = 0;
                }
            }

            return win;
        }

        private bool WinByDiagonal()
        {
            const int k_NumberOfDiagonalToWin = 4;
            int countWinDiagonal = 0;
            bool win = false;

            for (int i = 0; !win && i < m_RowsAndCols.Rows; i++)
            {
                for (int j = 0; !win && j < m_RowsAndCols.Cols; j++)
                {
                    if (j < m_RowsAndCols.Cols && m_Matrix[i, j] == m_SpecialNumberInMatrix)
                    {
                        countWinDiagonal = 1;
                        int moveOnRowsToSeekTheDisiredSumByDiagonal = i, moveOnColsToSeekTheDisiredSumByDiagonal = j;
                        while (!win && moveOnRowsToSeekTheDisiredSumByDiagonal + 1 < m_RowsAndCols.Rows && moveOnColsToSeekTheDisiredSumByDiagonal + 1 < m_RowsAndCols.Cols && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal + 1] == m_SpecialNumberInMatrix)
                        {
                            countWinDiagonal++;
                            moveOnColsToSeekTheDisiredSumByDiagonal++;
                            moveOnRowsToSeekTheDisiredSumByDiagonal++;
                            if (countWinDiagonal == k_NumberOfDiagonalToWin)
                            {
                                win = true;
                            }
                        }

                        if (countWinDiagonal < k_NumberOfDiagonalToWin)
                        {
                            countWinDiagonal = 1;
                            moveOnRowsToSeekTheDisiredSumByDiagonal = i;
                            moveOnColsToSeekTheDisiredSumByDiagonal = j;
                            while (!win && moveOnRowsToSeekTheDisiredSumByDiagonal + 1 < m_RowsAndCols.Rows && moveOnColsToSeekTheDisiredSumByDiagonal - 1 >= 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal - 1] == m_SpecialNumberInMatrix)
                            {
                                countWinDiagonal++;
                                moveOnColsToSeekTheDisiredSumByDiagonal--;
                                moveOnRowsToSeekTheDisiredSumByDiagonal++;
                                if (countWinDiagonal == k_NumberOfDiagonalToWin)
                                {
                                    win = true;
                                }
                            }
                        }

                        if (countWinDiagonal < k_NumberOfDiagonalToWin)
                        {
                            countWinDiagonal = 0;
                        }
                    }
                }
            }

            return win;
        }

        private bool WinByRows()
        {
            const int k_NumberOfRowsToWin = 4;
            int countWinRow = 0;
            bool win = false;
            for (int i = 0; i < m_RowsAndCols.Rows && !win; i++)
            {
                for (int j = 0; j < m_RowsAndCols.Cols && !win; j++)
                {
                    if (m_Matrix[i, j] == m_SpecialNumberInMatrix)
                    {
                        countWinRow++;
                    }
                    else
                    {
                        countWinRow = 0;
                    }

                    if (countWinRow == k_NumberOfRowsToWin)
                    {
                        win = true;
                    }
                }

                if (!win)
                {
                    countWinRow = 0;
                }
            }

            return win;
        }
    }
}
