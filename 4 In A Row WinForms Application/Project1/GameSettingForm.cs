namespace B16_Ex05
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    public class GameSettingForm : Form
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private CheckBox m_Player2CheckBox;
        private TextBox m_Player1TextBox;
        private TextBox m_Player2TextBox;
        private Label label;
        private Label label5;
        private Label label6;
        private NumericUpDown m_RowsNumericUpDown;
        private NumericUpDown m_ColsNumericUpDown;
        private Button buttonStart;

        public GameSettingForm()
        {
            InitializeComponent();
        }

        public string Player1Name
        {
            get
            {
                return m_Player1TextBox.Text;
            }           
        }

        public int Rows
        {
            get
            {
                return (int)m_RowsNumericUpDown.Value;
            }
        }

        public int Cols
        {
            get
            {
                return (int)m_ColsNumericUpDown.Value;
            }
        }

        public bool IsComputerPlayer
        {
            get
            {
                return !m_Player2CheckBox.Checked;
            }
        }

        public string Player2Name
        {
            get
            {
                return m_Player2TextBox.Text;
            }
        }

        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_Player2CheckBox = new System.Windows.Forms.CheckBox();
            this.m_Player1TextBox = new System.Windows.Forms.TextBox();
            this.m_Player2TextBox = new System.Windows.Forms.TextBox();
            this.label = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_RowsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.m_ColsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.m_RowsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_ColsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            this.buttonStart.Location = new System.Drawing.Point(48, 216);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(230, 32);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Strart!";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Players:";
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(58, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Player2:";
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(35, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Player1:";
            this.m_Player2CheckBox.AutoSize = true;
            this.m_Player2CheckBox.Location = new System.Drawing.Point(39, 99);
            this.m_Player2CheckBox.Name = "m_Player2CheckBox";
            this.m_Player2CheckBox.Size = new System.Drawing.Size(15, 14);
            this.m_Player2CheckBox.TabIndex = 4;
            this.m_Player2CheckBox.UseVisualStyleBackColor = true;
            this.m_Player2CheckBox.CheckedChanged += new System.EventHandler(this.Player2CheckBox_CheckedChanged);
            this.m_Player1TextBox.Location = new System.Drawing.Point(131, 54);
            this.m_Player1TextBox.Name = "m_Player1TextBox";
            this.m_Player1TextBox.Size = new System.Drawing.Size(147, 26);
            this.m_Player1TextBox.TabIndex = 5;
            this.m_Player2TextBox.Location = new System.Drawing.Point(131, 95);
            this.m_Player2TextBox.Name = "m_Player2TextBox";
            this.m_Player2TextBox.ReadOnly = true;
            this.m_Player2TextBox.Size = new System.Drawing.Size(147, 26);
            this.m_Player2TextBox.TabIndex = 6;
            this.m_Player2TextBox.Text = "Computer";
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label.Location = new System.Drawing.Point(44, 139);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(87, 20);
            this.label.TabIndex = 7;
            this.label.Tag = "1";
            this.label.Text = "BoardSize:";
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label5.Location = new System.Drawing.Point(47, 171);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Rows:";
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.Location = new System.Drawing.Point(177, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "Cols:";
            this.m_RowsNumericUpDown.Location = new System.Drawing.Point(107, 170);
            this.m_RowsNumericUpDown.Name = "m_RowsNumericUpDown";
            this.m_RowsNumericUpDown.Size = new System.Drawing.Size(44, 26);
            this.m_RowsNumericUpDown.TabIndex = 10;
            this.m_RowsNumericUpDown.Maximum = 10;
            this.m_RowsNumericUpDown.Minimum = 4;
            this.m_RowsNumericUpDown.Value = new decimal(new int[] 
            {
            4,
            0,
            0,
            0
            });
            this.m_ColsNumericUpDown.Location = new System.Drawing.Point(227, 171);
            this.m_ColsNumericUpDown.Name = "m_ColsNumericUpDown";
            this.m_ColsNumericUpDown.Size = new System.Drawing.Size(41, 26);
            this.m_ColsNumericUpDown.TabIndex = 11;
            this.m_ColsNumericUpDown.Maximum = 10;
            this.m_ColsNumericUpDown.Minimum = 4;
            this.m_ColsNumericUpDown.Value = new decimal(new int[] 
            {
            4,
            0,
            0,
            0
            });
            this.ClientSize = new System.Drawing.Size(334, 260);
            this.Controls.Add(this.m_ColsNumericUpDown);
            this.Controls.Add(this.m_RowsNumericUpDown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label);
            this.Controls.Add(this.m_Player2TextBox);
            this.Controls.Add(this.m_Player1TextBox);
            this.Controls.Add(this.m_Player2CheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStart);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameSettingForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "4 in a row";
            ((System.ComponentModel.ISupportInitialize)(this.m_RowsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_ColsNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
                this.DialogResult = DialogResult.OK;
                this.Close();         
        }

        private void Player2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_Player2CheckBox.Checked == true)
            {
                this.m_Player2TextBox.ReadOnly = false;
                this.m_Player2TextBox.Text = string.Empty;
            }
            else
            {
                this.m_Player2TextBox.ReadOnly = true;
                this.m_Player2TextBox.Text = "Computer";
            }
        }
    }
}
