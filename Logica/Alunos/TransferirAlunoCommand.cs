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
    [DatabaseRetry]
    public sealed class TransferirAlunoCommandHandler : ICommandHandler<TransferirAlunoCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public TransferirAlunoCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        public Result Handle(TransferirAlunoCommand command)
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

            var inscricao = aluno.RecuperarInscricao(command.NumeroInscricao);

            if (inscricao == null)
                return Result.Fail($"Nenhuma inscrição encontrada com o número: {command.NumeroInscricao}");

            inscricao.Atualizar(curso, grade);
            uow.Commit();

            return Result.Ok();
        }
    }
}
