using System;

namespace ShortURL
{
    public interface IZavorURL
    {
        int Clicked { get; set; }
        DateTime Created { get; set; }
        Guid ID { get; set; }
        string ShortenedURL { get; set; }
        string Token { get; set; }
        string URL { get; set; }
    }
}