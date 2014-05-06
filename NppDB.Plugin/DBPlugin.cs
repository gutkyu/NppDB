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
    public class NppDBPlugin : PluginBase, INppDBCommandHost
    {
        private const string PluginName = "NppDB";
        private string _cfgPath = null;
        private string _dbConnsPath = null;
        private FrmDatabaseExplore _frmDBExplorer = null;
        private int _cmdFrmDBExplorerIdx = -1;
        private Bitmap _imgMan = Properties.Resources.DBPPManage16;
        private Icon tbIcon = null;

        #region plugin interface
        public bool isUnicode()
        {
            return true;
        }

        public void setInfo(NppData notepadPlusData)
        {
            this.nppData = notepadPlusData;
            this.InitPlugin();
        }

        public IntPtr getFuncsArray(ref int nbF)
        {
            nbF = this._funcItems.Items.Count;
            return this._funcItems.NativePointer;
        }

        public bool messageProc(uint Message, UIntPtr wParam, IntPtr lParam)
        {
            switch ((WM)Message)
            {
                case WM.Move:
                case WM.Moving:
                case WM.Size:
                case WM.EnterSizeMove:
                case WM.ExitSizeMove:
                    UpdateCurrentSQLResult();
                    break;
                default:
                    break;
            }
            return false;
        }

        //todo implement to free _ptrPluginName in dispose()
        private IntPtr _ptrPluginName = IntPtr.Zero;
        public IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(PluginName);
            return _ptrPluginName;
        }

        public void beNotified(SCNotification nc)
        {
            switch (nc.nmhdr.code)
            {
                case (uint)NppMsg.NPPN_TBMODIFICATION:
                    this._funcItems.RefreshItems();
                    this.SetToolBarIcons();
                    break;
                case (uint)NppMsg.NPPN_SHUTDOWN:
                    this.FinalizePlugin();
                    break;
                case (uint)NppMsg.NPPN_FILECLOSED:
                    this.CloseSQLResult((int)nc.nmhdr.idFrom);
                    break;
                case (uint)NppMsg.NPPN_BUFFERACTIVATED:
                    break;
                case (uint)SciMsg.SCN_PAINTED:
                    this.UpdateCurrentSQLResult();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region initialize and finalize a plugin
        //initialize plugin's command menus  
        private void InitPlugin()
        {
            //plugin configuration

            NppDB.Core.DBServerManager.Instance.NppCommandHost = this;

            StringBuilder sbCfgPath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbCfgPath);
            string plgDir = Path.Combine(sbCfgPath.ToString(), PluginName);

            if (!Directory.Exists(plgDir)) { try { Directory.CreateDirectory(plgDir); } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("plugin dir : " + ex.Message); throw ex; } }

            _cfgPath = Path.Combine(plgDir, "config.xml");
            _dbConnsPath = Path.Combine(plgDir, "dbconnects.xml");
            if (File.Exists(_cfgPath)) { try { Options.Instance.LoadFromXml(_cfgPath); } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("config.xml : " + ex.Message); throw ex; } }
            if (File.Exists(_dbConnsPath)) { try { DBServerManager.Instance.LoadFromXml(_dbConnsPath); } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("dbconnects.xml : "+ ex.Message); throw ex; } }

            SetCommand(0, "Execute SQL", Execute, new ShortcutKey(false, false, false, Keys.F9));
            SetCommand(1, "Close SQL Result", CloseCurrentSQLResult);
            SetCommand(2, "Database Connect Manager", ToggleDBManager);
            _cmdFrmDBExplorerIdx = 2;
            SetCommand(3, "About", ShowAbout);

            //SetCommand(3, "Options", ShowOptions);

        }

        private void SetToolBarIcons()
        {

            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = _imgMan.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[_cmdFrmDBExplorerIdx]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);

        }
        private void FinalizePlugin()
        {
            Marshal.FreeHGlobal(_ptrPluginName);
            try
            {
                Options.Instance.SaveToXml(_cfgPath);
                DBServerManager.Instance.SaveToXml(_dbConnsPath);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("finalize plugin : "+ ex.Message);
            }
        }
        #endregion

        #region plugin commands

        private void Execute()
        {
            int bufID = GetCurrentBufferID();
            var result = SQLResultManager.Instance.GetSQLResult(bufID);
            if (result == null)
            {
                MessageBox.Show("result view opened before executing sql");
                return;
            }
            
            ShowSQLResult(result);
            string sqlQuery = "";
            try
            {
                sqlQuery = GetScintillaText(GetCurrentScintilla());
                if (sqlQuery.Length == 0) return;
            
            }
            catch (Exception ex)
            {
                result.SetError("query : " + ex.Message);
                return;
            }

            result.SetError("");

            result.Execute(sqlQuery);

        }

        private void ExecuteSQL(int bufferID, string query)
        {
            var result = SQLResultManager.Instance.GetSQLResult(bufferID);
            
            result.SetError("");
            result.Execute(query);

        }

        
        private void ToggleDBManager()
        {
            if (_frmDBExplorer == null)
            {
               
                _frmDBExplorer = new FrmDatabaseExplore();
                _frmDBExplorer.AddNotifyHandler(
                    //toogle menu item and toolbar button when docking dialog's close button click
                    (ref Message msg) =>
                    {
                        SCNotification nc = (SCNotification)Marshal.PtrToStructure(msg.LParam, typeof(SCNotification));
                        if (nc.nmhdr.code != (uint)DockMgrMsg.DMN_CLOSE) return;
                        Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[_cmdFrmDBExplorerIdx]._cmdID, 0);
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
                    g.DrawImage(_imgMan, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                NppTbData nppTbData = new NppTbData();
                nppTbData.hClient = _frmDBExplorer.Handle;
                nppTbData.pszName = _funcItems.Items[_cmdFrmDBExplorerIdx]._itemName;
                nppTbData.dlgID = _cmdFrmDBExplorerIdx;
                //default docking
                nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                nppTbData.hIconTab = (uint)tbIcon.Handle;

                nppTbData.pszModuleName = PluginName;
                IntPtr ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(nppTbData));
                Marshal.StructureToPtr(nppTbData, ptrNppTbData, false);

                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, ptrNppTbData);
                //toogle both menu item and toolbar button
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[_cmdFrmDBExplorerIdx]._cmdID, 1);

            }
            else
            {
                var nppMsg = NppMsg.NPPM_DMMSHOW;
                var toggleStatus = 1;
                if (_frmDBExplorer.Visible)
                {
                    nppMsg = NppMsg.NPPM_DMMHIDE;
                    toggleStatus = 0;
                }
                Win32.SendMessage(nppData._nppHandle, nppMsg, 0, _frmDBExplorer.Handle);
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[_cmdFrmDBExplorerIdx]._cmdID, toggleStatus);
            }

        }

        private void ShowOptions()
        {
            var dlg = new frmOption();
            dlg.ShowDialog();
        }

        private void ShowAbout()
        {
            var dlg = new frmAbout();
            dlg.ShowDialog();
        }

        #endregion

        private Control _currentCtr = null;
        internal void UpdateCurrentSQLResult()
        {
            if (SQLResultManager.Instance.Count == 0) return;//don't execute follow when loading 
            int bufID = GetCurrentBufferID();
            if (bufID == 0) return;
            var result = SQLResultManager.Instance.GetSQLResult(bufID);
            if (result == null)
            {
                if (_currentCtr != null) _currentCtr.Visible = false;
                hSplitBar.Visible = false;
                return;
            }
            if (_currentCtr != null && _currentCtr != result && _currentCtr.Visible ) _currentCtr.Visible = false;
            _currentCtr = result;
            SetResultPos(_currentCtr);   
        }

        private Control AddSQLResult(int bufID, IDBConnect connect, ISQLExecutor sqlExecutor)
        {
            var ctr = SQLResultManager.Instance.CreateSQLResult(bufID, connect, sqlExecutor);
            ctr.Height = _defaultSQLResultHeight;
            ctr.Visible = false;//prevent Flicker
            var ret = Win32.SetParent(ctr.Handle, nppData._nppHandle);
            if (ret == null || ret == IntPtr.Zero) System.Windows.Forms.MessageBox.Show("setparent fail");

            if(hSplitBar== null) hSplitBar = CreateSplitBar();

            return ctr;
        }

        bool isDrag = false;
        Control hSplitBar = null; 
        private Control CreateSplitBar()
        {
            var bar = new PictureBox() { Left = 0, Top = 300, Width = 400, Height = 6, Cursor = Cursors.SizeNS, Visible = false };
            Win32.SetParent(bar.Handle, nppData._nppHandle);
            bar.BringToFront();

            int preBarY = 0,
                preScinH = 0,
                preSqlH = 0;

            bar.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left) return;
                isDrag = true;
                preBarY = e.Y;
                RECT recScin;
                IntPtr hndScin = GetCurrentScintilla();
                Win32.GetWindowRect(hndScin, out recScin);
                preScinH = recScin.Bottom - recScin.Top;
                preSqlH = _currentCtr.Height;

                bar.BackColor = SystemColors.ActiveBorder;
                bar.BringToFront();

            };
            bar.MouseMove += (s, e) =>
            {

                if (!isDrag) return;

                RECT recScin;
                IntPtr hndScin = GetCurrentScintilla();
                Win32.GetWindowRect(hndScin, out recScin);
                var y = bar.Top + (e.Y - preBarY);
                if (bar.PointToScreen(new Point(0, y)).Y > recScin.Top + 100)
                    bar.Top = y;
                bar.BringToFront();

            };
            bar.MouseUp += (s, e) =>
            {
                if (!isDrag) return;
                bar.BackColor = SystemColors.ButtonFace;
                bar.BringToFront();

                int key = _currentCtr.Handle.ToInt32();

                RECT recScin;
                IntPtr hndScin = GetCurrentScintilla();
                Win32.GetWindowRect(hndScin, out recScin);

                IntPtr parent = Win32.GetParent(hndScin); //actually parent is nppData._scintillaMainHandle
                Point pRecScin = new Point(recScin.Left, recScin.Top);
                Win32.ScreenToClient(parent, ref pRecScin);

                int viewH = bar.Top - pRecScin.Y;
                int sqlH = preSqlH + (preScinH - viewH);
                int width = recScin.Right - recScin.Left;
                Win32.SetWindowPos(hndScin, IntPtr.Zero, pRecScin.X, pRecScin.Y, width, viewH, SetWindowPosFlags.NoZOrder | SetWindowPosFlags.ShowWindow);
                Win32.SetWindowPos(_currentCtr.Handle, IntPtr.Zero, pRecScin.X, bar.Top + bar.Height, width, sqlH, SetWindowPosFlags.NoZOrder | SetWindowPosFlags.ShowWindow);
                _preViewHeights[key] = viewH;

                isDrag = false;

            };
            return bar;
        }

        private void ShowSQLResult(SQLResult control)
        {
            _currentCtr = control;
            if (!control.Visible)
            {
                SetResultPos(control);
            }
            
            if (! control.LinkedDBConnect.IsOpened)
            {
                control.SetError("this database connection closed. open a database connection again.");
                return;
            }
             
        }

        private void CloseCurrentSQLResult()
        {
            int bufID = GetCurrentBufferID();
            CloseSQLResult(bufID);
        }

        internal void CloseSQLResult(int BufferID)
        {
            var result = SQLResultManager.Instance.GetSQLResult(BufferID);
            if (result == null) return;
            SQLResultManager.Instance.Remove(BufferID);
            _preViewHeights.Remove(result.Handle.ToInt32());
            if (_currentCtr == result) _currentCtr = null;
            hSplitBar.Visible = false;
            ResetViewPos(result);
            Win32.DestroyWindow(result.Handle);
        }

        private void NewFile()
        {
            Win32.SendMessage(nppData._nppHandle, WM.Command, (uint)NppMenuCmd.IDM_FILE_NEW, 0);
        }

        private Dictionary<int, int> _preViewHeights = new Dictionary<int, int>();
        private const int _defaultSQLResultHeight = 200;
        private void SetResultPos(Control control)
        {
            if (isDrag) return;

            int key = control.Handle.ToInt32();
            int preViewH = _preViewHeights.ContainsKey(key) ? _preViewHeights[key] : -1;
            int sqlH = control.Height;

            RECT recScin;
            try
            {
                IntPtr hndScin = GetCurrentScintilla();
                Win32.GetWindowRect(hndScin, out recScin);

                int viewH = recScin.Bottom - recScin.Top;
                if (viewH != preViewH) viewH -= sqlH + hSplitBar.Height;

                IntPtr parent = Win32.GetParent(hndScin); //actually parent is nppData._scintillaMainHandle
                Point p = new Point(recScin.Left, recScin.Top);
                Win32.ScreenToClient(parent, ref p);
                
                Win32.SetWindowPos(hndScin, IntPtr.Zero, p.X, p.Y, recScin.Right - recScin.Left, viewH , SetWindowPosFlags.NoZOrder | SetWindowPosFlags.ShowWindow);
                /*
                hSplitBar.Left = p.X; hSplitBar.Top = p.Y + viewH;hSplitBar.Width = recScin.Right - recScin.Left;
                hSplitBar.Visible = true;
                hSplitBar.BringToFront();
                 * */ 
                Win32.SetWindowPos(hSplitBar.Handle, IntPtr.Zero, p.X, p.Y + viewH, recScin.Right - recScin.Left, hSplitBar.Height,SetWindowPosFlags.ShowWindow);
                Win32.SetWindowPos(control.Handle, IntPtr.Zero, p.X, p.Y + viewH + hSplitBar.Height, recScin.Right - recScin.Left, sqlH, SetWindowPosFlags.NoZOrder | SetWindowPosFlags.ShowWindow);
                control.Visible = true;

                _preViewHeights[key] = viewH;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("setpos : " + ex.Message);
            }

        }

        private void ResetViewPos(Control control)
        {

            int sqlH = control.Height;

            RECT recScin;
            try
            {
                IntPtr hndScin = GetCurrentScintilla();
                Win32.GetWindowRect(hndScin, out recScin);

                IntPtr parent = Win32.GetParent(hndScin); //actually parent is nppData._scintillaMainHandle
                Point p = new Point(recScin.Left, recScin.Top);
                Win32.ScreenToClient(parent, ref p);

                Win32.SetWindowPos(hndScin, IntPtr.Zero, p.X, p.Y, recScin.Right - recScin.Left, recScin.Bottom - recScin.Top + sqlH, SetWindowPosFlags.NoZOrder | SetWindowPosFlags.ShowWindow);
                //System.IO.File.AppendAllText(@"D:\Download\npp.6.5.bin\plugins\nppdb\msg.txt", "p.Y:" + p.Y + " (viewH) :" + (viewH) + " sqlH:" + sqlH + " \n");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("setpos : " + ex.Message);
            }

        }

        private void ActivateBufferID(int bufferID)
        {
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_RELOADBUFFERID, bufferID, 0);
        }

        private int GetCurrentBufferID()
        {
            return Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTBUFFERID, 0, 0).ToInt32();
        }

        private string GetScintillaText(IntPtr scintillaHnd)
        {
            //terminated with '\0'
            int len = Win32.SendMessage(scintillaHnd, SciMsg.SCI_GETSELTEXT, 0, 0).ToInt32();

            int codePage = Win32.SendMessage(scintillaHnd, SciMsg.SCI_GETCODEPAGE, 0, 0).ToInt32();

            List<byte> chars = new List<byte>();

            IntPtr ptrChars = Marshal.AllocHGlobal(len);

            Win32.SendMessage(scintillaHnd, SciMsg.SCI_GETSELTEXT, 0, ptrChars);
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
                throw new ApplicationException("SCI_GETSELTEXT", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrChars);
            }

            string text = "";

            try
            {
                text = Encoding.GetEncoding(codePage).GetString(chars.ToArray());
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

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ApplicationException("a emptry, null or whitesapce text don't was executed");
            }

            return text;
        }

        private void AppendToScintillaText(IntPtr scintillaHnd, string text)
        {
            int codePage = Win32.SendMessage(scintillaHnd, SciMsg.SCI_GETCODEPAGE, 0, 0).ToInt32();
            var bytes = Encoding.GetEncoding(codePage).GetBytes(text);
            IntPtr ptrChars = Marshal.AllocHGlobal(bytes.Length);


            try
            {
                Marshal.Copy(bytes, 0, ptrChars, bytes.Length);
                Win32.SendMessage(scintillaHnd, SciMsg.SCI_APPENDTEXT, bytes.Length , ptrChars);
                //todo selection, scroll
            }
            catch (Exception ex)
            {
                throw new ApplicationException("SCI_APPENDTEXT", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrChars);
            }
        }

        public object Execute(NppDBCommandType type, object[] parameters)
        {
            try
            {
                switch (type)
                {
                    case NppDBCommandType.ActivateBuffer://id
                        ActivateBufferID((int)parameters[0]);
                        break;
                    case NppDBCommandType.AppendToCurrentView:// text
                        AppendToScintillaText(GetCurrentScintilla(), (string)parameters[0]);
                        break;
                    case NppDBCommandType.NewFile://null
                        NewFile();
                        break;
                    case NppDBCommandType.CreateResultView://id, IDBConnect 
                        return AddSQLResult((int)parameters[0], (IDBConnect)parameters[1],(ISQLExecutor) parameters[2] );
                    case NppDBCommandType.ExecuteSQL://id, text 
                        ExecuteSQL((int)parameters[0], (string)parameters[1]);
                        break;
                    case NppDBCommandType.GetActivatedBufferID:
                        return GetCurrentBufferID();
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return null;
        }

    }
}
