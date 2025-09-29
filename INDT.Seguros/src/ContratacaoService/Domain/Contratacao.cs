using System;

namespace ContratacaoService.Domain;

public class Contratacao
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PropostaId { get; set; }
    public string ContratadoPor { get; set; } = string.Empty;
    public DateTime DataContratacao { get; set; } = DateTime.UtcNow;
}
