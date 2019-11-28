using FluentNHibernate.Mapping;

namespace Logica.Alunos
{
    public class CursoMap : ClassMap<Curso>
    {
        public CursoMap()
        {
            Id(x => x.Id);

            Map(x => x.Nome);
            Map(x => x.Creditos);
        }
    }
}
