using Microsoft.AspNetCore.Mvc;
using Kad.PMSDemo.Web.Controllers;

namespace Kad.PMSDemo.Web.Public.Controllers
{
    public class HomeController : PMSDemoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}