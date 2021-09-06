namespace BLL.Services.Interfaces
{
    public interface ILogger
    {
        /*
         * [12/08/2021 - HanNQ] log infomation
         */
        void Information(string message);

        /*
         * [12/08/2021 - HanNQ] log warning
         */
        void Warning(string message);

        /*
         * [12/08/2021 - HanNQ] log debug
         */
        void Debug(string message);

        /*
         * [12/08/2021 - HanNQ] log error
         */
        void Error(string message);
    }
}
