namespace NppDB.Core
{
    partial class FrmSQLResult
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSQLResult));
            this.tclSqlResult = new System.Windows.Forms.TabControl();
            this.tabMsg = new System.Windows.Forms.TabPage();
            this.txtMsg = new System.Windows.Forms.RichTextBox();
            this.tabResult = new System.Windows.Forms.TabPage();
            this.grdResult = new System.Windows.Forms.DataGridView();
            this.tabSQLStatement = new System.Windows.Forms.TabPage();
            this.txtSQL = new System.Windows.Forms.RichTextBox();
            this.tspMain = new System.Windows.Forms.ToolStrip();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.btnRollback = new System.Windows.Forms.ToolStripButton();
            this.btnCommit = new System.Windows.Forms.ToolStripButton();
            this.sep0 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbxTables = new System.Windows.Forms.ToolStripComboBox();
            this.lblDB = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblAccount = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lblConnect = new System.Windows.Forms.ToolStripLabel();
            this.lblError = new System.Windows.Forms.Label();
            this.tclSqlResult.SuspendLayout();
            this.tabMsg.SuspendLayout();
            this.tabResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResult)).BeginInit();
            this.tabSQLStatement.SuspendLayout();
            this.tspMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tclSqlResult
            // 
            this.tclSqlResult.Controls.Add(this.tabMsg);
            this.tclSqlResult.Controls.Add(this.tabResult);
            this.tclSqlResult.Controls.Add(this.tabSQLStatement);
            this.tclSqlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tclSqlResult.Location = new System.Drawing.Point(0, 25);
            this.tclSqlResult.Multiline = true;
            this.tclSqlResult.Name = "tclSqlResult";
            this.tclSqlResult.SelectedIndex = 0;
            this.tclSqlResult.Size = new System.Drawing.Size(672, 402);
            this.tclSqlResult.TabIndex = 0;
            // 
            // tabMsg
            // 
            this.tabMsg.Controls.Add(this.txtMsg);
            this.tabMsg.Location = new System.Drawing.Point(4, 22);
            this.tabMsg.Name = "tabMsg";
            this.tabMsg.Padding = new System.Windows.Forms.Padding(3);
            this.tabMsg.Size = new System.Drawing.Size(664, 376);
            this.tabMsg.TabIndex = 0;
            this.tabMsg.Text = "Message";
            this.tabMsg.UseVisualStyleBackColor = true;
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.Color.Ivory;
            this.txtMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsg.HideSelection = false;
            this.txtMsg.Location = new System.Drawing.Point(3, 3);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.Size = new System.Drawing.Size(658, 370);
            this.txtMsg.TabIndex = 0;
            this.txtMsg.Text = "";
            // 
            // tabResult
            // 
            this.tabResult.Controls.Add(this.grdResult);
            this.tabResult.Location = new System.Drawing.Point(4, 22);
            this.tabResult.Name = "tabResult";
            this.tabResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabResult.Size = new System.Drawing.Size(664, 376);
            this.tabResult.TabIndex = 1;
            this.tabResult.Text = "Result";
            this.tabResult.UseVisualStyleBackColor = true;
            // 
            // grdResult
            // 
            this.grdResult.AllowUserToAddRows = false;
            this.grdResult.AllowUserToDeleteRows = false;
            this.grdResult.AllowUserToResizeRows = false;
            this.grdResult.BackgroundColor = System.Drawing.Color.Ivory;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdResult.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResult.Location = new System.Drawing.Point(3, 3);
            this.grdResult.Name = "grdResult";
            this.grdResult.ReadOnly = true;
            this.grdResult.RowHeadersVisible = false;
            this.grdResult.RowTemplate.Height = 23;
            this.grdResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdResult.Size = new System.Drawing.Size(658, 370);
            this.grdResult.TabIndex = 0;
            this.grdResult.VirtualMode = true;
            // 
            // tabSQLStatement
            // 
            this.tabSQLStatement.Controls.Add(this.txtSQL);
            this.tabSQLStatement.Location = new System.Drawing.Point(4, 22);
            this.tabSQLStatement.Name = "tabSQLStatement";
            this.tabSQLStatement.Size = new System.Drawing.Size(664, 376);
            this.tabSQLStatement.TabIndex = 2;
            this.tabSQLStatement.Text = "SQL Statement";
            this.tabSQLStatement.UseVisualStyleBackColor = true;
            // 
            // txtSQL
            // 
            this.txtSQL.BackColor = System.Drawing.Color.Ivory;
            this.txtSQL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQL.HideSelection = false;
            this.txtSQL.Location = new System.Drawing.Point(0, 0);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.ReadOnly = true;
            this.txtSQL.Size = new System.Drawing.Size(664, 376);
            this.txtSQL.TabIndex = 1;
            this.txtSQL.Text = "";
            // 
            // tspMain
            // 
            this.tspMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tspMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStop,
            this.btnRollback,
            this.btnCommit,
            this.sep0,
            this.toolStripLabel1,
            this.cbxTables,
            this.lblDB,
            this.toolStripSeparator3,
            this.lblAccount,
            this.toolStripSeparator4,
            this.lblConnect});
            this.tspMain.Location = new System.Drawing.Point(0, 0);
            this.tspMain.Name = "tspMain";
            this.tspMain.Size = new System.Drawing.Size(672, 25);
            this.tspMain.TabIndex = 1;
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(23, 22);
            this.btnStop.Text = "Stop";
            // 
            // btnRollback
            // 
            this.btnRollback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRollback.Image = ((System.Drawing.Image)(resources.GetObject("btnRollback.Image")));
            this.btnRollback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.Size = new System.Drawing.Size(23, 22);
            this.btnRollback.Text = "Rollback";
            // 
            // btnCommit
            // 
            this.btnCommit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCommit.Image = ((System.Drawing.Image)(resources.GetObject("btnCommit.Image")));
            this.btnCommit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(23, 22);
            this.btnCommit.Text = "Commit";
            // 
            // sep0
            // 
            this.sep0.Name = "sep0";
            this.sep0.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(67, 22);
            this.toolStripLabel1.Text = "Current DB";
            // 
            // cbxTables
            // 
            this.cbxTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTables.Name = "cbxTables";
            this.cbxTables.Size = new System.Drawing.Size(121, 25);
            // 
            // lblDB
            // 
            this.lblDB.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblDB.Name = "lblDB";
            this.lblDB.Size = new System.Drawing.Size(27, 22);
            this.lblDB.Text = "     ";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // lblAccount
            // 
            this.lblAccount.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(19, 22);
            this.lblAccount.Text = "   ";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // lblConnect
            // 
            this.lblConnect.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblConnect.Name = "lblConnect";
            this.lblConnect.Size = new System.Drawing.Size(19, 22);
            this.lblConnect.Text = "   ";
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblError.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblError.ForeColor = System.Drawing.Color.Brown;
            this.lblError.Location = new System.Drawing.Point(0, 0);
            this.lblError.Margin = new System.Windows.Forms.Padding(0);
            this.lblError.Name = "lblError";
            this.lblError.Padding = new System.Windows.Forms.Padding(30);
            this.lblError.Size = new System.Drawing.Size(672, 427);
            this.lblError.TabIndex = 2;
            this.lblError.Text = "fdasf";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmSQLResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 427);
            this.Controls.Add(this.tclSqlResult);
            this.Controls.Add(this.tspMain);
            this.Controls.Add(this.lblError);
            this.Name = "FrmSQLResult";
            this.Text = "FrmSQLResult";
            this.Activated += new System.EventHandler(this.FrmSQLResult_Activated);
            this.Load += new System.EventHandler(this.FrmSQLResult_Load);
            this.tclSqlResult.ResumeLayout(false);
            this.tabMsg.ResumeLayout(false);
            this.tabResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdResult)).EndInit();
            this.tabSQLStatement.ResumeLayout(false);
            this.tspMain.ResumeLayout(false);
            this.tspMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tclSqlResult;
        private System.Windows.Forms.TabPage tabMsg;
        private System.Windows.Forms.TabPage tabResult;
        private System.Windows.Forms.RichTextBox txtMsg;
        private System.Windows.Forms.DataGridView grdResult;
        private System.Windows.Forms.TabPage tabSQLStatement;
        private System.Windows.Forms.RichTextBox txtSQL;
        private System.Windows.Forms.ToolStrip tspMain;
        private System.Windows.Forms.ToolStripComboBox cbxTables;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripButton btnRollback;
        private System.Windows.Forms.ToolStripButton btnCommit;
        private System.Windows.Forms.ToolStripSeparator sep0;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel lblDB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel lblAccount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel lblConnect;
        private System.Windows.Forms.Label lblError;

    }

}