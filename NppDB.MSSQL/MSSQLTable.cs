using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NppDB.Comm;

namespace NppDB.MSSQL
{
    public class MSSQLTable : NppDB.Comm.ITable
    {
        public MSSQLTable(MSSQLDatabase database) { Parent = database; _cols = new List<IColumnInfo>(); }
        public string Name { set; get; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }

        protected List<IColumnInfo> _cols = null;
        public IEnumerable<IColumnInfo> Columns
        {
            get { return _cols; }
        }
        public virtual void Refresh()
        {
            //SqlCommand cmd = new SqlCommand("select distinct column_name, data_type, character_maximum_length from information_schema.columns where table_catalog='" + Parent.Name + "' and table_name='" + Name + "' order by table_catalog, table_schema, ordinal_position ", ((MSSQLConnect)(Parent.Parent)).Connection);
            SqlCommand cmd = new SqlCommand("select distinct column_name, data_type, character_maximum_length from " + Parent.Name + ".information_schema.columns where table_name='" + Name + "'", ((MSSQLConnect)(Parent.Parent)).Connection);
            var reader = cmd.ExecuteReader();
            var dt = new System.Data.DataTable();
            dt.Load(reader);
            if (dt.Rows.Count > 0)
            {
                _cols.Clear();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    var col = new MSSQLColumnInfo(this);
                    col.Name = row["column_name"].ToString();
                    col.ColumnType = row["data_type"].ToString() + (row["character_maximum_length"] is DBNull ? "" : "(" + row["character_maximum_length"].ToString() + ")");
                    _cols.Add(col);
                }
            }
        }
        public IDatabase Parent { get; private set; }
    }
}
