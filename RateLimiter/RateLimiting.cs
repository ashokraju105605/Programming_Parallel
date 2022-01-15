using System;
//using System.DateTime;
using System.Timers;
public class RateLimiting
{
    public static void Main(String[] args)
    {
        Console.WriteLine("Jai Shree Ram");
        RateLimiter rl = new RateLimiter(2, 1, true);
        int packet = 0;
        while (true)
        {
            Thread.Sleep(400);
            rl.allowToProceed(packet++);
        }
    }
    public class RateLimiter
    {
        public int capacity;
        public int availableTokens;
        public int reqlimit;
        public int timelimit;
        public int rate;
        public TimeOnly lastTimeStamp;
        public bool refillbythread;
        public bool timerset;
        public RateLimiter(int reql, int tl, bool refillwiththread)
        {
            capacity = reql;
            availableTokens = reql;
            reqlimit = reql;
            timelimit = tl;
            rate = reqlimit / timelimit;
            lastTimeStamp = TimeOnly.FromDateTime(DateTime.Now);
            refillbythread = refillwiththread;
        }
        public bool allowToProceed(int packet)
        {
            refill();
            int tokenscount = getAvailableTokens();
            if (tokenscount > 0)
            {
                lock (this)
                {
                    tokenscount--;
                    availableTokens--;
                }
                Console.WriteLine(" Token Providing Success " + packet);
                return true;
            }
            else
            {
                Console.WriteLine("Token Providing Fail " + packet);
                return false;
            }

        }
        public int getAvailableTokens()
        {
            return availableTokens;
        }
        public async void refill()
        {
            if (refillbythread)
            {
                if (!timerset)
                {
                    timerset = true;
                    var tmr = new PeriodicTimer(TimeSpan.FromMilliseconds(500));

                    while (await tmr.WaitForNextTickAsync())
                    {
                        lock (this)
                        {
                            Console.WriteLine("running Periodically every 500 seconds");
                            availableTokens = Math.Min(availableTokens + 1, capacity);
                        }
                    }
                }
                return;
            }
            else
            {
                TimeOnly curtime = TimeOnly.FromDateTime(DateTime.Now);
                if (curtime > lastTimeStamp)
                {
                    TimeSpan elapsedTime = curtime - lastTimeStamp;
                    int tokenstobeadded = (int)elapsedTime.TotalSeconds * rate;
                    if (tokenstobeadded > 0)
                    {
                        lock (this)
                        {
                            availableTokens = Math.Min(capacity, availableTokens + tokenstobeadded);
                        }
                        lastTimeStamp = curtime;
                    }
                }
            }
        }
        public bool consumeRequestIfAllowed(int request)
        {
            if (allowToProceed(request))
            {
                Console.WriteLine("request allowed " + request);
                return true;
            }
            else
            {
                Console.WriteLine("Request Block " + request);
                return false;
            }
        }
        public void refillbyThread()
        {
            lock (this)
            {
                availableTokens = Math.Min(availableTokens + 1, capacity);
            }
        }
    }
}