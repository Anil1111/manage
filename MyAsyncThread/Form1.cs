using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAsyncThread
{
    /// <summary>
    /// 什么时候用多线程？ 任务能并发运行；提升速度；优化体验
    /// 
    /// 进程：一个程序运行时，占用的全部计算资源的总和
    /// 线程：程序执行流的最小单位；任何操作都是由线程完成的；
    ///       线程是依托于进程存在的，一个进程可以包含多个线程；
    ///       线程也可以有自己的计算资源
    /// 多线程：多个执行流同时运行
    /// 是对方法执行的描述
    /// 同步：完成计算之后，再进入下一行
    /// 异步：不会等待方法的完成，会直接进入下一行  非阻塞
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static readonly object Lock = new object();
        /// <summary>
        /// 3.0 Task 是基于ThreadPool  Task增加了多个API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTask_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"****************btnTask_Click Start {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            TaskFactory taskFactory = new TaskFactory();
            List<Task> taskList = new List<Task>
            {
                 taskFactory.StartNew(o => this.Coding("爱书客", "Client"), "爱书客"),
                 taskFactory.StartNew(o => this.Coding("风动寂野", "Portal"), "风动寂野"),
                 taskFactory.StartNew(o => this.Coding("笑看风云", "Service"), "笑看风云")
            };
            //线程锁，线程安全
            taskList.Add(
                taskFactory.StartNew(() =>
                {
                    lock (Lock)
                    {
                        this.DoSomethingLong("btnTask_Click");
                    }
                }
            ));

            //taskFactory.StartNew(
            //    () => this.DoSomethingLong("btnTask_Click")
            //);
            //taskFactory.ContinueWhenAny(taskList.ToArray(), t =>
            //{
            //    Console.WriteLine(t.AsyncState);
            //    Console.WriteLine($"部署环境，联调测试。。。【{Thread.CurrentThread.ManagedThreadId.ToString("00")}】");
            //});
            taskFactory.ContinueWhenAll(taskList.ToArray(), tList =>
            {
                ///Console.WriteLine(tList[0].AsyncState);
                Console.WriteLine($"部署环境，联调测试。。。【{Thread.CurrentThread.ManagedThreadId.ToString("00")}】");
            });
            //Task.Run(() =>
            //{
            //    this.DoSomethingLong("btnTask_Click");
            //});
            Console.WriteLine($"****************btnTask_Click End {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
        }

        /// <summary>
        /// 编码做项目
        /// </summary>
        /// <param name="name"></param>
        /// <param name="project"></param>
        private void Coding(string name, string project)
        {
            Console.WriteLine($"****************Coding {name} Start {project} {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            long lResult = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                lResult += i;
            }
            //Thread.Sleep(2000);
            Console.WriteLine($"****************Coding {name} End {project} {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {lResult}***************");
        }

        /// <summary>
        /// 一个比较耗时耗资源的私有方法
        /// </summary>
        /// <param name="name"></param>
        private void DoSomethingLong(string name)
        {
            Console.WriteLine($"****************DoSomethingLong {name} Start {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            long lResult = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                lResult += i;
            }
            //Thread.Sleep(2000);
            Console.WriteLine($"****************DoSomethingLong {name} End {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {lResult}***************");
        }

        private void btnAsyncAwait_Click(object sender, EventArgs e)
        {
            AwaitAsyncLibrary.AwaitAsyncILSpy.Show();
        }
    }
}
