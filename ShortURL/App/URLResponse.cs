namespace ShortURL
{
    public class URLResponse : IURLResponse
    {
        public string Url { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
    }
}
