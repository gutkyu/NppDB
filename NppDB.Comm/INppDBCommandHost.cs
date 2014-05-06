using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NppDB.Comm
{
    public interface INppDBCommandHost
    {
        object Execute(NppDBCommandType type, object[] parameters);
    }
}
