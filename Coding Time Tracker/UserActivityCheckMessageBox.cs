using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Coding_Time_Tracker
{
    public class UserActivityCheckMessageBox
    {
        public static MessageBoxAnswer Show()
        {
            int result = MessageBox(IntPtr.Zero, "This is a periodic check to see if you are still using " +
                "the monitored applications. WARNING: *Please save all your work. " +
                "Pressing \"No\" will shut down all monitored applications.*", "Are you still coding?", 4);
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




        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr h, string m, string c, int type);
    }
}
