using Microsoft.AspNetCore.Mvc;
using PropostaService.Application;
using PropostaService.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropostaService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropostasController : ControllerBase
    {
        private readonly IPropostaAppService _app;

        public PropostasController(IPropostaAppService app) => _app = app;

        public record CreatePropostaDto(string Cliente, decimal Valor, string Descricao);
        public record UpdateStatusDto(PropostaStatus Status);

        [HttpPost]
        public async Task<ActionResult<Proposta>> Create(CreatePropostaDto dto)
        {
            try
            {
                var p = await _app.CreateAsync(dto.Cliente, dto.Valor, dto.Descricao);
                return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proposta>>> GetAll()
            => Ok(await _app.ListAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Proposta>> GetById(Guid id)
        {
            var p = await _app.GetByIdAsync(id);
            return p is null ? NotFound() : Ok(p);
        }

        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusDto dto)
        {
            var ok = await _app.ChangeStatusAsync(id, dto.Status);
            return ok ? NoContent() : BadRequest("Não foi possível alterar o status.");
        }
    }
}
