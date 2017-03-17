namespace C16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PlayerState
    {
        private string m_Name = string.Empty;
        private int m_Score;

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
    }
}
