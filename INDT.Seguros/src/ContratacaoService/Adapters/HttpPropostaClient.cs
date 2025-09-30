using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContratacaoService.Application.Dto;
using ContratacaoService.Ports;

namespace ContratacaoService.Adapters
{
    public class HttpPropostaClient : IPropostaClient
    {
        private readonly HttpClient _http;
        public HttpPropostaClient(HttpClient http) => _http = http;

        public async Task<PropostaDto?> GetPropostaByIdAsync(Guid id)
        {
            try
            {
                return await _http.GetFromJsonAsync<PropostaDto>($"api/propostas/{id}");
            }
            catch
            {
                // tratar erros como null para simplificar
                return null;
            }
        }
    }
}
