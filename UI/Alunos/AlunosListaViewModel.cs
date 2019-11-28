using System.Collections.Generic;
using UI.API;
using UI.Common;

namespace UI.Alunos
{
    public sealed class AlunosListaViewModel : ViewModel
    {
        public static string[] Cursos { get; } = { "", "Matemática", "Português", "Química", "Ciências", "Física", "Biologia", "Inglês" };
        public static string[] NumeroDeCursos { get; } = { "", "0", "1", "2" };

        public string CursoSelecionado { get; set; } = "";
        public string NumeroDeCursosSelecionado { get; set; } = "";

        public Command PesquisarCommand { get; }
        public Command CriarAlunoCommand { get; }
        public Command<AlunoDto> AtualizarAlunoCommand { get; }
        public Command<AlunoDto> ExcluirAlunoCommand { get; }
        public IReadOnlyList<AlunoDto> Alunos { get; private set; }

        public AlunosListaViewModel()
        {
            PesquisarCommand = new Command(Pesquisar);
            CriarAlunoCommand = new Command(CriarAluno);
            AtualizarAlunoCommand = new Command<AlunoDto>(p => p != null, AtualizarAluno);
            ExcluirAlunoCommand = new Command<AlunoDto>(p => p != null, ExcluirAluno);

            Pesquisar();
        }

        private void ExcluirAluno(AlunoDto dto)
        {
            ApiClient.Excluir(dto.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            Pesquisar();
        }

        private void AtualizarAluno(AlunoDto dto)
        {
            var viewModel = new AlunoViewModel(dto);
            _dialogService.ShowDialog(viewModel);

            Pesquisar();
        }

        private void CriarAluno()
        {
            var viewModel = new AlunoViewModel();
            _dialogService.ShowDialog(viewModel);

            Pesquisar();
        }

        private void Pesquisar()
        {
            Alunos = ApiClient.RecuperarLista(CursoSelecionado, NumeroDeCursosSelecionado).ConfigureAwait(false).GetAwaiter().GetResult();

            Notify(nameof(Alunos));
        }
    }
}
