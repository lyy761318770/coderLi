using log4net;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demura
{
    public class AppLog
    {
        private static string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\SysLog\";

        private static readonly log4net.ILog logComm = log4net.LogManager.GetLogger("AppLog");

        static AppLog()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
        }
        public static readonly object o = new object();
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="isWrite">是否写</param>
        /// <param name="action">写日志的方法</param>
        /// <param name="info">日志文件名，便于分开日志文件</param>
        private static void WriteLog(string msg, bool isWrite, Action<object> action, string info = "")
        {
            if (isWrite)
            {
                lock (o)
                {
                    string filename = $"AppLog_{action.Method.Name}_{info}_{DateTime.Now.ToString("yyyyMMdd")}.log";
                    var repository = LogManager.GetRepository();

                    #region MyRegion
                    var appenders = repository.GetAppenders();
                    if (appenders.Length > 0)
                    {
                        RollingFileAppender targetApder = null;
                        foreach (var Apder in appenders)
                        {
                            if (Apder.Name == "AppLog")
                            {
                                targetApder = Apder as RollingFileAppender;
                                break;
                            }
                        }
                        if (targetApder.Name == "AppLog")//如果是文件输出类型日志，则更改输出路径
                        {
                            if (targetApder != null)
                            {
                                if (!targetApder.File.Contains(filename))
                                {
                                    targetApder.File = @"SysLog\" + filename;
                                    targetApder.ActivateOptions();
                                }
                            }
                        }
                    }
                    #endregion
                    action(msg);
                    //logComm.Error(msg + "\n");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="info">日志文件名，便于分开日志文件</param>
        /// <param name="isWrite">是否写入</param>
        public static void WriteError(string msg, string info = "", bool isWrite = true)
        {
            WriteLog(msg, isWrite, logComm.Error, info);
        }
        public static void WriteInfo(string msg, string info = "", bool isWrite = true)
        {
            WriteLog(msg, isWrite, logComm.Info, info);
        }
        public static void WriteWarn(string msg, string info = "", bool isWrite = true)
        {
            WriteLog(msg, isWrite, logComm.Warn, info);
        }
        public static void WriteFatal(string msg, string info = "", bool isWrite = true)
        {
            WriteLog(msg, isWrite, logComm.Fatal, info);
        }
    }
}
