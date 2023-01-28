using Coding_Time_Tracker;
using Coding_Time_Tracker.Services;
using System.Diagnostics;

string fileName = "file.cttf";

List<ApplicationToMonitor> targetApps = new List<ApplicationToMonitor>();
targetApps.Add(ApplicationToMonitor.VS2022);
targetApps.Add(ApplicationToMonitor.VSCode);
ApplicationMonitor processMonitor = new ApplicationMonitor(targetApps);
Console.ReadKey();