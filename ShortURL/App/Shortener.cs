using LiteDB;
using System;
using System.Linq;
using ShortURL.Models;

namespace ShortURL.App
{
    public class Shortener
    {
		public string Token { get; set; } 
		private ZavorURL _url;

        public Shortener(string url)
        {
            var db = new LiteDatabase(@"Filename=DataBase/URL.db; Connection=shared");
            var urls = db.GetCollection<ZavorURL>();
            while (urls.Exists(u => u.Token == GenerateToken())){}
            _url = new ZavorURL() { Token = Token, URL = url, ShortenedURL = new ZavorConfig().Config.BASE_URL + Token };
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
