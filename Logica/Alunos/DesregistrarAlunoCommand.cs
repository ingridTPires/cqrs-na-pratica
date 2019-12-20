using CSharpFunctionalExtensions;
using Logica.Utils;

namespace Logica.Alunos
{
    public class DesregistrarAlunoCommand : ICommand
    {
        public DesregistrarAlunoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
    [DatabaseRetry]
    public sealed class DesregistrarAlunoCommandHandler : ICommandHandler<DesregistrarAlunoCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public DesregistrarAlunoCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        public Result Handle(DesregistrarAlunoCommand command)
        {
            var uow = new UnitOfWork(_sessionFactory);
            var alunoRepositorio = new AlunoRepositorio(uow);
            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            alunoRepositorio.Excluir(aluno);
            uow.Commit();

            return Result.Ok();
        }
    }
}
