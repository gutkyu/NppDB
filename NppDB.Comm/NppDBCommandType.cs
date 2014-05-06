using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NppDB.Comm
{
    public enum NppDBCommandType
    {
        NewFile,
        CreateResultView,
        GetActivatedBufferID,
        ExecuteSQL,
        AppendToCurrentView,
        ActivateBuffer
    }

}
