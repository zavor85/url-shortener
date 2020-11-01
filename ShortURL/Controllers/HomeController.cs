using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShortURL.App;
using ShortURL.Models;

namespace ShortURL.Controllers
{
    public class HomeController : Controller
    {
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
                if (new LiteDB.LiteDatabase(@"Filename=DataBase/URL.db; Connection=shared")
                    .GetCollection<ZavorURL>().Exists(u => u.ShortenedURL == url))
                {
                    Response.StatusCode = 405;
                    return Json(new URLResponse() { Url = url, Status = "Already shortened", Token = null });
                }
                Shortener shortURL = new Shortener(url);
                return Json(shortURL.Token);
            }
            catch (Exception ex)
            {
                if (ex.Message == "URL already exists")
                {
                    Response.StatusCode = 400;
                    return Json(new URLResponse() { Url = url, Status = "URL already exists", Token = 
                        new LiteDB.LiteDatabase(@"Filename=DataBase/URL.db; Connection=shared")
                            .GetCollection<ZavorURL>().Find(u => u.URL == url).FirstOrDefault().Token });
                }
                throw new Exception(ex.Message);
            }
        }

        [HttpGet, Route("/{token}")]
        public IActionResult ZavorRedirect([FromRoute] string token)
        {
            return Redirect(new LiteDB.LiteDatabase(@"Filename=DataBase/URL.db; Connection=shared")
                .GetCollection<ZavorURL>().FindOne(u => u.Token == token).URL);
        }
    }
}
