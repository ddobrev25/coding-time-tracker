using Coding_Time_Tracker;
using System.Diagnostics;


string fileName = "file.cttf";
FileManager file = new FileManager(fileName);
TimeSpan checkDelay = TimeSpan.FromMilliseconds(100); //The delay between application running checks. The lower the check delay, the more accurate the calculations. But worse performance.
TimeSpan fileUpdateDelay = TimeSpan.FromSeconds(1); //How often to update the file. The lower, the more accurate. But worse performance.
_ = StartApplicationMonitoring(checkDelay, fileUpdateDelay);

Thread.Sleep(999999999);


async Task StartApplicationMonitoring(TimeSpan checkDelay, TimeSpan fileUpdateDelay)
{
    // Initialize application monitor object.
    ApplicationMonitor monitor = new ApplicationMonitor(Application.VS2022, Application.VSCode);

    // Declare time running variable and set it to a starting value of 0.
    TimeSpan elapsedTimeRunning = TimeSpan.Zero;

    while (true)
    {
        //Asynchronously delay the checks for the provided period.
        await Task.Delay(checkDelay);


        //If no application is running, continue and check again after the provided check delay.
        if (!monitor.IsAnyApplicationRunning())
        {
            continue;
        }

        //If at least one application is running, increase the running time by the check delay.
        elapsedTimeRunning += checkDelay;

        //If the time running has not reached the update delay, continue.
        if (elapsedTimeRunning <= fileUpdateDelay)
        {
            continue;
        }

        //If the time running has exceeded the update delay, it is time to update the value in the file.

        //If the key has not yet been written to the file, write it with the current time running value.
        if (!file.KeyExistsInFile(LineKey.TotalTime))
        {
            file.WriteValue(LineKey.TotalTime, elapsedTimeRunning.ToString());
        }

        // Else if there is a value already, update it.
        else
        {
            //Read the current total time from the file.
            string totalTime = file.ReadValue(LineKey.TotalTime);

            //Parse it to a time span.
            TimeSpan currentValue = TimeSpan.Parse(totalTime);

            //Calculate the new value by adding the current value and the time running.
            TimeSpan newValue = currentValue + elapsedTimeRunning;

            //Write the new value to the file.
            file.WriteValue(LineKey.TotalTime, newValue.ToString());
        }

        //Annul the time running value
        elapsedTimeRunning = TimeSpan.Zero;
    }
}