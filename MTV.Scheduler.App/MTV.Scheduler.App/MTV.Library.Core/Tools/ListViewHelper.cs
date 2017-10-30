using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MTV.Library.Core.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public static class ListViewHelper {
        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54);
        private const int LVM_GETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 55);
        private const int LVS_EX_DOUBLEBUFFER = 0x10000;
        private const int LVS_EX_BORDERSELECT = 0x8000;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        public static void SetPrettyStyle(this ListView listView) {
            var oldStyles = (int)SendMessage(listView.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, (IntPtr)0, (IntPtr)0);
            SendMessage(listView.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, (IntPtr)0, (IntPtr)(oldStyles | LVS_EX_DOUBLEBUFFER));
        }
    }
}
