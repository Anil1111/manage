using MyDelegateEvent.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDelegateEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                MyDelegate myDelegate = new MyDelegate();
                myDelegate.Show();
            }
            {
                ListExtend test = new ListExtend();
                test.Show();
            }
            {
                Cat cat = new Cat();
                cat.MiaoDelegateHandlerEvent += new MiaoDelegate(new Baby().Cry);
                cat.MiaoDelegateHandlerEvent += new MiaoDelegate(new Mother().Wispher);
                cat.MiaoNewEvent();
                Console.WriteLine("***************************");
            }
            Console.Read();
        }
    }
}
