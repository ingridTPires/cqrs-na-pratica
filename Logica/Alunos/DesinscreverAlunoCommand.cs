using CSharpFunctionalExtensions;
using Logica.Utils;

namespace Logica.Alunos
{
    public class DesinscreverAlunoCommand : ICommand
    {
        public DesinscreverAlunoCommand(long id, int numeroInscricao, string comentario)
        {
            Id = id;
            NumeroInscricao = numeroInscricao;
            Comentario = comentario;
        }

        public long Id { get; set; }
        public int NumeroInscricao { get; set; }
        public string Comentario { get; set; }
    }

    public sealed class DesinscreverAlunoCommandHandler : ICommandHandler<DesinscreverAlunoCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public DesinscreverAlunoCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Result Handle(DesinscreverAlunoCommand command)
        {
            var alunoRepositorio = new AlunoRepositorio(_unitOfWork);
            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            if (string.IsNullOrEmpty(command.Comentario))
                return Result.Fail($"É necessario informar um comentário para desinscrever de um curso");

            var inscricao = aluno.RecuperarInscricao(command.NumeroInscricao);

            if (inscricao == null)
                return Result.Fail($"Nenhuma inscrição encontrada com o número: {command.NumeroInscricao}");

            aluno.RemoverInscricao(inscricao, command.Comentario);

            _unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
