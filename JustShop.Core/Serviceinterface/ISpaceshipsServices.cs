using JustShop2.Core.Domain;
using JustShop2.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustShop2.Core.Serviceinterface
{
    public interface ISpaceshipsServices
    {
        Task<Spaceship> DetailAsync(Guid id);

        Task<Spaceship> Update(SpaceshipDto dto);

        Task<Spaceship> Delete(Guid id);

        Task<Spaceship> Create(SpaceshipDto dto);
    }
}
