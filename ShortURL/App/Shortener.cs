using LiteDB;
using System;
using System.Linq;

namespace ShortURL
{
    public class Shortener : IShortener
    {
        public string Token { get; set; }
        private ZavorURL _url;
        private string _baseUrl = "http://localhost:7000/";

        public Shortener(string url)
        {
            var urls = new LiteDatabase(@"Filename=URL_DATABASE.db; Connection=shared").GetCollection<ZavorURL>();
            _url = new ZavorURL() { Token = GenerateToken(), URL = url, ShortenedURL = _baseUrl + Token };
            if (urls.Exists(u => u.URL == url))
                throw new Exception("URL already exists");
            urls.Insert(_url);
        }

        private string GenerateToken()
        {
            string urlSafe = string.Empty;
            // Getting all lowercase, uppercase letters and numbers from 0 to 9
            Enumerable.Range(48, 75).Where(i => i < 58 || i > 64 && i < 91 || i > 96).OrderBy(o => new Random().Next()).ToList().ForEach(i => urlSafe += Convert.ToChar(i));
            Token = urlSafe.Substring(new Random().Next(0, urlSafe.Length), new Random().Next(4, 6));
            return Token;
        }

    }
}
