using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using NppDB;
using NppDB.Comm;

namespace NppDB.Core
{
    public class DBServerManager 
    {
        private List<IDBConnect> _dbConnects = new List<IDBConnect>();

        private DBServerManager()
        {
            LoadConnectTypes();
        }
        
        public void Register(IDBConnect dbConnect)
        {
            if (_dbConnects.Any(x => x.Title.Equals(dbConnect.Title))) throw new ApplicationException("Connecton Name exists!");
           
            //retrieve DB list
            //retrieve Table listfor each DB 
            _dbConnects.Add(dbConnect);
        }

        public void Unregister(IDBConnect dbConnect)
        {
            _dbConnects.Remove(dbConnect);
            List<int> removeIDs = new List<int>();
            foreach (var result in SQLResultManager.Instance.GetSQLResults(dbConnect)) removeIDs.Add(SQLResultManager.Instance.GetID(result));
            removeIDs.ForEach(x=> SQLResultManager.Instance.Remove(x));
        }

        public void Refresh()
        {
            _dbConnects.AsParallel().ForAll((x) =>{try { x.Refresh(); } catch { }});
        }

        public IEnumerable<IDBConnect> Connections { get { return _dbConnects; } }

        
        
        public void SaveToXml(string path)
        {
            var dir = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(path);

            XmlDocument xdoc = new XmlDocument();
            XmlNode xconnects = xdoc.CreateElement("connects");
            xdoc.AppendChild(xconnects);

            foreach (var connect in _dbConnects)
            {
                var sw = new StringWriter(new StringBuilder());
                var serializer = new XmlSerializer(connect.GetType(), GetXmlOver());
                serializer.Serialize(sw, connect);
                sw.Flush();
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(sw.ToString());
                var xcnn = xdoc.ImportNode(xd.DocumentElement, true);
                xcnn.Attributes.RemoveAll();
                xconnects.AppendChild(xcnn);
            }
            xdoc.Save(path);
        }

        //except TreeNode's Property
        private XmlAttributeOverrides GetXmlOver()
        {
            XmlAttributeOverrides xmlOver = new XmlAttributeOverrides();
            foreach (var prop in typeof(System.Windows.Forms.TreeNode).GetProperties())
            {
                xmlOver.Add(typeof(System.Windows.Forms.TreeNode), prop.Name, new XmlAttributes { XmlIgnore = true });
            }
            return xmlOver;
        }

        public void LoadFromXml(string path)
        {
            
            if (!System.IO.File.Exists(path)) throw new ApplicationException("file not exists : " + path);

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);
            
            try
            {
                var conns = new List<IDBConnect>();
                foreach (var dbTyp in _dbTypes)
                {
                    foreach (XmlNode node in xdoc.SelectNodes(@"//connects/" + dbTyp.ConnectType.Name))
                    {
                        XmlSerializer serializer = new XmlSerializer(dbTyp.ConnectType, GetXmlOver());
                        var conn = serializer.Deserialize(new System.IO.StringReader(node.OuterXml)) as IDBConnect;
                        if (NppCommandHost != null && conn is INppDBCommandClient) ((INppDBCommandClient)conn).SetCommandHost(NppCommandHost);
                        conns.Add(conn);
                    }
                }

                _dbConnects.Clear();
                _dbConnects.AddRange(conns);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.InnerException.Message + "\n" + ex.InnerException.StackTrace, (ex.InnerException != null).ToString());
            }
        }

        public INppDBCommandHost NppCommandHost { get; set; }

        public IDBConnect CreateConnect(DatabaseType databaseType)
        {
            var connect = databaseType.ConnectType.Assembly.CreateInstance(databaseType.ConnectType.FullName) as IDBConnect;
            if (NppCommandHost != null && connect is INppDBCommandClient) ((INppDBCommandClient)connect).SetCommandHost(NppCommandHost);
            return connect;
        }

        public IEnumerable<DatabaseType> GetDatabaseTypes()
        {
            return _dbTypes;
        }

        private List<DatabaseType> _dbTypes = new List<DatabaseType>();
        private void LoadConnectTypes()
        {
            string dir = Path.GetDirectoryName(Uri.UnescapeDataString(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath));
            foreach( var filePath in Directory.GetFiles(dir,"*.dll"))
            {

                Assembly assem = null;
                try
                {
                    assem = System.Reflection.Assembly.LoadFrom(filePath);
                }
                catch
                {
                    continue;
                }

                foreach (var typ in assem.GetTypes())
                {
                    if (!typ.IsClass) continue;
                    foreach (var attr in typ.GetCustomAttributes(typeof(ConnectAttr), false))
                    {
                        //System.Windows.Forms.MessageBox.Show(attr.ToString());
                        ConnectAttr cnattr = attr as ConnectAttr;
                        if (cnattr == null) continue;
                        _dbTypes.Add(new DatabaseType { Conn = cnattr, ConnectType = typ });
                    }
                }
            }
            
            
        }


        private static DBServerManager _svrMan = null;
        public static DBServerManager Instance
        {
            get
            {
                if (_svrMan == null) _svrMan = new DBServerManager();
                return _svrMan;
            }
        }


    }

    public class DatabaseType
    {
        public string Id { get { return Conn.Id; } }
        public string Title { get { return Conn.Title; } }
        internal ConnectAttr Conn { get; set; }
        internal Type ConnectType { get; set; }
        public override string ToString()
        {
            return Title;
        }
    }

}
