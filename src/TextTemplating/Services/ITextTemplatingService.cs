namespace TextTemplating.Services
{
    public interface ITextTemplatingService
    {
        string ProcessT4Template(string content);
    }
}
