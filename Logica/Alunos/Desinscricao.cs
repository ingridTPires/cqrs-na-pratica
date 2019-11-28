using System;

namespace Logica.Alunos
{
    public class Desinscricao : Entidade
    {
        public virtual Aluno Aluno { get; protected set; }
        public virtual Curso Curso { get; protected set; }
        public virtual DateTime Data { get; protected set; }
        public virtual string Comentario { get; protected set; }

        protected Desinscricao()
        {
        }

        public Desinscricao(Aluno aluno, Curso curso, string comentario)
            : this()
        {
            Aluno = aluno;
            Curso = curso;
            Comentario = comentario;
            Data = DateTime.UtcNow;
        }
    }
}
