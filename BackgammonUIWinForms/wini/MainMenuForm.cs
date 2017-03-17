using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backgammon
{
    class MainMenuForm : Form
    {
        private TextBox _secondPlayerName;
        private CheckBox checkBox1;
        private Label label1;
        private Label label2;
        private Button _cancelButton;
        private Button _okButton;
        private TextBox _firstPlayerName;
        private bool _isOkButtonPressed;
        private bool _onePlayer = true;

        public bool IsOkButtonPressed
        {
            get
            {
                return _isOkButtonPressed;
            }

            set
            {
                _isOkButtonPressed = value;
            }
        }

        public bool OnePlayer
        {
            get
            {
                return _onePlayer;
            }

            set
            {
                _onePlayer = value;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return _secondPlayerName.Text;
            }
        }

        public string FirstPlayerName
        {
            get
            {
                return _firstPlayerName.Text;
            }
        }

        public MainMenuForm()
        {
            InitializeComponent();
            CenterToParent();
        }

        private void InitializeComponent()
        {
            this._firstPlayerName = new System.Windows.Forms.TextBox();
            this._secondPlayerName = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _firstPlayerName
            // 
            this._firstPlayerName.Location = new System.Drawing.Point(107, 70);
            this._firstPlayerName.Name = "_firstPlayerName";
            this._firstPlayerName.Size = new System.Drawing.Size(100, 20);
            this._firstPlayerName.TabIndex = 0;
            this._firstPlayerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this._firstPlayerName_KeyDown);
            // 
            // _secondPlayerName
            // 
            this._secondPlayerName.Location = new System.Drawing.Point(145, 108);
            this._secondPlayerName.Name = "_secondPlayerName";
            this._secondPlayerName.ReadOnly = true;
            this._secondPlayerName.Size = new System.Drawing.Size(100, 20);
            this._secondPlayerName.TabIndex = 1;
            this._secondPlayerName.Text = "Computer";
            this._secondPlayerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this._secondPlayerName_KeyDown);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(124, 111);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "First Player Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Second Player Name";
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(107, 202);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 5;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(26, 202);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 6;
            this._okButton.Text = "Ok";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // MainMenuForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this._secondPlayerName);
            this.Controls.Add(this._firstPlayerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainMenuForm";
            this.Text = "Backgammon";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            IsOkButtonPressed = true;
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                _secondPlayerName.Text = string.Empty;
                _secondPlayerName.ReadOnly = false;
                _onePlayer = false;
            }
            else
            {
                _secondPlayerName.Text = "Computer";
                _secondPlayerName.ReadOnly = true;
                _onePlayer = true;
            }
        }

        private void _firstPlayerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OkButton_Click(null, null);
            }
        }

        private void _secondPlayerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OkButton_Click(null, null);
            }
        }
    }
}
