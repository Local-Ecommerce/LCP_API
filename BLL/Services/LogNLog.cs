using NLog;

namespace BLL.Services
{
    public class LogNLog : Interfaces.ILogger
    {
        private static ILogger s_logSystem = LogManager.GetLogger("logSystem");
        private static ILogger s_logUserBehavior = LogManager.GetLogger("logUserBehavior");

        public LogNLog() { }


        /// <summary>
        /// Log Debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            s_logSystem.Debug(message);
        }


        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            s_logSystem.Error(message);
        }


        /// <summary>
        /// Log Infomation
        /// </summary>
        /// <param name="message"></param>
        public void Information(string message)
        {
            s_logSystem.Info(message);
        }

        public void UserBehavior(string message)
        {
            s_logUserBehavior.Info(message);
        }


        /// <summary>
        /// Log Warning
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message)
        {
            s_logSystem.Warn(message);
        }
    }
}
