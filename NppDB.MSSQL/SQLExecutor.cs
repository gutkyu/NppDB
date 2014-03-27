using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NppDB.MSSQL
{
    public class MSSQLExecutor : NppDB.Comm.ISQLExecutor
    {
        private SqlCommand _cmd = null;
        public MSSQLExecutor(SqlConnection connection )
        {
            _cmd = connection.CreateCommand();
        }

        private System.Threading.Thread _execTh = null;
        private bool _completed = true;
        public void Execute( string sqlQuery, bool useTransaction, Action<Exception> callback)
        {
            _completed = false;
            _execTh = null;

            _cmd.CommandText = sqlQuery;
            if (useTransaction)
            {
                try
                {
                    _cmd.Transaction = _cmd.Connection.BeginTransaction();
                }
                catch (Exception ex)
                {
                    callback(ex);
                    return;
                }
            }

            _execTh = new System.Threading.Thread(new System.Threading.ThreadStart(
                delegate
                {
                    try
                    {
                        var rd = _cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(rd);
                        Result = dt;
                    }
                    catch (Exception ex)
                    {
                        callback(ex);
                        if (useTransaction) _cmd.Transaction.Rollback();
                        _completed = true; 
                        return;
                    }
                    callback(null);
                    _completed = true; 
                }));
            _execTh.IsBackground = true;
            _execTh.Start();
        }



        public Boolean CanExecute()
        {
            //return _execTh == null || _completed;
            return _completed;
        }

        public DataTable Result
        {
            get;
            private set;
        }

        
        public void Stop()
        {
            if (!_completed) _cmd.Cancel();
            if(_execTh != null ) _execTh.Abort();
            _execTh = null;
            _completed = false;
        }

        public bool CanStop()
        {
            return !_completed ;
        }

        public void Commit()
        {
            _cmd.Transaction.Commit();
        }

        public bool CanCommit()
        {
            return _completed && _cmd.Transaction != null;
        }

        public void Rollback()
        {
            _cmd.Transaction.Rollback();
        }

        public bool CanRollback()
        {
            return _completed && _cmd.Transaction != null;
        }

        public string CurrentDatabase
        {
            get { return _cmd.Connection.Database; }
            set
            {
                try
                {
                    _cmd.Connection.ChangeDatabase(value);
                }
                catch (ArgumentException ex0)
                {
                    throw new ApplicationException("The database name(" + value + ") is not valid.", ex0);
                }
                catch (InvalidOperationException ex1) {
                    throw new ApplicationException("The connection is not open.", ex1);
                }
                catch (SqlException ex2) {
                    throw new ApplicationException("Cannot change this database("+value+").", ex2);
                }
            }
        }

    }
}
