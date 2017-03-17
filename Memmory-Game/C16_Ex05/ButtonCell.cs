namespace C16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    public class ButtonCell : Button
    {
        private readonly int r_RowIndex;

        private readonly int r_ColIndex;

        private readonly int r_Id;

        public int ID
        {
            get { return r_Id; }
        }

        public int RowIndex
        {
            get { return r_RowIndex; }
        }

        public int ColIndex
        {
            get { return r_ColIndex; }
        } 

        public ButtonCell(int i_Id, int i_RowIndex, int i_ColIndex)
        {
            r_Id = i_Id;
            r_RowIndex = i_RowIndex;
            r_ColIndex = i_ColIndex;
        }
    }
}
