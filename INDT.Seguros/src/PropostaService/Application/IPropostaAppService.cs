using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PropostaService.Domain;

namespace PropostaService.Application
{
    public interface IPropostaAppService
    {
        Task<Proposta> CreateAsync(string cliente, decimal valor, string descricao);
        Task<IEnumerable<Proposta>> ListAsync();
        Task<Proposta?> GetByIdAsync(Guid id);
        Task<bool> ChangeStatusAsync(Guid id, PropostaStatus newStatus);
    }
}
