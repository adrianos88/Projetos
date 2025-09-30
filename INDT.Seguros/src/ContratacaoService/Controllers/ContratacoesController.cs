using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ContratacaoService.Application;

namespace ContratacaoService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratacoesController : ControllerBase
    {
        private readonly ContratacaoAppService _app;
        public ContratacoesController(ContratacaoAppService app) => _app = app;

        public record CreateContratacaoDto(Guid PropostaId, string ContratadoPor);

        [HttpPost]
        public async Task<IActionResult> Create(CreateContratacaoDto dto)
        {
            var (ok, message) = await _app.CreateContratacaoAsync(dto.PropostaId, dto.ContratadoPor);
            if (!ok) return BadRequest(message);
            return Created("", null);
        }
    }
}
