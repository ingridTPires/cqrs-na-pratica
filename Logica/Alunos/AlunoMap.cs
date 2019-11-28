using FluentNHibernate.Mapping;

namespace Logica.Alunos
{
    public class AlunoMap : ClassMap<Aluno>
    {
        public AlunoMap()
        {
            Id(x => x.Id);

            Map(x => x.Nome);
            Map(x => x.Email);

            HasMany(x => x.Inscricoes).Access.CamelCaseField(Prefix.Underscore).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Desincricoes).Access.CamelCaseField(Prefix.Underscore).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
