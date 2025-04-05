using Seraphine.Data;
using Microsoft.EntityFrameworkCore;

namespace Seraphine.Core
{
    public static class DatabaseInitializer
    {
        public static void ApplyMigrations(IHost app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<ApplicationContext>();
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<ApplicationContext>>();
                logger.LogError(ex, "Ocorreu um erro ao aplicar as migrations do banco de dados.");
            }
        }
    }
}
