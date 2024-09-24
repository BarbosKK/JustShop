using Microsoft.AspNetCore.Mvc;
using JustShop2.Data;
using JustShop2.Models.Spaceships;
using JustShop2.Core.Serviceinterface;

namespace JustShop2.Controllers
{
    public class SpaceshipsController : Controller
    {
        private readonly JustShop2Context _context;
        private readonly ISpaceshipsServices _spaceshipsServices;
        public SpaceshipsController
            (
                JustShop2Context context,
                ISpaceshipsServices spaceshipsServices
            )
        {
            _context = context;
            _spaceshipsServices = spaceshipsServices;
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

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var spaceship = await _spaceshipsServices.DetailAsync(id);

            if(spaceship == null)
            {
                return View("Error");
            }

            var vm = new SpaceshipsDetailsViewModel();
            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Typename = spaceship.Typename;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.SpaceshipModel = spaceship.SpaceshipModel;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }
    }
}
