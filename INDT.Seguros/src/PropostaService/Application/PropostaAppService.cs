using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PropostaService.Domain;

namespace PropostaService.Application
{
    public class PropostaAppService : IPropostaAppService
    {
        private readonly IPropostaRepository _repo;

        public PropostaAppService(IPropostaRepository repo)
        {
            _repo = repo;
        }

        public async Task<Proposta> CreateAsync(string cliente, decimal valor, string descricao)
        {
            // Regras de domínio simples podem ser aplicadas aqui
            if (string.IsNullOrWhiteSpace(cliente)) throw new ArgumentException("Cliente obrigatório");
            if (valor <= 0) throw new ArgumentException("Valor deve ser maior que zero");

            var p = new Proposta
            {
                Cliente = cliente,
                Valor = valor,
                Descricao = descricao
            };

            await _repo.AddAsync(p);
            await _repo.SaveChangesAsync();
            return p;
        }

        public async Task<IEnumerable<Proposta>> ListAsync() => await _repo.ListAsync();

        public async Task<Proposta?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);

        public async Task<bool> ChangeStatusAsync(Guid id, PropostaStatus newStatus)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p is null) return false;

            // business rule: only change from EmAnalise
            if (p.Status != PropostaStatus.EmAnalise) return false;

            p.Status = newStatus;
            _repo.Update(p);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
