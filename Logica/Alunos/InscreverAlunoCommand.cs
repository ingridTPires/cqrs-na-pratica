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

    public sealed class InscreverAlunoCommandHandler : ICommandHandler<InscreverAlunoCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public InscreverAlunoCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Result Handle(InscreverAlunoCommand command)
        {
            var alunoRepositorio = new AlunoRepositorio(_unitOfWork);
            var cursoRepositorio = new CursoRepositorio(_unitOfWork);
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
            _unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
