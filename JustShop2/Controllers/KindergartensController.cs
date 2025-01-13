using JustShop2.Core.Dto;
using JustShop2.Core.ServiceInterface;
using JustShop2.Models.Kindergartens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustShop2.Data;
using JustShop2.Models.Kindergartens;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JustShop2.Controllers
{
    public class KindergartensController : Controller
    {
        private readonly JustShop2Context _context;
        private readonly IKindergartenServices _kindergartenServices;
        private readonly IFileServices _fileServices;

        public KindergartensController(
            JustShop2Context context,
            IKindergartenServices kindergartenServices,
            IFileServices fileServices)
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    KindergartenName = x.KindergartenName,
                    GroupName = x.GroupName,
                    Teacher = x.Teacher
                });
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var result = new KindergartenCreateUpdateViewModel();
            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                KindergartenName = vm.KindergartenName,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                Teacher = vm.Teacher,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        KindergartenId = x.KindergartenId
                    }).ToArray()
            };

            var result = await _kindergartenServices.Create(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), vm);
        }

        [HttpGet]
        public async Task<IActionResult>Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }
            var images = await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    KindergartenId = y.KindergartenId,
                    ImageId = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = $"data:image/gif;base64,{Convert.ToBase64String(y.ImageData)}"
                }).ToArrayAsync();

            
            
            var vm = new KindergartenDetailsViewModel
            {
                Id = kindergarten.Id,
                KindergartenName = kindergarten.KindergartenName,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                Teacher = kindergarten.Teacher,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt
            };
            vm.Image.AddRange(images);

            return View(vm);


        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }
            var images = await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    KindergartenId = y.KindergartenId,
                    ImageId = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = $"data:image/gif;base64,{Convert.ToBase64String(y.ImageData)}"
                }).ToArrayAsync();

            var vm = new KindergartenCreateUpdateViewModel
            {
                Id = kindergarten.Id,
                KindergartenName = kindergarten.KindergartenName,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                Teacher = kindergarten.Teacher,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt
            };
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                KindergartenName = vm.KindergartenName,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                Teacher = vm.Teacher,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = DateTime.Now,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        KindergartenId = x.KindergartenId
                    }).ToArray()
            };

            var result = await _kindergartenServices.Update(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);
            if (kindergarten == null)
            {
                return NotFound();
            }
            var images = await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    KindergartenId = y.KindergartenId,
                    ImageId = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = $"data:image/gif;base64,{Convert.ToBase64String(y.ImageData)}"
                }).ToArrayAsync();

            var vm = new KindergartenDeleteViewModel
            {
                Id = kindergarten.Id,
                KindergartenName = kindergarten.KindergartenName,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                Teacher = kindergarten.Teacher,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt
            };
            vm.Image.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var kindergartenId = await _kindergartenServices.Delete(id);
            if (kindergartenId == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(KindergartenImageViewModel vm)
        {
            var dto = new FileToDatabaseDto
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveImageFromDatabase(dto);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}