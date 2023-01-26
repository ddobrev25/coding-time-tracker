using Coding_Time_Tracker;
using Coding_Time_Tracker.Services;





string fileName = "file.cttf";

FileService file = new FileService(fileName);
Console.WriteLine(file.WriteFileProperty(LineProperties.TotalTime, "1h")); 
//Console.WriteLine(file.ReadLineProperty(LineProperties.TotalTime));
