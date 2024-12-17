using Microsoft.AspNetCore.Mvc;

namespace JustShop2.Controllers
{
    public class KindergartensController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
