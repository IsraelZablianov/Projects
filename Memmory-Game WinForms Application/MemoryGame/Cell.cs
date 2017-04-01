namespace C16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Cell
    {
        private bool m_Covered = true;
        private string m_Content = " ";
        private bool m_IsWaitingForPartner;
        private bool m_FoundPartner;

        public bool FoundPartner
        {
            get { return m_FoundPartner; }
            set { m_FoundPartner = value; }
        }

        public bool WaitingForPartner
        {
            get { return m_IsWaitingForPartner; }
            set { m_IsWaitingForPartner = value; }
        }

        public string Content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }

        public bool Covered
        {
            get { return m_Covered; }
            set { m_Covered = value; }
        }

        public string ContentProvided()
        {
            return m_Covered ? " " : m_Content;
        }

        public void UpdateFoundPartner()
        {
            WaitingForPartner = false;
            FoundPartner = true;
            Covered = false;
        }
    }
}
