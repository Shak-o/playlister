using Microsoft.AspNetCore.Mvc;
using PlayLister.Client.Models;
using System.Diagnostics;
using PlayLister.Services.Interfaces;
using PlayLister.Services.Models;
using PlayLister.Services.Models.ServiceModels;

namespace PlayLister.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlaylistConverter _converter;

        public HomeController(ILogger<HomeController> logger, IPlaylistConverter converter)
        {
            _logger = logger;
            _converter = converter;
        }

        public async Task<IActionResult> Index()
        {
            //var list = await _converter.GetYoutubePlaylists("UC7E2aOwJF2-7xXQJ5xImTEQ");
            return View();
        }

        [HttpGet("list")]
        public async Task<IActionResult> ConvertList(string playlistLink)
        {
            var keyWord = "list=";
            int index = playlistLink.IndexOf(keyWord);
            string id = playlistLink.Substring(index + keyWord.Length);

            var list = await _converter.GetPlaylistItems(id);

            ViewBag.Pages = (list.TotalResults / list.ResultsPerPage);
            ViewBag.CurrentPage = 0;

            //await _converter.MakeSpotifyPlaylist(id, HttpContext.Request.Cookies["accessToken"]);

            return View(list);
        }

        [HttpGet("listPage")]
        public async Task<IActionResult> Page(string id, int page)
        {
            PlaylistServiceModel data = await _converter.GetPlaylistDataPerPage(id, page);

            ViewBag.Pages = (data.TotalResults / data.ResultsPerPage);
            ViewBag.CurrentPage = page;

            return View(data);
        }

        [HttpGet("remove")]
        public async Task<IActionResult> Remove(int id, string playlistId, int pageRed)
        {
            await _converter.RemoveItemFromPlaylist(id, playlistId);
            return RedirectToAction("Page", new {id = playlistId, page = pageRed });
        }

        [HttpGet("test")]
        public async Task Test()
        {
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}