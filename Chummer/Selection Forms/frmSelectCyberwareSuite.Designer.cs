namespace Chummer
{
    public sealed partial class frmSelectCyberwareSuite
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lstCyberware = new System.Windows.Forms.ListBox();
            this.lblGradeLabel = new System.Windows.Forms.Label();
            this.lblGrade = new System.Windows.Forms.Label();
            this.lblEssence = new System.Windows.Forms.Label();
            this.lblEssenceLabel = new System.Windows.Forms.Label();
            this.lblCost = new System.Windows.Forms.Label();
            this.lblCostLabel = new System.Windows.Forms.Label();
            this.lblCyberwareLabel = new System.Windows.Forms.Label();
            this.lblCyberware = new System.Windows.Forms.Label();
            this.tlpMain = new Chummer.BufferedTableLayoutPanel(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tlpMain.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.AutoSize = true;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(0, 0);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 10;
            this.cmdCancel.Tag = "String_Cancel";
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.AutoSize = true;
            this.cmdOK.Location = new System.Drawing.Point(81, 0);
            this.cmdOK.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 9;
            this.cmdOK.Tag = "String_OK";
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lstCyberware
            // 
            this.lstCyberware.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCyberware.FormattingEnabled = true;
            this.lstCyberware.Location = new System.Drawing.Point(3, 3);
            this.lstCyberware.Name = "lstCyberware";
            this.tlpMain.SetRowSpan(this.lstCyberware, 6);
            this.lstCyberware.Size = new System.Drawing.Size(295, 417);
            this.lstCyberware.Sorted = true;
            this.lstCyberware.TabIndex = 0;
            this.lstCyberware.SelectedIndexChanged += new System.EventHandler(this.lstCyberware_SelectedIndexChanged);
            this.lstCyberware.DoubleClick += new System.EventHandler(this.lstCyberware_DoubleClick);
            // 
            // lblGradeLabel
            // 
            this.lblGradeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGradeLabel.AutoSize = true;
            this.lblGradeLabel.Location = new System.Drawing.Point(320, 6);
            this.lblGradeLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblGradeLabel.Name = "lblGradeLabel";
            this.lblGradeLabel.Size = new System.Drawing.Size(39, 13);
            this.lblGradeLabel.TabIndex = 1;
            this.lblGradeLabel.Tag = "Label_Grade";
            this.lblGradeLabel.Text = "Grade:";
            // 
            // lblGrade
            // 
            this.lblGrade.AutoSize = true;
            this.lblGrade.Location = new System.Drawing.Point(365, 6);
            this.lblGrade.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblGrade.Name = "lblGrade";
            this.lblGrade.Size = new System.Drawing.Size(42, 13);
            this.lblGrade.TabIndex = 2;
            this.lblGrade.Text = "[Grade]";
            // 
            // lblEssence
            // 
            this.lblEssence.AutoSize = true;
            this.lblEssence.Location = new System.Drawing.Point(365, 31);
            this.lblEssence.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblEssence.Name = "lblEssence";
            this.lblEssence.Size = new System.Drawing.Size(54, 13);
            this.lblEssence.TabIndex = 4;
            this.lblEssence.Text = "[Essence]";
            // 
            // lblEssenceLabel
            // 
            this.lblEssenceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEssenceLabel.AutoSize = true;
            this.lblEssenceLabel.Location = new System.Drawing.Point(308, 31);
            this.lblEssenceLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblEssenceLabel.Name = "lblEssenceLabel";
            this.lblEssenceLabel.Size = new System.Drawing.Size(51, 13);
            this.lblEssenceLabel.TabIndex = 3;
            this.lblEssenceLabel.Tag = "Label_Essence";
            this.lblEssenceLabel.Text = "Essence:";
            // 
            // lblCost
            // 
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(365, 56);
            this.lblCost.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(34, 13);
            this.lblCost.TabIndex = 6;
            this.lblCost.Text = "[Cost]";
            // 
            // lblCostLabel
            // 
            this.lblCostLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCostLabel.AutoSize = true;
            this.lblCostLabel.Location = new System.Drawing.Point(328, 56);
            this.lblCostLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblCostLabel.Name = "lblCostLabel";
            this.lblCostLabel.Size = new System.Drawing.Size(31, 13);
            this.lblCostLabel.TabIndex = 5;
            this.lblCostLabel.Tag = "Label_Cost";
            this.lblCostLabel.Text = "Cost:";
            // 
            // lblCyberwareLabel
            // 
            this.lblCyberwareLabel.AutoSize = true;
            this.tlpMain.SetColumnSpan(this.lblCyberwareLabel, 2);
            this.lblCyberwareLabel.Location = new System.Drawing.Point(304, 81);
            this.lblCyberwareLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblCyberwareLabel.Name = "lblCyberwareLabel";
            this.lblCyberwareLabel.Size = new System.Drawing.Size(144, 13);
            this.lblCyberwareLabel.TabIndex = 7;
            this.lblCyberwareLabel.Tag = "Label_SelectCyberwareSuite_PartsInSuite";
            this.lblCyberwareLabel.Text = "Parts in this Cyberware Suite:";
            // 
            // lblCyberware
            // 
            this.lblCyberware.AutoSize = true;
            this.tlpMain.SetColumnSpan(this.lblCyberware, 2);
            this.lblCyberware.Location = new System.Drawing.Point(304, 106);
            this.lblCyberware.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblCyberware.Name = "lblCyberware";
            this.lblCyberware.Size = new System.Drawing.Size(63, 13);
            this.lblCyberware.TabIndex = 8;
            this.lblCyberware.Text = "[Cyberware]";
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpMain.Controls.Add(this.lstCyberware, 0, 0);
            this.tlpMain.Controls.Add(this.lblGradeLabel, 1, 0);
            this.tlpMain.Controls.Add(this.lblCyberware, 1, 4);
            this.tlpMain.Controls.Add(this.lblGrade, 2, 0);
            this.tlpMain.Controls.Add(this.lblCyberwareLabel, 1, 3);
            this.tlpMain.Controls.Add(this.lblEssenceLabel, 1, 1);
            this.tlpMain.Controls.Add(this.lblCost, 2, 2);
            this.tlpMain.Controls.Add(this.lblEssence, 2, 1);
            this.tlpMain.Controls.Add(this.lblCostLabel, 1, 2);
            this.tlpMain.Controls.Add(this.flowLayoutPanel1, 1, 5);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(9, 9);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 6;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(606, 423);
            this.tlpMain.TabIndex = 11;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.tlpMain.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.cmdOK);
            this.flowLayoutPanel1.Controls.Add(this.cmdCancel);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(447, 397);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(156, 23);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // frmSelectCyberwareSuite
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectCyberwareSuite";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "Title_SelectCyberwareSuite";
            this.Text = "Select a Cyberware Suite";
            this.Load += new System.EventHandler(this.frmSelectCyberwareSuite_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ListBox lstCyberware;
        private System.Windows.Forms.Label lblGradeLabel;
        private System.Windows.Forms.Label lblGrade;
        private System.Windows.Forms.Label lblEssence;
        private System.Windows.Forms.Label lblEssenceLabel;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.Label lblCostLabel;
        private System.Windows.Forms.Label lblCyberwareLabel;
        private System.Windows.Forms.Label lblCyberware;
        private Chummer.BufferedTableLayoutPanel tlpMain;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
