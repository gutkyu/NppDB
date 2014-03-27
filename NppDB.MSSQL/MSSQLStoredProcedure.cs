using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NppDB.Comm;

namespace NppDB.MSSQL
{
    public class MSSQLStoredProcedure : NppDB.Comm.IStoredProcedure
    {
        public MSSQLStoredProcedure(MSSQLDatabase database) { Parent = database; _cols = new List<IColumnInfo>(); }
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

        }

        public IDatabase Parent { get; private set; }
    }
}
