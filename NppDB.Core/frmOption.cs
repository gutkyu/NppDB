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
    public partial class frmOption : Form
    {
        public frmOption()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            cbxUseTrans.Checked = (bool)Options.Instance["forcetrans"].Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Options.Instance["forcetrans"].Value = cbxUseTrans.Checked;
            this.DialogResult =  System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void frmOption_Load(object sender, EventArgs e)
        {
        }

    }
}
