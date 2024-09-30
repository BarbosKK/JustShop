using Microsoft.AspNetCore.Mvc;
using JustShop2.Data;
using JustShop2.Models.Spaceships;
using JustShop2.Core.Serviceinterface;
using JustShop2.Core.Dto;
using JustShop2.Core.Domain;
using JustShop2.ApplicationServices.Services;

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

 //       [HttpGet]
 //       public IActionResult Create();
//        {
//            Space
//        }

        [HttpPost]

        public async Task<IActionResult> Create(SpaceshipCreateUpdateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Typename = vm.Typename,
                BuiltDate = vm.BuiltDate,
                SpaceshipModel = vm.SpaceshipModel,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
            };

            var result = await _spaceshipsServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), vm);
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

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipsServices.DetailAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipCreateUpdateViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Typename = spaceship.Typename;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.SpaceshipModel = spaceship.SpaceshipModel;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View("CreateUpdate", vm);
        }

        [HttpPost]

        public async Task<IActionResult> Update(SpaceshipCreateUpdateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Typename = vm.Typename,
                SpaceshipModel = vm.SpaceshipModel,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt

            };

            var result = await _spaceshipsServices.Update(dto);

            if(result == null)
            {
                return RedirectToAction(nameof(Index));

            }

            return RedirectToAction(nameof(Index), vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipsServices.DetailAsync(id);
            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipCreateUpdateViewModel();

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

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var spaceship = await _spaceshipsServices.Delete(id);

            if(spaceship == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

