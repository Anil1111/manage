using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AwaitAsyncLibrary
{
    /// <summary>
    /// await/async关键字
    /// 任何一个方法 都可以增加async
    /// await 放在task前面     一般成对出现  只有async是没有意义的  只有await是报错的
    /// await/async 要么不用  要么用到底
    /// </summary>
    public class AwaitAsyncILSpy
    {
        public static void Show()
        {
            Console.WriteLine($"start {Thread.CurrentThread.ManagedThreadId.ToString("00")}");
            Async();
            Console.WriteLine($"aaa {Thread.CurrentThread.ManagedThreadId.ToString("00")}");
        }

        static async void Async()
        {
            Console.WriteLine($"ddd {Thread.CurrentThread.ManagedThreadId.ToString("00")}");
            await Task.Run(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine($"bbb {Thread.CurrentThread.ManagedThreadId.ToString("00")}");
            });
            Console.WriteLine($"ccc {Thread.CurrentThread.ManagedThreadId.ToString("00")}");
        }
    }


}
