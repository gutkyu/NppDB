using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NppDB;
using NppDB.Comm;

namespace NppDB.Core
{
    public partial class frmSelectDbType : Form
    {
        public frmSelectDbType()
        {
            InitializeComponent();
        }

        private void frmSelectDbType_Load(object sender, EventArgs e)
        {
            cbxDbTypes.Items.AddRange(DBServerManager.Instance.GetDatabaseTypes().ToArray());
            if(cbxDbTypes.Items.Count> 0) cbxDbTypes.SelectedIndex = 0;
        }

        public DatabaseType SelectedDatabaseType
        {
            get
            {
                if (this.DialogResult != System.Windows.Forms.DialogResult.OK) return null;
                else return (DatabaseType)cbxDbTypes.SelectedItem;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult =  System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult =  System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void frmSelectDbType_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
