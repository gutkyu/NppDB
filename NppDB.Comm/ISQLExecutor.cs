using System;
using System.Data;
namespace NppDB.Comm
{
    public interface ISQLExecutor
    {
        bool CanExecute();
        void Execute(string sqlQuery, Action<Exception,DataTable> callback);
        bool CanStop();
        void Stop();
    }
}
