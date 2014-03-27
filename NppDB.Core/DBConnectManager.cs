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
            if (_dbConnects.Any(x => x.Name.Equals(dbConnect.Name))) throw new ApplicationException("Connecton Name exists!");
           
            //retrieve DB list
            //retrieve Table listfor each DB 
            _dbConnects.Add(dbConnect);
        }

        public void Unregister(string ConnectionName)
        {
            _dbConnects.Where(x => x.Name.Equals(ConnectionName)).ToList().ForEach(x => _dbConnects.Remove(x));
        }

        //ServerName or ServerName/Database or ServerName/Database/Table etc
        public object FindObject(string[] fullPath)
        {
            try
            {

                if (fullPath.Length == 0 || fullPath.Length == 3)
                {
                    return null;
                }

                var result0 = _dbConnects.Where(x => x.Name.Equals(fullPath[0]));
                if (fullPath.Length == 1)
                {
                    return result0.Count() == 0 ? null : result0.First();
                }

                var result1 = result0.First().Databases.Where(x => x.Name.Equals(fullPath[1]));
                if (fullPath.Length == 2)
                {
                    return result1.Count() == 0 ? null : result1.First();
                }

                if (fullPath[2] == "SystemTables")
                {
                    var result2 = result1.First().SysTables.Where(x => x.Name.Equals(fullPath[3]));
                    if (fullPath.Length == 4)
                    {
                        return result2.Count() == 0 ? null : result2.First();
                    }
                }
                else if (fullPath[2] == "Tables")
                {
                    var result2 = result1.First().Tables.Where(x => x.Name.Equals(fullPath[3]));
                    if (fullPath.Length == 4)
                    {
                        return result2.Count() == 0 ? null : result2.First();
                    }
                }
                else if (fullPath[2] == "Views")
                {
                    var result2 = result1.First().Views.Where(x => x.Name.Equals(fullPath[3]));
                    if (fullPath.Length == 4)
                    {
                        return result2.Count() == 0 ? null : result2.First();
                    }
                }
                else if (fullPath[2] == "StoredProcedures")
                {
                    var result2 = result1.First().StoredProcedures.Where(x => x.Name.Equals(fullPath[3]));
                    if (fullPath.Length == 4)
                    {
                        return result2.Count() == 0 ? null : result2.First();
                    }
                }

                

                /*
                foreach (var path in sepPath)
                {
                    result = _dbConnects.Where(x => x.Name.Equals(path));
                }*/
                return null;
            }
            catch
            {
                return null;
            }
        }

        

        public IDBConnect GetDBConnect(string connectName)
        {
            IDBConnect ret = null;
            try { ret = _dbConnects.First(x => x.Name.Equals(connectName)); } catch { }
            return ret;
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
            StringBuilder strXmlBD = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(strXmlBD);

            foreach (var connect in _dbConnects)
            {
                XmlSerializer serializer = new XmlSerializer(connect.GetType());
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

        public void LoadFromXml(string path)
        {
            
            if (!System.IO.File.Exists(path)) throw new ApplicationException("file not exists : " + path);

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);

            var conns = new List<IDBConnect>();
            foreach(var dbTyp in _dbTypes)
            {
                foreach(XmlNode node in xdoc.SelectNodes(@"//connects/"+dbTyp.ConnectType.Name))
                {
                    XmlSerializer serializer = new XmlSerializer(dbTyp.ConnectType);
                    conns.Add(serializer.Deserialize(new System.IO.StringReader(node.OuterXml)) as IDBConnect);
                }
            }

            _dbConnects.Clear();
            _dbConnects.AddRange(conns);
        }

        public IDBConnect CreateConnect(DatabaseType databaseType)
        {
            return databaseType.ConnectType.Assembly.CreateInstance(databaseType.ConnectType.FullName) as IDBConnect;
        }

        public IEnumerable<DatabaseType> GetDatabaseTypes()
        {
            return _dbTypes;
        }

        private List<DatabaseType> _dbTypes = new List<DatabaseType>();
        private void LoadConnectTypes()
        {
            string dir = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            //System.Windows.Forms.MessageBox.Show(dir);
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
