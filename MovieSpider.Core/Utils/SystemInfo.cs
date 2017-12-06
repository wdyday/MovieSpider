using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Utils
{
    public class SystemInfo
    {
        public static string GetCurrentProcessMemory()
        {

            //System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            //var memory = (currentProcess.WorkingSet64 / 1024 / 1024).ToString() + "M (" + (currentProcess.WorkingSet64 / 1024).ToString() + "KB)";

            var ps = Process.GetCurrentProcess();
            PerformanceCounter pfc = new PerformanceCounter("Process", "Working Set - Private", ps.ProcessName);   //第二个参数就是得到只有工作集
            var memory = (pfc.NextValue() / 1024 / 1024).ToString() + "M (" + (pfc.NextValue() / 1024).ToString() + "KB)";

            return memory;
        }
    }
}
