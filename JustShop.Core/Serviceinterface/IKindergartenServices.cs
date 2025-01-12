using JustShop2.Core.Domain;
using JustShop2.Core.Dto;


namespace JustShop2.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<Kindergarten> DetailAsync(Guid id);
        Task<Kindergarten> Update(KindergartenDto dto);
        Task<Kindergarten> Delete(Guid id);
        Task<Kindergarten> Create(KindergartenDto dto);
    }
}
