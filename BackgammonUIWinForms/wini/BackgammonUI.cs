using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using BackgammonLogic;

namespace Backgammon
{
    class BackgammonUI : Form
    {
        private Game _game = new Game();
        private TableLayoutPanel _table;
        private Label _checker2 = new Label();
        private TextBox _moveTextBox;
        private Label _moveLabel;
        private PictureBox _firstDie;
        private PictureBox _secondDie;
        private Label _checker1 = new Label();
        private Player _player1 = new Player(Cell.X);
        private Player _player2 = new Player(Cell.O);
        private TextBox _scoreTextBox;
        private bool _endOfFirstPlayerTurn;
        private bool _endOfSecondPlayerTurn;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label18;
        private Label label19;
        private Label label20;
        private Label label11;
        private Label label21;
        private Label label22;
        private Label label23;
        private Label label24;
        private Label label10;
        private Label label17;
        private bool _endOfGame;
        private PictureBox _compDie1;
        private TextBox _compText;
        private bool _isRooledAllreadyFirstPlayer;
        private bool _isRooledAllreadySecondPlayer;
        private PictureBox _compDie2;
        private TextBox _exceptionInfo;
        private DialogResult _rematch = DialogResult.No;
        private Image[] _images = new Image[6];
        private MainMenuForm _mainMenuForm = new MainMenuForm();

        public BackgammonUI()
        {
            InitializeComponent();
            _images[0] = Properties.Resources.Dice_1;
            _images[1] = Properties.Resources.Dice_2;
            _images[2] = Properties.Resources.Dice_3;
            _images[3] = Properties.Resources.Dice_4;
            _images[4] = Properties.Resources.Dice_5;
            _images[5] = Properties.Resources.Dice_6;
            _compDie1.Visible = false;
            _compDie2.Visible = false;
            _compText.Visible = false;
            _checker2.BackColor = Color.Blue;
            _checker1.BackColor = Color.Red;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(new Rectangle(0, 0, 20, 20));
            _checker2.Region = new Region(path);
            _checker1.Region = new Region(path);
            _game.Computer.Tool = Cell.O;
            _game.Computer.Name = "Watson";
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, _table, new object[] { true });
            PrintAllBoard();
        }

        public void MainMenu()
        {
            _mainMenuForm.ShowDialog();
            do
            {
                if (_mainMenuForm.IsOkButtonPressed)
                {
                    _player1.Name = _mainMenuForm.FirstPlayerName;
                    if (_mainMenuForm.OnePlayer)
                    {
                        _player2 = _game.Computer;
                        SinglePlayer();
                    }
                    else
                    {
                        _player2.Name = _mainMenuForm.SecondPlayerName;
                        TwoPlayers();
                    }

                    ShowDialog();
                }
            } while (_rematch == DialogResult.Yes);
        }

        private void SinglePlayer()
        {
            if (!_endOfGame && !_endOfFirstPlayerTurn)
            {
                Play(_player1, ref _endOfFirstPlayerTurn);
                IfWinShowMsg(_player1);
            }
            if (!_endOfGame && _endOfFirstPlayerTurn)
            {
                _compText.Text = $"{_player2.Name} Turn... ";
                ComputerPlay();
                IfWinShowMsg(_player2);
                _isRooledAllreadyFirstPlayer = false;
            }
            if(!_endOfGame && !_isRooledAllreadyFirstPlayer)
            {
                RollTheDice(_player1, ref _endOfFirstPlayerTurn);
                _isRooledAllreadyFirstPlayer = true;
            }

            PrintAllBoard();
        }

        private void RollTheDice(Player player,ref bool endOfTurn)
        {
            _moveLabel.Text = $"{player.Name} Turn: ";
            endOfTurn = !_game.PlayerRollTheDice(player);
            SetPlayerDiceImage();
            PrintScoring();
        }

        private void IfWinShowMsg(Player player)
        {
            if (_game.PlayerIsWin(player))
            {
                _game.InitializeGame();
                PrintAllBoard();
                PrintScoring();
                _rematch = MessageBox.Show(
$"{player.Name} won!! {Environment.NewLine} Do you want a rematch?", "Win!!!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(_rematch == DialogResult.No)
                {
                    Close();
                }
                else
                {
                    _endOfSecondPlayerTurn = false;
                    _endOfFirstPlayerTurn = false;
                    _isRooledAllreadyFirstPlayer = false;
                    _isRooledAllreadySecondPlayer = false;
                }
            }
        }

        private void TwoPlayers()
        {
                if (!_endOfGame && !_endOfFirstPlayerTurn)
                {
                    Play(_player1, ref _endOfFirstPlayerTurn);
                    IfWinShowMsg(_player1);
                }
                else if (!_endOfGame)
                {
                    Play(_player2, ref _endOfSecondPlayerTurn);
                    IfWinShowMsg(_player2);
                    _isRooledAllreadySecondPlayer = true;

                    if (_endOfSecondPlayerTurn)
                    {
                        _endOfFirstPlayerTurn = false; 
                        _isRooledAllreadyFirstPlayer = false;
                    }
                }
            if (!_endOfGame && _endOfFirstPlayerTurn && !_isRooledAllreadySecondPlayer)
            {
                RollTheDice(_player2, ref _endOfSecondPlayerTurn);
            }
            else if (!_endOfGame && !_isRooledAllreadyFirstPlayer)
            {
                RollTheDice(_player1, ref _endOfFirstPlayerTurn);
                _isRooledAllreadyFirstPlayer = true;
                _isRooledAllreadySecondPlayer = false;
            }

            PrintAllBoard();
        }

        private void ComputerPlay()
        {
            bool endOfTurn = false;
            if (_game.PlayerRollTheDice(_game.Computer))
            {
                SetComputerDice();
                while (!endOfTurn)
                {
                    endOfTurn = _game.ComputerNextMove();
                }
            }
            else
            {
                SetComputerDice();
                _exceptionInfo.Text = $"{_game.Computer.Name} don't have a valid move, turn is pass... ";
                _exceptionInfo.Visible = true;
            }
        }

        private void SetPlayerDiceImage()
        {
            _firstDie.Image = _images[int.Parse(_game.PlayerDice().Substring(1, 1)) - 1];
            _secondDie.Image = _images[int.Parse(_game.PlayerDice().Substring(5, 1)) - 1];
        }

        private void SetComputerDice()
        {
            _compDie1.Visible = true;
            _compDie2.Visible = true;
            _compText.Visible = true;
            _compDie1.Image = _images[int.Parse(_game.PlayerDice().Substring(1, 1)) - 1];
            _compDie2.Image = _images[int.Parse(_game.PlayerDice().Substring(5, 1)) - 1];
        }

        private void Play(Player player,ref bool endOfTurn)
        {
            string[] data;
            bool validInput = false;
            int firstArg = 0, secondArg = 0;
            data = _moveTextBox.Text.Split(',');
            _exceptionInfo.Visible = false;
            if (endOfTurn)
            {
                _exceptionInfo.Text = $"{player.Name} don't have a valid move, turn is pass... ";
                _exceptionInfo.Visible = true;
            }
            else if (data.Length == 2)
            {
                validInput = int.TryParse(data[0], out firstArg)
                    && int.TryParse(data[1], out secondArg);
                if (validInput)
                {
                    endOfTurn = _game.PlayerNextMove(player, firstArg, secondArg, false);
                }
            }
            else if (data.Length == 1)
            {
                if (data[0].ToLower() == "q")
                {
                    validInput = true;
                    endOfTurn = true;
                }
                else
                {
                    validInput = int.TryParse(data[0], out firstArg);
                    if (validInput)
                    {
                        endOfTurn = _game.PlayerNextMove(player, firstArg, false);
                    }
                }
            }

            _moveTextBox.Text = string.Empty;
        }

        private void PrintScoring()
        {
            _scoreTextBox.Text = string.Format(
@"Scoring
{0} : {1}
{2} : {3}", _player1.Name, _player1.AmountOfWins,
_player2.Name, _player2.AmountOfWins);
        }

        private void PrintAllBoard()
        {
            CleanTable();
            PrintEatenZone();
            PrintUpBoard(6);
            PrintDownBoard(6);
        }

        private void CleanTable()
        {
            for (int i = 1; i < _table.RowCount - 1; i++)
            {
                for (int j = 1; j < _table.ColumnCount - 1; j++)
                {
                    _table.Controls.Remove(_table.GetControlFromPosition(j, i));
                }
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackgammonUI));
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this._moveTextBox = new System.Windows.Forms.TextBox();
            this._moveLabel = new System.Windows.Forms.Label();
            this._firstDie = new System.Windows.Forms.PictureBox();
            this._secondDie = new System.Windows.Forms.PictureBox();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this.label17 = new System.Windows.Forms.Label();
            this._compDie1 = new System.Windows.Forms.PictureBox();
            this._scoreTextBox = new System.Windows.Forms.TextBox();
            this._compText = new System.Windows.Forms.TextBox();
            this._compDie2 = new System.Windows.Forms.PictureBox();
            this._exceptionInfo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this._firstDie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._secondDie)).BeginInit();
            this._table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._compDie1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._compDie2)).BeginInit();
            this.SuspendLayout();
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(98, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 30);
            this.label16.TabIndex = 15;
            this.label16.Text = "23";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(138, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(34, 30);
            this.label15.TabIndex = 14;
            this.label15.Text = "22";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(178, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 30);
            this.label14.TabIndex = 13;
            this.label14.Text = "21";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(213, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 30);
            this.label13.TabIndex = 12;
            this.label13.Text = "20";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(255, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 30);
            this.label12.TabIndex = 11;
            this.label12.Text = "19";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(384, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 30);
            this.label9.TabIndex = 8;
            this.label9.Text = "17";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(423, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 30);
            this.label8.TabIndex = 7;
            this.label8.Text = "16";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(463, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 30);
            this.label7.TabIndex = 6;
            this.label7.Text = "15";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(503, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 30);
            this.label6.TabIndex = 5;
            this.label6.Text = "14";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(542, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 30);
            this.label5.TabIndex = 4;
            this.label5.Text = "13";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(57, 390);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(98, 390);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(138, 390);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "3";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(178, 390);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 30);
            this.label4.TabIndex = 3;
            this.label4.Text = "4";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(213, 390);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(36, 30);
            this.label18.TabIndex = 17;
            this.label18.Text = "5";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(255, 390);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(45, 30);
            this.label19.TabIndex = 18;
            this.label19.Text = "6";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(348, 390);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(30, 30);
            this.label20.TabIndex = 19;
            this.label20.Text = "7";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(384, 390);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 30);
            this.label11.TabIndex = 10;
            this.label11.Text = "8";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(423, 390);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(34, 30);
            this.label21.TabIndex = 20;
            this.label21.Text = "9";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(463, 390);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(34, 30);
            this.label22.TabIndex = 21;
            this.label22.Text = "10";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(503, 390);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(33, 30);
            this.label23.TabIndex = 22;
            this.label23.Text = "11";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(542, 390);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(33, 30);
            this.label24.TabIndex = 23;
            this.label24.Text = "12";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(348, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 30);
            this.label10.TabIndex = 9;
            this.label10.Text = "18";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _moveTextBox
            // 
            this._moveTextBox.BackColor = System.Drawing.Color.Chocolate;
            this._moveTextBox.ForeColor = System.Drawing.SystemColors.Info;
            this._moveTextBox.Location = new System.Drawing.Point(590, 46);
            this._moveTextBox.Name = "_moveTextBox";
            this._moveTextBox.Size = new System.Drawing.Size(87, 20);
            this._moveTextBox.TabIndex = 2;
            this._moveTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._moveTextBox_KeyDown);
            // 
            // _moveLabel
            // 
            this._moveLabel.BackColor = System.Drawing.Color.Chocolate;
            this._moveLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._moveLabel.Cursor = System.Windows.Forms.Cursors.No;
            this._moveLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this._moveLabel.Location = new System.Drawing.Point(590, 22);
            this._moveLabel.Name = "_moveLabel";
            this._moveLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._moveLabel.Size = new System.Drawing.Size(87, 21);
            this._moveLabel.TabIndex = 3;
            this._moveLabel.Text = "Welcom:";
            this._moveLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _firstDie
            // 
            this._firstDie.Image = ((System.Drawing.Image)(resources.GetObject("_firstDie.Image")));
            this._firstDie.Location = new System.Drawing.Point(593, 177);
            this._firstDie.Name = "_firstDie";
            this._firstDie.Size = new System.Drawing.Size(39, 41);
            this._firstDie.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._firstDie.TabIndex = 0;
            this._firstDie.TabStop = false;
            // 
            // _secondDie
            // 
            this._secondDie.Image = ((System.Drawing.Image)(resources.GetObject("_secondDie.Image")));
            this._secondDie.Location = new System.Drawing.Point(638, 177);
            this._secondDie.Name = "_secondDie";
            this._secondDie.Size = new System.Drawing.Size(38, 41);
            this._secondDie.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._secondDie.TabIndex = 1;
            this._secondDie.TabStop = false;
            // 
            // _table
            // 
            this._table.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this._table.BackColor = System.Drawing.Color.Transparent;
            this._table.BackgroundImage = Properties.Resources.Backgammon;
            this._table.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._table.ColumnCount = 15;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this._table.Controls.Add(this.label16, 12, 0);
            this._table.Controls.Add(this.label15, 11, 0);
            this._table.Controls.Add(this.label14, 10, 0);
            this._table.Controls.Add(this.label13, 9, 0);
            this._table.Controls.Add(this.label12, 8, 0);
            this._table.Controls.Add(this.label9, 5, 0);
            this._table.Controls.Add(this.label8, 4, 0);
            this._table.Controls.Add(this.label7, 3, 0);
            this._table.Controls.Add(this.label6, 2, 0);
            this._table.Controls.Add(this.label5, 1, 0);
            this._table.Controls.Add(this.label1, 13, 13);
            this._table.Controls.Add(this.label2, 12, 13);
            this._table.Controls.Add(this.label3, 11, 13);
            this._table.Controls.Add(this.label4, 10, 13);
            this._table.Controls.Add(this.label18, 9, 13);
            this._table.Controls.Add(this.label19, 8, 13);
            this._table.Controls.Add(this.label20, 6, 13);
            this._table.Controls.Add(this.label11, 5, 13);
            this._table.Controls.Add(this.label21, 4, 13);
            this._table.Controls.Add(this.label22, 3, 13);
            this._table.Controls.Add(this.label23, 2, 13);
            this._table.Controls.Add(this.label24, 1, 13);
            this._table.Controls.Add(this.label10, 6, 0);
            this._table.Controls.Add(this.label17, 13, 0);
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 14;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._table.Size = new System.Drawing.Size(632, 419);
            this._table.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(57, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 30);
            this.label17.TabIndex = 24;
            this.label17.Text = "24";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _compDie1
            // 
            this._compDie1.BackColor = System.Drawing.Color.Red;
            this._compDie1.Location = new System.Drawing.Point(638, 283);
            this._compDie1.Name = "_compDie1";
            this._compDie1.Size = new System.Drawing.Size(38, 37);
            this._compDie1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._compDie1.TabIndex = 6;
            this._compDie1.TabStop = false;
            // 
            // _scoreTextBox
            // 
            this._scoreTextBox.BackColor = System.Drawing.Color.Chocolate;
            this._scoreTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this._scoreTextBox.Location = new System.Drawing.Point(590, 89);
            this._scoreTextBox.Multiline = true;
            this._scoreTextBox.Name = "_scoreTextBox";
            this._scoreTextBox.ReadOnly = true;
            this._scoreTextBox.Size = new System.Drawing.Size(87, 68);
            this._scoreTextBox.TabIndex = 4;
            // 
            // _compText
            // 
            this._compText.BackColor = System.Drawing.Color.Red;
            this._compText.ForeColor = System.Drawing.Color.White;
            this._compText.Location = new System.Drawing.Point(593, 257);
            this._compText.Name = "_compText";
            this._compText.Size = new System.Drawing.Size(87, 20);
            this._compText.TabIndex = 6;
            this._compText.Text = "Computer Dice";
            this._compText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _compDie2
            // 
            this._compDie2.BackColor = System.Drawing.Color.Red;
            this._compDie2.Location = new System.Drawing.Point(593, 283);
            this._compDie2.Name = "_compDie2";
            this._compDie2.Size = new System.Drawing.Size(39, 37);
            this._compDie2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._compDie2.TabIndex = 7;
            this._compDie2.TabStop = false;
            // 
            // _exceptionInfo
            // 
            this._exceptionInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this._exceptionInfo.ForeColor = System.Drawing.Color.White;
            this._exceptionInfo.Location = new System.Drawing.Point(590, 357);
            this._exceptionInfo.Multiline = true;
            this._exceptionInfo.Name = "_exceptionInfo";
            this._exceptionInfo.Size = new System.Drawing.Size(86, 59);
            this._exceptionInfo.TabIndex = 8;
            this._exceptionInfo.Text = "arfaefafaef";
            this._exceptionInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._exceptionInfo.Visible = false;
            // 
            // Win
            // 
            this.BackColor = System.Drawing.Color.Chocolate;
            this.BackgroundImage = global::Backgammon.Properties.Resources.Backgammon;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(679, 419);
            this.Controls.Add(this._exceptionInfo);
            this.Controls.Add(this._compDie2);
            this.Controls.Add(this._compText);
            this.Controls.Add(this._scoreTextBox);
            this.Controls.Add(this._firstDie);
            this.Controls.Add(this._secondDie);
            this.Controls.Add(this._moveLabel);
            this.Controls.Add(this._moveTextBox);
            this.Controls.Add(this._table);
            this.Controls.Add(this._compDie1);
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Win";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            ((System.ComponentModel.ISupportInitialize)(this._firstDie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._secondDie)).EndInit();
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._compDie1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._compDie2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label Clone(Label checker)
        {
            var label = new Label();
            label.BackColor = checker.BackColor;
            label.Region = checker.Region;
            return label;
        }

        private void PrintDownBoard(int visibleSize)
        {
            for (int i = _game.Board.GetLength(0) - 1; i > _game.Board.GetLength(0) - visibleSize; i--)
            {
                for (int j = 0; j < (_game.Board.GetLength(1) / 2); j++)
                {
                    SetContent(i, j, i - 2, (_game.Board.GetLength(1) / 2) - j);
                }
            }
        }

        private void PrintUpBoard(int visibleSize)
        {
            for (int i = _game.Board.GetLength(0) - 1; i >= _game.Board.GetLength(0) - visibleSize; i--)
            {
                for (int j = _game.Board.GetLength(1) - 1; j > (_game.Board.GetLength(1) / 2) - 1; j--)
                {
                    SetContent(i, j, _game.Board.GetLength(0) - i, j - (_game.Board.GetLength(1) / 2) + 1);
                }
            }
        }

        private void PrintEatenZone()
        {
            for (int i = 0; i < _game.EatenTools.Count && i < _table.RowCount - 5; i++)
            {
                if (_game.EatenTools[i] == Cell.X)
                {
                    var label = Clone(_checker2);
                    label.Dock = DockStyle.Fill;
                    _table.Controls.Add(label, 7, i + 4);

                }
                else if (_game.EatenTools[i] == Cell.O)
                {
                    var label = Clone(_checker1);
                    label.Dock = DockStyle.Fill;
                    _table.Controls.Add(label, 7, i + 4);
                }
            }
        }

        private void SetContent(int i, int j, int setInRow, int setInCol)
        {
            setInCol = (setInCol >= 7 ? ++setInCol : setInCol);
            if (_game.Board[i, j] == Cell.X)
            {
                var label = Clone(_checker2);
                label.Dock = DockStyle.Fill;
                _table.Controls.Add(label, setInCol, setInRow);

            }
            else if (_game.Board[i, j] == Cell.O)
            {
                var label = Clone(_checker1);
                label.Dock = DockStyle.Fill;
                _table.Controls.Add(label, setInCol, setInRow);
            }
            else if(_table.GetControlFromPosition(setInCol, setInRow) != null)
            {
                _table.Controls.Remove(_table.GetControlFromPosition(setInCol, setInRow));
            }
        }

        private void _moveTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_mainMenuForm.OnePlayer)
                {
                    SinglePlayer();
                }
                else
                {
                    TwoPlayers();
                }
            }
        }
    }
}
