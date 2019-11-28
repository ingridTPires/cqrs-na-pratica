namespace Logica.Alunos
{
    public class Inscricao : Entidade
    {
        public virtual Aluno Aluno { get; protected set; }
        public virtual Curso Curso { get; protected set; }
        public virtual Grade Grade { get; protected set; }

        protected Inscricao()
        {
        }

        public Inscricao(Aluno aluno, Curso curso, Grade grade)
            : this()
        {
            Aluno = aluno;
            Curso = curso;
            Grade = grade;
        }

        public virtual void Atualizar(Curso curso, Grade grade)
        {
            Curso = curso;
            Grade = grade;
        }
    }

    public enum Grade
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        F = 5
    }
}
