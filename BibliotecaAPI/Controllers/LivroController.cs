using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly LivroRepository _livroRepository;

        public LivroController(LivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }

        [HttpGet("livros-cadastrados")]
        public async Task<IActionResult> ListagemLivros()
        {
            var dados = await _livroRepository.Listar();
            return Ok(dados);
        }

        // POST api/<LivroController>
        [HttpPost("registrar-livro")]
        public async Task<IActionResult> CadastrarLivrosDB([FromBody] Livro livro)
        {
            var livroId = await _livroRepository.CadastrarLivrosDB(livro);
            return Ok(new { mensagem = "Livro registrado com sucesso." });
        }


        // PUT api/<LivroController>/5
        [HttpPut("atualizar-livro")]
        public async Task<IActionResult> AtualizarLivroDB(int id, [FromBody] Livro dados)
        {
            dados.Id = id;
            await _livroRepository.AtualizarLivroDB(dados);
            return Ok(new { mensagem = "Livro atualizado com sucesso." });
        }

        // DELETE api/<LivroController>/5
        [HttpDelete("deletar-livro")]
        public async Task<IActionResult> DeletarPorId(int id)
        {
            var resultado = await _livroRepository.DeletarPorId(id);
            if (resultado > 0)
            {
                return Ok(new { mensagem = "Livro deletado com sucesso." });
            }
            else
            {
                return Ok(new { mensagem = "Livro não foi deletado pois está emprestado." });
            }
        }

        // CONSULTA api/<LivroController>/5
        [HttpGet("consultar-livros")]
        public async Task<IActionResult> ConsultarLivros([FromQuery] string? genero = null, [FromQuery] string? autor = null, [FromQuery] int? anoPublicacao = null)
        {
            var livros = await _livroRepository.ConsultarLivros(genero, autor, anoPublicacao);
            return Ok(livros);
        }

    }
}
