using JustShop2.Core.Domain;
using JustShop2.Core.Dto;
using JustShop2.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using JustShop2.Data;
using System;
using System.Threading.Tasks;

namespace JustShop2.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly JustShop2Context _context;
        private readonly IFileServices _fileServices;

        public KindergartenServices(JustShop2Context context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten kindergarten = new Kindergarten
            {
                Id = Guid.NewGuid(),
                GroupName = dto.GroupName,
                ChildrenCount = dto.ChildrenCount ?? 0,
                KindergartenName = dto.KindergartenName,
                Teacher = dto.Teacher,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, kindergarten);
            }

            await _context.Kindergartens.AddAsync(kindergarten);
            await _context.SaveChangesAsync();
            return kindergarten;
        }

        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            var existingKindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (existingKindergarten == null)
            {
                throw new Exception("Kindergarten not found");
            }

            existingKindergarten.GroupName = dto.GroupName;
            existingKindergarten.ChildrenCount = dto.ChildrenCount ?? 0;
            existingKindergarten.KindergartenName = dto.KindergartenName;
            existingKindergarten.Teacher = dto.Teacher;
            existingKindergarten.UpdatedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, existingKindergarten);
            }

            _context.Kindergartens.Update(existingKindergarten);
            await _context.SaveChangesAsync();
            return existingKindergarten;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            if (kindergarten == null)
            {
                throw new Exception("Kindergarten not found");
            }

            var images = await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id,
                    ImageTitle = y.ImageTitle,
                    KindergartenId = y.KindergartenId
                })
                .ToArrayAsync();

            await _fileServices.RemoveImagesFromDatabase(images);

            _context.Kindergartens.Remove(kindergarten);
            await _context.SaveChangesAsync();
            return kindergarten;
        }
    }
}