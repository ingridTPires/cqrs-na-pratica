using CSharpFunctionalExtensions;
using Logica.Utils;

namespace Logica.Alunos
{
    public class RegistrarAlunoCommand : ICommand
    {
        public RegistrarAlunoCommand(string nome, string email, string curso1, string curso1Grade, string curso2, string curso2Grade)
        {
            Nome = nome;
            Email = email;
            Curso1 = curso1;
            Curso1Grade = curso1Grade;
            Curso2 = curso2;
            Curso2Grade = curso2Grade;
        }

        public string Nome { get; set; }
        public string Email { get; set; }

        public string Curso1 { get; set; }
        public string Curso1Grade { get; set; }
        public string Curso2 { get; set; }
        public string Curso2Grade { get; set; }

    }

    public sealed class RegistrarAlunoCommandHandler : ICommandHandler<RegistrarAlunoCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public RegistrarAlunoCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Handle(RegistrarAlunoCommand command)
        {
            var cursoRepositorio = new CursoRepositorio(_unitOfWork);
            var alunoRepositorio = new AlunoRepositorio(_unitOfWork);
            var aluno = new Aluno(command.Nome, command.Email);

            if (command.Curso1 != null && command.Curso1Grade != null)
            {
                var curso = cursoRepositorio.RecuperarPorNome(command.Curso1);
                aluno.Inscrever(curso, EnumExtensions.Parse<Grade>(command.Curso1Grade));
            }

            if (command.Curso2 != null && command.Curso2Grade != null)
            {
                var curso = cursoRepositorio.RecuperarPorNome(command.Curso2);
                aluno.Inscrever(curso, EnumExtensions.Parse<Grade>(command.Curso2Grade));
            }

            alunoRepositorio.Salvar(aluno);
            _unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
