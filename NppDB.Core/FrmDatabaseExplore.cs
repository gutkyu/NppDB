using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NppDB;
using NppDB.Comm;


namespace NppDB.Core
{

    public partial class FrmDatabaseExplore : Form
    {
        public FrmDatabaseExplore()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            trvDBList.ImageList = new ImageList() { ColorDepth = ColorDepth.Depth32Bit };
            trvDBList.ImageList.Images.Add(NppDB.Core.Properties.Resources.bullet);
            trvDBList.ImageList.Images.Add("Group", NppDB.Core.Properties.Resources.Folder);
            trvDBList.ImageList.Images.Add("Database", NppDB.Core.Properties.Resources.Database);
            trvDBList.ImageList.Images.Add("Table", NppDB.Core.Properties.Resources.Table);

            foreach (var dbcnn in DBServerManager.Instance.Connections)
            {
                var node = dbcnn as TreeNode;
                var id = DBServerManager.Instance.GetDatabaseTypes().First(x => x.ConnectType == dbcnn.GetType()).Id;
                SetTreeNodeImage(node, id);
            
                trvDBList.Nodes.Add((TreeNode)dbcnn);
            }

            btnRegister.Enabled = true;
            btnUnregister.Enabled = false;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = false;
            btnRefresh.Enabled = false;
            
            if (trvDBList.TopNode != null)
                trvDBList.ItemHeight = trvDBList.TopNode.Bounds.Height + 4;
            
            trvDBList.ShowNodeToolTips = true;
        }

        private List<NotifyHandler> _notifyHnds = new List<NotifyHandler>();
        public void AddNotifyHandler(NotifyHandler handler)
        {
            _notifyHnds.Add(handler);
        }

        protected override void WndProc(ref Message m)
        {
            if (_notifyHnds.Count > 0 && m.Msg == 0x4e)//WM_NOTIFY
                foreach (var hnd in _notifyHnds) hnd(ref m);
            
            base.WndProc(ref m);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                RegisterConnect();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                return;
            }
        }

        private void RegisterConnect()
        {
            var dlg = new frmSelectDbType();
            if (dlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK) return;
            var selDbType = dlg.SelectedDatabaseType;
            var dbcnn = DBServerManager.Instance.CreateConnect(selDbType);
            if (!dbcnn.CheckLogin()) return;

            string tmpName = dbcnn.GetDefaultTitle();
            int maxVal = -1;

            var regrex = new System.Text.RegularExpressions.Regex("^" + System.Text.RegularExpressions.Regex.Escape(dbcnn.ServerAddress) + "[ ]*\\([ ]*([0-9]+)[ ]*\\)$");
            try
            {
                maxVal = DBServerManager.Instance.Connections.Where(x => x.Title.StartsWith(tmpName)).Max(x =>
                {
                    var groups = regrex.Match(dbcnn.Title).Groups;
                    return groups.Count > 0 ? int.Parse(groups[0].Value) : -1;
                });
            }
            catch (InvalidOperationException) { }

            dbcnn.Title = tmpName + (maxVal == -1 ? "" : "(" + maxVal + 1 + ")");

            dbcnn.Connect();
            DBServerManager.Instance.Register(dbcnn);
            var id = selDbType.Id;
            var node = dbcnn as TreeNode;

            SetTreeNodeImage(node, id);

            trvDBList.Nodes.Add(node);

            if (trvDBList.TopNode != null && trvDBList.ItemHeight != trvDBList.TopNode.Bounds.Height + 4)
                trvDBList.ItemHeight = trvDBList.TopNode.Bounds.Height + 4;

            dbcnn.Refresh();

            ((TreeNode)dbcnn).Expand();
        }

        private void SetTreeNodeImage(TreeNode node, string id)
        {
            var iconProvider = node as IIconProvider;
            if(!trvDBList.ImageList.Images.ContainsKey(id)) 
                trvDBList.ImageList.Images.Add(id, iconProvider.GetIcon());
            node.SelectedImageKey = node.ImageKey = id;
        }

        private void btnUnregister_Click(object sender, EventArgs e)
        {
            UnregisterConnect();

        }

        private void UnregisterConnect()
        {
            if (trvDBList.SelectedNode == null || trvDBList.SelectedNode.Level > 0) return;
            DBServerManager.Instance.Unregister((IDBConnect)trvDBList.SelectedNode);
            trvDBList.Nodes.Remove(trvDBList.SelectedNode);

            btnRegister.Enabled = true;
            btnUnregister.Enabled = false;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = false;
            btnRefresh.Enabled = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var dbconn = trvDBList.SelectedNode as IDBConnect;
            if (dbconn == null) return;
            try
            {
                if (!dbconn.CheckLogin()) return;

                dbconn.Connect();
                dbconn.Refresh();

                ((TreeNode)dbconn).Expand();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message: ""));
            }
        }
        
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            var dbconn = trvDBList.SelectedNode as IDBConnect;
            if (dbconn == null) return;
            dbconn.Disconnect();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (trvDBList.SelectedNode == null) return;
            var refreshable = trvDBList.SelectedNode as  IRefreshable;
            if (refreshable == null) return;
            refreshable.Refresh();
        }

        private void trvDBList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnUnregister.Enabled = e.Node is IDBConnect;
            var dbconn = GetRootParent(e.Node) as IDBConnect;
            btnConnect.Enabled = e.Node is IDBConnect && !dbconn.IsOpened;
            btnDisconnect.Enabled = e.Node is IDBConnect && dbconn.IsOpened;
            btnRefresh.Enabled = e.Node is IRefreshable && dbconn.IsOpened ;

        }

        private void trvDBList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                e.Node.ContextMenuStrip = CreateMenu(e.Node);
            }
        }

        private TreeNode GetRootParent(TreeNode node)
        {
            if (node.Parent == null) return node;
            return GetRootParent(node.Parent);
        }

        private void trvDBList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            IRefreshable r = null;
            if (e.Button != System.Windows.Forms.MouseButtons.Left || (r = e.Node as IRefreshable) == null) return;
            var dbconn = GetRootParent(e.Node) as IDBConnect;
            if ( e.Node.Nodes.Count == 0)
            {
                if (!dbconn.IsOpened)
                {
                    try
                    {
                        if (!dbconn.CheckLogin()) return;
                        dbconn.Connect();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                        return;
                    }
                }
                r.Refresh();
            }
            e.Node.Expand();
        }

        private ContextMenuStrip CreateMenu(TreeNode node)
        {
            var menuCreator = node as IMenuProvider;
            if (menuCreator == null) return null;

            var menu = menuCreator.GetMenu();

            if (node is IDBConnect)
            {
                var mUnreg = new ToolStripButton("Unregister", null, (s, e) => { UnregisterConnect(); });

                menu.Items.Insert(0, mUnreg);
                menu.Items.Insert(1, new ToolStripSeparator());
            }

            return menu;
        }
  
    }
}
