using System;
namespace NppDB.Comm
{
    public interface IColumnInfo
    {
        string ColumnType { get; }
        string Name { get; set; }
        ITable Parent { get; }
    }
}
