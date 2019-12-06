using CSharpFunctionalExtensions;
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

    public sealed class EditarInformacoesPessoaisHandler : ICommandHandler<EditarInformacoesPessoaisCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public EditarInformacoesPessoaisHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Result Handle(EditarInformacoesPessoaisCommand command)
        {
            var alunoRepositorio = new AlunoRepositorio(_unitOfWork);
            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}"); 

            aluno.Nome = command.Nome;
            aluno.Email = command.Email;

            _unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
