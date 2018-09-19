using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDelegateEvent
{
    /// <summary>
    /// 委托：是一个类，继承自System.MulticastDelegate，里面内置了几个方法
    ///       委托的意义：委托解耦，减少重复代码
    /// </summary>
    public class MyDelegate
    {
        public delegate void NoReturnNoPara();//1 声明委托
        public delegate string WithReturnWithPara(int x);
        public void Show()
        {
            NoReturnNoPara noReturnNoPara = new NoReturnNoPara(DoNothing);//2 委托的实例化
            noReturnNoPara.Invoke();//3 委托实例的调用

            {
                WithReturnWithPara withReturnWithPara = new WithReturnWithPara(this.DoNothing2);
                string result = withReturnWithPara.Invoke(2);
                Console.WriteLine(result);
            }
            {
                //多播委托：一个变量保存多个方法，可以增减；invoke的时候可以按顺序执行
                //+= 为委托实例按顺序增加方法，形成方法链，Invoke时，按顺序依次执行
                NoReturnNoPara method = new NoReturnNoPara(this.DoNothing);
                method += new NoReturnNoPara(this.DoNothing);
                method += new NoReturnNoPara(DoNothingStatic);
                method.Invoke();

                foreach (NoReturnNoPara item in method.GetInvocationList())
                {
                    item.Invoke();
                }

                //-= 为委托实例移除方法，从方法链的尾部开始匹配，遇到第一个完全吻合的，移除且只移除一个，没有也不异常
                method -= new NoReturnNoPara(this.DoNothing);
                method -= new NoReturnNoPara(DoNothingStatic);
                method.Invoke();
            }
        }

        private void DoNothing()
        {
            Console.WriteLine("This is DoNothing");
        }
        private string DoNothing2(int val)
        {
            return $"This is DoNothing2={val}";
        }
        private static void DoNothingStatic()
        {
            Console.WriteLine("This is DoNothingStatic");
        }
    }
}
