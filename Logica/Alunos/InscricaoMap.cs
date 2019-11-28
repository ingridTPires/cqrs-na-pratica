using FluentNHibernate.Mapping;

namespace Logica.Alunos
{
    public class InscricaoMap : ClassMap<Inscricao>
    {
        public InscricaoMap()
        {
            Id(x => x.Id);

            Map(x => x.Grade).CustomType<int>();

            References(x => x.Aluno);
            References(x => x.Curso);
        }
    }
}
