using System;
//using System.DateTime;
public class RateLimiting
{
    public static void Main(String[] args)
    {
        Console.WriteLine("Jai Shree Ram");
        RateLimiter rl = new RateLimiter(2, 1);
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
        public RateLimiter(int reql, int tl)
        {
            capacity = reql;
            availableTokens = reql;
            reqlimit = reql;
            timelimit = tl;
            rate = reqlimit / timelimit;
            lastTimeStamp = TimeOnly.FromDateTime(DateTime.Now);
        }
        public bool allowToProceed(int packet)
        {
            refill();
            int tokenscount = getAvailableTokens();
            if (tokenscount > 0)
            {
                tokenscount--;
                availableTokens--;
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
        public void refill()
        {
            TimeOnly curtime = TimeOnly.FromDateTime(DateTime.Now);
            if (curtime > lastTimeStamp)
            {
                TimeSpan elapsedTime = curtime - lastTimeStamp;
                int tokenstobeadded = (int)elapsedTime.TotalSeconds * rate;
                if (tokenstobeadded > 0)
                {
                    availableTokens = Math.Min(capacity, availableTokens + tokenstobeadded);
                    lastTimeStamp = curtime;
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
            availableTokens = Math.Min(availableTokens + 1, capacity);
        }
    }
}