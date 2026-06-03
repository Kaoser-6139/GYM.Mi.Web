namespace GYM.Infrastructure
{
    public interface IGeminiService
    {
        Task<string> GetResponseAsync(string prompt);
        Task<string> ChatAsync(string message);
    }
}
