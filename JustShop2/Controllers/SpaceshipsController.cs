using Microsoft.AspNetCore.Mvc;
using JustShop2.Data;
using JustShop2.Models.Spaceships;

namespace JustShop2.Controllers
{
    public class SpaceshipsController : Controller
    {
        private readonly JustShop2Context _context;

        public SpaceshipsController
            (
            JustShop2Context context
            )
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.Spaceships
                .Select(x => new SpaceshipsIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Typename = x.Typename,
                    BuiltDate = x.BuiltDate,
                    Crew = x.Crew,
                });

            return View(result);

        }
    }
}
