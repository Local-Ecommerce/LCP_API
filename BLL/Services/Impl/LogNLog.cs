using NLog;

namespace BLL.Services.Impl
{
    public class LogNLog : ILogger
    {
        private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        //contruction
        public LogNLog() { }

        /*
         * [12/08/2021 - HanNQ] log debug
         */
        public void Debug(string message)
        {
            logger.Debug(message);
        }

        /*
         * [12/08/2021 - HanNQ] log error
         */
        public void Error(string message)
        {
            logger.Error(message);
        }

        /*
         * [12/08/2021 - HanNQ] log infomation
         */
        public void Information(string message)
        {
            logger.Info(message);
        }

        /*
         * [12/08/2021 - HanNQ] log warning
         */
        public void Warning(string message)
        {
            logger.Warn(message);
        }
    }
}
