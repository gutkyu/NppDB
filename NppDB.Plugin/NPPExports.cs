using System;
using System.Runtime.InteropServices;


namespace NppDB
{
    public class NPPExports
    {
        public static bool isUnicode()
        {
            return true;
        }

        public static void setInfo(NppData notepadPlusData)
        {
            PluginBase.nppData = notepadPlusData;
            PluginBase.CommandMenuInit();
        }

        public static IntPtr getFuncsArray(ref int nbF)
        {
            nbF = PluginBase._funcItems.Items.Count;
            return PluginBase._funcItems.NativePointer;
        }

        public static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        static IntPtr _ptrPluginName = IntPtr.Zero;
        public static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(PluginBase.PluginName);
            return _ptrPluginName;
        }

        public static void beNotified(SCNotification nc)
        {

            if (nc.nmhdr.code == (uint)NppMsg.NPPN_TBMODIFICATION)
            {
                PluginBase._funcItems.RefreshItems();
                PluginBase.SetToolBarIcon();
            }
            else if (nc.nmhdr.code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                PluginBase.PluginCleanUp();
                Marshal.FreeHGlobal(_ptrPluginName);
            }
        }

        public static string PluginName { get { return PluginBase.PluginName; } }
    }
}
