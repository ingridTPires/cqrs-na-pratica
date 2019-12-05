using UI.API;
using UI.Common;

namespace UI.Alunos
{
    public sealed class TransferirAlunoViewModel : ViewModel
    {
        public TransferirAlunoViewModel(long alunoId)
        {
            _alunoId = alunoId;

            OkCommand = new Command(Salvar);
            CancelarCommand = new Command(() => DialogResult = false);
        }

        public static string[] Cursos { get; } = { "", "Matemática", "Português", "Química", "Ciências", "Física", "Biologia", "Inglês" };
        public static string[] Grades { get; } = { "", "A", "B", "C", "D", "F" };

        private readonly long _alunoId;
        public string Curso { get; set; }
        public string Grade { get; set; }

        public Command OkCommand { get; }
        public Command CancelarCommand { get; }

        public override string Caption => "Transferir aluno de curso";
        public override double Height => 230;

        public bool DialogResult { get; private set; }

        private void Salvar()
        {
            var dto = new AlunoTransferenciaDto
            {
                Id = _alunoId,
                Curso = Curso,
                Grade = Grade,
                NumeroInscricao = 1
            };

            var result = ApiClient.Transferir(dto).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
