using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.Xml.Serialization;
using NppDB.Comm;

namespace NppDB.MSSQL
{
    [XmlRoot]
    [ConnectAttr(Id = "MSSQLConnect", Title = "MS SQL Server")]
    public class MSSQLConnect : NppDB.Comm.IDBConnect
    {

        //only one
        [XmlElement]
        public string Name { set; get; }
        //ip[:port]
        [XmlElement]
        public string ServerAddress { set; get; }
        [XmlElement]
        public string Account { set; get; }
        [XmlIgnore]
        public string Password { set; get; }

        [XmlElement]
        public string InitialCatalog { set; get; }
        [XmlElement]
        public int ConnectTimeout { set; get; }

        private SqlConnection _conn = null;


        public bool CheckLogin()
        {
            frmMSSQLConnect dlg = new frmMSSQLConnect();
            dlg.ServerAddress = this.ServerAddress;
            dlg.LoginID = this.Account;
            dlg.Password = this.Password;
            dlg.InitCatalog = this.InitialCatalog;
            dlg.ConnectTimeout = this.ConnectTimeout;

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return false;
            this.ServerAddress = dlg.ServerAddress.Trim();
            this.Account = dlg.LoginID.Trim();
            this.Password = dlg.Password.Trim();
            this.InitialCatalog = dlg.InitCatalog.Trim();
            this.ConnectTimeout = dlg.ConnectTimeout;

            
            return true;
        }

        public void Connect()
        {
            if (_conn == null) _conn = new SqlConnection();
            string curConnStr = GetConnectionString();
            if (_conn.ConnectionString != curConnStr) _conn.ConnectionString = curConnStr;
            if (_conn.State != System.Data.ConnectionState.Open)
            {
                try { _conn.Open(); }
                catch (Exception ex)
                { throw new ApplicationException("connect fail", ex); }
            }
        }
        public void Disconnect()
        {
            if (_conn == null) return;
            if (_conn.State != System.Data.ConnectionState.Closed) _conn.Close();

        }

        public bool IsOpened { get { return _conn != null && _conn.State == System.Data.ConnectionState.Open; } }

        internal SqlConnection Connection { get { return _conn; } }

        public ISQLExecutor CreateSQLExecutor() { return new MSSQLExecutor(_conn); }

        public void Refresh()
        {
            SqlCommand cmd = new SqlCommand("EXEC sp_databases", _conn);
            var reader = cmd.ExecuteReader();
            var dt = new System.Data.DataTable();
            dt.Load(reader);
            if (_dbs == null) _dbs = new List<IDatabase>();
            if (dt.Rows.Count > 0)
            {
                _dbs.Clear();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    var db = new MSSQLDatabase(this);
                    db.Name = row["DATABASE_NAME"].ToString();
                    _dbs.Add(db);
                }
            }
        }
        public string GetConnectionString()
        {
            var cSBuilder = new SqlConnectionStringBuilder();
            cSBuilder["server"] = ServerAddress;
            cSBuilder["user id"] = Account;
            cSBuilder["password"] = Password;
            if (!string.IsNullOrEmpty(InitialCatalog)) cSBuilder["initial catalog"] = InitialCatalog;
            if (ConnectTimeout > 0) cSBuilder["Connect Timeout"] = ConnectTimeout;

            return cSBuilder.ConnectionString;
        }


        public void Init()
        {
            Name = ""; ServerAddress = ""; Account = ""; Password = ""; InitialCatalog = ""; ConnectTimeout = -1;
            Disconnect();
            _conn = null;
        }

        private List<IDatabase> _dbs;
        public IEnumerable<IDatabase> Databases { get { return _dbs; } }

    }

}
