using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NppDB.Comm;

namespace NppDB.MSSQL
{
    public class MSSQLParametaInfo : NppDB.Comm.IParametaInfo
    {
        public MSSQLParametaInfo(MSSQLTable table) { Parent = table; }
        public string Name { get; set; }
        public string ParamType { get; set; }
        public ITable Parent { get; private set; }
    }
}
