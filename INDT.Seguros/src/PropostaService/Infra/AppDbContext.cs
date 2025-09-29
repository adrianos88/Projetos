using Microsoft.EntityFrameworkCore;
using PropostaService.Domain;

namespace PropostaService.Infra;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }
    public DbSet<Proposta> Propostas => Set<Proposta>();
}
