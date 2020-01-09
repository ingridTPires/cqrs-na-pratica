using Dapper;
using Logica.Models;
using Logica.Utils;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Logica.Alunos
{
    public sealed class RecuperarAlunosQuery : IQuery<List<AlunoDto>>
    {
        public RecuperarAlunosQuery(string cursoNome, int? numero)
        {
            CursoNome = cursoNome;
            Numero = numero;
        }

        public string CursoNome { get; set; }
        public int? Numero { get; set; }
    }

    public sealed class RecuperarAlunosQueryHandler : IQueryHandler<RecuperarAlunosQuery, List<AlunoDto>>
    {
        private readonly ConnectionString _connectionString;

        public RecuperarAlunosQueryHandler(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public List<AlunoDto> Handle(RecuperarAlunosQuery query)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var sql = @"SELECT a.*, i.[Grade], c.[Nome] AS [Curso], c.[Creditos] FROM [Aluno] a
                            LEFT JOIN (SELECT i.[AlunoID], COUNT(*) AS [Numero] FROM [Inscricao] i GROUP BY i.[AlunoID]) t ON (a.[AlunoID] = t.[AlunoID])
                            LEFT JOIN [Inscricao] i ON (i.[AlunoID] = a.[AlunoID])
                            LEFT JOIN [Curso] c ON (i.[CursoID] = c.[CursoID])
                            WHERE (c.[Nome] = @CursoNome OR @CursoNome IS NULL)
                            AND (ISNULL(t.[Numero], 0) = @Numero OR @Numero IS NULL)
                            ORDER BY a.[AlunoID] ASC;";

                var alunosDb = conn.Query<AlunoDB>(sql, new
                {
                    query.CursoNome,
                    query.Numero
                });

                var ids = alunosDb.GroupBy(p => p.AlunoID)
                  .Select(p => p.Key);

                var alunos = new List<AlunoDto>(ids.Count());

                foreach (var id in ids)
                {
                    var resultado = alunosDb.Where(p => p.AlunoID == id).ToList();

                    var dto = new AlunoDto()
                    {
                        Id = resultado[0].AlunoID,
                        Nome = resultado[0].Nome,
                        Email = resultado[0].Email,
                        Curso1 = resultado[0].Curso,
                        Curso1Creditos = resultado[0].Creditos,
                        Curso1Grade = resultado[0]?.Grade.ToString()
                    };

                    if (resultado.Count > 1)
                    {
                        dto.Curso2 = resultado[1].Curso;
                        dto.Curso2Creditos = resultado[1].Creditos;
                        dto.Curso2Grade = resultado[1]?.Grade.ToString();
                    }

                    alunos.Add(dto);
                }

                return alunos;

            }
        }
        private class AlunoDB
        {
            public readonly long AlunoID;
            public readonly string Nome;
            public readonly string Email;
            public readonly string Curso;
            public readonly Grade? Grade;
            public readonly int? Creditos;

            public AlunoDB(long alunoID, string nome, string email, Grade? grade, string curso, int? creditos)
            {
                AlunoID = alunoID;
                Nome = nome;
                Email = email;
                Grade = grade;
                Curso = curso;
                Creditos = creditos;
            }
        }

    }
}
