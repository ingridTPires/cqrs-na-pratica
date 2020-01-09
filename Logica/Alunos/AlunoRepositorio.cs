using Logica.Utils;
using System.Linq;

namespace Logica.Alunos
{
    public sealed class AlunoRepositorio
    {
        private readonly UnitOfWork _unitOfWork;

        public AlunoRepositorio(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Aluno RecuperarPorId(long id)
        {
            return _unitOfWork.Get<Aluno>(id);
        }

        public void Salvar(Aluno aluno)
        {
            _unitOfWork.SaveOrUpdate(aluno);
        }

        public void Excluir(Aluno aluno)
        {
            _unitOfWork.Delete(aluno);
        }
    }

    public sealed class CursoRepositorio
    {
        private readonly UnitOfWork _unitOfWork;

        public CursoRepositorio(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Curso RecuperarPorNome(string name)
        {
            return _unitOfWork.Query<Curso>().SingleOrDefault(x => x.Nome == name);
        }
    }
}
