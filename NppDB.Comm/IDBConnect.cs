using System;
namespace NppDB.Comm
{
    public interface IDBConnect
    {
        void Reset();
        string Account { get; set; }
        string GetDefaultTitle();
        bool CheckLogin();
        void Connect();
        void Disconnect();
        string Title { get; set; }
        string Password { get; set; }
        void Refresh();
        string ServerAddress { get; set; }
        bool IsOpened { get; }
    }
}
