using CSharpFunctionalExtensions;
using UI.API;
using UI.Common;

namespace UI.Alunos
{
    public sealed class AlunoViewModel : ViewModel
    {
        public static string[] Cursos { get; } = { "", "Matemática", "Português", "Química", "Ciências", "Física", "Biologia", "Inglês" };
        public static string[] Grades { get; } = { "", "A", "B", "C", "D", "F" };

        private readonly bool _isNew;
        public AlunoDto Aluno { get; }

        public Command OkCommand { get; }
        public Command CancelarCommand { get; }

        public override string Caption => (_isNew ? "Adicionar" : "Editar") + " Aluno";
        public override double Height => 450;

        public AlunoViewModel(AlunoDto aluno = null)
        {
            if (aluno == null)
            {
                Aluno = new AlunoDto();
                _isNew = true;
            }
            else
            {
                Aluno = aluno;
                _isNew = false;
            }

            OkCommand = new Command(Salvar);
            CancelarCommand = new Command(() => DialogResult = false);
        }

        private void Salvar()
        {
            Result resultado;

            if (_isNew)
            {
                resultado = ApiClient.Criar(Aluno).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                resultado = ApiClient.Atualizar(Aluno).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            if (resultado.IsFailure)
            {
                CustomMessageBox.ShowError(resultado.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
