using CSharpFunctionalExtensions;
using Logica.Utils;

namespace Logica.Alunos
{
    public sealed class EditarInformacoesPessoaisHandler : ICommand
    {
        public EditarInformacoesPessoaisHandler(long id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        public long Id { get; }
        public string Nome { get; }
        public string Email { get; }
    }

    public sealed class EditarInformacoesPessoaisCommandHandler : ICommandHandler<EditarInformacoesPessoaisHandler>
    {
        private readonly UnitOfWork _unitOfWork;

        public EditarInformacoesPessoaisCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Result Handle(EditarInformacoesPessoaisHandler command)
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
