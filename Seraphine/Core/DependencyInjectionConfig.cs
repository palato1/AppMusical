using Seraphine.Core.Interface;
using Microsoft.EntityFrameworkCore;
using Seraphine.Data;
using Seraphine.ServiceInterface;
using Seraphine.Service;

namespace Seraphine.Core;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjectionServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationContext>(x => x.UseNpgsql(configuration.GetConnectionString("DataBase")));

        services.AddScoped<INotificator, Notificator>();
        services.AddScoped<ICredentialUser, CredentialUser>();

        
        services.AddScoped<ISpotifyService, SpotifyService>();
    }
}