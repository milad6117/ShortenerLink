using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShortenerLink.Service.Services;
using ShortenerLink.Web.Models;

namespace ShortenerLink.Web.Controllers;

public class HomeController : Controller
{
    #region Field
    private readonly ILinkShortenerService _linkShortenerService;

    public HomeController(ILinkShortenerService linkShortenerService)
    {
        _linkShortenerService = linkShortenerService;
    }
    #endregion

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ShortenLink(string longUrl)
    {
        if (string.IsNullOrEmpty(longUrl)) return View("Index");

        string baseUrl = $"{Request.Scheme}://{Request.Host}";
        string shortUrl = await _linkShortenerService.CreateShortUrlAsync(longUrl, baseUrl);

        ViewBag.ShortUrl = shortUrl;
        return View("Index");
    }


    [HttpGet("/{code}")]
    public async Task<IActionResult> RedirectTo(string code)
    {
        var longUrl = await _linkShortenerService.GetLongUrlAsync(code);

        if (longUrl == null) return NotFound();

        return Redirect(longUrl);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
