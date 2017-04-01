namespace B16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing;

    public class GameUI : Form
    {
        private Game m_game = new Game();
        private Label label1;
        private Label label2;
        private GameSettingForm m_GameSettingForm = new GameSettingForm();
        private Label m_Player1Wins;
        private Label m_Player2Wins;
        private List<Button> m_SelectColButtons = new List<Button>();
        private Button[,] m_Board;
        private bool m_NewRound = false;
        private bool m_FirstPlayerTurn = true;

        public GameUI()
        {
            InitializeComponent();
        }

        private void NewGame()
        {
            m_game.NewGame();
            EmptyTheBoard();
            m_FirstPlayerTurn = true;
            m_NewRound = true;
            foreach (Button cell in m_SelectColButtons)
            {
                cell.Enabled = true; 
            }
        }

        private void BuildMatrix()
        {
            m_game.BuildMatrix(m_GameSettingForm.Rows, m_GameSettingForm.Cols);
        }

        public void StartGame()
        {
            m_GameSettingForm.ShowDialog();
            m_Board = new Button[m_GameSettingForm.Rows, m_GameSettingForm.Cols];
            if (m_GameSettingForm.DialogResult == DialogResult.OK)
            {
                BuildMatrix();
                if (!m_GameSettingForm.IsComputerPlayer)
                {
                    PlayWithFriend();
                }
                else
                {
                    PlayWithComputer();
                }

                BuildBoard();
                this.ShowDialog();
            }
        }

        private void Rematch(string i_Title, string i_Msg)
        {
            if (MessageBox.Show(i_Msg, i_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                NewGame();
            }
            else
            {
                for (double i = 1; i > 0; i -= 0.00003)
                {
                    this.Opacity = i;
                }

                this.Close();
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            int colSelected = int.Parse((sender as Button).Text);

            if (m_GameSettingForm.IsComputerPlayer)
            {
                TurnOfPlayer(m_game.Player1, m_game.ComputerPlayer.Player, colSelected);
                if (!m_NewRound)
                {
                    TurnOfComputer(m_game.ComputerPlayer, m_game.Player1); 
                }
            }
            else
            {
                if (m_FirstPlayerTurn)
                {
                    m_FirstPlayerTurn = false;
                    TurnOfPlayer(m_game.Player1, m_game.Player2, colSelected);
                }
                else
                {
                    m_FirstPlayerTurn = true;
                    TurnOfPlayer(m_game.Player2, m_game.Player1, colSelected);
                }               
            }

            m_NewRound = false;
            if (m_game.Player1.ColIsFull(colSelected))
            {
                m_SelectColButtons[colSelected - 1].Enabled = false;
            }
        }

        private void MoveOfPlayer(Player i_Player, int i_Move)
        {
            i_Player.PlayerMove(i_Move);
        }

        private void BuildBoard()
        {
            this.Height = (m_GameSettingForm.Rows * 50) + 150;
            this.Width = (m_GameSettingForm.Cols * 50) + 50;

            for (int i = 1; i <= m_GameSettingForm.Cols; i++)
            {
                Button cell = new Button();
                this.Controls.Add(cell);
                cell.Click += Button_Click;
                cell.Width = 40;
                cell.Height = 30;
                cell.Text = i.ToString();
                m_SelectColButtons.Add(cell);
            }

            m_SelectColButtons[0].Top = 20;
            m_SelectColButtons[0].Left = (ClientSize.Width - ((m_GameSettingForm.Cols * m_SelectColButtons[0].Width) + (m_GameSettingForm.Cols * 10) - 10)) / 2;
            for (int i = 1; i < m_GameSettingForm.Cols; i++)
            {
                m_SelectColButtons[i].Top = m_SelectColButtons[i - 1].Top;
                m_SelectColButtons[i].Left = m_SelectColButtons[i - 1].Left + m_SelectColButtons[i - 1].Width + 10;
            }

            for (int i = 0; i < m_GameSettingForm.Rows; i++)
            {
                for (int j = 0; j < m_GameSettingForm.Cols; j++)
                {
                    Button cell = new Button();
                    this.Controls.Add(cell);
                    cell.Width = 40;
                    cell.Height = 40;
                    m_Board[i, j] = cell;
                }
            }

            EmptyTheBoard();
            m_Board[0, 0].Top = 60;
            m_Board[0, 0].Left = m_SelectColButtons[0].Left;
            for (int i = 0; i < m_GameSettingForm.Rows; i++)
            {
                for (int j = 1; j < m_GameSettingForm.Cols; j++)
                {
                    m_Board[i, j].Top = m_Board[i, j - 1].Top;
                    m_Board[i, j].Left = m_Board[i, j - 1].Left + m_Board[i, j - 1].Width + 10;
                }

                if (i + 1 != m_GameSettingForm.Rows)
                {
                    m_Board[i + 1, 0].Top = m_Board[i, 0].Top + m_Board[i, 0].Height + 10;
                    m_Board[i + 1, 0].Left = m_Board[i, 0].Left;
                }
            }

            this.label1.Top = this.ClientSize.Height - 35;
            this.label1.Left = this.ClientSize.Width / 4;
            this.m_Player1Wins.Top = this.label1.Top + 5;
            this.m_Player1Wins.Left = this.label1.Left + this.label1.Width;

            this.label2.Top = this.label1.Top;
            this.label2.Left = this.m_Player1Wins.Left + this.m_Player1Wins.Width + 30;
            this.m_Player2Wins.Top = this.label2.Top + 5;
            this.m_Player2Wins.Left = this.label2.Left + this.label2.Width;
        }

        private void EmptyTheBoard()
        {
            foreach (Button cell in m_Board)
            {
                cell.Text = string.Empty;
            }
        }

        private void PrintBoard(int i_Player1SpeacialNumber, int i_Player2SpeacialNumber, int[,] i_Matrix)
        {
            for (int i = 0; i < m_GameSettingForm.Rows; i++)
            {
                for (int j = 0; j < m_GameSettingForm.Cols; j++)
                {
                    if (i_Matrix[i, j] == i_Player1SpeacialNumber)
                    {
                        m_Board[i, j].Text = "X";
                    }
                    else if (i_Matrix[i, j] == i_Player2SpeacialNumber)
                    {
                        m_Board[i, j].Text = "O";
                    }
                }
            }
        }

        private void PlayWithComputer()
        {
            m_game.PlayWithComputer();
            InitializePlayersName(m_game.Player1, m_game.ComputerPlayer.Player);
        }

        private void PlayWithFriend()
        {
            m_game.PlayWithFriend();
            InitializePlayersName(m_game.Player1, m_game.Player2);
        }

        private void TurnOfPlayer(Player i_PlayerThatItIsHisTurn, Player i_SecondPlayer, int i_Move)
        {
            bool win = false;
            bool tie = false;
            const int k_FirstPlayerSpecialNumber = 1;
            const int k_SecondPlayerSpecialNumber = 2;
            MoveOfPlayer(i_PlayerThatItIsHisTurn, i_Move);
            PrintBoard(k_FirstPlayerSpecialNumber, k_SecondPlayerSpecialNumber, m_game.Matrix);
            tie = IfThereIsATie(i_PlayerThatItIsHisTurn, i_SecondPlayer);
            win = ThereIsAWinner(i_PlayerThatItIsHisTurn);
            if (win)
            {
                BalanceWins(i_PlayerThatItIsHisTurn, i_SecondPlayer);
            }
        }

        private void TurnOfComputer(ComputerPlayer i_ComputerPlayer, Player i_SecondPlayer)
        {
            bool win = false;
            bool tie = false;
            const int k_FirstPlayerSpecialNumber = 1;
            const int k_SecondPlayerSpecialNumber = 2;
            i_ComputerPlayer.ComputerAIMove();
            PrintBoard(k_FirstPlayerSpecialNumber, k_SecondPlayerSpecialNumber, m_game.Matrix);
            tie = IfThereIsATie(i_ComputerPlayer.Player, i_SecondPlayer);
            win = ThereIsAWinner(i_ComputerPlayer.Player);
            if (win)
            {
                BalanceWins(i_ComputerPlayer.Player, i_SecondPlayer);
            }
        }

        private bool ThereIsAWinner(Player i_Player)
        {
            bool win = false;
            win = i_Player.IfPlayerWin();
            if (win)
            {
                Rematch("A Win!", string.Format("{0} Won!!{1}Another Round?", i_Player.NamePlayer, Environment.NewLine));
            }

            return win;
        }

        private void BalanceWins(Player i_Player1, Player i_Player2)
        {
            if (i_Player1.SpecialNumberInMatrix == 1)
            {
                m_Player1Wins.Text = i_Player1.NumOfWins.ToString();
                m_Player2Wins.Text = i_Player2.NumOfWins.ToString();
            }
            else
            {
                m_Player2Wins.Text = i_Player1.NumOfWins.ToString();
                m_Player1Wins.Text = i_Player2.NumOfWins.ToString();
            }           
        }

        private bool IfThereIsATie(Player i_Player1, Player i_Player2)
        {
            bool tie = false;
            if (!i_Player1.ThereIsPlaceInMatrix() && !i_Player1.IfPlayerWin() && !i_Player2.IfPlayerWin())
            {
                tie = true;
                Rematch("A Tie!", string.Format("Tie!!{0}Another Round?", Environment.NewLine));
            }

            return tie;
        }

        private void InitializePlayersName(Player i_Player1, Player i_Player2)
        {
            i_Player1.NamePlayer = m_GameSettingForm.Player1Name;
            i_Player2.NamePlayer = m_GameSettingForm.Player2Name;
            this.label1.Text = m_GameSettingForm.Player1Name + ": ";
            this.label2.Text = m_GameSettingForm.Player2Name + ": ";
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_Player1Wins = new System.Windows.Forms.Label();
            this.m_Player2Wins = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(181, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 18);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.UseMnemonic = false;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 18);
            this.m_Player1Wins.AutoSize = true;
            this.m_Player1Wins.Location = new System.Drawing.Point(0, 0);
            this.m_Player1Wins.Name = "m_Player1Wins";
            this.m_Player1Wins.Size = new System.Drawing.Size(13, 13);
            this.m_Player1Wins.TabIndex = 2;
            this.m_Player1Wins.Text = "0";
            this.m_Player2Wins.AutoSize = true;
            this.m_Player2Wins.Location = new System.Drawing.Point(0, 0);
            this.m_Player2Wins.Name = "m_Player2Wins";
            this.m_Player2Wins.Size = new System.Drawing.Size(13, 13);
            this.m_Player2Wins.TabIndex = 3;
            this.m_Player2Wins.Text = "0";
            this.ClientSize = new System.Drawing.Size(341, 344);
            this.Controls.Add(this.m_Player2Wins);
            this.Controls.Add(this.m_Player1Wins);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "GameUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "4 in a row!!";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
