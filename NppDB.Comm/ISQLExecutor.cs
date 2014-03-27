using System;
namespace NppDB.Comm
{
    public interface ISQLExecutor
    {
        bool CanCommit();
        bool CanExecute();
        bool CanRollback();
        bool CanStop();
        void Commit();
        void Execute(string sqlQuery, bool useTransaction, Action<Exception> callback);
        void Rollback();
        void Stop();
        System.Data.DataTable Result{get;}
        string CurrentDatabase { get; set; }
    }
}
