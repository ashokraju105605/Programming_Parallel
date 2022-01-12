using System;
using System.Timers;
//using System.Threading;
class TimerJobs
{
    static System.Timers.Timer timer;
    private static System.Timers.Timer aTimer;

    static int count;
    public static void main(String[] args)
    {
        Console.WriteLine("Jai Shree Ram From Timers");
        //Schedule_timer();


        SetTimer();

        Console.WriteLine("\nPress the Enter key to exit the application...\n");
        Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
        // Console.ReadLine();
        // aTimer.Stop();
        // aTimer.Dispose();
        Thread.Sleep(10000);  // main thread exit means background threads also will be lost.
        Console.WriteLine("Terminating the application...");
    }
    static async void Schedule_timer()
    {

        await Method1(); // will just go ahead with rest of function.
        //Method1().Wait(); // -- will wait for the async task to finish
        DateTime newt = DateTime.Now;
        //DateTime schT = new DateTime(DateTime.Now.AddSeconds(10));

        timer = new System.Timers.Timer();
    }

    static async Task Method1()
    {
        Console.WriteLine("Entered Method1");
        var tmr = new PeriodicTimer(TimeSpan.FromSeconds(2));

        while (await tmr.WaitForNextTickAsync())
        {
            Console.WriteLine("running Periodically every 2 seconds");
        }
    }
    private static void SetTimer()
    {
        // Create a timer with a two second interval.
        aTimer = new System.Timers.Timer(2000);
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }
    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        count++;
        Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                          e.SignalTime);
        if (count == 5)
        {
            aTimer.Stop();
            aTimer.Dispose();
        }
    }
}