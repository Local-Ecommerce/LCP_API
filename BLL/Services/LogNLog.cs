using NLog;

namespace BLL.Services
{
    public class LogNLog : Interfaces.ILogger
    {
        private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public LogNLog() { }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Information(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }
    }
}
