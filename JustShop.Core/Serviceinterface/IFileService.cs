using JustShop2.Core.Domain;
using JustShop2.Core.Dto;


namespace JustShop2.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceship spaceship);
        void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain);
        Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dtos);
        void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain);
        Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto);
        Task<FileToDatabase> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos);
        //Task<FileToDatabase> RemoveFilesFromDatabase(FileToDatabaseDto[] images);
        //Task RemoveFileFromDatabase(FileToDatabaseDto dto);
        //Task RemoveFilesFromDatabase(FileToDatabaseDto dto);
    }
}