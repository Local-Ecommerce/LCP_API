namespace BLL.Services.Interfaces
{
    public interface ILogger
    {
        /// <summary>
        /// Log Infomation
        /// </summary>
        /// <param name="message"></param>
        void Information(string message);

        /// <summary>
        /// Log Warning
        /// </summary>
        /// <param name="message"></param>
        void Warning(string message);

        /// <summary>
        /// Log Debug
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);


        /// <summary>
        /// User Behavior
        /// </summary>
        /// <param name="message"></param>
        void UserBehavior(string message);
    }
}
