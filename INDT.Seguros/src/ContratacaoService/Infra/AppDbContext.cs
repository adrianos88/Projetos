using Microsoft.EntityFrameworkCore;
using ContratacaoService.Domain;

namespace ContratacaoService.Infra;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }
    public DbSet<Contratacao> Contratacoes => Set<Contratacao>();
}
