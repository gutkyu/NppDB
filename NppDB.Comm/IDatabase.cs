using System;
namespace NppDB.Comm
{
    public interface IDatabase
    {
        string Name { get; set; }
        IDBConnect Parent { get; }
        void Refresh(string Category);
        System.Collections.Generic.IEnumerable<ITable> Tables { get; }
        System.Collections.Generic.IEnumerable<IView> Views { get ;  }
        System.Collections.Generic.IEnumerable<ITable> SysTables { get ;  }
        System.Collections.Generic.IEnumerable<IStoredProcedure> StoredProcedures { get ; }
    }
}
