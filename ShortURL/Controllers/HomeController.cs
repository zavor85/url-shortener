using System;
using System.Linq;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace ShortURL
{
    public class HomeController : Controller
    {
        private readonly ILiteCollection<ZavorURL> _liteDbCollection = new LiteDB.LiteDatabase(@"Filename=URL_DATABASE.db; Connection=shared").GetCollection<ZavorURL>();

        [HttpGet, Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, Route("/")]
        public IActionResult PostURL([FromBody] string url)
        {
            try
            {
                if (!url.Contains("http"))
                {
                    url = "http://" + url;
                }
                if (_liteDbCollection.Exists(u => u.ShortenedURL == url))
                {
                    Response.StatusCode = 405;
                    return Json(new URLResponse() { Url = url, Status = "Already shortened", Token = null });
                }
                IShortener shortURL = new Shortener(url);
                return Json(shortURL.Token);
            }
            catch (Exception ex)
            {
                if (ex.Message == "URL already exists")
                {
                    Response.StatusCode = 400;
                    return Json(new URLResponse() { Url = url, Status = "URL already exists", Token = 
                        _liteDbCollection.Find(u => u.URL == url).FirstOrDefault().Token });
                }
                throw new Exception(ex.Message);
            }
        }

        [HttpGet, Route("/{token}")]
        public IActionResult ZavorRedirect([FromRoute] string token)
        {
            return Redirect(_liteDbCollection.FindOne(u => u.Token == token).URL);
        }
    }
}
