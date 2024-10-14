﻿using JustShop2.Core.Dto;
using JustShop2.Core.ServiceInterface;
using JustShop2.Data;
using JustShop2.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace JustShop2.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        private readonly JustShop2Context _context;
        private readonly IFileServices _fileServices;

        public RealEstateServices
            (
                JustShop2Context context,
                IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = Guid.NewGuid();
            realEstate.Size = dto.Size;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, realEstate);
            }

            await _context.RealEstates.AddAsync(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> GetAsync(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate domain = new();

            domain.Id = dto.Id;
            domain.Size = dto.Size;
            domain.Location = dto.Location;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, domain);
            }

            _context.RealEstates.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var result = await _context.RealEstates       //otsing: ankeeti
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabases  //otsing: piltid ankeeti all
                .Where(x => x.RealEstateId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id,
                    ImageTitle = y.ImageTitle,
                    RealEstateId = y.RealEstateId
                }).ToArrayAsync();

            await _fileServices.RemoveImagesFromDatabase(images);
            _context.RealEstates.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }

    }
}
