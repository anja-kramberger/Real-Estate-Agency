using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using AgencijaZaNekretnine.Data;

namespace AgencijaZaNekretnine.Models
{
    public static class SchedulerInitializerExtension
    {
        public static IHost InitializeDatabase(this IHost webHost)
        {
            var serviceScopeFactory =
            (IServiceScopeFactory?)webHost.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory!.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AgencijaNekretninaContext>();
                //dbContext.Database.EnsureDeleted();
                //dbContext.Database.EnsureCreated();
            }
            return webHost;
        }
    }
}
