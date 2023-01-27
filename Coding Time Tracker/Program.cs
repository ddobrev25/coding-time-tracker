using Coding_Time_Tracker;
using Coding_Time_Tracker.Services;




string fileName = "file.cttf";
TimeTrigger tt = new TimeTrigger(5);
tt.Triggered += () =>
{
    Console.WriteLine("triggered");
};
Thread.Sleep(7000);
Thread.Sleep(100000);