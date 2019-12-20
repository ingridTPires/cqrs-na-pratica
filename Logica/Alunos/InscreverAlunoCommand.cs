using CSharpFunctionalExtensions;
using Logica.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.Alunos
{
    public class InscreverAlunoCommand : ICommand
    {
        public InscreverAlunoCommand(long id, string curso, string grade)
        {
            Id = id;
            Curso = curso;
            Grade = grade;
        }

        public long Id { get; set; }
        public string Curso { get; set; }
        public string Grade { get; set; }

    }
    [DatabaseRetry]
    public sealed class InscreverAlunoCommandHandler : ICommandHandler<InscreverAlunoCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public InscreverAlunoCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        public Result Handle(InscreverAlunoCommand command)
        {
            var uow = new UnitOfWork(_sessionFactory);
            var alunoRepositorio = new AlunoRepositorio(uow);
            var cursoRepositorio = new CursoRepositorio(uow);
            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            var curso = cursoRepositorio.RecuperarPorNome(command.Curso);

            if (curso == null)
                return Result.Fail($"O curso é incorreto: {command.Curso}.");

            var gradeSucesso = Enum.TryParse(command.Grade, out Grade grade);

            if (!gradeSucesso)
                return Result.Fail($"A grade é incorreta: {command.Grade}.");

            aluno.Inscrever(curso, grade);
            uow.Commit();

            return Result.Ok();
        }
    }
}
