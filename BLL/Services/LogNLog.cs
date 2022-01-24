using NLog;

namespace BLL.Services
{
    public class LogNLog : Interfaces.ILogger
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LogNLog() { }


        /// <summary>
        /// Log Debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            logger.Debug(message);
        }


        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            logger.Error(message);
        }


        /// <summary>
        /// Log Infomation
        /// </summary>
        /// <param name="message"></param>
        public void Information(string message)
        {
            logger.Info(message);
        }


        /// <summary>
        /// Log Warning
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message)
        {
            logger.Warn(message);
        }
    }
}
