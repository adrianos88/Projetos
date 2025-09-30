using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ContratacaoService.Application;
using ContratacaoService.Domain;
using ContratacaoService.Infra;
using ContratacaoService.Ports;
using ContratacaoService.Application.Dto;

namespace Contratacao.Tests
{
    // Fake simples do IPropostaClient para controlar o retorno
    class FakePropostaClient : IPropostaClient
    {
        private readonly PropostaDto? _toReturn;
        public FakePropostaClient(PropostaDto? toReturn) => _toReturn = toReturn;
        public Task<PropostaDto?> GetPropostaByIdAsync(Guid id) => Task.FromResult(_toReturn);
    }

    public class ContratacaoAppServiceTests
    {
        [Fact]
        public async Task CreateContratacaoAsync_Should_Create_When_Proposta_Aprovada()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // cria um DTO indicando que a proposta está Aprovada
            var propostaDto = new PropostaDto
            {
                Id = Guid.NewGuid(),
                Cliente = "Joao",
                Valor = 1000m,
                Descricao = "Teste",
                Status = "Aprovada"
            };

            var fakeClient = new FakePropostaClient(propostaDto);

            await using (var db = new AppDbContext(options))
            {
                var appService = new ContratacaoAppService(db, fakeClient);

                var (ok, message) = await appService.CreateContratacaoAsync(propostaDto.Id, "TesteUser");

                Assert.True(ok, "Contratacao deveria ser criada quando proposta estiver Aprovada.");
                Assert.Null(message);

                var saved = await db.Contratacoes.FirstOrDefaultAsync();
                Assert.NotNull(saved);
                Assert.Equal(propostaDto.Id, saved!.PropostaId);
                Assert.Equal("TesteUser", saved.ContratadoPor);
            }
        }

        [Fact]
        public async Task CreateContratacaoAsync_Should_NotCreate_When_Proposta_NotAprovada()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var propostaDto = new PropostaDto
            {
                Id = Guid.NewGuid(),
                Cliente = "Joao",
                Valor = 1000m,
                Descricao = "Teste",
                Status = "EmAnalise"
            };

            var fakeClient = new FakePropostaClient(propostaDto);

            await using (var db = new AppDbContext(options))
            {
                var appService = new ContratacaoAppService(db, fakeClient);

                var (ok, message) = await appService.CreateContratacaoAsync(propostaDto.Id, "TesteUser");

                Assert.False(ok, "Contratação não deve ocorrer se a proposta não estiver aprovada.");
                Assert.NotNull(message);

                var saved = await db.Contratacoes.FirstOrDefaultAsync();
                Assert.Null(saved);
            }
        }
    }
}
