using FluentNHibernate.Mapping;

namespace Logica.Alunos
{
    public class DesinscricaoMap : ClassMap<Desinscricao>
    {
        public DesinscricaoMap()
        {
            Id(x => x.Id);

            Map(x => x.Data);
            Map(x => x.Comentario);

            References(x => x.Aluno);
            References(x => x.Curso);
        }
    }
}
