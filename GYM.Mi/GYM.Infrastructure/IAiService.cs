namespace GYM.Infrastructure
{
    public interface IAiService
    {
        Task<string> GetResponseAsync(string prompt);
        Task<string> ChatAsync(string message);
    }
}
