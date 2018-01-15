using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Utils
{
    public static class WinServiceUtil
    {
        public static void Reset(string serviceName)
        {
            SetRestartInfo(serviceName, DateTime.MaxValue, false);
        }

        public static bool StartService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            ServiceControllerStatus status = sc.Status;
            switch (status)
            {
                case ServiceControllerStatus.StopPending:
                case ServiceControllerStatus.Stopped:
                    sc.Start();
                    break;
                default: break;
            }
            sc.WaitForStatus(ServiceControllerStatus.Running);
            status = sc.Status;//再次获取服务状态

            return (status == ServiceControllerStatus.Running);
        }

        public static bool StopService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            ServiceControllerStatus status = sc.Status;
            switch (status)
            {
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.ContinuePending:
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    break;
            }
            status = sc.Status;//再次获取服务状态

            return (status == ServiceControllerStatus.Stopped);
        }

        public static bool PauseService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            ServiceControllerStatus status = sc.Status;
            switch (status)
            {
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                    sc.Pause();
                    sc.WaitForStatus(ServiceControllerStatus.Paused);
                    break;
            }
            status = sc.Status;//再次获取服务状态

            return (status == ServiceControllerStatus.Paused);
        }

        public static bool ResumeService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            ServiceControllerStatus status = sc.Status;
            switch (status)
            {
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.PausePending:
                    sc.Pause();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    break;
            }
            status = sc.Status;//再次获取服务状态

            return (status == ServiceControllerStatus.Running);
        }

        public static void Restart(string serviceName, string batFileName)
        {
            var proc = new Process();
            //proc.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory.EndsWith(@"\")
            //    ? AppDomain.CurrentDomain.BaseDirectory + "Bats"
            //    : AppDomain.CurrentDomain.BaseDirectory + @"\Bats";

            //var fileName = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\Bats\\RestartJob.bat";
            //_logger.Info("[RestartJob] FileName: " + fileName);

            proc.StartInfo.FileName = batFileName;
            //proc.StartInfo.Arguments = string.Format("10");//this is argument
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();

            SetRestartInfo(serviceName, DateTime.Now, true);
        }

        public static bool IsRestarted(string serviceName)
        {
            var restartInfo = GetRestartInfo(serviceName);

            return (restartInfo.RestartTime != DateTime.MinValue
                && restartInfo.RestartTime != DateTime.MaxValue
                && restartInfo.IsRestarted);
        }

        public static DateTime GetRestartTime(string serviceName)
        {
            var restartInfo = GetRestartInfo(serviceName);
            return restartInfo.RestartTime;
        }

        public static RestartInfo GetRestartInfo(string serviceName)
        {
            var restartInfos = GetRestartInfos();
            var restartInfo = restartInfos.Where(r => r.ServiceName == serviceName).FirstOrDefault();

            return restartInfo != null ? restartInfo : new RestartInfo() { IsRestarted = false, RestartTime = DateTime.MaxValue };
        }

        #region private

        private static List<RestartInfo> GetRestartInfos()
        {
            var logFileName = GetLogPath();
            var restartInfos = FileUtil.GetJson<List<RestartInfo>>(logFileName);
            return restartInfos != null ? restartInfos : new List<RestartInfo>();
        }

        /// <summary>
        /// 保存 RestartInfo 至 文件 ServiceLogs\Log.Json
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="restartTime"></param>
        /// <param name="isRestart"></param>
        private static void SetRestartInfo(string serviceName, DateTime restartTime, bool isRestart)
        {
            var restartInfos = GetRestartInfos();
            var restartInfo = restartInfos.Where(r => r.ServiceName == serviceName).FirstOrDefault();
            if (restartInfo != null)
            {
                restartInfo.RestartTime = restartTime;
                restartInfo.IsRestarted = isRestart;
            }
            else
            {
                restartInfo = new RestartInfo()
                {
                    ServiceName = serviceName,
                    RestartTime = restartTime,
                    IsRestarted = isRestart
                };
                restartInfos.Add(restartInfo);
            }

            FileUtil.Save(GetLogPath(), restartInfos);
        }

        private static string GetLogPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\ServiceLogs\\Log.Json";
        }

        #endregion
    }

    public class RestartInfo
    {
        public string ServiceName { get; set; }
        public bool IsRestarted { get; set; }
        public DateTime RestartTime { get; set; }
    }
}