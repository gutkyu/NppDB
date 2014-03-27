using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NppDB.Comm
{
    public class ConnectAttr : System.Attribute
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}
