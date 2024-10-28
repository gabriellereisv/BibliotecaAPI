using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // POST api/<UsuarioController>
        [HttpPost("cadastrar-usuario")]
        public async Task<IActionResult> CadastrarUsuarioDB([FromBody] Usuario usuario)
        {
            var usuarioId = await _usuarioRepository.CadastrarUsuarioDB(usuario);
            return Ok(new { mensagem = "Usuário registrado com sucesso." });
        }

        // GET api/<UsuarioController>/buscar-usuario
        [HttpGet("buscar-usuario")]
        public async Task<IActionResult> BuscarUsuarios(string? nome = null, string? email = null)
        {
            var usuarios = await _usuarioRepository.BuscarUsuarios(nome, email);
            return Ok(usuarios);
        }
    }
}
