using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Linq;

using NppDB.Comm;
using NppDB.Core;

namespace NppDB
{
    partial class PluginBase
    {
        internal const string PluginName = "NppDB";
        static string _cfgPath = null;
        static string _dbConnsPath = null;
        static FrmSQLResult _frmSqlResult = null;
        static FrmDatabaseExplore _frmDBExplorer = null;
        static internal int idfrmSqlResult = -1;
        static internal int idfrmDBExplorer = -1;
        static Bitmap imgRes = Properties.Resources.DBPPResult16;
        static Bitmap imgMan = Properties.Resources.DBPPManage16;
        static Icon tbIcon = null;

        static internal void CommandMenuInit()
        {
            StringBuilder sbCfgPath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbCfgPath);
            string plgDir = Path.Combine(sbCfgPath.ToString(), PluginName);

            if (!Directory.Exists(plgDir)) { try { Directory.CreateDirectory(plgDir); } catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); throw ex; } }

            _cfgPath = Path.Combine(plgDir, "config.xml");
            _dbConnsPath = Path.Combine(plgDir, "dbconnects.xml");
            if (File.Exists(_cfgPath)) { try { Options.Instance.LoadFromXml(_cfgPath); } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("config.xml : " + ex.Message); throw ex; } }
            if (File.Exists(_dbConnsPath)) { try { DBServerManager.Instance.LoadFromXml(_dbConnsPath); } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("dbconnects.xml : "+ ex.Message); throw ex; } }
    
            SetCommand(0, "Execute SQL", ExecuteSql, new ShortcutKey(false, true, false, Keys.Q));
            SetCommand(1, "Database Connect Manager", ToggleDBManager);
            idfrmDBExplorer = 1;
            SetCommand(2, "SQL Result", ToggleSQLResult);
            idfrmSqlResult = 2;
            SetCommand(3, "Options", ShowOptions);
        }

        static internal void SetToolBarIcon()
        {

            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = imgMan.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[idfrmDBExplorer]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);

            tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = imgRes.GetHbitmap();
            pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[idfrmSqlResult]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);

        }
        static internal void PluginCleanUp()
        {
            Options.Instance.SaveToXml(_cfgPath);
            DBServerManager.Instance.SaveToXml(_dbConnsPath);
        }

        #region " Menu functions "

        static void ExecuteSql()
        {
            

            int len = Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GETSELTEXT, 0, 0).ToInt32();

            if (len == 0) return;
            int codePage = Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GETCODEPAGE, 0, 0).ToInt32();

            List<byte> chars = new List<byte>();

            IntPtr ptrChars = Marshal.AllocHGlobal(len);
            Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GETSELTEXT, 0, ptrChars);
            int offset = 0;
            byte b = 0;
            try
            {
                while ((b = Marshal.ReadByte(ptrChars, offset++)) != 0)
                {
                    chars.Add(b);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
                return;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrChars);
            }


            if (_frmSqlResult == null || !_frmSqlResult.Visible)
            {
                ToggleSQLResult();
            }

            string sqlQuery = "";
            try
            {
                try
                {
                    sqlQuery = Encoding.GetEncoding(codePage).GetString(chars.ToArray());
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ApplicationException("invalid codepage : " + codePage.ToString());

                }
                catch (ArgumentException)
                {
                    throw new ApplicationException("unsupport codepage : " + codePage.ToString());
                }
                catch (NotSupportedException)
                {
                    throw new ApplicationException("unsupport codepage : " + codePage.ToString());
                }

                if (string.IsNullOrWhiteSpace(sqlQuery))
                {
                    throw new ApplicationException("a emptry, null or whitesapce text don't was executed");
                }

                if (_frmSqlResult == null || _frmSqlResult.GetCurrrentConnect() == null)
                {
                    if (_frmDBExplorer == null || !_frmDBExplorer.Visible)
                    {
                        ToggleDBManager();
                    }

                    if (DBServerManager.Instance.Connections.Count() == 0)
                        throw new ApplicationException( "register and open a database connection to sql query.");
                    else
                        throw new ApplicationException( "select and open a database connection to execute sql query.");
                }


                if (!_frmSqlResult.GetCurrrentConnect().IsOpened)
                {
                    throw new ApplicationException("this database connection closed. open a database connection again.");
                }
            }
            catch(Exception ex)
            {
                _frmSqlResult.SetError(ex.Message);
                return;
            }

            _frmSqlResult.SetError("");

            _frmSqlResult.Execute(sqlQuery);

        }

        private static NotifyHandler GetDialogCloseHandler(int idForm)
        {
            return delegate(ref Message msg)
            {
                SCNotification nc = (SCNotification)Marshal.PtrToStructure(msg.LParam, typeof(SCNotification));
                if (nc.nmhdr.code == (uint)DockMgrMsg.DMN_CLOSE)
                {
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idForm]._cmdID, 0);
                }
            };
        }

        static void ToggleDBManager()
        {
            if (_frmDBExplorer == null)
            {
                _frmDBExplorer = new FrmDatabaseExplore();
                _frmDBExplorer.AddNotifyHandler(GetDialogCloseHandler(idfrmDBExplorer));
                _frmDBExplorer.SetOpenDBHandler((x,y)=>{
                    if (_frmSqlResult == null || !_frmSqlResult.Visible) ToggleSQLResult();
                    _frmSqlResult.SetError("");
                     _frmSqlResult.SetCurrentConnect(x, y);
                });
               
                _frmDBExplorer.SetSelectTableHandler((x) =>
                {
                    if (_frmSqlResult == null || !_frmSqlResult.Visible) ToggleSQLResult();
                    var path = x;
                    _frmSqlResult.SetError("");
                    _frmSqlResult.SetCurrentConnect(DBServerManager.Instance.GetDBConnect(path.First()) as IDBConnect, path[1]);
                    _frmSqlResult.Execute("select top 100 * from " + path.Last(), false);
                });
                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(imgMan, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                NppTbData nppTbData = new NppTbData();
                nppTbData.hClient = _frmDBExplorer.Handle;
                nppTbData.pszName = _funcItems.Items[idfrmDBExplorer]._itemName;
                nppTbData.dlgID = idfrmDBExplorer;
                nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                nppTbData.hIconTab = (uint)tbIcon.Handle;

                nppTbData.pszModuleName = PluginName;
                IntPtr ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(nppTbData));
                Marshal.StructureToPtr(nppTbData, ptrNppTbData, false);

                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, ptrNppTbData);
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idfrmDBExplorer]._cmdID, 1);

            }
            else
            {
                if (!_frmDBExplorer.Visible)
                {
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, _frmDBExplorer.Handle);
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idfrmDBExplorer]._cmdID, 1);
                }
                else
                {
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, _frmDBExplorer.Handle);
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idfrmDBExplorer]._cmdID, 0);
                }
            }

        }

        static void ToggleSQLResult()
        {

            if (_frmSqlResult == null)
            {
                _frmSqlResult = new FrmSQLResult();
                _frmSqlResult.AddNotifyHandler(GetDialogCloseHandler(idfrmSqlResult));
                _frmSqlResult.SetUseTransaction((bool)Options.Instance["forcetrans"].Value);

                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(imgRes, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                NppTbData nppTbData = new NppTbData();
                nppTbData.hClient = _frmSqlResult.Handle;
                nppTbData.pszName = _funcItems.Items[idfrmSqlResult]._itemName;
                nppTbData.dlgID = idfrmSqlResult;
                nppTbData.uMask = NppTbMsg.DWS_DF_CONT_BOTTOM | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                nppTbData.hIconTab = (uint)tbIcon.Handle;

                nppTbData.pszModuleName = PluginName;
                IntPtr ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(nppTbData));
                Marshal.StructureToPtr(nppTbData, ptrNppTbData, false);
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, ptrNppTbData);
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idfrmSqlResult]._cmdID, 1);

            }
            else
            {
                if (!_frmSqlResult.Visible)
                {
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, _frmSqlResult.Handle);
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idfrmSqlResult]._cmdID, 1);
                }
                else
                {
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, _frmSqlResult.Handle);
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idfrmSqlResult]._cmdID, 0);
                }
            }

            if (_frmSqlResult.GetCurrrentConnect() == null)
            {
                if (DBServerManager.Instance.Connections.Count() == 0)
                    _frmSqlResult.SetError("register and open a database connection to sql query.");
                else
                    _frmSqlResult.SetError("select and open a database connection to execute sql query.");
                return;
            }

            if (!_frmSqlResult.GetCurrrentConnect().IsOpened)
            {
                _frmSqlResult.SetError("this database connection closed. open a database connection again.");
                return;
            }
        }

        static void ShowOptions()
        {
            var dlg = new frmOption();
            dlg.ShowDialog();
            if (_frmSqlResult != null) _frmSqlResult.SetUseTransaction((bool)Options.Instance["forcetrans"].Value);
        }
        #endregion
    }
}
