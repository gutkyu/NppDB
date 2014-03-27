using System;
namespace NppDB.Comm
{
    public interface IParametaInfo
    {
        string Name { get; set; }
        string ParamType { get; set; }
        ITable Parent { get; }
    }
}
