using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropostaService.Domain;
using PropostaService.Infra;

namespace PropostaService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropostasController : ControllerBase
{
    private readonly AppDbContext _db;
    public PropostasController(AppDbContext db) => _db = db;

    public record CreatePropostaDto(string Cliente, decimal Valor, string Descricao);
    public record UpdateStatusDto(PropostaStatus Status);

    [HttpPost]
    public async Task<ActionResult<Proposta>> Create(CreatePropostaDto dto)
    {
        var p = new Proposta { Cliente = dto.Cliente, Valor = dto.Valor, Descricao = dto.Descricao };
        _db.Propostas.Add(p);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Proposta>>> GetAll()
        => await _db.Propostas.AsNoTracking().OrderByDescending(p => p.CriadaEm).ToListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Proposta>> GetById(Guid id)
    {
        var p = await _db.Propostas.FindAsync(id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusDto dto)
    {
        var p = await _db.Propostas.FindAsync(id);
        if (p is null) return NotFound();

        if (p.Status != PropostaStatus.EmAnalise)
            return BadRequest("Status só pode ser alterado a partir de EmAnalise.");

        p.Status = dto.Status;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
