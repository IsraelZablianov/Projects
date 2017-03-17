namespace C16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Text;

    public class GameUIForm : Form
    {
        private readonly GameLogic r_GameLogic = new GameLogic();
        private readonly SettingsForm r_SettingsForm = new SettingsForm();
        private Color[,] m_CellsColor;
        private Color m_FirstPlayerColor = System.Drawing.Color.FromArgb(192, 192, 255);
        private Color m_SecondPlayerColor = Color.GreenYellow;
        private ButtonCell[,] m_ButtonsCells;
        private DialogResult m_DialogResult;
        private Label m_FirstPlayerNameLabel;
        private Label m_SecondPlayerNameLabel;
        private Label m_CurrentPlayerNameLabel;
        private Label m_FirstPlayerScoreLabel;
        private Label m_SecondPlayerScoreLabel;

        public GameUIForm()
        {
            InitializeComponent();
            r_GameLogic.OnCellSelected += updateCellsColor;
            r_GameLogic.OnCellSelected += updateButtonsContent;
            r_GameLogic.OnPlayerChange += updateCurrentPlayer;
            r_GameLogic.OnGameOver += onGameEnd;
            r_GameLogic.OnScoreChange += updateScores;
        }

        public void Start()
        {
            initialGameSettings();
            ShowDialog();
        }

        private void performValidMoveOfCurrentPlayer(ButtonCell i_Button)
        {
            r_GameLogic.DoMove(i_Button.RowIndex, i_Button.ColIndex);
        }

        private bool isPlayrWantToRematch()
        {
            m_DialogResult = MessageBox.Show("Do you want to rematch?", "Game Over", MessageBoxButtons.YesNo);

            return m_DialogResult == DialogResult.Yes;
        }

        private void displayScoresAndWinner()
        {
            string msg;

            if (r_GameLogic.FirstPlayerScore == r_GameLogic.SecondPlayerScore)
            {
                msg = "Tie!!";
            }
            else
            {
                string winner = r_GameLogic.FirstPlayerScore > r_GameLogic.SecondPlayerScore
                ? r_GameLogic.FirstPlayerName : r_GameLogic.SecondPlayerName;
                msg = string.Format(
    @"====== A Win =======
{0} Won.
{1} score {2}.
{3} score {4}.",
                   winner,
                   r_GameLogic.FirstPlayerName,
                   r_GameLogic.FirstPlayerScore,
                   r_GameLogic.SecondPlayerName,
                   r_GameLogic.SecondPlayerScore);
            }

            MessageBox.Show(msg, "Game Over");
        }

        private void updateCurrentPlayer()
        {
            updateScores();
            m_CurrentPlayerNameLabel.Text = string.Format("Current player: {0}", r_GameLogic.CurrentPlayerName);
            m_CurrentPlayerNameLabel.BackColor = r_GameLogic.CurrentPlayerName == r_GameLogic.FirstPlayerName ? m_FirstPlayerColor : m_SecondPlayerColor;
        }

        private void updateScores()
        {
            m_FirstPlayerScoreLabel.Text = string.Format("{0} pairs", r_GameLogic.FirstPlayerScore);
            m_SecondPlayerScoreLabel.Text = string.Format("{0} pairs", r_GameLogic.SecondPlayerScore);
        }

        private void updateButtonsContent()
        {
            for (int i = 0; i < r_GameLogic.Board.GetLength(0); i++)
            {
                for (int j = 0; j < r_GameLogic.Board.GetLength(1); j++)
                {
                    m_ButtonsCells[i, j].Text = r_GameLogic.Board[i, j].ContentProvided();
                }
            }
        }

        private void updateCellsColor()
        {
            for (int i = 0; i < r_GameLogic.Board.GetLength(0); i++)
            {
                for (int j = 0; j < r_GameLogic.Board.GetLength(1); j++)
                {
                    if (r_GameLogic.Board[i, j].Covered)
                    {
                        m_CellsColor[i, j] = Color.Gray;
                    }
                    else if (!r_GameLogic.Board[i, j].Covered && !r_GameLogic.Board[i, j].FoundPartner)
                    {
                        m_CellsColor[i, j] = r_GameLogic.CurrentPlayerName == r_GameLogic.FirstPlayerName ?
                        m_FirstPlayerColor : m_SecondPlayerColor;
                    }

                    m_ButtonsCells[i, j].BackColor = m_CellsColor[i, j];
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            performValidMoveOfCurrentPlayer(sender as ButtonCell);

            if (r_GameLogic.IsComputerTurn())
            {
                r_GameLogic.DoMove();
            }
        }

        private void onGameEnd()
        {
            displayScoresAndWinner();

            if (isPlayrWantToRematch())
            {
                r_GameLogic.Reset();
                initialCellsColor();
                updateButtonsContent();
                updateCurrentPlayer();
            }
            else
            {
                Close();
            }
        }

        private void initialGameSettings()
        {
            r_SettingsForm.ShowDialog();
            r_GameLogic.PlayWithComputer = r_SettingsForm.IsCompuerPlayer;
            r_GameLogic.InitialGame(r_SettingsForm.RowsSize, r_SettingsForm.ColsSize);
            m_CellsColor = new Color[r_SettingsForm.RowsSize, r_SettingsForm.ColsSize];
            initializeButonCells();
            initialCellsColor();
            updateCurrentPlayer();
            initialPlayersName();
        }

        private void initialPlayersName()
        {
            r_GameLogic.FirstPlayerName = r_SettingsForm.FirstPlayerName;
            r_GameLogic.SecondPlayerName = r_SettingsForm.SecondPlayerName;
            m_FirstPlayerNameLabel.Text = r_GameLogic.FirstPlayerName;
            m_FirstPlayerNameLabel.BackColor = m_FirstPlayerColor;
            m_SecondPlayerNameLabel.Text = r_GameLogic.SecondPlayerName;
            m_SecondPlayerNameLabel.BackColor = m_SecondPlayerColor;
        }

        private void initializeButonCells()
        {
            m_ButtonsCells = new ButtonCell[r_SettingsForm.RowsSize, r_SettingsForm.ColsSize];
            int buttonsWidth = 60, buttonsheight = 44, margin = 20, space = 10, namesHeight = 120, XCordinate = 26, YCordinate = 26;
            Size buttonSize = new Size(buttonsWidth, buttonsheight);
            int height = namesHeight + (r_SettingsForm.RowsSize * (buttonsheight + space - 1)) + (2 * margin);
            int width = (r_SettingsForm.ColsSize * (buttonsWidth + space + 1)) + (2 * margin);
            this.ClientSize = new Size(width, height);
            int index = 0;

            for (int i = 0; i < r_GameLogic.Board.GetLength(0); i++)
            {
                for (int j = 0; j < r_GameLogic.Board.GetLength(1); j++)
                {
                    ButtonCell button = new ButtonCell(index, i, j);
                    this.Controls.Add(button);
                    button.Click += button_Click;
                    m_ButtonsCells[i, j] = button;
                    index++;
                    m_ButtonsCells[i, j].Size = buttonSize;

                    if (j == 0)
                    {
                        button.Location = new Point(XCordinate, YCordinate);
                        YCordinate += buttonsheight + space;
                    }
                    else
                    {
                        button.Location = new Point(m_ButtonsCells[i, j - 1].Location.X + buttonsWidth + space, m_ButtonsCells[i, j - 1].Location.Y);
                    }
                }
            }
        }

        private void initialCellsColor()
        {
            for (int i = 0; i < m_CellsColor.GetLength(0); i++)
			{
                for (int j = 0; j < m_CellsColor.GetLength(1); j++)
                {
                    m_CellsColor[i, j] = Color.Gray;
                    m_ButtonsCells[i, j].BackColor = m_CellsColor[i, j];
                }
            }
        }

        private void InitializeComponent()
        {
            this.m_FirstPlayerNameLabel = new System.Windows.Forms.Label();
            this.m_SecondPlayerNameLabel = new System.Windows.Forms.Label();
            this.m_CurrentPlayerNameLabel = new System.Windows.Forms.Label();
            this.m_FirstPlayerScoreLabel = new System.Windows.Forms.Label();
            this.m_SecondPlayerScoreLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.m_FirstPlayerNameLabel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_FirstPlayerNameLabel.AutoSize = true;
            this.m_FirstPlayerNameLabel.Location = new System.Drawing.Point(23, 325);
            this.m_FirstPlayerNameLabel.Name = "m_FirstPlayerNameLabel";
            this.m_FirstPlayerNameLabel.Size = new System.Drawing.Size(63, 13);
            this.m_FirstPlayerNameLabel.TabIndex = 0;
            this.m_FirstPlayerNameLabel.Text = "First player: ";
            this.m_SecondPlayerNameLabel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_SecondPlayerNameLabel.AutoSize = true;
            this.m_SecondPlayerNameLabel.Location = new System.Drawing.Point(23, 353);
            this.m_SecondPlayerNameLabel.Name = "m_SecondPlayerNameLabel";
            this.m_SecondPlayerNameLabel.Size = new System.Drawing.Size(78, 13);
            this.m_SecondPlayerNameLabel.TabIndex = 2;
            this.m_SecondPlayerNameLabel.Text = "Second player:";
            this.m_CurrentPlayerNameLabel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_CurrentPlayerNameLabel.AutoSize = true;
            this.m_CurrentPlayerNameLabel.Location = new System.Drawing.Point(23, 296);
            this.m_CurrentPlayerNameLabel.Name = "m_CurrentPlayerNameLabel";
            this.m_CurrentPlayerNameLabel.Size = new System.Drawing.Size(78, 13);
            this.m_CurrentPlayerNameLabel.TabIndex = 3;
            this.m_CurrentPlayerNameLabel.Text = "Current player: ";
            this.m_FirstPlayerScoreLabel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_FirstPlayerScoreLabel.AutoSize = true;
            this.m_FirstPlayerScoreLabel.Location = new System.Drawing.Point(107, 325);
            this.m_FirstPlayerScoreLabel.Name = "m_FirstPlayerScoreLabel";
            this.m_FirstPlayerScoreLabel.Size = new System.Drawing.Size(78, 13);
            this.m_FirstPlayerScoreLabel.TabIndex = 4;
            this.m_FirstPlayerScoreLabel.Text = "Current player: ";
            this.m_SecondPlayerScoreLabel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_SecondPlayerScoreLabel.AutoSize = true;
            this.m_SecondPlayerScoreLabel.Location = new System.Drawing.Point(107, 353);
            this.m_SecondPlayerScoreLabel.Name = "m_SecondPlayerScoreLabel";
            this.m_SecondPlayerScoreLabel.Size = new System.Drawing.Size(78, 13);
            this.m_SecondPlayerScoreLabel.TabIndex = 5;
            this.m_SecondPlayerScoreLabel.Text = "Current player: ";
            this.ClientSize = new System.Drawing.Size(603, 407);
            this.Controls.Add(this.m_SecondPlayerScoreLabel);
            this.Controls.Add(this.m_FirstPlayerScoreLabel);
            this.Controls.Add(this.m_CurrentPlayerNameLabel);
            this.Controls.Add(this.m_SecondPlayerNameLabel);
            this.Controls.Add(this.m_FirstPlayerNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GameUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Memory Game";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
