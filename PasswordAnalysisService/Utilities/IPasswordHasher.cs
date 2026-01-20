namespace PasswordAnalysisService.Utilities
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
