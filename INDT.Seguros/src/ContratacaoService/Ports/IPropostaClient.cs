using System;
using System.Threading.Tasks;
using ContratacaoService.Application.Dto;

namespace ContratacaoService.Ports
{
    public interface IPropostaClient
    {
        Task<PropostaDto?> GetPropostaByIdAsync(Guid id);
    }
}
