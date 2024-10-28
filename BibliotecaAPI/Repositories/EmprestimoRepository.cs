using BibliotecaAPI.Models;
using Dapper;
using MySql.Data.MySqlClient;
using Mysqlx;
using System.Data;

namespace BibliotecaAPI.Repositories
{
    public class EmprestimoRepository
    {
        private readonly string _connectionString;

        public EmprestimoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        private IDbConnection Connection =>
            new MySqlConnection(_connectionString);

        public async Task<int> RegistrarEmprestimo(Emprestimo emprestimo)
        {
            using (var conn = Connection)
            {

                var sqlVerificarSeLivroDisponivel = "SELECT Disponivel FROM Livros WHERE Id = @LivroId";
                var disponivel = await conn.ExecuteScalarAsync<int>(sqlVerificarSeLivroDisponivel, new { LivroId = emprestimo.LivroId });

                if(disponivel == 0)
                {
                    return 0;
                }
 
                var sql = "INSERT INTO Emprestimos (LivroId, UsuarioId, DataEmprestimo,DataDevolucao) " +
                          "VALUES (@LivroId, @UsuarioId, @DataEmprestimo,@DataDevolucao);" +
                          "SELECT LAST_INSERT_ID();";

                emprestimo.DataDevolucao = emprestimo.DataEmprestimo.AddDays(14);

                var emprestimoId = await conn.ExecuteScalarAsync<int>(sql, new
                {
                    LivroId = emprestimo.LivroId,
                    UsuarioId = emprestimo.UsuarioId,
                    DataEmprestimo = emprestimo.DataEmprestimo,
                    DataDevolucao = emprestimo.DataDevolucao
                });

                var sqlAtualizarLivroDisponivel = "UPDATE Livros SET Disponivel = 0 WHERE Id = @LivroId";

                await conn.ExecuteAsync(sqlAtualizarLivroDisponivel, new { LivroId = emprestimo.LivroId });

                return emprestimoId;
            }
        }
        public async Task<bool> RegistrarDevolucaoDB(int emprestimoId)
        {
            using (var conn = Connection)
            {
                var sqlAtualizarEmprestimo = "UPDATE Emprestimos SET DataDevolucao = @DataDevolucao WHERE Id = @EmprestimoId";
                await conn.ExecuteAsync(sqlAtualizarEmprestimo, new
                {
                    DataDevolucao = DateTime.Now,
                    EmprestimoId = emprestimoId
                });

                var sqlAtualizarLivroDisponivel = "UPDATE Livros SET Disponivel = 1 WHERE Id = (SELECT LivroId FROM Emprestimos WHERE Id = @EmprestimoId)";
                await conn.ExecuteAsync(sqlAtualizarLivroDisponivel, new { EmprestimoId = emprestimoId });

                return true;
            }
        }

        public async Task<IEnumerable<Emprestimo>> ConsultarHistoricoEmprestimos(int usuarioId)
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Emprestimos WHERE UsuarioId = @UsuarioId";
                return await conn.QueryAsync<Emprestimo>(sql, new { UsuarioId = usuarioId });
            }
        }


    }
}