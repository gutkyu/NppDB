namespace NppDB.Core
{
    partial class SQLResult
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLResult));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tspMain = new System.Windows.Forms.ToolStrip();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.sep0 = new System.Windows.Forms.ToolStripSeparator();
            this.lblElapsed = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblAccount = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lblConnect = new System.Windows.Forms.ToolStripLabel();
            this.tclSqlResult = new System.Windows.Forms.TabControl();
            this.tabMsg = new System.Windows.Forms.TabPage();
            this.txtMsg = new System.Windows.Forms.RichTextBox();
            this.tabResult = new System.Windows.Forms.TabPage();
            this.grdResult = new System.Windows.Forms.DataGridView();
            this.lblError = new System.Windows.Forms.Label();
            this.tspMain.SuspendLayout();
            this.tclSqlResult.SuspendLayout();
            this.tabMsg.SuspendLayout();
            this.tabResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResult)).BeginInit();
            this.SuspendLayout();
            // 
            // tspMain
            // 
            this.tspMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tspMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStop,
            this.sep0,
            this.lblElapsed,
            this.toolStripSeparator3,
            this.lblAccount,
            this.toolStripSeparator4,
            this.lblConnect});
            this.tspMain.Location = new System.Drawing.Point(0, 0);
            this.tspMain.Name = "tspMain";
            this.tspMain.Padding = new System.Windows.Forms.Padding(0);
            this.tspMain.Size = new System.Drawing.Size(557, 25);
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
            // sep0
            // 
            this.sep0.Name = "sep0";
            this.sep0.Size = new System.Drawing.Size(6, 25);
            // 
            // lblElapsed
            // 
            this.lblElapsed.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblElapsed.Name = "lblElapsed";
            this.lblElapsed.Size = new System.Drawing.Size(19, 22);
            this.lblElapsed.Text = "   ";
            this.lblElapsed.ToolTipText = "elapsed time";
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
            // tclSqlResult
            // 
            this.tclSqlResult.Controls.Add(this.tabMsg);
            this.tclSqlResult.Controls.Add(this.tabResult);
            this.tclSqlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tclSqlResult.Location = new System.Drawing.Point(0, 25);
            this.tclSqlResult.Margin = new System.Windows.Forms.Padding(0);
            this.tclSqlResult.Multiline = true;
            this.tclSqlResult.Name = "tclSqlResult";
            this.tclSqlResult.Padding = new System.Drawing.Point(0, 0);
            this.tclSqlResult.SelectedIndex = 0;
            this.tclSqlResult.Size = new System.Drawing.Size(557, 366);
            this.tclSqlResult.TabIndex = 0;
            // 
            // tabMsg
            // 
            this.tabMsg.Controls.Add(this.txtMsg);
            this.tabMsg.Location = new System.Drawing.Point(4, 22);
            this.tabMsg.Margin = new System.Windows.Forms.Padding(0);
            this.tabMsg.Name = "tabMsg";
            this.tabMsg.Size = new System.Drawing.Size(549, 340);
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
            this.txtMsg.Location = new System.Drawing.Point(0, 0);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.Size = new System.Drawing.Size(549, 340);
            this.txtMsg.TabIndex = 0;
            this.txtMsg.Text = "";
            // 
            // tabResult
            // 
            this.tabResult.Controls.Add(this.grdResult);
            this.tabResult.Location = new System.Drawing.Point(4, 22);
            this.tabResult.Margin = new System.Windows.Forms.Padding(0);
            this.tabResult.Name = "tabResult";
            this.tabResult.Size = new System.Drawing.Size(549, 340);
            this.tabResult.TabIndex = 1;
            this.tabResult.Text = "Result";
            this.tabResult.UseVisualStyleBackColor = true;
            // 
            // grdResult
            // 
            this.grdResult.AllowUserToAddRows = false;
            this.grdResult.AllowUserToDeleteRows = false;
            this.grdResult.AllowUserToResizeRows = false;
            this.grdResult.BackgroundColor = System.Drawing.Color.White;
            this.grdResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
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
            this.grdResult.GridColor = System.Drawing.SystemColors.Control;
            this.grdResult.Location = new System.Drawing.Point(0, 0);
            this.grdResult.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.grdResult.Name = "grdResult";
            this.grdResult.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdResult.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grdResult.RowHeadersWidth = 10;
            this.grdResult.RowTemplate.Height = 23;
            this.grdResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdResult.ShowEditingIcon = false;
            this.grdResult.ShowRowErrors = false;
            this.grdResult.Size = new System.Drawing.Size(549, 340);
            this.grdResult.TabIndex = 0;
            this.grdResult.VirtualMode = true;
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
            this.lblError.Size = new System.Drawing.Size(557, 391);
            this.lblError.TabIndex = 3;
            this.lblError.Text = "fdasf";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SQLResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tclSqlResult);
            this.Controls.Add(this.tspMain);
            this.Controls.Add(this.lblError);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SQLResult";
            this.Size = new System.Drawing.Size(557, 391);
            this.tspMain.ResumeLayout(false);
            this.tspMain.PerformLayout();
            this.tclSqlResult.ResumeLayout(false);
            this.tabMsg.ResumeLayout(false);
            this.tabResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspMain;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripSeparator sep0;
        private System.Windows.Forms.ToolStripLabel lblElapsed;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel lblAccount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel lblConnect;
        private System.Windows.Forms.TabControl tclSqlResult;
        private System.Windows.Forms.TabPage tabMsg;
        private System.Windows.Forms.RichTextBox txtMsg;
        private System.Windows.Forms.TabPage tabResult;
        private System.Windows.Forms.DataGridView grdResult;
        private System.Windows.Forms.Label lblError;
    }
}
