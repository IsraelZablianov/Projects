using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceCompare
{
    public class Report : Form
    {
        private Label label4;
        private Label label1;
        private ListBox _listOfStores;
        private TextBox _reportInformation;
        private Dictionary<string, string> _fullReportOfStores;

        public Report(Dictionary<string, string> FullReportOfStores)
        {
            _fullReportOfStores = FullReportOfStores;
            InitializeComponent();
            LoadStoresName();
        }

        private void LoadStoresName()
        {
            _listOfStores.Items.AddRange(_fullReportOfStores.Keys.ToArray());
        }

        private void InitializeComponent()
        {
            this._listOfStores = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._reportInformation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _listOfStores
            // 
            this._listOfStores.FormattingEnabled = true;
            this._listOfStores.Location = new System.Drawing.Point(25, 35);
            this._listOfStores.Name = "_listOfStores";
            this._listOfStores.Size = new System.Drawing.Size(219, 212);
            this._listOfStores.TabIndex = 0;
            this._listOfStores.SelectedIndexChanged += new System.EventHandler(this.ListOfStores_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(72, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "סניפים נבחרים להשוואה";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Harrington", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(329, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 14);
            this.label1.TabIndex = 15;
            this.label1.Text = "מחירים";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _reportInformation
            // 
            this._reportInformation.Location = new System.Drawing.Point(250, 35);
            this._reportInformation.Multiline = true;
            this._reportInformation.Name = "_reportInformation";
            this._reportInformation.ReadOnly = true;
            this._reportInformation.Size = new System.Drawing.Size(219, 211);
            this._reportInformation.TabIndex = 16;
            // 
            // Report
            // 
            this.BackgroundImage = global::FinalProjectPriceCompare.Properties.Resources.products;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(528, 348);
            this.Controls.Add(this._reportInformation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._listOfStores);
            this.ForeColor = System.Drawing.Color.DeepPink;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ListOfStores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if((sender as ListBox).SelectedItem != null)
            {
                _reportInformation.Text = string.Empty;
                _reportInformation.Text = _fullReportOfStores[(string)(sender as ListBox).SelectedItem];
            }
        }
    }
}
