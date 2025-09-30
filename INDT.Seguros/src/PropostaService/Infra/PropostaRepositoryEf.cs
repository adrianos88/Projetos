using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropostaService.Domain;

namespace PropostaService.Infra
{
    public class PropostaRepositoryEf : IPropostaRepository
    {
        private readonly AppDbContext _db;
        public PropostaRepositoryEf(AppDbContext db) => _db = db;

        public async Task AddAsync(Proposta proposta)
        {
            await _db.Propostas.AddAsync(proposta);
        }

        public async Task<Proposta?> GetByIdAsync(Guid id)
        {
            return await _db.Propostas.FindAsync(id);
        }

        public async Task<IEnumerable<Proposta>> ListAsync()
        {
            return await _db.Propostas.AsNoTracking().OrderByDescending(p => p.CriadaEm).ToListAsync();
        }

        public void Update(Proposta proposta)
        {
            _db.Propostas.Update(proposta);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
