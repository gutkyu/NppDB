using System;
namespace NppDB.Comm
{
    public interface IDBConnect
    {
        void Init();
        string Account { get; set; }
        bool CheckLogin();
        void Connect();
        System.Collections.Generic.IEnumerable<IDatabase> Databases { get; }
        void Disconnect();
        string Name { get; set; }
        string Password { get; set; }
        void Refresh();
        string ServerAddress { get; set; }
        string GetConnectionString();
        bool IsOpened { get; }
        ISQLExecutor CreateSQLExecutor();

    }
}
