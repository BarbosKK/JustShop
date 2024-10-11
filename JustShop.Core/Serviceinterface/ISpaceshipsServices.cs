using JustShop2.Core.Domain;
using JustShop2.Core.Dto;

namespace JustShop2.Core.ServiceInterface
{
    public interface ISpaceshipsServices
    {
        Task<Spaceship> DetailAsync(Guid id);
        Task<Spaceship> Update(SpaceshipDto dto);
        Task<Spaceship> Delete(Guid id);
        Task<Spaceship> Create(SpaceshipDto dto);
    }
}
