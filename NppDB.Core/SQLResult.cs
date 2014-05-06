using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NppDB.Comm;

namespace NppDB.Core
{
    public partial class SQLResult : UserControl
    {
        public SQLResult(IDBConnect connect,ISQLExecutor sqlExecutor)
        {
            InitializeComponent();
            Init();
            SetConnect(connect, sqlExecutor);
        }

        private void Init()
        {
            this.btnStop.Click += (s, e) =>
            {
                btnStop.Enabled = false;
                try
                {
                    _exec.Stop();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    btnStop.Enabled = _exec.CanStop();
                }

            };
            grdResult.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdResult.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdResult.ColumnHeadersDefaultCellStyle.Padding = new Padding(3);
            grdResult.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdResult.AutoSize = false;
            grdResult.Sorted += (s, e) => { Numbering(); };
            
        }
        private void Numbering()
        {
            int idx = 0;
            foreach (DataGridViewRow row in grdResult.Rows)
            {
                row.HeaderCell.Value = idx++.ToString();
            }
        }
        private void AdjustResizeColumnRow()
        {
            //grdResult.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            grdResult.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            grdResult.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        public IDBConnect LinkedDBConnect { get; private set; }

        private ISQLExecutor _exec = null;
        private void SetConnect(IDBConnect connect, ISQLExecutor sqlExecutor)
        {
            if (_exec == null)
            {
                _exec = sqlExecutor;
            }
            LinkedDBConnect = connect;
            lblConnect.Text = connect.Title;
            lblAccount.Text = connect.Account;
            lblElapsed.Text = "";
            btnStop.Enabled = false;
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

        public void Execute(string sql)
        {

            if (!_exec.CanExecute())
            {
                MessageBox.Show("트랜잭션 상태에 있거나 종료하지 않은 작업이 있습니다.");
                return;
            }

            btnStop.Enabled = true;

            var startPoint = DateTime.Now;
            grdResult.DataSource = null;
            txtMsg.Clear();

            _exec.Execute(sql, (err, dt) =>
            {
                var elapsed = (DateTime.Now - startPoint).ToString("c");
                if (err == null)
                {
                    this.Invoke(new Action(delegate
                    {
                        try
                        {
                            btnStop.Enabled = _exec.CanStop();
                            txtMsg.AppendText("return " + dt.Rows.Count + " rows");
                            lblElapsed.Text = elapsed;
                            grdResult.DataSource = dt;

                            tclSqlResult.SelectTab(1);
                            Numbering();
                            AdjustResizeColumnRow();
                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show(ex1.Message);
                        }
                    }));
                }
                else
                {
                    this.Invoke(new Action(delegate
                    {
                        btnStop.Enabled = _exec.CanStop();
                        string errMsg = "";
                        if (err is System.Data.SqlClient.SqlException)
                            errMsg = err.Message ;
                        else
                            errMsg = err.Message + "\n" + err.GetType().Name + "\n" + err.StackTrace;
                        txtMsg.AppendText(errMsg);
                        lblElapsed.Text = elapsed;
                        //tabMsg.Show();
                        tclSqlResult.SelectTab(0);
                    }));
                }

            });
        }
    }
}
