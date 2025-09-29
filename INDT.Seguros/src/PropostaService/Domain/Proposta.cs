using System;

namespace PropostaService.Domain;

public enum PropostaStatus { EmAnalise, Aprovada, Rejeitada }

public class Proposta
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Cliente { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public PropostaStatus Status { get; set; } = PropostaStatus.EmAnalise;
    public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
}
