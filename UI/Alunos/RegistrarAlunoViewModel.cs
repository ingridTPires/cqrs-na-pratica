using UI.API;
using UI.Common;

namespace UI.Alunos
{
    public sealed class RegistrarAlunoViewModel : ViewModel
    {
        public RegistrarAlunoViewModel()
        {
            Aluno = new AlunoNovoDto();

            OkCommand = new Command(Salvar);
            CancelarCommand = new Command(() => DialogResult = false);
        }

        public static string[] Cursos { get; } = { "", "Matemática", "Português", "Química", "Ciências", "Física", "Biologia", "Inglês" };
        public static string[] Grades { get; } = { "", "A", "B", "C", "D", "F" };

        public AlunoNovoDto Aluno { get; }

        public Command OkCommand { get; }
        public Command CancelarCommand { get; }

        public override string Caption => "Registrar Aluno";
        public override double Height => 450;

        private void Salvar()
        {
            var result = ApiClient.Registrar(Aluno).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
