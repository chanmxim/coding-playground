using assignment01.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace assignment01.Controllers;

[Authorize]
public class BaseController : Controller
{
    
}