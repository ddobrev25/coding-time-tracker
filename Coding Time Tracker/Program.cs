using Coding_Time_Tracker;
using System.Diagnostics;
using System.Runtime.InteropServices;

string fileName = "file.cttf";


//Initialize file manager object.
FileManager file = new FileManager(fileName);

// Initialize application monitor object.
ApplicationMonitor monitor = new ApplicationMonitor(Application.VSCode, Application.VS2022);


//The delay between checks of whether applications are running.
//The lower the check delay, the more accurate the calculations, but the worse the performance.
TimeSpan checkDelay = TimeSpan.FromMilliseconds(100);


//How often to update the file. The lower, the more accurate but the worse the performance.
TimeSpan fileUpdateDelay = TimeSpan.FromSeconds(1);


//How often the user will be checked if they are still active.
TimeSpan userActivityCheckDelay = TimeSpan.FromMinutes(30);

//Initialize periodic trigger object.
PeriodicTrigger userActivityCheckTrigger = new PeriodicTrigger(userActivityCheckDelay);

//Subscribe for the triggered event.
userActivityCheckTrigger.Triggered += UserActivityCheckTrigger_Triggered;

//Start the monitorig.
await StartApplicationMonitoring(checkDelay, fileUpdateDelay);



void UserActivityCheckTrigger_Triggered()
{

    //If there is no application running, no need to show the message.
    if (!monitor.IsAnyApplicationRunning())
    {
        return;
    }

    //Show the user a message box asking whether they are stll coding or not and save the answer to a variable.
    MessageBoxAnswer answer = MessageBoxHelper.ShowActivityCheckMessageBox();

    //If the answer is not no then return and continue normal execution.
    if (answer != MessageBoxAnswer.No)
    {
        return;
    }

    //This code is reached only if the answer was no. In that case close all monitored applications, as they are not used.

    //Loop through all monitored applications.
    foreach (var app in monitor.Applications)
    {
        //Get the process name of the application.
        string processName = monitor.GetApplicationProcessName(app);

        //Find the process instance running with the given process name.
        var process = Process.GetProcessesByName(processName).FirstOrDefault();

        //If the process is not null, kill it.
        process?.Kill();
    }
}

async Task StartApplicationMonitoring(TimeSpan checkDelay, TimeSpan fileUpdateDelay)
{

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