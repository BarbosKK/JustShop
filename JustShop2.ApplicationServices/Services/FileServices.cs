﻿using JustShop2.Core.Domain;
using JustShop2.Core.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using JustShop2.Core.ServiceInterface;
using JustShop2.Data;


namespace JustShop2.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly IHostEnvironment _webHost;
        private readonly JustShop2Context _context;

        public FileServices
            (
                IHostEnvironment webHost,
                JustShop2Context context
            )
        {
            _webHost = webHost;
            _context = context;
        }


        public void FilesToApi(SpaceshipDto dto, Spaceship spaceship)
        {
            if (!Directory.Exists(_webHost.ContentRootPath + "\\multipleFileUpload\\"))
            {
                Directory.CreateDirectory(_webHost.ContentRootPath + "\\multipleFileUpload\\");
            }

            foreach (var file in dto.Files)
            {
                string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "multipleFileUpload");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);

                    FileToApi path = new FileToApi
                    {
                        Id = Guid.NewGuid(),
                        ExistingFilePath = uniqueFileName,
                        SpaceshipId = spaceship.Id
                    };

                    _context.FileToApis.AddAsync(path);
                }
            }
        }

        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dtos)
        {
            foreach (var dto in dtos)
            {
                var imageId = await _context.FileToApis
                    .FirstOrDefaultAsync(x => x.ExistingFilePath == dto.ExistingFilePath);

                var filePath = _webHost.ContentRootPath + "\\multipleFileUpload\\"
                    + imageId.ExistingFilePath;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.FileToApis.Remove(imageId);
                await _context.SaveChangesAsync();
            }

            return null;
        }

        public void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                foreach (var image in dto.Files)
                {
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase()
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = image.FileName,
                            RealEstateId = domain.Id
                        };

                        image.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.FileToDatabases.Add(files);
                    }
                }
            }
        }
        public async Task<FileToDatabase> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos)
        {
            foreach(var dto in dtos)
            {
                var image = await _context.FileToDatabases
                    .Where(x => x.Id == dto.Id)
                    .FirstOrDefaultAsync();

                _context.FileToDatabases.Remove(image);
                await _context.SaveChangesAsync();
            }

            return null;
        }

        public async Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto)
        {

                var image = await _context.FileToDatabases
                    .Where(x => x.Id == dto.Id)
                    .FirstOrDefaultAsync();

                _context.FileToDatabases.Remove(image);
                await _context.SaveChangesAsync();


            return image;
        }
    }
}