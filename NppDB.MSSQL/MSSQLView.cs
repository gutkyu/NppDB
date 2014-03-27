using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NppDB.Comm;

namespace NppDB.MSSQL
{
    public class MSSQLView : NppDB.Comm.IView
    {
        public MSSQLView(MSSQLDatabase database) { Parent = database; _cols = new List<IColumnInfo>(); }
        public string Name { set; get; }
        public string ViewSchema { get; set; }
        public string ViewName { get; set; }

        private List<IColumnInfo> _cols = null;
        public IEnumerable<IColumnInfo> Columns
        {
            get { return _cols; }
        }
        public void Refresh()
        {
        }
        public IDatabase Parent { get; private set; }
    }
}
