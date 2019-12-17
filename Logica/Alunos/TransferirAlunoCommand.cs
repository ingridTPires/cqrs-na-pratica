using CSharpFunctionalExtensions;
using Logica.Utils;
using System;

namespace Logica.Alunos
{
    public class TransferirAlunoCommand : ICommand
    {
        public TransferirAlunoCommand(long id, int numeroInscricao, string curso, string grade)
        {
            Id = id;
            NumeroInscricao = numeroInscricao;
            Curso = curso;
            Grade = grade;
        }

        public long Id { get; set; }
        public int NumeroInscricao { get; set; }
        public string Curso { get; set; }
        public string Grade { get; set; }
    }

    public sealed class TransferirAlunoCommandHandler : ICommandHandler<TransferirAlunoCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public TransferirAlunoCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Result Handle(TransferirAlunoCommand command)
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

            var inscricao = aluno.RecuperarInscricao(command.NumeroInscricao);

            if (inscricao == null)
                return Result.Fail($"Nenhuma inscrição encontrada com o número: {command.NumeroInscricao}");

            inscricao.Atualizar(curso, grade);
            _unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
