namespace PasswordAnalysisService.Logic
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string message) : base(message) { }
    }

}
