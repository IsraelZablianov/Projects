namespace C16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    public class SettingsForm : Form
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox m_TextBoxFirstName;
        private TextBox m_TextBoxFriend;
        private Button m_PlayAgainstFriend;
        private Button m_BoardSize;
        private Button m_Start;
        private List<string> r_BoardSizesAlowed = new List<string>();
        private int m_CurrentSizeIndex;

        public SettingsForm()
        {
            InitializeComponent();
            r_BoardSizesAlowed.Add("4x4");
            r_BoardSizesAlowed.Add("4x5");
            r_BoardSizesAlowed.Add("4x6");
            r_BoardSizesAlowed.Add("5x4");
            r_BoardSizesAlowed.Add("5x6");
            r_BoardSizesAlowed.Add("6x4");
            r_BoardSizesAlowed.Add("6x5");
            r_BoardSizesAlowed.Add("6x6");
            m_BoardSize.Text = r_BoardSizesAlowed[0];
        }

        public int RowsSize
        {
            get
            {
                return int.Parse(m_BoardSize.Text[0].ToString());
            }
        }

        public int ColsSize
        {
            get
            {
                return int.Parse(m_BoardSize.Text[2].ToString());
            }
        }

        public string FirstPlayerName
        {
            get
            {
                return m_TextBoxFirstName.Text;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return m_TextBoxFriend.Text;
            }
        }

        public bool IsCompuerPlayer
        {
            get
            {
                return !m_TextBoxFriend.Enabled;
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_TextBoxFirstName = new System.Windows.Forms.TextBox();
            this.m_TextBoxFriend = new System.Windows.Forms.TextBox();
            this.m_PlayAgainstFriend = new System.Windows.Forms.Button();
            this.m_BoardSize = new System.Windows.Forms.Button();
            this.m_Start = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, (byte)177);
            this.label1.Location = new System.Drawing.Point(34, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "First player name: ";
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, (byte)177);
            this.label2.Location = new System.Drawing.Point(34, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Board size: ";
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, (byte)177);
            this.label3.Location = new System.Drawing.Point(34, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Second player name: ";
            this.m_TextBoxFirstName.Location = new System.Drawing.Point(208, 35);
            this.m_TextBoxFirstName.Name = "m_TextBoxFirstName";
            this.m_TextBoxFirstName.Size = new System.Drawing.Size(140, 20);
            this.m_TextBoxFirstName.TabIndex = 3;
            this.m_TextBoxFriend.Enabled = false;
            this.m_TextBoxFriend.Location = new System.Drawing.Point(208, 78);
            this.m_TextBoxFriend.Name = "m_TextBoxFriend";
            this.m_TextBoxFriend.Size = new System.Drawing.Size(140, 20);
            this.m_TextBoxFriend.TabIndex = 4;
            this.m_TextBoxFriend.Text = "-computer-";
            this.m_PlayAgainstFriend.Location = new System.Drawing.Point(362, 79);
            this.m_PlayAgainstFriend.Name = "m_PlayAgainstFriend";
            this.m_PlayAgainstFriend.Size = new System.Drawing.Size(97, 22);
            this.m_PlayAgainstFriend.TabIndex = 5;
            this.m_PlayAgainstFriend.Text = "Against A Friend";
            this.m_PlayAgainstFriend.UseVisualStyleBackColor = true;
            this.m_PlayAgainstFriend.Click += new System.EventHandler(this.buttonPlayAgainstFriend_Click);
            this.m_BoardSize.BackColor = System.Drawing.Color.FromArgb(192, 192, 255);
            this.m_BoardSize.Location = new System.Drawing.Point(37, 186);
            this.m_BoardSize.Name = "m_BoardSize";
            this.m_BoardSize.Size = new System.Drawing.Size(136, 87);
            this.m_BoardSize.TabIndex = 6;
            this.m_BoardSize.UseVisualStyleBackColor = false;
            this.m_BoardSize.Click += new System.EventHandler(this.buttonBoardSize_Click);
            this.m_Start.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
            this.m_Start.Location = new System.Drawing.Point(362, 250);
            this.m_Start.Name = "m_Start";
            this.m_Start.Size = new System.Drawing.Size(97, 23);
            this.m_Start.TabIndex = 7;
            this.m_Start.Text = "Start!";
            this.m_Start.UseVisualStyleBackColor = false;
            this.m_Start.Click += new System.EventHandler(this.buttonStart_Click);
            this.ClientSize = new System.Drawing.Size(471, 304);
            this.Controls.Add(this.m_Start);
            this.Controls.Add(this.m_BoardSize);
            this.Controls.Add(this.m_PlayAgainstFriend);
            this.Controls.Add(this.m_TextBoxFriend);
            this.Controls.Add(this.m_TextBoxFirstName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Memory Game - Settings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void buttonPlayAgainstFriend_Click(object sender, EventArgs e)
        {
            m_TextBoxFriend.Enabled = !m_TextBoxFriend.Enabled;

            if (m_TextBoxFriend.Enabled)
            {
                m_TextBoxFriend.Text = string.Empty;
            }
            else
            {
                m_TextBoxFriend.Text = "-computer-";
            }
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            m_CurrentSizeIndex++;
            m_CurrentSizeIndex %= r_BoardSizesAlowed.Count;
            m_BoardSize.Text = r_BoardSizesAlowed[m_CurrentSizeIndex];
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
