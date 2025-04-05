using Microsoft.EntityFrameworkCore;

namespace Seraphine.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options) // EntityFrameworkCore\Add-Migration {SUBSTITUIR_NOME_VERSAO_BANCO}
{
    //public DbSet<Usuario> Usuario { get; set; }
}