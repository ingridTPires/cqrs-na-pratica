using UI.API;
using UI.Common;

namespace UI.Alunos
{
    public class EditarInformacoesPessoaisViewModel : ViewModel
    {
        public EditarInformacoesPessoaisViewModel(long alunoId, string nome, string email)
        {
            _alunoId = alunoId;
            Nome = nome;
            Email = email;

            OkCommand = new Command(Salvar);
            CancelarCommand = new Command(() => DialogResult = false);
        }

        private readonly long _alunoId;
        public string Nome { get; set; }
        public string Email { get; set; }

        public Command OkCommand { get; }
        public Command CancelarCommand { get; }

        public override string Caption => "Editar informações pessoais";
        public override double Height => 240;

        private void Salvar()
        {
            var dto = new AlunoInformacoesPessoaisDto
            {
                Id = _alunoId,
                Email = Email,
                Nome = Nome
            };

            var result = ApiClient.EditarInformacoesPessoais(dto).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.IsFailure)
            {
                CustomMessageBox.ShowError(result.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
