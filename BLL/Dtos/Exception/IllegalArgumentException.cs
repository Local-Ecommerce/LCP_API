namespace BLL.Dtos.Exception
{
    public class IllegalArgumentException : System.Exception
    {
        public IllegalArgumentException() : base("Invalid parameter(s)")
        {
        }

        public IllegalArgumentException(string errorMessage) : base(errorMessage)
        {
        }
    }
}