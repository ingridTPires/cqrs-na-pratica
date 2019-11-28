using Logica.Utils;
using System.Collections.Generic;
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

        public IReadOnlyList<Aluno> RecuperarLista(string cursoNome, int? numeroDeCursos)
        {
            IQueryable<Aluno> query = _unitOfWork.Query<Aluno>();

            if (!string.IsNullOrWhiteSpace(cursoNome))
            {
                query = query.Where(x => x.Inscricoes.Any(e => e.Curso.Nome == cursoNome));
            }

            List<Aluno> resultado = query.ToList();

            if (numeroDeCursos != null)
            {
                resultado = resultado.Where(x => x.Inscricoes.Count == numeroDeCursos).ToList();
            }

            return resultado;
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
