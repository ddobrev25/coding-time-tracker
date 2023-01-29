using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Coding_Time_Tracker
{
    /// <summary>
    /// Helper for showing message boxes.
    /// </summary>
    public static class MessageBoxHelper
    {

        /// <summary>
        /// Shows a message box asking the user if they are still actively using the monitored applications.
        /// </summary>
        /// <returns>The answer of the user.</returns>
        public static MessageBoxAnswer ShowActivityCheckMessageBox()
        {
            int result = MessageBox(IntPtr.Zero, "This is a periodic check to see if you are still using " +
                "the monitored applications. WARNING: *Please save all your work. " +
                "Pressing \"No\" will shut down all monitored applications.*", "Are you still coding?", MESSAGEBOX_TYPE_YESNO);
            switch (result)
            {
                case (int)MessageBoxAnswer.Yes:
                    return MessageBoxAnswer.Yes;
                case (int)MessageBoxAnswer.No:
                    return MessageBoxAnswer.No;
                default:
                    return MessageBoxAnswer.No;
            }
        }

        private const int MESSAGEBOX_TYPE_YESNO = 0x4;

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr h, string m, string c, int type);
    }
}
