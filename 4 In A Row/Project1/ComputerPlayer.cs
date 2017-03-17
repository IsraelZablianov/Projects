namespace B16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ComputerPlayer
    {
        private Player m_ComputerPlayer;
        private SizeOfMatrix m_RowsAndCols;
        private int[,] m_Matrix;
        private Random m_Random;

        public ComputerPlayer(int i_HisNumberInMatrix, int[,] i_Matrix)
        {
            m_ComputerPlayer = new Player(i_HisNumberInMatrix, i_Matrix);
            m_RowsAndCols = m_ComputerPlayer.GetRowsAndColsOfMatrix();
            m_Matrix = i_Matrix;
            m_Random = new Random();
        }

        public void ComputerAIMove()
        {
            bool makedAMove = false;
            makedAMove = MakeACalculetDecision();

            while (!makedAMove)
            {
                int randSelect = m_Random.Next(1, m_RowsAndCols.Cols + 1);
                makedAMove = m_ComputerPlayer.PlayerMove(randSelect);
            }
        }

        public Player Player
        {
            get
            {
                return m_ComputerPlayer;
            }
        }

        private void RandomMove()
        {
            bool goodInput = false;
            Random rnd = new Random();
            int randSelect = rnd.Next(1, m_RowsAndCols.Cols + 1);
            goodInput = m_ComputerPlayer.PlayerMove(randSelect);
            while (!goodInput)
            {
                randSelect = rnd.Next(1, m_RowsAndCols.Cols + 1);
                goodInput = m_ComputerPlayer.PlayerMove(randSelect);
            }
        }

        private bool BlockOpponent(int i_ContinumToBlock)
        {
            bool makedAMove = false;
            makedAMove = BlockOpponentIfGetToDesiredDiagonal(i_ContinumToBlock);
            if (!makedAMove)
            {
                makedAMove = BlockOpponentIfGetToDesiredRows(i_ContinumToBlock);
                if (!makedAMove)
                {
                    makedAMove = BlockOpponentIfGetToDesiredCols(i_ContinumToBlock);
                }
            }

            return makedAMove;
        }

        private bool CompleteContinum(int i_ContinumToComplete)
        {
            bool makedAMove = false;
            makedAMove = CompleteDesiredDiagonal(i_ContinumToComplete);
            if (!makedAMove)
            {
                makedAMove = CompleteDesiredCols(i_ContinumToComplete);
                if (!makedAMove)
                {
                    makedAMove = CompleteDesiredRows(i_ContinumToComplete);
                }
            }

            return makedAMove;
        }

        private bool MakeACalculetDecision()
        {
            bool makedAMove = false;
            const int k_NumberOfContinumToDealWith = 2;
            const int k_CriticalNumberOfContinum = 3;
            makedAMove = CompleteContinum(k_CriticalNumberOfContinum);
            if (!makedAMove)
            {
                makedAMove = BlockOpponent(k_CriticalNumberOfContinum);
            }

            if (!makedAMove)
            {
                makedAMove = BlockOpponent(k_NumberOfContinumToDealWith);
            }

            if (!makedAMove)
            {
                makedAMove = CompleteContinum(k_NumberOfContinumToDealWith);
            }

            return makedAMove;
        }

        private bool BlockOpponentIfGetToDesiredDiagonal(int i_NumberOfDesiredDiagonal)
        {
            int countWinDiagonal = 0;
            int DesiredPlaceToBlock = 0;
            bool blocked = false;

            for (int i = 0; i < m_RowsAndCols.Rows && !blocked; i++)
            {
                for (int j = 0; j < m_RowsAndCols.Cols && !blocked; j++)
                {
                    if (m_Matrix[i, j] != m_ComputerPlayer.SpecialNumberInMatrix && m_Matrix[i, j] != 0)
                    {
                        countWinDiagonal = 1;
                        int moveOnRowsToSeekTheDisiredSumByDiagonal = i, moveOnColsToSeekTheDisiredSumByDiagonal = j;
                        while (!blocked && moveOnRowsToSeekTheDisiredSumByDiagonal + 1 < m_RowsAndCols.Rows && moveOnColsToSeekTheDisiredSumByDiagonal + 1 < m_RowsAndCols.Cols && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal + 1] != m_ComputerPlayer.SpecialNumberInMatrix && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal + 1] != 0)
                        {
                            countWinDiagonal++;
                            moveOnColsToSeekTheDisiredSumByDiagonal++;
                            moveOnRowsToSeekTheDisiredSumByDiagonal++;
                            if (countWinDiagonal == i_NumberOfDesiredDiagonal && (moveOnColsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal) >= 0 && (moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal) >= 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal] != 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal, moveOnColsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal] == 0)
                            {
                                DesiredPlaceToBlock = moveOnColsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal;
                                DesiredPlaceToBlock++;
                                blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                            }
                            else if (countWinDiagonal == i_NumberOfDesiredDiagonal && (moveOnColsToSeekTheDisiredSumByDiagonal + 1) < m_RowsAndCols.Cols && (moveOnRowsToSeekTheDisiredSumByDiagonal + 1) < m_RowsAndCols.Rows && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal + 1] == 0)
                            {
                                if (moveOnRowsToSeekTheDisiredSumByDiagonal + 2 < m_RowsAndCols.Rows && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 2, moveOnColsToSeekTheDisiredSumByDiagonal + 1] != 0)
                                {
                                    DesiredPlaceToBlock = moveOnColsToSeekTheDisiredSumByDiagonal + 1;
                                    DesiredPlaceToBlock++;
                                    blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                                }
                                else if (moveOnRowsToSeekTheDisiredSumByDiagonal + 2 >= m_RowsAndCols.Rows)
                                {
                                    DesiredPlaceToBlock = moveOnColsToSeekTheDisiredSumByDiagonal + 1;
                                    DesiredPlaceToBlock++;
                                    blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                                }
                            }
                        }

                        if (!blocked)
                        {
                            countWinDiagonal = 1;
                            moveOnRowsToSeekTheDisiredSumByDiagonal = i;
                            moveOnColsToSeekTheDisiredSumByDiagonal = j;
                            while (!blocked && moveOnRowsToSeekTheDisiredSumByDiagonal + 1 < m_RowsAndCols.Rows && moveOnColsToSeekTheDisiredSumByDiagonal - 1 >= 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal - 1] != m_ComputerPlayer.SpecialNumberInMatrix && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal - 1] != 0)
                            {
                                countWinDiagonal++;
                                moveOnColsToSeekTheDisiredSumByDiagonal--;
                                moveOnRowsToSeekTheDisiredSumByDiagonal++;
                                if (countWinDiagonal == i_NumberOfDesiredDiagonal && (moveOnColsToSeekTheDisiredSumByDiagonal + i_NumberOfDesiredDiagonal) < m_RowsAndCols.Cols && (moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal) >= 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal + i_NumberOfDesiredDiagonal] != 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal, moveOnColsToSeekTheDisiredSumByDiagonal + i_NumberOfDesiredDiagonal] == 0)
                                {
                                    DesiredPlaceToBlock = moveOnColsToSeekTheDisiredSumByDiagonal + i_NumberOfDesiredDiagonal;
                                    DesiredPlaceToBlock++;
                                    blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                                }
                                else if (countWinDiagonal == i_NumberOfDesiredDiagonal && (moveOnColsToSeekTheDisiredSumByDiagonal - 1) >= 0 && (moveOnRowsToSeekTheDisiredSumByDiagonal + 1) < m_RowsAndCols.Rows && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal - 1] == 0)
                                {
                                    if (moveOnRowsToSeekTheDisiredSumByDiagonal + 2 < m_RowsAndCols.Rows && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 2, moveOnColsToSeekTheDisiredSumByDiagonal - 1] != 0)
                                    {
                                        DesiredPlaceToBlock = moveOnColsToSeekTheDisiredSumByDiagonal - 1;
                                        DesiredPlaceToBlock++;
                                        blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                                    }
                                    else if (moveOnRowsToSeekTheDisiredSumByDiagonal + 2 >= m_RowsAndCols.Rows)
                                    {
                                        DesiredPlaceToBlock = moveOnColsToSeekTheDisiredSumByDiagonal - 1;
                                        DesiredPlaceToBlock++;
                                        blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                                    }
                                }
                            }
                        }

                        if (!blocked)
                        {
                            countWinDiagonal = 0;
                        }
                    }
                }
            }

            return blocked;
        }

        private bool CompleteDesiredDiagonal(int i_NumberOfDesiredDiagonal)
        {
            int countWinDiagonal = 0;
            int DesiredPlaceComplete = 0;
            bool complete = false;

            for (int i = 0; i < m_RowsAndCols.Rows && !complete; i++)
            {
                for (int j = 0; j < m_RowsAndCols.Cols && !complete; j++)
                {
                    if (m_Matrix[i, j] == m_ComputerPlayer.SpecialNumberInMatrix)
                    {
                        countWinDiagonal = 1;
                        int moveOnRowsToSeekTheDisiredSumByDiagonal = i, moveOnColsToSeekTheDisiredSumByDiagonal = j;
                        while (!complete && moveOnRowsToSeekTheDisiredSumByDiagonal + 1 < m_RowsAndCols.Rows && moveOnColsToSeekTheDisiredSumByDiagonal + 1 < m_RowsAndCols.Cols && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal + 1] == m_ComputerPlayer.SpecialNumberInMatrix)
                        {
                            countWinDiagonal++;
                            moveOnColsToSeekTheDisiredSumByDiagonal++;
                            moveOnRowsToSeekTheDisiredSumByDiagonal++;
                            if (countWinDiagonal == i_NumberOfDesiredDiagonal && (moveOnColsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal) >= 0 && (moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal) >= 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal] != 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal, moveOnColsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal] == 0)
                            {
                                DesiredPlaceComplete = moveOnColsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal;
                                DesiredPlaceComplete++;
                                complete = m_ComputerPlayer.PlayerMove(DesiredPlaceComplete);
                            }
                            else if (countWinDiagonal == i_NumberOfDesiredDiagonal && (moveOnColsToSeekTheDisiredSumByDiagonal + 1) < m_RowsAndCols.Cols && (moveOnRowsToSeekTheDisiredSumByDiagonal + 1) < m_RowsAndCols.Rows && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal + 1] == 0)
                            {
                                if (moveOnRowsToSeekTheDisiredSumByDiagonal + 2 < m_RowsAndCols.Rows && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 2, moveOnColsToSeekTheDisiredSumByDiagonal + 1] != 0)
                                {
                                    DesiredPlaceComplete = moveOnColsToSeekTheDisiredSumByDiagonal + 1;
                                    DesiredPlaceComplete++;
                                    complete = m_ComputerPlayer.PlayerMove(DesiredPlaceComplete);
                                }
                                else if (moveOnRowsToSeekTheDisiredSumByDiagonal + 2 >= m_RowsAndCols.Rows)
                                {
                                    DesiredPlaceComplete = moveOnColsToSeekTheDisiredSumByDiagonal + 1;
                                    DesiredPlaceComplete++;
                                    complete = m_ComputerPlayer.PlayerMove(DesiredPlaceComplete);
                                }
                            }
                        }

                        if (!complete)
                        {
                            countWinDiagonal = 1;
                            moveOnRowsToSeekTheDisiredSumByDiagonal = i;
                            moveOnColsToSeekTheDisiredSumByDiagonal = j;
                            while (!complete && moveOnRowsToSeekTheDisiredSumByDiagonal + 1 < m_RowsAndCols.Rows && moveOnColsToSeekTheDisiredSumByDiagonal - 1 >= 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal - 1] == m_ComputerPlayer.SpecialNumberInMatrix)
                            {
                                countWinDiagonal++;
                                moveOnColsToSeekTheDisiredSumByDiagonal--;
                                moveOnRowsToSeekTheDisiredSumByDiagonal++;
                                if (countWinDiagonal == i_NumberOfDesiredDiagonal && (moveOnColsToSeekTheDisiredSumByDiagonal + i_NumberOfDesiredDiagonal) < m_RowsAndCols.Cols && (moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal) >= 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal + i_NumberOfDesiredDiagonal] != 0 && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal - i_NumberOfDesiredDiagonal, moveOnColsToSeekTheDisiredSumByDiagonal + i_NumberOfDesiredDiagonal] == 0)
                                {
                                    DesiredPlaceComplete = moveOnColsToSeekTheDisiredSumByDiagonal + i_NumberOfDesiredDiagonal;
                                    DesiredPlaceComplete++;
                                    complete = m_ComputerPlayer.PlayerMove(DesiredPlaceComplete);
                                }
                                else if (countWinDiagonal == i_NumberOfDesiredDiagonal && (moveOnColsToSeekTheDisiredSumByDiagonal - 1) >= 0 && (moveOnRowsToSeekTheDisiredSumByDiagonal + 1) < m_RowsAndCols.Rows && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 1, moveOnColsToSeekTheDisiredSumByDiagonal - 1] == 0)
                                {
                                    if (moveOnRowsToSeekTheDisiredSumByDiagonal + 2 < m_RowsAndCols.Rows && m_Matrix[moveOnRowsToSeekTheDisiredSumByDiagonal + 2, moveOnColsToSeekTheDisiredSumByDiagonal - 1] != 0)
                                    {
                                        DesiredPlaceComplete = moveOnColsToSeekTheDisiredSumByDiagonal - 1;
                                        DesiredPlaceComplete++;
                                        complete = m_ComputerPlayer.PlayerMove(DesiredPlaceComplete);
                                    }
                                    else if (moveOnRowsToSeekTheDisiredSumByDiagonal + 2 >= m_RowsAndCols.Rows)
                                    {
                                        DesiredPlaceComplete = moveOnColsToSeekTheDisiredSumByDiagonal - 1;
                                        DesiredPlaceComplete++;
                                        complete = m_ComputerPlayer.PlayerMove(DesiredPlaceComplete);
                                    }
                                }
                            }
                        }

                        if (!complete)
                        {
                            countWinDiagonal = 0;
                        }
                    }
                }
            }

            return complete;
        }

        private bool BlockOpponentIfGetToDesiredCols(int i_NumberOfDesiredCols)
        {
            int countWinCol = 0;
            int DesiredPlaceToBlock = 0;
            bool blocked = false;
            for (int i = 0; i < m_RowsAndCols.Cols && !blocked; i++)
            {
                for (int j = 0; j < m_RowsAndCols.Rows && !blocked; j++)
                {
                    if (m_Matrix[j, i] != m_ComputerPlayer.SpecialNumberInMatrix && m_Matrix[j, i] != 0)
                    {
                        countWinCol++;
                    }
                    else
                    {
                        countWinCol = 0;
                    }

                    if (countWinCol == i_NumberOfDesiredCols && (j - i_NumberOfDesiredCols) > 0 && m_Matrix[j - i_NumberOfDesiredCols, i] == 0)
                    {
                        DesiredPlaceToBlock = i;
                        DesiredPlaceToBlock++;
                        blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                    }
                }

                if (!blocked)
                {
                    countWinCol = 0;
                }
            }

            return blocked;
        }

        private bool CompleteDesiredCols(int i_NumberOfDesiredCols)
        {
            int countWinCol = 0;
            int DesiredPlaceToComplete = 0;
            bool complete = false;
            for (int i = 0; i < m_RowsAndCols.Cols && !complete; i++)
            {
                for (int j = 0; j < m_RowsAndCols.Rows && !complete; j++)
                {
                    if (m_Matrix[j, i] == m_ComputerPlayer.SpecialNumberInMatrix)
                    {
                        countWinCol++;
                    }
                    else
                    {
                        countWinCol = 0;
                    }

                    if (countWinCol == i_NumberOfDesiredCols && (j - i_NumberOfDesiredCols) > 0 && m_Matrix[j - i_NumberOfDesiredCols, i] == 0)
                    {
                        DesiredPlaceToComplete = i;
                        DesiredPlaceToComplete++;
                        complete = m_ComputerPlayer.PlayerMove(DesiredPlaceToComplete);
                    }
                }

                if (!complete)
                {
                    countWinCol = 0;
                }
            }

            return complete;
        }

        private bool BlockOpponentIfGetToDesiredRows(int i_NumberOfDesiredRows)
        {
            int countWinRow = 0;
            int DesiredPlaceToBlock = 0;
            bool blocked = false;
            for (int i = 0; i < m_RowsAndCols.Rows && !blocked; i++)
            {
                for (int j = 0; j < m_RowsAndCols.Cols && !blocked; j++)
                {
                    if (m_Matrix[i, j] != m_ComputerPlayer.SpecialNumberInMatrix && m_Matrix[i, j] != 0)
                    {
                        countWinRow++;
                    }
                    else
                    {
                        countWinRow = 0;
                    }

                    if (countWinRow == i_NumberOfDesiredRows && (j + 1) < m_RowsAndCols.Cols && m_Matrix[i, j + 1] == 0)
                    {
                        if ((i + 1) < m_RowsAndCols.Rows && m_Matrix[i + 1, j + 1] != 0)
                        {
                            DesiredPlaceToBlock = j + 1;
                            DesiredPlaceToBlock++;
                            blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                        }
                        else if ((i + 1) >= m_RowsAndCols.Rows)
                        {
                            DesiredPlaceToBlock = j + 1;
                            DesiredPlaceToBlock++;
                            blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                        }
                    }
                    else if (countWinRow == i_NumberOfDesiredRows && (j - i_NumberOfDesiredRows) >= 0 && m_Matrix[i, j - i_NumberOfDesiredRows] == 0)
                    {
                        if ((i + 1) < m_RowsAndCols.Rows && m_Matrix[i + 1, j - i_NumberOfDesiredRows] != 0)
                        {
                            DesiredPlaceToBlock = j - i_NumberOfDesiredRows;
                            DesiredPlaceToBlock++;
                            blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                        }
                        else if ((i + 1) >= m_RowsAndCols.Rows)
                        {
                            DesiredPlaceToBlock = j - i_NumberOfDesiredRows;
                            DesiredPlaceToBlock++;
                            blocked = m_ComputerPlayer.PlayerMove(DesiredPlaceToBlock);
                        }
                    }
                }

                if (!blocked)
                {
                    countWinRow = 0;
                }
            }

            return blocked;
        }

        private bool CompleteDesiredRows(int i_NumberOfDesiredRows)
        {
            int countWinRow = 0;
            int DesiredPlaceToComplete = 0;
            bool complete = false;
            for (int i = 0; i < m_RowsAndCols.Rows && !complete; i++)
            {
                for (int j = 0; j < m_RowsAndCols.Cols && !complete; j++)
                {
                    if (m_Matrix[i, j] == m_ComputerPlayer.SpecialNumberInMatrix)
                    {
                        countWinRow++;
                    }
                    else
                    {
                        countWinRow = 0;
                    }

                    if (countWinRow == i_NumberOfDesiredRows && (j + 1) < m_RowsAndCols.Cols && m_Matrix[i, j + 1] == 0)
                    {
                        if ((i + 1) < m_RowsAndCols.Rows && m_Matrix[i + 1, j + 1] != 0)
                        {
                            DesiredPlaceToComplete = j + 1;
                            DesiredPlaceToComplete++;
                            complete = m_ComputerPlayer.PlayerMove(DesiredPlaceToComplete);
                        }
                        else if ((i + 1) >= m_RowsAndCols.Rows)
                        {
                            DesiredPlaceToComplete = j + 1;
                            DesiredPlaceToComplete++;
                            complete = m_ComputerPlayer.PlayerMove(DesiredPlaceToComplete);
                        }
                    }
                    else if (countWinRow == i_NumberOfDesiredRows && (j - i_NumberOfDesiredRows) >= 0 && m_Matrix[i, j - i_NumberOfDesiredRows] == 0)
                    {
                        if ((i + 1) < m_RowsAndCols.Rows && m_Matrix[i + 1, j - i_NumberOfDesiredRows] != 0)
                        {
                            DesiredPlaceToComplete = j - i_NumberOfDesiredRows;
                            DesiredPlaceToComplete++;
                            complete = m_ComputerPlayer.PlayerMove(DesiredPlaceToComplete);
                        }
                        else if ((i + 1) >= m_RowsAndCols.Rows)
                        {
                            DesiredPlaceToComplete = j - i_NumberOfDesiredRows;
                            DesiredPlaceToComplete++;
                            complete = m_ComputerPlayer.PlayerMove(DesiredPlaceToComplete);
                        }
                    }
                }

                if (!complete)
                {
                    countWinRow = 0;
                }
            }

            return complete;
        }
    }
}
