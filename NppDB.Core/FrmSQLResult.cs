using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NppDB;
using NppDB.Comm;


namespace NppDB.Core
{
    public partial class FrmSQLResult : Form
    {
        public FrmSQLResult()
        {
            InitializeComponent();
            Init();
        }

        private ToolStripItem[] _tranCtrs = null;
        private void Init()
        {
            _tranCtrs = new ToolStripItem[] { btnStop, btnCommit, btnRollback, sep0 };
            SetUseTransaction(false);

            this.btnStop.Click += (s, e) => { SetTransEnabledPropertiesWithFalse(); _exec.Stop(); SetTransEnabledProperties(); SetTransVisibleProperties(); };
            this.btnCommit.Click += (s, e) => { SetTransEnabledPropertiesWithFalse(); _exec.Commit(); SetTransEnabledProperties(); SetTransVisibleProperties(); };
            this.btnRollback.Click += (s, e) => { SetTransEnabledPropertiesWithFalse(); _exec.Rollback(); SetTransEnabledProperties(); SetTransVisibleProperties(); };

        }

        private List<NotifyHandler> _notifyHnds = new List<NotifyHandler>();
        public void AddNotifyHandler(NotifyHandler handler)
        {
            _notifyHnds.Add(handler);
        }

        protected override void WndProc(ref Message m)
        {
            if (_notifyHnds.Count > 0 && m.Msg == 0x4e)//WM_NOTIFY
                foreach (var hnd in _notifyHnds) hnd(ref m);
            base.WndProc(ref m);
        }

        private IDBConnect _currConn = null;
        private ISQLExecutor _exec = null;
        public void SetCurrentConnect(IDBConnect connect, string defaultDBName)
        {
            if (_exec == null || _currConn != connect)
            {
                _currConn = connect;
                _exec = _currConn.CreateSQLExecutor();
            }
            cbxTables.Items.Clear();
            var dbList = _currConn.Databases;
            if (dbList.Count() > 0)
            {
                cbxTables.Items.AddRange(dbList.Select((d) => d.Name).ToArray());
            }
            if (!string.IsNullOrWhiteSpace(defaultDBName) && _exec.CurrentDatabase != defaultDBName) _exec.CurrentDatabase = defaultDBName;
            cbxTables.SelectedIndex = cbxTables.Items.IndexOf(_exec.CurrentDatabase);

            lblConnect.Text = _currConn.Name;
            lblAccount.Text = _currConn.Account;
            lblDB.Text = _exec.CurrentDatabase;

            SetTransEnabledPropertiesWithFalse();
        }

        public IDBConnect GetCurrrentConnect()
        {
            return _currConn;
        }

        private bool _isTransMode = false;
        public void SetUseTransaction(bool use)
        {
            if (use && _exec != null && !_exec.CanExecute())
            {
                MessageBox.Show("트랜잭션 상태에 있거나 종료하지 않은 작업이 있습니다. 작업이 종료된 다음 실행하세요.");
                return;
            }
            _isTransMode = use;
            SetTransVisibleProperties();
        }

        public void SetError(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                lblError.Visible = false;
                tspMain.Visible = true;
                tclSqlResult.Visible = true;
            }
            else
            {
                lblError.Visible = true;
                tspMain.Visible = false;
                tclSqlResult.Visible = false;
            }
            lblError.Text = message;
        }

        public void Execute(string sql, bool forceNoTransaction = false)
        {
            if (!_exec.CanExecute())
            {
                MessageBox.Show("트랜잭션 상태에 있거나 종료하지 않은 작업이 있습니다.");
                return;
            }

            if (cbxTables.SelectedIndex < 0)
            {
                MessageBox.Show("please, select database!");
                return;
            }
            if (_exec == null)
            {
                string currDBNM = cbxTables.SelectedItem.ToString();
                try
                {
                    _exec.CurrentDatabase = currDBNM;
                }
                catch
                {
                    MessageBox.Show("failed to select database '" + currDBNM + "'");
                    return;
                }

            }

            if (!_isTransMode|| forceNoTransaction)
            {
                Array.ForEach(_tranCtrs, (x) => { x.Visible = false; });
            }
            else
            {
                btnStop.Enabled = true;
                btnRollback.Enabled = false;
                btnCommit.Enabled = false;
            }

            txtSQL.Clear();
            txtSQL.AppendText(sql);

            if (cbxTables.SelectedIndex < 0)
            {
                SetError("select a current database.");
                cbxTables.Select();
                cbxTables.Focus();
                return;
            }
            if (_exec.CurrentDatabase != cbxTables.SelectedItem.ToString())
            {
                _exec.CurrentDatabase = cbxTables.SelectedItem.ToString();
            }

            _exec.Execute(sql, _isTransMode && !forceNoTransaction, (e) =>
            {
                if (e == null)
                {
                    this.Invoke(new Action(delegate
                    {
                        grdResult.DataSource = _exec.Result;
                        txtMsg.Clear();
                        txtMsg.AppendText("return " + _exec.Result.Rows.Count + " rows");
                        //tabResult.Show();
                        tclSqlResult.SelectTab(1);
                        SetTransEnabledProperties();
                        SetTransVisibleProperties();
                    }));
                }
                else
                {
                    this.Invoke (new Action(delegate
                    {
                        grdResult.DataSource = null;
                        txtMsg.Clear();
                        txtMsg.AppendText(e.Message);
                        //tabMsg.Show();
                        tclSqlResult.SelectTab(0);
                        SetTransEnabledProperties();
                        SetTransVisibleProperties();
                    }));
                }

            });
        }

        private void SetTransVisibleProperties()
        {
            Array.ForEach(_tranCtrs, (x) => { x.Visible = _isTransMode; });
        }     

        private void SetTransEnabledProperties()
        {
            if (_isTransMode)
            {
                btnStop.Enabled = _exec.CanStop();
                btnRollback.Enabled = _exec.CanRollback();
                btnCommit.Enabled = _exec.CanCommit();
            }
        }

        private void SetTransEnabledPropertiesWithFalse()
        {
            if (_isTransMode)
            {
                btnStop.Enabled = btnRollback.Enabled = btnCommit.Enabled = false;
            }
        }

        private void FrmSQLResult_Activated(object sender, EventArgs e)
        {
        }

        private void FrmSQLResult_Load(object sender, EventArgs e)
        {
        }
        
    }
}
