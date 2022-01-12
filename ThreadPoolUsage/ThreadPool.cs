using System;
using System.Collections.Generic;
using System.Text;
//Sample 01: Required Namespace
using System.Threading;
namespace Thread_Pool
{
    class Program
    {
        //Sample 02: Define Task/Wait Callback function
        private static void TaskCallBack(Object ThreadNumber)
        {
            string ThreadName = "Thread " + ThreadNumber.ToString();

            for (int i = 1; i < 10; i++)
                Console.WriteLine(ThreadName + ": " + i.ToString());

            Console.WriteLine(ThreadName + " Finished...");
        }
        static void main(string[] args)
        {
            //Sample 03: Create Thread Pool
            for (int task = 1; task < 51; task++)
                ThreadPool.QueueUserWorkItem(new WaitCallback(TaskCallBack), task);

            Thread.Sleep(10000);
        }
    }
}