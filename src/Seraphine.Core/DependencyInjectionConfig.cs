using Seraphine.Core.Interface;
using Seraphine.Data;
using Seraphine.Repository;
using Seraphine.RepositoryInterface;
using Seraphine.Service;
using Seraphine.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using IFileService = Seraphine.ServiceInterface.IFileService;

namespace Seraphine.Core;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjectionServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationContext>(x => x.UseSqlServer(configuration.GetConnectionString("DataBase")));

        services.AddScoped<INotificator, Notificator>();
        services.AddScoped<ICredentialUser, CredentialUser>();

        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IFilialService, FilialService>();
        services.AddScoped<IMovimentacaoVendaService, MovimentacaoVendaService>();
        services.AddScoped<ICampanhaService, CampanhaService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IGrupoUsuarioService, GrupoUsuarioService>();
        services.AddScoped<IClienteService, ClienteService>();

        services.AddScoped<IFilialRepository, FilialRepository>();
        services.AddScoped<IMovimentacaoVendaRepository, MovimentacaoVendaRepository>();
        services.AddScoped<ICampanhaRepository, CampanhaRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IGrupoUsuarioRepository, GrupoUsuarioRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
    }
}