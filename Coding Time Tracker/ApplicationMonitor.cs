using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Coding_Time_Tracker
{
    /// <summary>
    /// Allows monitoring whether applications are running or not.
    /// </summary>
    public class ApplicationMonitor
    {
        /// <summary>
        /// Sets the target applications that will be monitored.
        /// </summary>
        /// <param name="applicationsToMonitor">The applications that will be monitored.</param>
        public ApplicationMonitor(ApplicationToMonitor[] applicationsToMonitor)
        {
            foreach (var targetApp in applicationsToMonitor)
            {
                _processNamesToMonitor.Add(_appToProcessNameMapping[targetApp]);
            }
        }


        /// <summary>
        /// Sets the target applications that will be monitored.
        /// </summary>
        /// <param name="applicationsToMonitor">The applications that will be monitored.</param>
        public ApplicationMonitor(List<ApplicationToMonitor> applicationsToMonitor)
        {
            foreach (var targetApp in applicationsToMonitor)
            {
                _processNamesToMonitor.Add(_appToProcessNameMapping[targetApp]);
            }
        }


        /// <summary>
        /// Current applications that are monitored.
        /// </summary>
        public List<ApplicationToMonitor> CurrentApplications
        {
            get
            {
                List<ApplicationToMonitor> apps = new List<ApplicationToMonitor>();
                foreach (string processName in _processNamesToMonitor)
                {
                    apps.Add(_appToProcessNameMapping.FirstOrDefault(x => x.Value == processName).Key);
                }
                return apps;
            }
        }



        /// <summary>
        /// Checks whether any of the given applications is currently running.
        /// </summary>
        /// <returns>Whether any of the given applications is currently running.</returns>
        public bool IsAnyApplicationRunning()
        {
            foreach (string processName in _processNamesToMonitor)
            {
                if (IsProcessRunning(processName))
                {
                    return true;
                }
            }
            return false;
        }
        private readonly Dictionary<ApplicationToMonitor, string> _appToProcessNameMapping = new Dictionary<ApplicationToMonitor, string>()
        {
            { ApplicationToMonitor.VS2022, "devenv" },
            { ApplicationToMonitor.VSCode, "code"},
        };
        private List<string> _processNamesToMonitor = new List<string>();

        private bool IsProcessRunning(string processName) => Process.GetProcessesByName(processName).Length > 0;

    }
}
