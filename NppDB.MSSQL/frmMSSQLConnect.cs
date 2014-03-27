using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NppDB.MSSQL
{
    public partial class frmMSSQLConnect : Form
    {
        public frmMSSQLConnect()
        {
            InitializeComponent();
        }

        public frmMSSQLConnect(MSSQLConnect connect):this()
        {
            ServerAddress = connect.ServerAddress;
            LoginID = connect.Account;
            InitCatalog = connect.InitialCatalog;
            ConnectTimeout = connect.ConnectTimeout;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(this.cbxServer.Text))
            {
                System.Windows.Forms.MessageBox.Show("server address incorrect.");
                return;
            }

            if (string.IsNullOrWhiteSpace(this.cbxLogin.Text))
            {
                System.Windows.Forms.MessageBox.Show("login id or password is invalid");
                return;
            }

            if (string.IsNullOrWhiteSpace(this.txtPwd.Text))
            {
                System.Windows.Forms.MessageBox.Show("login id or password is invalid");
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public string ServerAddress
        {
            get { return this.cbxServer.Text; }
            set { this.cbxServer.Text = value; }
        }

        public string LoginID
        {
            get { return this.cbxLogin.Text; }
            set { this.cbxLogin.Text = value; }
        }

        public string Password
        {
            get { return this.txtPwd.Text; }
            set { this.txtPwd.Text = value; }
        }

        public string InitCatalog
        {
            get { return this.txtInitCat.Text; }
            set { this.txtInitCat.Text = value; }
        }

        public int ConnectTimeout
        {
            get
            {
                int ret = 0;
                int.TryParse(this.txtTimeout.Text, out ret);
                return ret;
            }
            set
            {
                this.txtTimeout.Text = value.ToString();
            }
        }
    }
}
