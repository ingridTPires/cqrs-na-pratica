using CSharpFunctionalExtensions;
using Logica.Decorators;
using Logica.Utils;

namespace Logica.Alunos
{
    public sealed class EditarInformacoesPessoaisCommand : ICommand
    {
        public EditarInformacoesPessoaisCommand(long id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        public long Id { get; }
        public string Nome { get; }
        public string Email { get; }
    }

    [AuditLog]
    [DatabaseRetry]
    public sealed class EditarInformacoesPessoaisCommandHandler : ICommandHandler<EditarInformacoesPessoaisCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public EditarInformacoesPessoaisCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        public Result Handle(EditarInformacoesPessoaisCommand command)
        {
            var uow = new UnitOfWork(_sessionFactory);
            var alunoRepositorio = new AlunoRepositorio(uow);
            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}"); 

            aluno.Nome = command.Nome;
            aluno.Email = command.Email;

            uow.Commit();

            return Result.Ok();
        }
    }
}
