using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NppDB.Comm;

namespace NppDB.MSSQL
{
    public class MSSQLDatabase : NppDB.Comm.IDatabase
    {
        public MSSQLDatabase(MSSQLConnect dbConnect)
        {
            Parent = dbConnect;
            _tables = new List<ITable>();
            _views = new List<IView>();
            _sysTables = new List<ITable>();
            _storedProcedures = new List<IStoredProcedure>();
        }
        public string Name { set; get; }

        private List<ITable> _tables = null;
        public IEnumerable<ITable> Tables { get { return _tables; } }
        private List<IView> _views = null;
        public IEnumerable<IView> Views { get { return _views; } }
        private List<ITable> _sysTables = null;
        public IEnumerable<ITable> SysTables { get { return _sysTables; } }
        private List<IStoredProcedure> _storedProcedures = null;
        public IEnumerable<IStoredProcedure> StoredProcedures { get { return _storedProcedures; } }
        /*
        public void Refresh() 
        {
            RefreshTables();
            RefreshSystemTables();
            RefreshViews();
            RefreshStoredProcedure();
        }
        */


        public void Refresh(string Category)
        {
            if (Category == "SystemTables") RefreshSystemTables();
            else if (Category == "Tables") RefreshTables();
            else if (Category == "Views") RefreshViews();
            else if (Category == "StoredProcedures") RefreshStoredProcedure();
            else System.Windows.Forms.MessageBox.Show("Unsupported : " + Category );
        }
        private void RefreshTables()
        {
            //SqlCommand cmd = new SqlCommand("select distinct table_schema, table_name from information_schema.tables where table_catalog='" + Name + "'", ((MSSQLConnect)Parent).Connection);
            SqlCommand cmd = new SqlCommand("select distinct table_schema, table_name from " + Name + ".information_schema.tables ", ((MSSQLConnect)Parent).Connection);
            var reader = cmd.ExecuteReader();
            var dt = new System.Data.DataTable();
            dt.Load(reader);
            if (dt.Rows.Count > 0)
            {
                _tables.Clear();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    var tbl = new MSSQLTable(this);
                    tbl.Name = row["table_name"].ToString();
                    tbl.TableSchema = row["table_schema"].ToString();
                    tbl.TableName = tbl.TableSchema + "." + tbl.Name;
                    _tables.Add(tbl);
                }
            }
        }

        private void RefreshViews()
        {
            //SqlCommand cmd = new SqlCommand("select distinct table_schema, table_name from information_schema.tables where table_catalog='" + Name + "'", ((MSSQLConnect)Parent).Connection);
            SqlCommand cmd = new SqlCommand("select distinct table_schema, table_name from " + Name + ".information_schema.views ", ((MSSQLConnect)Parent).Connection);
            var reader = cmd.ExecuteReader();
            var dt = new System.Data.DataTable();
            dt.Load(reader);
            if (_views == null) _tables = new List<ITable>();
            if (dt.Rows.Count > 0)
            {
                _views.Clear();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    var vw = new MSSQLView(this);
                    vw.Name = row["table_name"].ToString();
                    vw.ViewSchema = row["table_schema"].ToString();
                    vw.ViewName = vw.ViewSchema + "." + vw.Name;
                    _views.Add(vw);
                }
            }
        }

        private void RefreshSystemTables()
        {

            SqlCommand cmd = new SqlCommand(string.Format("select o.name as table_name, s.name as table_schema  from  {0}.sys.objects o join {0}.sys.schemas s on o.schema_id = s.schema_id where type = 'S'",Name), ((MSSQLConnect)Parent).Connection);
            var reader = cmd.ExecuteReader();
            var dt = new System.Data.DataTable();
            dt.Load(reader);
            if (dt.Rows.Count > 0)
            {
                _sysTables.Clear();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    var tbl = new MSSQLSystemTable(this);
                    tbl.Name = row["table_name"].ToString();
                    tbl.TableSchema = row["table_schema"].ToString();
                    tbl.TableName = tbl.TableSchema + "." + tbl.Name;
                    _sysTables.Add(tbl);
                }
            }
        }

        private void RefreshStoredProcedure()
        {
            SqlCommand cmd = new SqlCommand(string.Format("select o.name as sp_name, s.name as sp_schema  from  {0}.sys.objects o join {0}.sys.schemas s on o.schema_id = s.schema_id where type = 'P'", Name), ((MSSQLConnect)Parent).Connection);
            var reader = cmd.ExecuteReader();
            var dt = new System.Data.DataTable();
            dt.Load(reader);
            if (dt.Rows.Count > 0)
            {
                _storedProcedures.Clear();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    var sp = new MSSQLStoredProcedure(this);
                    sp.Name = row["sp_name"].ToString();
                    sp.TableSchema = row["sp_schema"].ToString();
                    sp.TableName = sp.TableSchema + "." + sp.Name;
                    _storedProcedures.Add(sp);
                }
            }
        }

        public IDBConnect Parent { get; private set; }
    }
}
