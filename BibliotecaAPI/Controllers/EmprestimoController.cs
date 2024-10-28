using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ControllerBase
    {
        private readonly EmprestimoRepository _emprestimoRepository;

        public EmprestimoController(EmprestimoRepository emprestimoRepository)
        {
            _emprestimoRepository = emprestimoRepository;
        }

        // POST api/emprestimo/registrar-emprestimo
        [HttpPost("registrar-emprestimo")]
        public async Task<IActionResult> RegistrarEmprestimo([FromBody] Emprestimo emprestimo)
        {
            var resultado = await _emprestimoRepository.RegistrarEmprestimo(emprestimo);

            if (resultado > 0)
            {
                return Ok(new { mensagem = "Empréstimo registrado com sucesso." });
            }

            return Ok(new { mensagem = "Livro não disponível para empréstimo." });
        }


        // POST api/emprestimo/registrar-devolucao
        [HttpPost("registrar-devolucao")]
        public async Task<IActionResult> RegistrarDevolucaoDB(int emprestimoId)
        {
      
                await _emprestimoRepository.RegistrarDevolucaoDB(emprestimoId);
                return Ok(new { mensagem = "Devolução registrada com sucesso." });

        }

        // GET api/emprestimo/consultar-historico/{usuarioId}
        [HttpGet("consultar-historico")]
        public async Task<IActionResult> ConsultarHistoricoEmprestimos(int usuarioId)
        {
            var historico = await _emprestimoRepository.ConsultarHistoricoEmprestimos(usuarioId);
            return Ok(historico);
        }
    }
}
