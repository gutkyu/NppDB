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
            foreach (var dbcnn in DBServerManager.Instance.Connections)
            {
                var nodes = trvDBList.Nodes.Find(dbcnn.ServerAddress, false);
                var node = CreateConnectNode(dbcnn.Name, dbcnn.Name);
                trvDBList.Nodes.Add(node);
                node.Nodes.Add(CreateEmptyNode());

            }
            trvDBList.SelectedNode = null;
            btnRegister.Enabled = true;
            btnUnregister.Enabled = !btnRegister.Enabled;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = false;
            btnRefresh.Enabled = false;
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
            IDBConnect dbCnn = null;
            try
            {
                dbCnn = RegisterConnect();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                return;
            }
            if (dbCnn == null) return;
            var nodes = trvDBList.Nodes.Find(dbCnn.Name, false);
            trvDBList.SelectedNode = nodes[0];
        }

        private IDBConnect RegisterConnect()
        {
            var dlg = new frmSelectDbType();
            if (dlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK) return null;
            var selDbType = dlg.SelectedDatabaseType;
            var dbcnn = DBServerManager.Instance.CreateConnect(selDbType);
            if (!dbcnn.CheckLogin()) return null;

            string tmpName = dbcnn.ServerAddress;
            int maxVal = -1;

            var regrex = new System.Text.RegularExpressions.Regex("^" + System.Text.RegularExpressions.Regex.Escape(dbcnn.ServerAddress) + "[ ]*\\([ ]*([0-9]+)[ ]*\\)$");
            try
            {
                maxVal = DBServerManager.Instance.Connections.Where(x => x.Name.StartsWith(tmpName)).Max(x =>
                {
                    var groups = regrex.Match(dbcnn.Name).Groups;
                    return groups.Count > 0 ? int.Parse(groups[0].Value) : -1;
                });
            }
            catch (InvalidOperationException) { }

            dbcnn.Name = dbcnn.ServerAddress + (maxVal == -1 ? "" : "(" + maxVal + 1 + ")");


            dbcnn.Connect();
            DBServerManager.Instance.Register(dbcnn);
            dbcnn.Refresh();

            var nodes = trvDBList.Nodes.Find(dbcnn.ServerAddress, false);
            TreeNode node = null;
            if (nodes.Length == 0)
            {
                node = CreateConnectNode(dbcnn.Name, dbcnn.Name);
                trvDBList.Nodes.Add(node);
            }
            else
                node = nodes[0];

            FillNodes(node, dbcnn);

            node.Expand();

            return dbcnn;
        }

        private bool OpenConnect(IDBConnect connect)
        {
            if (!connect.CheckLogin()) return false;

            connect.Connect();
            connect.Refresh();
            
#if DEBUG
            //DBServerManager.Instance.SaveToXml(@"c:\lastconnect.xml");

#endif

            var nodes = trvDBList.Nodes.Find(connect.ServerAddress, false);
            TreeNode node = null;
            if (nodes.Length == 0)
            {
                node = CreateConnectNode(connect.Name, connect.Name);
                trvDBList.Nodes.Add(node);
            }
            else
                node = nodes[0];
            node.Nodes.Clear();
            FillNodes(node, connect);

            //node.Expand();
            return true;
        }

        private TreeNode CreateConnectNode(string key, string serverName)
        {
            TreeNode node = new TreeNode(serverName) { Name = key };
            return node;
        }

        private void FillNodes(TreeNode node, IDBConnect dbConnect)
        {
            foreach (var db in dbConnect.Databases)
            {
                var dbNode = CreateDBNode(db.Name, db);
                node.Nodes.Add(dbNode);
                foreach (TreeNode subNode in dbNode.Nodes)
                {
                    var emptyNode = CreateEmptyNode();
                    subNode.Nodes.Add(emptyNode);
                }
            }
        }

        private TreeNode CreateDBNode(string key, IDatabase db)
        {
            TreeNode node = new TreeNode(db.Name) { Name = key };
            node.Nodes.Add(new TreeNode("System Tables") { Name = "SystemTables" });
            node.Nodes.Add(new TreeNode("Tables") { Name = "Tables" });
            node.Nodes.Add(new TreeNode("Views") { Name = "Views" });
            node.Nodes.Add(new TreeNode("Stored Procedure") { Name = "StoredProcedures" });
            return node;
        }


        private List<TreeNode> _emptyNode = new List<TreeNode>();
        private TreeNode CreateEmptyNode()
        {
            var node = new TreeNode();
            _emptyNode.Add( node );
            return node;
        }

        private void FillNodes(TreeNode node, IDatabase db)
        {
            FillNodes(node, db.SysTables);
            FillNodes(node, db.Tables);
            FillNodes(node, db.Views);
            FillNodes(node, db.StoredProcedures);
        }

        private void FillNodes(TreeNode node, IEnumerable<ITable> tables)
        {
            foreach (var tbl in tables)
            {
                var tblNode = CreateTableNode(tbl.Name, tbl);
                node.Nodes.Add(tblNode);
                var emptyNode = CreateEmptyNode();
                tblNode.Nodes.Add(emptyNode);
            }
        }

        private void FillNodes(TreeNode node, IEnumerable<IView> views)
        {
            foreach (var view in views)
            {
                var vwNode = CreateViewNode(view.Name, view);
                node.Nodes.Add(vwNode);
                var emptyNode = CreateEmptyNode();
                vwNode.Nodes.Add(emptyNode);
            }
        }

        private void FillNodes(TreeNode node, IEnumerable<IStoredProcedure> storedProcedures)
        {
            foreach (var sp in storedProcedures)
            {
                var spNode = CreateStoredProcedureNode(sp.Name, sp);
                node.Nodes.Add(spNode);
                var emptyNode = CreateEmptyNode();
                spNode.Nodes.Add(emptyNode);
            }
        }

        private TreeNode CreateTableNode(string key, ITable table)
        {
            TreeNode node = new TreeNode(table.Name) { Name = key };
            return node;
        }

        private TreeNode CreateViewNode(string key, IView view)
        {
            TreeNode node = new TreeNode(view.Name) { Name = key };
            return node;
        }

        private TreeNode CreateStoredProcedureNode(string key, IStoredProcedure storedProcedure)
        {
            TreeNode node = new TreeNode(storedProcedure.Name) { Name = key };
            return node;
        }

        private void FillNodes(TreeNode node, ITable table)
        {
            foreach (var col in table.Columns)
            {
                var colNode = CreateColumnNode(col.Name, col);
                node.Nodes.Add(colNode);
            }
        }

        private void FillNodes(TreeNode node, IView view)
        {
            foreach (var col in view.Columns)
            {
                var colNode = CreateColumnNode(col.Name, col);
                node.Nodes.Add(colNode);
            }
        }

        private void FillNodes(TreeNode node, IStoredProcedure storedProcedure)
        {

        }

        private TreeNode CreateColumnNode(string key, IColumnInfo column)
        {
            TreeNode node = new TreeNode(column.Name + " " + column.ColumnType) { Name = key };
            return node;
        }

        private void btnUnregister_Click(object sender, EventArgs e)
        {
            var selNode = trvDBList.SelectedNode;
            if (selNode.Level > 0) return;
            try
            {
                UnregisterConnect();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                return;
            }
            SetButtonEnableForLevel0(selNode);
        }

        private void UnregisterConnect()
        {
            if (trvDBList.SelectedNode.Level > 0) return;
            string serverName = GetKeyFullPath(trvDBList.SelectedNode)[0];
            DBServerManager.Instance.Unregister(serverName);
            trvDBList.Nodes.Remove(trvDBList.SelectedNode);
#if DEBUG
            DBServerManager.Instance.SaveToXml(@"c:\lastconnect.xml");

#endif
        }

        private List<string> GetKeyFullPath(TreeNode node)
        {
            List<string> path = new List<string>();
        PathFirst:
            if (node == null) { return path; }
            path.Insert(0, node.Name);
            node = node.Parent;
            goto PathFirst;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var selNode = trvDBList.SelectedNode;
            if (selNode.Level > 0) return;
            if (selNode.FirstNode != null && _emptyNode.Contains(selNode.FirstNode)) selNode.FirstNode.Text = "Connecting...";
            IDBConnect dbcnn = DBServerManager.Instance.GetDBConnect(GetKeyFullPath(selNode)[0]);
            if (dbcnn == null) { MessageBox.Show("not found " + selNode.Name); selNode.FirstNode.Text = "not found " + selNode.Name;return; }
            try
            {
                OpenConnect(dbcnn);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message: ""));
                if (_emptyNode.Contains(selNode.FirstNode)) selNode.FirstNode.Text = "Try Refresh Again!";
                return;
            }
            SetButtonEnableForLevel0(selNode);
        }
        
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            var selNode = trvDBList.SelectedNode;
            if (selNode.Level > 0) return;
            try
            {
                Disconnect();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                return;
            }
            SetButtonEnableForLevel0(selNode);
        }

        private void Disconnect()
        {
            var selNode = trvDBList.SelectedNode;
            if (selNode.Level > 0) return;
            IDBConnect conn = DBServerManager.Instance.GetDBConnect(GetKeyFullPath(selNode)[0]);
            conn.Disconnect();

            selNode.Nodes.Clear();
            selNode.Nodes.Add(CreateEmptyNode());
            selNode.Collapse();

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (trvDBList.SelectedNode == null) return;
            RefreshTreeNode(trvDBList.SelectedNode);
        }

        private void trvDBList_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void FrmDatabaseExplore_Load(object sender, EventArgs e)
        {

            
        }


        private void trvDBList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_emptyNode.Contains(e.Node)) return;

            switch (e.Node.Level)
            {
                case 0:
                    {
                        SetButtonEnableForLevel0(e.Node);
                        break;
                    }
                case 1:
                    {
                        btnUnregister.Enabled = false;
                        btnConnect.Enabled = false;
                        btnDisconnect.Enabled = false;
                        var dbConn = DBServerManager.Instance.GetDBConnect(e.Node.Parent.Text);
                        btnRefresh.Enabled = dbConn.IsOpened;
                        break;
                    }
                case 2:
                    break;
                case 3:
                    {
                        btnUnregister.Enabled = false;
                        btnConnect.Enabled = false;
                        btnDisconnect.Enabled = false;
                        var dbConn = DBServerManager.Instance.GetDBConnect(e.Node.Parent.Parent.Parent.Text);
                        btnRefresh.Enabled = dbConn.IsOpened;
                        break;
                    }
                case 4:
                    {
                        btnUnregister.Enabled = false;
                        btnConnect.Enabled = false;
                        btnDisconnect.Enabled = false;
                        var dbConn = DBServerManager.Instance.GetDBConnect(e.Node.Parent.Parent.Parent.Parent.Text);
                        btnRefresh.Enabled = dbConn.IsOpened;
                        break;
                    }
            }
        }

        private void SetButtonEnableForLevel0(TreeNode node)
        {
            btnUnregister.Enabled = true;
            var dbConn = DBServerManager.Instance.GetDBConnect(node.Text);
            btnConnect.Enabled = !dbConn.IsOpened;
            btnDisconnect.Enabled = dbConn.IsOpened;
            btnRefresh.Enabled = dbConn.IsOpened;
        }

        private void trvDBList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (_emptyNode.Contains(e.Node)) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                e.Node.ContextMenuStrip = CreateMenu(e.Node);
            }
        }

        private Action<IDBConnect, string> _openHnd = null;
        public void SetOpenDBHandler(Action<IDBConnect, string> handler)
        {
            _openHnd = handler;
        }
        private Action<string[]> _selTabHnd = null;
        public void SetSelectTableHandler(Action<string[]> handler)
        {
            _selTabHnd = handler;
        }

        private ContextMenuStrip CreateMenu(TreeNode node)
        {
            var menus = new ContextMenuStrip();
            if (node.Level == 0)
            {
                var mUnreg = new ToolStripButton("Unregister", null, (s, e) => { UnregisterConnect(); });
                var mConn = new ToolStripButton("Connect", null, (s, e) =>
                {
                    var dbcnn = DBServerManager.Instance.GetDBConnect(GetKeyFullPath(node)[0]);
                    if (dbcnn == null) { MessageBox.Show("not found " + node.Name); return; }
                    if (!dbcnn.IsOpened && dbcnn.Password == null)
                    {
                        try
                        {
                            OpenConnect(dbcnn);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                            return;
                        }
                    }
                });
                mConn.Enabled = !DBServerManager.Instance.GetDBConnect(node.Text).IsOpened;
                var mDisConn = new ToolStripButton("Disconnect", null, (s, e) => { Disconnect(); });
                mDisConn.Enabled = DBServerManager.Instance.GetDBConnect(node.Text).IsOpened;
                menus.Items.Add(mUnreg);
                menus.Items.Add(mConn);
                menus.Items.Add(mDisConn);

            }
            
            menus.Items.Add(new ToolStripButton("Refresh", null, (s, e) => {if( trvDBList.SelectedNode!= null) RefreshTreeNode(trvDBList.SelectedNode); }));

            if (node.Level == 1)
            {
                menus.Items.Add(new ToolStripSeparator());
                menus.Items.Add(new ToolStripButton("Open", null, (s, e) => {
                    var cnnNm = GetKeyFullPath(node)[0];
                    var dbNm = GetKeyFullPath(node)[1];
                    if (_openHnd != null) _openHnd(DBServerManager.Instance.GetDBConnect(cnnNm) as IDBConnect, dbNm);
                }));
            }

            if (node.Level == 3 && ((new string[]{"Tables","SystemTables", "Views"}).Contains(node.Parent.Name)))
            {
                menus.Items.Add(new ToolStripSeparator());
                menus.Items.Add(new ToolStripButton("Select ...", null, (s, e) => {
                    if (_selTabHnd != null) _selTabHnd(GetKeyFullPath(node).ToArray());
                }));
            }


            return menus;
        }

        private void trvDBList_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            
        }

        private void trvDBList_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var selNode = e.Node;
            
            if (selNode.Nodes.Count == 1 && _emptyNode.Contains(selNode.FirstNode))
            {
                var dbcnn = DBServerManager.Instance.GetDBConnect(GetKeyFullPath(selNode)[0]);
                if (dbcnn == null) { MessageBox.Show("not found " + selNode.Name); selNode.Collapse(); return; }
                if (!dbcnn.IsOpened)
                {
                    try
                    {
                        if(!OpenConnect(dbcnn)) selNode.Collapse();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                        selNode.Collapse();
                        return;
                    }

                }else RefreshTreeNode(selNode);

                SetButtonEnableForLevel0(selNode);
            }
        }

        private void RefreshTreeNode(TreeNode node)
        {

            if (node.FirstNode != null && _emptyNode.Contains(node.FirstNode)) node.FirstNode.Text = "Loading...";
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                object obj = DBServerManager.Instance.FindObject(GetKeyFullPath(node.Level == 2? node.Parent :node).ToArray());
                if (obj == null) return;

                var mi = obj.GetType().GetMethod("Refresh");
                object[] param = null;
                if (node.Level == 2) param = new object[] { node.Name };
                if (mi != null && node.Level != 1) obj.GetType().GetMethod("Refresh").Invoke(obj, param);
                
                if (node.Level == 0)
                {
                    node.Nodes.Clear();
                    FillNodes(node, (IDBConnect)obj);
                }
                else if (node.Level == 1)
                {
                    //node.Nodes.Clear();
                    //FillNodes(node, (IDatabase)obj);
                    foreach (TreeNode subNode in node.Nodes)
                    {
                        subNode.Nodes.Clear();
                        subNode.Nodes.Add(CreateEmptyNode());
                        subNode.Collapse();
                    }
                }
                else if (node.Level == 2)
                {
                    node.Nodes.Clear();
                    var db = (IDatabase)obj;
                    var cat = node.Name;
                    if (cat == "Tables")
                    {
                        FillNodes(node, db.Tables);
                    }
                    else if (cat == "SystemTables")
                    {
                        FillNodes(node, db.SysTables);
                    }
                    else if (cat == "Views")
                    {
                        FillNodes(node, db.Views);
                    }
                    else if (cat == "StoredProcedures")
                    {
                        FillNodes(node, db.StoredProcedures);
                    }
                    return;
                }
                else if (node.Level == 3)
                {
                    node.Nodes.Clear();
                    var cat = node.Parent.Name;
                    if (cat == "Tables")
                    {
                        FillNodes(node, (ITable)obj);
                    }
                    else if (cat == "SystemTables")
                    {
                        FillNodes(node, (ITable)obj);
                    }
                    else if (cat == "Views")
                    {
                        FillNodes(node, (IView)obj);
                    }
                    else if (cat == "StoredProcedures")
                    {
                        FillNodes(node, (IStoredProcedure)obj);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
                if (_emptyNode.Contains(node.FirstNode)) node.FirstNode.Text = "Try Refresh Again!";
                return;
            }
            finally
            {
                this.Enabled = true;
                this.Cursor = null;
            }

        }

    }
}
