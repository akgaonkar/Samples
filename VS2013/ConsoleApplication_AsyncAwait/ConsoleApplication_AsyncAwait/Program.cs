using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication_AsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main thread Id:" + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Before await in Main");
            var returnValue = SlowOperationAsync();
            var returnValue2 = SlowOperationAsync();
            Console.WriteLine("After await in Main");
            Task.WaitAll(returnValue, returnValue2);
            Console.WriteLine(returnValue.Result);
            Console.WriteLine(returnValue2.Result);
            
            Console.WriteLine("Main thread Id:" + Thread.CurrentThread.ManagedThreadId);
            Console.ReadLine();
        }

        private async static Task<int> SlowOperationAsync()
        {
            Console.WriteLine("SlowOperationAsync thread Id:" + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Before await in SlowOperationAsync");
            await Task.Delay(3000);
            Console.WriteLine("SlowOperationAsync thread Id:" + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("After await in SlowOperationAsync");
            return Thread.CurrentThread.ManagedThreadId;
        }
    }
}
