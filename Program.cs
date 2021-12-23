using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HelloWorld
{
    class Program
    {

        decimal totalBalance = 50000;
        private Object myLock = new Object();
        static Object _lock = new Object();
        static void main(string[] args)
        {
            Task.Factory.StartNew(() => { Console.WriteLine("Jai Shree Ram task factory"); });

            Task task1 = new Task(() => PrintMessage());
            task1.Start();

            Task task2 = new Task(() => { PrintMessage(); });
            task2.Start();

            Task task3 = new Task(new Action(PrintMessage));
            task3.Start();

            Task task4 = new Task(delegate { PrintMessage(); });
            task4.Start();

            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate (object state) { PrintMessage(); }), null);

            ThreadPool.QueueUserWorkItem(Method2, "text");

            Task<double> task5 = Task.Run(() =>
            {
                return CalculateSum(10);
            });
            Console.WriteLine($"Sum is: {task5.Result}");

            Mutex m = new Mutex(); // named Mutexes are OS resource and can be used between processes
            m.WaitOne();
            m.ReleaseMutex();

            Semaphore sem = new Semaphore(2, 3); // named Semaphores are OS resources and can be used between processes.
            sem.WaitOne();   // semaphores are signalling mechanism.
            sem.Release();

            Task<int> task6 = Task.Run(() =>
                                        {
                                            Monitor.Enter(_lock); // Monitor is also similar to lock method
                                            Monitor.Exit(_lock);
                                            return 10;
                                        });
            task6.ContinueWith((i) =>
            {
                Console.WriteLine("TasK Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            task6.ContinueWith((i) =>
            {
                Console.WriteLine("Task Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            var completedTask = task6.ContinueWith((i) =>
            {
                Console.WriteLine("Callback after Task Completed with Result: " + i.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Task task = Task.Run(() =>
            {
                Random randomNumbers = new Random();
                long sum = 0;
                int count = 1000000;
                for (int i = 1; i <= count; i++)
                {
                    int randomNumber = randomNumbers.Next(0, 101);
                    sum += randomNumber;
                }
                Thread.Sleep(1000);
                Console.WriteLine("Total:{0}", sum);
                Console.WriteLine("Count: {0}", count);

                return 5;
            });
            task.Wait();  // blocking wait construct.

            Console.WriteLine("result returned by task: ");

            Thread thread1 = new Thread(Method1);
            thread1.Start();

            Thread thread2 = new Thread(Method2);
            thread2.Start();

            thread1.Join(); // blocking method to wait for thread.
            Console.WriteLine("After Thread1");

            thread2.Join();
            Console.WriteLine("After Thread2");


            Program program = new Program();
            program.WithDraw(5000);


            int result = 0;
            Thread thread = new System.Threading.Thread(() =>
            {
                result = 1;
            });
            thread.Start();
            thread.Join(); //Blocks the calling thread until the thread terminates (is done) 
            Console.WriteLine("from Main method result : " + result); //is 1


            var sw = new Stopwatch(); //
            sw.Start();
            Task delay = Task.Delay(5000);
            Console.WriteLine("async: Running for {0} seconds", sw.Elapsed.TotalSeconds);
            //await delay;
            delay.Wait();

            Task t = TestMethod();
            t.Wait(); // if we don't apply the wait here main thread will go to completion 
                      //and this background thread will keep running


        }
        static double CalculateSum(int num)
        {
            double sum = 0;
            for (int count = 1; count <= num; count++)
            {
                sum += count;
            }
            return sum;
        }

        static void PrintMessage()
        {
            Console.WriteLine("Testing Printing Message");
        }
        public static async Task TestMethod()
        {
            int result = await Task.Run(() =>
                            {
                                Task.Delay(5000);
                                return 1;
                            });
            Console.WriteLine("From TestMethod: " + result); //is 1
        }

        private static void Method2(object obj)
        {
            Thread.Sleep(3000); // doesn't consume CPU, but a blocking method 
            Console.WriteLine("Thread1 Executed. Argument: " + obj);
        }

        private static void Method1(object obj)
        {
            Console.WriteLine("Thread2 Executed");
        }


        public void WithDraw(decimal amount)
        {
            lock (myLock) // all threads need to wait on same lock object.
            {
                if (amount > totalBalance)
                {
                    Console.WriteLine("Insufficient Amount.");
                }

                totalBalance -= amount;
                Console.WriteLine("Total Balance {0}", totalBalance);
            }
        }
    }
}


/* 

Thread Synchronization Deals with the following conditions:
Deadlock
Starvation
Priority Inversion
Busy Waiting



Synchronization is handled with the following four categories:  

The following are the four categories to handle Synchronization mechanism:

Blocking Methods -- Thread.Sleep(1000), thrd.Join(), tsk.Wait();

Locking Construct -- lock(lockobj), Monitor.Enter(lockobj), Monitor.Exit(lockobj)

Signaling -- mtx.WaitOne(), mtx.ReleaseMutex(); sem.WaitOne(), sem.Release();

No Blocking Synchronization

*/