namespace ShortURL
{
    public interface IURLResponse
    {
        string Status { get; set; }
        string Token { get; set; }
        string Url { get; set; }
    }
}