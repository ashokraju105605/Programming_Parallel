using System;
//using System.Timers.Timer;
using System.Threading;
class TimerJobs
{
    static Timer timer;
    public static void Main(String[] args)
    {
        Console.WriteLine("Jai Shree Ram From Timers");
        Schedule_timer();
    }
    static void Schedule_timer()
    {

        Method1().Wait();

        DateTime newt = DateTime.Now;
        //DateTime schT = new DateTime(DateTime.Now.AddSeconds(10));

        //timer = new Timer()
    }

    static async Task Method1()
    {
        var tmr = new PeriodicTimer(TimeSpan.FromSeconds(2));

        while (await tmr.WaitForNextTickAsync())
        {
            Console.WriteLine("running Periodically every 2 seconds");
        }
    }
}