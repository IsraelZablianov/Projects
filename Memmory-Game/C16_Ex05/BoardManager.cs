namespace C16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BoardManager
    {
        private readonly Random r_RandomProducer = new Random();
        private char m_FirstLetterCellContent = 'A';
        private int m_MaxSize = int.MaxValue;
        private int m_MinSize = 0;
        private Cell[,] m_Board;

        public int MaxSize
        {
            get { return m_MaxSize; }
            set { m_MaxSize = value; }
        }

        public int MinSize
        {
            get { return m_MinSize; }
            set { m_MinSize = value; }
        }

        public Cell[,] Board
        {
            get { return m_Board; }
            set { m_Board = value; }
        }

        public List<Cell> GetCoveredCells()
        {
            List<Cell> coveredCells = new List<Cell>();

            foreach (Cell cell in m_Board)
            {
                if (cell.Covered)
                {
                    coveredCells.Add(cell);
                }
            }

            return coveredCells;
        }

        public List<Cell> GetCellsWithOutPartner()
        {
            List<Cell> cellsWithOutPartner = new List<Cell>();

            foreach (Cell cell in m_Board)
            {
                if (!cell.FoundPartner)
                {
                    cellsWithOutPartner.Add(cell);
                }
            }

            return cellsWithOutPartner;
        }

        public void SetRandomBoardContent()
        {
            List<Cell> cells = GetCoveredCells();
            m_FirstLetterCellContent = 'A';

            while (cells.Count != 0)
            {
                int selectedPlace = r_RandomProducer.Next(cells.Count);
                cells[selectedPlace].Content = m_FirstLetterCellContent.ToString();
                cells.Remove(cells[selectedPlace]);

                if (cells.Count % 2 == 0)
                {
                    m_FirstLetterCellContent++;
                }
            }
        }

        public bool IsValidBoardSize(int i_Rows, int i_Cols)
        {
            bool validBoardSize;
            validBoardSize = (i_Rows <= m_MaxSize) && (i_Cols <= m_MaxSize)
                && (i_Rows >= m_MinSize) && (i_Cols >= m_MinSize)
                && ((i_Rows * i_Cols) % 2 == 0);

            return validBoardSize;
        }

        public void CoverCellsNotWaitingForPartner()
        {
            foreach (Cell cell in m_Board)
            {
                if ((!cell.WaitingForPartner) && (!cell.FoundPartner))
                {
                    cell.Covered = true;
                }
            }
        }

        public bool IsAllCellsUncovered()
        {
            return GetCoveredCells().Count == 0;
        }
    }
}
