using System;

namespace ShortURL.Models
{
	public class ZavorURL
    {
        public Guid ID { get; set; }
        public string URL { get; set; }
        public string ShortenedURL { get; set; }
        public string Token { get; set; }
        public int Clicked { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
