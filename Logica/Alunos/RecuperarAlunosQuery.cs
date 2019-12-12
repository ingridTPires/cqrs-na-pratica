using Logica.Models;
using Logica.Utils;
using System.Collections.Generic;
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
        private readonly UnitOfWork _unitOfWork;

        public RecuperarAlunosQueryHandler(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public List<AlunoDto> Handle(RecuperarAlunosQuery query)
        {
            var alunos = new AlunoRepositorio(_unitOfWork).RecuperarLista(query.CursoNome, query.Numero);
            var dtos = alunos.Select(x => ConverterParaDto(x)).ToList();
            return dtos;
        }

        private AlunoDto ConverterParaDto(Aluno aluno)
        {
            return new AlunoDto
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Curso1 = aluno.PrimeiraInscricao?.Curso?.Nome,
                Curso1Grade = aluno.PrimeiraInscricao?.Grade.ToString(),
                Curso1Creditos = aluno.PrimeiraInscricao?.Curso?.Creditos,
                Curso2 = aluno.SegundaInscricao?.Curso?.Nome,
                Curso2Grade = aluno.SegundaInscricao?.Grade.ToString(),
                Curso2Creditos = aluno.SegundaInscricao?.Curso?.Creditos,
            };
        }
    }
}
