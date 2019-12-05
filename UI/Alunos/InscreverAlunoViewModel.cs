using UI.API;
using UI.Common;

namespace UI.Alunos
{
    public sealed class InscreverAlunoViewModel : ViewModel
    {
        public static string[] Cursos { get; } = { "", "Matemática", "Português", "Química", "Ciências", "Física", "Biologia", "Inglês" };
        public static string[] Grades { get; } = { "", "A", "B", "C", "D", "F" };

        public InscreverAlunoViewModel(long alunoId)
        {
            _alunoId = alunoId;

            OkCommand = new Command(Salvar);
            CancelarCommand = new Command(() => DialogResult = false);
        }

        private readonly long _alunoId;
        public string Curso { get; set; }
        public string Grade { get; set; }

        public Command OkCommand { get; }
        public Command CancelarCommand { get; }

        public override string Caption => "Increver aluno";
        public override double Height => 230;

        private void Salvar()
        {
            var dto = new AlunoInscricaoDto
            {
                Id = _alunoId,
                Curso = Curso,
                Grade = Grade
            };

            var result = ApiClient.Inscrever(dto).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
