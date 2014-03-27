using System;
namespace NppDB.Comm
{
    public interface IView
    {
        System.Collections.Generic.IEnumerable<IColumnInfo> Columns { get; }
        string Name { get; set; }
        IDatabase Parent { get; }
        void Refresh();
        string ViewName { get; set; }
        string ViewSchema { get; set; }
    }
}
