using System;

namespace ContratacaoService.Application.Dto
{
    public class PropostaDto
    {
        public Guid Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
