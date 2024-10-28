using BibliotecaAPI.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BibliotecaAPI.Repositories
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection =>
            new MySqlConnection(_connectionString);

        public async Task<int> CadastrarUsuarioDB(Usuario dados)
        {
            var sql = "INSERT INTO Usuarios (Nome, Email) VALUES (@Nome, @Email)";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, new
                {
                    Nome = dados.Nome,
                    Email = dados.Email,
                });
            }
        }

        public async Task<IEnumerable<Usuario>> BuscarUsuarios(string? nome = null, string? email = null)
        {
            var sql = "SELECT * FROM Usuarios WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(nome))
            {
                sql += " AND Nome = @Nome";
                parameters.Add("Nome", nome); 
            }

            if (!string.IsNullOrEmpty(email))
            {
                sql += " AND Email = @Email";
                parameters.Add("Email", email);
            }

            using (var conn = Connection)
            {
                return await conn.QueryAsync<Usuario>(sql, parameters);
            }
        }

    }
}
