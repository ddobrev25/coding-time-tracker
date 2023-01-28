using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding_Time_Tracker
{
    /// <summary>
    /// Enumeration of possible line properties.
    /// </summary>
    public enum LineProperty 
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None,


        /// <summary>
        /// Total time the target applications have run.
        /// </summary>
        TotalTime,
    }


    /// <summary>
    /// Enumeration of target apps for monitoring.
    /// </summary>
    public enum ApplicationToMonitor
    {
        /// <summary>
        /// Visual Studio 2022.
        /// </summary>
        VS2022,


        /// <summary>
        /// Visual Studio Code.
        /// </summary>
        VSCode,
    }
}
