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
        public ApplicationMonitor(params Application[] applicationsToMonitor)
        {
            Applications = new List<Application>();
            foreach (var targetApp in applicationsToMonitor)
            {
                Applications.Add(targetApp);
            }
        }


        /// <summary>
        /// Sets the target applications that will be monitored.
        /// </summary>
        /// <param name="applicationsToMonitor">The applications that will be monitored.</param>
        public ApplicationMonitor(List<Application> applicationsToMonitor)
        {
            Applications = new List<Application>();
            foreach (var targetApp in applicationsToMonitor)
            {
                Applications.Add(targetApp);
            }
        }



        /// <summary>
        /// Adds an application to the list of monitored applications.
        /// </summary>
        /// <param name="applicationToAdd">The application to add.</param>
        public void AddApplication(Application applicationToAdd)
        {
            Applications.Add(applicationToAdd);
        }



        /// <summary>
        /// Removes an application from the list of monitored applications.
        /// </summary>
        /// <param name="applicationToRemove">The application to remove.</param>
        public void RemoveApplication(Application applicationToRemove)
        {
            Applications.Remove(applicationToRemove);
        }



        /// <summary>
        /// Current applications that are monitored.
        /// </summary>
        public List<Application> Applications { get; }




        /// <summary>
        /// Checks whether a given application is running.
        /// </summary>
        /// <param name="application">The application to check for.</param>
        /// <returns>Whether the application is running.</returns>
        public bool IsApplicationRunning(Application application)
        {
            return IsProcessRunning(_processNameToApplicationMapping[application]);
        }



        /// <summary>
        /// Checks whether at least one of the provided applications is currently running.
        /// </summary>
        /// <returns>Whether at least one of the provided applications is currently running.</returns>
        public bool IsAnyApplicationRunning()
        {
            if (Applications.Count == 0)
            {
                return false;
            }

            foreach (var app in Applications)
            {
                string processName = _processNameToApplicationMapping[app];
                if (IsProcessRunning(processName))
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Checks whether all the provided applications are running.
        /// </summary>
        /// <returns>Whether all the provided applications are running.</returns>
        public bool AreAllApplicationsRunning()
        {
            if (Applications.Count == 0)
            {
                return false;
            }
            foreach (var app in Applications)
            {
                string processName = _processNameToApplicationMapping[app];
                if (!IsProcessRunning(processName))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Mapping of process names to applications.
        /// </summary>
        private readonly Dictionary<Application, string> _processNameToApplicationMapping = new Dictionary<Application, string>()
        {
            { Application.VS2022, "devenv" },
            { Application.VSCode, "code"},
        };


        /// <summary>
        /// Checks if a process is running given a process name.
        /// </summary>
        /// <param name="processName">The process name.</param>
        /// <returns>Whether a process is running.</returns>
        private bool IsProcessRunning(string processName) => Process.GetProcessesByName(processName).Length > 0;

    }
}
