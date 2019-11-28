namespace Logica.Alunos
{
    public class Curso : Entidade
    {
        public virtual string Nome { get; protected set; }
        public virtual int Creditos { get; protected set; }
    }
}
