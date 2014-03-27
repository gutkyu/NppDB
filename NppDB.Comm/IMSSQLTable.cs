using System;
namespace NppDB.Comm
{
    public interface ITable
    {
        string Name { get; set; }
        System.Collections.Generic.IEnumerable<IColumnInfo> Columns { get; }
        IDatabase Parent { get; }
        void Refresh();
    }
}
