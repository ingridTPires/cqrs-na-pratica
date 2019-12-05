using UI.API;
using UI.Common;

namespace UI.Alunos
{
    public sealed class DesinscreverAlunoViewModel : ViewModel
    {
        public DesinscreverAlunoViewModel(long alunoId, int numeroInscricao)
        {
            _alunoId = alunoId;
            _numeroInscricao = numeroInscricao;

            OkCommand = new Command(Save);
            CancelarCommand = new Command(() => DialogResult = false);
        }

        private readonly long _alunoId;
        private readonly int _numeroInscricao;
        public string Comentario { get; set; }

        public Command OkCommand { get; }
        public Command CancelarCommand { get; }

        public override string Caption => "Desinscrever Aluno";
        public override double Height => 180;

        private void Save()
        {
            var dto = new AlunoDesinscricaoDto
            {
                Id = _alunoId,
                Comentario = Comentario,
                NumeroInscricao = _numeroInscricao
            };

            var result = ApiClient.Desinscrever(dto).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
