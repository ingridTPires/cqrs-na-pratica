using CSharpFunctionalExtensions;
using Logica.Decorators;
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
    [DatabaseRetry]
    public sealed class DesinscreverAlunoCommandHandler : ICommandHandler<DesinscreverAlunoCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public DesinscreverAlunoCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        public Result Handle(DesinscreverAlunoCommand command)
        {
            var uow = new UnitOfWork(_sessionFactory);
            var alunoRepositorio = new AlunoRepositorio(uow);
            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            if (string.IsNullOrEmpty(command.Comentario))
                return Result.Fail($"É necessario informar um comentário para desinscrever de um curso");

            var inscricao = aluno.RecuperarInscricao(command.NumeroInscricao);

            if (inscricao == null)
                return Result.Fail($"Nenhuma inscrição encontrada com o número: {command.NumeroInscricao}");

            aluno.RemoverInscricao(inscricao, command.Comentario);

            uow.Commit();

            return Result.Ok();
        }
    }
}
