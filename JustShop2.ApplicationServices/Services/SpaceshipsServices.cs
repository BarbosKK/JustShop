using JustShop2.Core.Domain;
using JustShop2.Core.Serviceinterface;
using JustShop2.Data;
using Microsoft.EntityFrameworkCore;


namespace JustShop2.ApplicationServices.Services
{
    public class SpaceshipsServices : ISpaceshipsServices

    {
        private readonly JustShop2Context _context;

        public SpaceshipsServices
            (
                JustShop2Context context
            )
        {
            _context = context;
        }

        public async Task<Spaceship> DetailAsync(Guid id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync( x=> x.Id == id );

            return result;
        }
    }
}
