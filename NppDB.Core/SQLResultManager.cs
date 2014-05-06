using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NppDB.Comm;

namespace NppDB.Core
{
    public class SQLResultManager
    {

        private Dictionary<int, SQLResult> _bind = new Dictionary<int, SQLResult>();
        public SQLResult CreateSQLResult(int id, IDBConnect connect, ISQLExecutor sqlExecutor)
        {
            if (_bind.ContainsKey(id)) throw new ApplicationException("id("+ id+") already exists");
            var ret = _bind[id] = new SQLResult(connect, sqlExecutor) { Visible = false };//Visible = false to prevent Flicker
            return ret;
        }
        public int Count
        {
            get { return _bind.Count; }
        }
        public void Remove(int id)
        {
            _bind.Remove(id);
        }
        public SQLResult GetSQLResult(int id)
        {
            return _bind.ContainsKey(id) ? _bind[id] : null;
        }
        public IEnumerable<SQLResult> GetSQLResults(IDBConnect connect)
        {
            return _bind.Where(x => x.Value.LinkedDBConnect == connect).Select(x => x.Value);
        }

        public int GetID(SQLResult result)
        {
            return _bind.FirstOrDefault(x => x.Value == result).Key;
        }

        private static SQLResultManager _inst = null;
        public static SQLResultManager Instance
        {
            get
            {
                if (_inst == null) _inst = new SQLResultManager();
                return _inst;
            }
        }
    }
}
