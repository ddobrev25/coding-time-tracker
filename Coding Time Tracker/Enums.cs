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
    public enum LineKey 
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None,


        /// <summary>
        /// Total time the target applications have run.
        /// </summary>
        TotalTime,



        /// <summary>
        /// The time that the file was created.
        /// </summary>
        TimeCreated,
    }


    /// <summary>
    /// Enumeration of target apps for monitoring.
    /// </summary>
    public enum Application
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

    public enum MessageBoxAnswer
    {
        Yes = 6,
        No = 7,
    }
}
