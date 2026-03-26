using System.Diagnostics;
using assignment01.Areas.Identity.Models;
using assignment01.Data;
using Microsoft.AspNetCore.Mvc;
using assignment01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace assignment01.Controllers;

[Authorize]
public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<User> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    public IActionResult NotFound(int statusCode)
    {
        if (statusCode == 404)
        {
            return View("NotFound");
        }

        return View("Error");
    }

    public IActionResult ServerError(int statusCode)
    {
        if (statusCode >= 500 && statusCode <= 599)
        {
            string errorType = string.Empty;
            switch (statusCode)
            {
                case 500:
                    errorType = "Internal Server Error";
                    break;
                case 501:
                    errorType = "Not Implemented";
                    break;
                case 502:
                    errorType = "Bad Gateway";
                    break;
                case 503:
                    errorType = "Service Unavailable";
                    break;
                case 504:
                    errorType = "Gateway Timeout";
                    break;
                case 505:
                    errorType = "HTTP Version Not Supported";
                    break;
                case 506:
                    errorType = "Variant Also Negotiates";
                    break;
                case 507:
                    errorType = "Insufficient Storage";
                    break;
                case 508:
                    errorType = "Loop Detected";
                    break;
                case 510:
                    errorType = "Not Extended";
                    break;
                case 511:
                    errorType = "Network Authentication Required";
                    break;
                default:
                    errorType = "Generic Server Error";
                    break;
            }

            ViewBag.Title = $"{statusCode} - {errorType}";
            return View("NotFound");
        }

        return View("Error");
    }
}