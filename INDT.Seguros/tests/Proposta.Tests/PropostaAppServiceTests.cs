using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using PropostaService.Domain;
using PropostaService.Application; // PropostaAppService
using PropostaService.Infra; // PropostaRepositoryEf

namespace Proposta.Tests
{
    public class PropostaAppServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_CreateProposta_WithStatusEmAnalise()
        {
            // usa database in-memory com nome único
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var db = new AppDbContext(options))
            {
                var repo = new PropostaRepositoryEf(db);
                var app = new PropostaAppService(repo);

                var result = await app.CreateAsync("Joao", 1000m, "Seguro Teste");

                // valida se foi persistido
                var fromDb = await repo.GetByIdAsync(result.Id);
                Assert.NotNull(fromDb);
                Assert.Equal(PropostaStatus.EmAnalise, fromDb!.Status);
                Assert.Equal("Joao", fromDb.Cliente);
                Assert.Equal(1000m, fromDb.Valor);
            }
        }
    }
}
