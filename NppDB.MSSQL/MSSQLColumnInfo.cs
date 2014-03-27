using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NppDB.Comm;

namespace NppDB.MSSQL
{
    public class MSSQLColumnInfo : NppDB.Comm.IColumnInfo
    {
        public MSSQLColumnInfo(MSSQLTable table) { Parent = table; }
        public string Name { get; set; }
        public string ColumnType { get; set; }
        public ITable Parent { get; private set; }
    }

}
