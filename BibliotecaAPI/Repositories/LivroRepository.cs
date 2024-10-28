using BibliotecaAPI.Models;
using Dapper;
using MySql.Data.MySqlClient;
using Mysqlx;
using System.Data;

namespace BibliotecaAPI.Repositories
{
    public class LivroRepository
    {
        private readonly string _connectionString;

        public LivroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        private IDbConnection Connection =>
            new MySqlConnection(_connectionString);

        public async Task<IEnumerable<Livro>> Listar()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Livros";

                return await conn.QueryAsync<Livro>(sql);
            }
        }
        public async Task<int> CadastrarLivrosDB(Livro dados)
        {
            var sql = "INSERT INTO Livros (Titulo,Autor,AnoPublicacao,Genero,Disponivel) " +
                "values (@Titulo,@Autor,@AnoPublicacao,@Genero,@Disponivel)";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql,
                    new
                    {
                        Titulo = dados.Titulo,
                        Autor = dados.Autor,
                        AnoPublicacao = dados.AnoPublicacao,
                        Genero = dados.Genero,
                        Disponivel = dados.Disponivel,


                    });
            }
        }

        public async Task<int> AtualizarLivroDB(Livro dados)
        {
            var sql = "UPDATE Livros set Titulo = @Titulo, " +
                "Autor = @Autor, " +
                "AnoPublicacao = @AnoPublicacao," +
                " Genero = @Genero, " +
                "Disponivel = @Disponivel WHERE Id = @id";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, dados);
            }
        }

        public async Task<int> DeletarPorId(int id)
        {
                var sql = "DELETE FROM Livros WHERE Id = @id AND Disponivel =1";

                using (var conn = Connection)
                {
                    return await conn.ExecuteAsync(sql, new { Id = id });
                }
 
        }

        public async Task<IEnumerable<Livro>> ConsultarLivros(string genero = null, string autor = null, int? anoPublicacao = null)
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Livros WHERE 1=1";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(genero))
                {
                    sql += " AND Genero = @Genero";
                    parameters.Add("Genero", genero);
                }

                if (!string.IsNullOrEmpty(autor))
                {
                    sql += " AND Autor = @Autor";
                    parameters.Add("Autor", autor);
                }

                if (anoPublicacao.HasValue)
                {
                    sql += " AND AnoPublicacao = @AnoPublicacao";
                    parameters.Add("AnoPublicacao", anoPublicacao.Value);
                }

                return await conn.QueryAsync<Livro>(sql, parameters);
            }
        }


    }
}
