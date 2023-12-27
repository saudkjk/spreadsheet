
namespace SpreadsheetGUI
{
    partial class Form1
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
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.contentsBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cellNameBox = new System.Windows.Forms.TextBox();
            this.cellValueBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeSelectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCellContentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.additionalFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip3 = new System.Windows.Forms.MenuStrip();
            this.saveDOTFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dependeesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSelectedCellDependeesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSelectedCellDependentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.menuStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 95);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(691, 335);
            this.spreadsheetPanel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(165, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 37);
            this.button1.TabIndex = 1;
            this.button1.Text = "Change contents";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contentsBox
            // 
            this.contentsBox.Location = new System.Drawing.Point(165, 51);
            this.contentsBox.Name = "contentsBox";
            this.contentsBox.Size = new System.Drawing.Size(140, 22);
            this.contentsBox.TabIndex = 2;
            this.contentsBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox1_PreviewKeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(358, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Selected cell name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(532, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Selected cell value";
            // 
            // cellNameBox
            // 
            this.cellNameBox.Location = new System.Drawing.Point(361, 38);
            this.cellNameBox.Name = "cellNameBox";
            this.cellNameBox.ReadOnly = true;
            this.cellNameBox.Size = new System.Drawing.Size(124, 22);
            this.cellNameBox.TabIndex = 6;
            // 
            // cellValueBox
            // 
            this.cellValueBox.Location = new System.Drawing.Point(535, 38);
            this.cellValueBox.Name = "cellValueBox";
            this.cellValueBox.ReadOnly = true;
            this.cellValueBox.Size = new System.Drawing.Size(123, 22);
            this.cellValueBox.TabIndex = 7;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 56);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(703, 28);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 28);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(703, 28);
            this.menuStrip2.TabIndex = 9;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeSelectionsToolStripMenuItem,
            this.editCellContentsToolStripMenuItem,
            this.additionalFeatureToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // changeSelectionsToolStripMenuItem
            // 
            this.changeSelectionsToolStripMenuItem.Name = "changeSelectionsToolStripMenuItem";
            this.changeSelectionsToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.changeSelectionsToolStripMenuItem.Text = "Change selections?";
            this.changeSelectionsToolStripMenuItem.Click += new System.EventHandler(this.changeSelectionsToolStripMenuItem_Click);
            // 
            // editCellContentsToolStripMenuItem
            // 
            this.editCellContentsToolStripMenuItem.Name = "editCellContentsToolStripMenuItem";
            this.editCellContentsToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.editCellContentsToolStripMenuItem.Text = "Edit cell contents?";
            this.editCellContentsToolStripMenuItem.Click += new System.EventHandler(this.editCellContentsToolStripMenuItem_Click);
            // 
            // additionalFeatureToolStripMenuItem
            // 
            this.additionalFeatureToolStripMenuItem.Name = "additionalFeatureToolStripMenuItem";
            this.additionalFeatureToolStripMenuItem.Size = new System.Drawing.Size(245, 26);
            this.additionalFeatureToolStripMenuItem.Text = "How to save DOT files?";
            this.additionalFeatureToolStripMenuItem.Click += new System.EventHandler(this.additionalFeatureToolStripMenuItem_Click);
            // 
            // menuStrip3
            // 
            this.menuStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveDOTFilesToolStripMenuItem});
            this.menuStrip3.Location = new System.Drawing.Point(0, 0);
            this.menuStrip3.Name = "menuStrip3";
            this.menuStrip3.Size = new System.Drawing.Size(703, 28);
            this.menuStrip3.TabIndex = 10;
            this.menuStrip3.Text = "menuStrip3";
            // 
            // saveDOTFilesToolStripMenuItem
            // 
            this.saveDOTFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dependeesToolStripMenuItem,
            this.saToolStripMenuItem,
            this.saveSelectedCellDependeesToolStripMenuItem,
            this.saveSelectedCellDependentsToolStripMenuItem});
            this.saveDOTFilesToolStripMenuItem.Name = "saveDOTFilesToolStripMenuItem";
            this.saveDOTFilesToolStripMenuItem.Size = new System.Drawing.Size(118, 24);
            this.saveDOTFilesToolStripMenuItem.Text = "Save DOT files";
            // 
            // dependeesToolStripMenuItem
            // 
            this.dependeesToolStripMenuItem.Name = "dependeesToolStripMenuItem";
            this.dependeesToolStripMenuItem.Size = new System.Drawing.Size(291, 26);
            this.dependeesToolStripMenuItem.Text = "Save all dependees";
            this.dependeesToolStripMenuItem.Click += new System.EventHandler(this.dependeesToolStripMenuItem_Click);
            // 
            // saToolStripMenuItem
            // 
            this.saToolStripMenuItem.Name = "saToolStripMenuItem";
            this.saToolStripMenuItem.Size = new System.Drawing.Size(291, 26);
            this.saToolStripMenuItem.Text = "Save all dependents";
            this.saToolStripMenuItem.Click += new System.EventHandler(this.dependentsToolStripMenuItem_Click);
            // 
            // saveSelectedCellDependeesToolStripMenuItem
            // 
            this.saveSelectedCellDependeesToolStripMenuItem.Name = "saveSelectedCellDependeesToolStripMenuItem";
            this.saveSelectedCellDependeesToolStripMenuItem.Size = new System.Drawing.Size(291, 26);
            this.saveSelectedCellDependeesToolStripMenuItem.Text = "Save selected cell dependees";
            this.saveSelectedCellDependeesToolStripMenuItem.Click += new System.EventHandler(this.saveSelectedCellDependeesToolStripMenuItem_Click);
            // 
            // saveSelectedCellDependentsToolStripMenuItem
            // 
            this.saveSelectedCellDependentsToolStripMenuItem.Name = "saveSelectedCellDependentsToolStripMenuItem";
            this.saveSelectedCellDependentsToolStripMenuItem.Size = new System.Drawing.Size(291, 26);
            this.saveSelectedCellDependentsToolStripMenuItem.Text = "Save selected cell dependents";
            this.saveSelectedCellDependentsToolStripMenuItem.Click += new System.EventHandler(this.saveSelectedCellDependentsToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(703, 433);
            this.Controls.Add(this.cellValueBox);
            this.Controls.Add(this.cellNameBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.contentsBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.menuStrip3);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.menuStrip3.ResumeLayout(false);
            this.menuStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox contentsBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox cellNameBox;
        private System.Windows.Forms.TextBox cellValueBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeSelectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editCellContentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem additionalFeatureToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.MenuStrip menuStrip3;
        private System.Windows.Forms.ToolStripMenuItem saveDOTFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dependeesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSelectedCellDependeesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSelectedCellDependentsToolStripMenuItem;
    }
}

