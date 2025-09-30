using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropostaService.Domain
{
    public interface IPropostaRepository
    {
        Task AddAsync(Proposta proposta);
        Task<Proposta?> GetByIdAsync(Guid id);
        Task<IEnumerable<Proposta>> ListAsync();
        void Update(Proposta proposta); // altera entidade em memória
        Task SaveChangesAsync();
    }
}
