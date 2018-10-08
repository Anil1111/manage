using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Utility
{
    public class CmdUtil
    {
        /// <summary>
        /// 打开软件
        /// </summary>
        /// <param name="programName">软件路径加名称（.exe,.bat,.cmd文件）</param>
        public static void RunProgram(string programName)
        {
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.FileName = programName;
            pi.UseShellExecute = false;
            pi.CreateNoWindow = true;
            Process pcmd = Process.Start(pi);

            //等待进程结束
            while (pcmd.HasExited == false)
            {
                pcmd.WaitForExit(1000);
            }

            pcmd.Close();
            pcmd.Dispose();
            pcmd.Kill();
        }

        /// <summary>
        /// 打开控制台执行拼接完成的批处理命令字符串
        /// </summary>
        /// <param name="inputAction">需要执行的命令委托方法：每次调用 <paramref name="inputAction"/> 中的参数都会执行一次</param>
        /// CmdUtil.ExecBatCommand(p =>
        //  {
        //    //p(@"taskkill /im conhost.exe /f");
        //    //p("exit 0");
        //  });
        public static void ExecBatCommand(Action<Action<string>> inputAction)
        {
            Process pro = null;
            StreamWriter sIn = null;
            StreamReader sOut = null;
            try
            {
                pro = new Process();
                pro.StartInfo.FileName = "cmd.exe";
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.CreateNoWindow = true;
                pro.StartInfo.RedirectStandardInput = true;
                pro.StartInfo.RedirectStandardOutput = true;
                pro.StartInfo.RedirectStandardError = true;

                pro.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                pro.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                pro.Start();
                sIn = pro.StandardInput;
                sIn.AutoFlush = true;

                pro.BeginOutputReadLine();
                inputAction(value => sIn.WriteLine(value));

                pro.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (pro != null && !pro.HasExited)
                    pro.Kill();
                if (sIn != null)
                    sIn.Close();
                if (sOut != null)
                    sOut.Close();
                if (pro != null)
                    pro.Close();
            }
        }
    }
}
