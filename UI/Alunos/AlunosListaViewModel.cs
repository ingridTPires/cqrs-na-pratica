using API.Models;
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
        public Command RegistrarAlunoCommand { get; }
        public Command<AlunoDto> EditarInformacoesPessoaisCommand { get; }
        public Command<AlunoDto> ExcluirAlunoCommand { get; }
        public IReadOnlyList<AlunoDto> Alunos { get; private set; }

        public AlunosListaViewModel()
        {
            PesquisarCommand = new Command(Pesquisar);
            RegistrarAlunoCommand = new Command(RegistrarAluno);
            EditarInformacoesPessoaisCommand = new Command<AlunoDto>(p => p != null, EditarInformacoesPessoais);
            ExcluirAlunoCommand = new Command<AlunoDto>(p => p != null, ExcluirAluno);

            Pesquisar();
        }

        private void ExcluirAluno(AlunoDto dto)
        {
            //ApiClient.Excluir(dto.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            Pesquisar();
        }

        private void EditarInformacoesPessoais(AlunoDto dto)
        {
            var viewModel = new EditarInformacoesPessoaisViewModel(dto.Id, dto.Nome, dto.Email);
            _dialogService.ShowDialog(viewModel);

            Pesquisar();
        }

        private void RegistrarAluno()
        {
            var viewModel = new RegistrarAlunoViewModel();
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
