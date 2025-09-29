using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContratacaoService.Domain;
using ContratacaoService.Infra;

namespace ContratacaoService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContratacoesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly HttpClient _http;

    public ContratacoesController(AppDbContext db, IHttpClientFactory factory)
    {
        _db = db;
        _http = factory.CreateClient("propostas");
    }

    private record PropostaDto(Guid Id, string Cliente, decimal Valor, string Descricao, string Status);
    public record CreateContratacaoDto(Guid PropostaId, string ContratadoPor);

    [HttpPost]
    public async Task<ActionResult<Contratacao>> Create(CreateContratacaoDto dto)
    {
        var proposta = await _http.GetFromJsonAsync<PropostaDto>($"api/propostas/{dto.PropostaId}");
        if (proposta is null) return BadRequest("Proposta não encontrada no PropostaService.");

        if (!string.Equals(proposta.Status, "Aprovada", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Só é possível contratar propostas Aprovadas.");

        var c = new Contratacao { PropostaId = dto.PropostaId, ContratadoPor = dto.ContratadoPor };
        _db.Contratacoes.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Contratacao>> GetById(Guid id)
    {
        var c = await _db.Contratacoes.FindAsync(id);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contratacao>>> GetAll()
        => await _db.Contratacoes.AsNoTracking().OrderByDescending(c => c.DataContratacao).ToListAsync();
}
