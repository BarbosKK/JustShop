using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JustShop2.ApplicationServices.Services;
using JustShop2.Core.ServiceInterface;
using JustShop2.Data;
namespace JustShop2.Tests
{
    public class TestBase : IDisposable
    {
        protected IServiceProvider ServiceProvider { get; }
        protected JustShop2Context _context;
        public TestBase()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            _context = ServiceProvider.GetService<JustShop2Context>();
        }
        protected virtual void ConfigureServices(IServiceCollection services)
        {
            // Registreeri teenused
            services.AddScoped<IKindergartenServices, KindergartenServices>();
            // Lisa andmebaasi kontekst in-memory andmebaasiga
            services.AddDbContext<JustShop2Context>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
        }
        protected T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
        public void Dispose()
        {
            // Puhasta ressursid
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
