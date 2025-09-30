using System;
using System.Threading.Tasks;
using ContratacaoService.Application.Dto;
using ContratacaoService.Domain;
using ContratacaoService.Infra;
using ContratacaoService.Ports;

namespace ContratacaoService.Application
{
    public class ContratacaoAppService
    {
        private readonly AppDbContext _db;
        private readonly IPropostaClient _propostaClient;

        public ContratacaoAppService(AppDbContext db, IPropostaClient propostaClient)
        {
            _db = db;
            _propostaClient = propostaClient;
        }

        public async Task<(bool ok, string? message)> CreateContratacaoAsync(Guid propostaId, string contratadoPor)
        {
            var proposta = await _propostaClient.GetPropostaByIdAsync(propostaId);
            if (proposta is null) return (false, "Proposta não encontrada.");

            if (!string.Equals(proposta.Status, "Aprovada", StringComparison.OrdinalIgnoreCase))
                return (false, "Proposta não aprovada.");

            var c = new Contratacao
            {
                PropostaId = propostaId,
                ContratadoPor = contratadoPor
            };

            _db.Contratacoes.Add(c);
            await _db.SaveChangesAsync();
            return (true, null);
        }
    }
}
