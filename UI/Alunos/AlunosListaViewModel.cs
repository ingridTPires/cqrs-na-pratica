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
        public Command<long> InscreverAlunoCommand { get; }
        public Command<long> TransferirAlunoCommand { get; set; }
        public Command<long> DesinscreverAlunoCommand { get; set; }

        public IReadOnlyList<AlunoDto> Alunos { get; private set; }

        public AlunosListaViewModel()
        {
            PesquisarCommand = new Command(Pesquisar);
            RegistrarAlunoCommand = new Command(RegistrarAluno);
            EditarInformacoesPessoaisCommand = new Command<AlunoDto>(p => p != null, EditarInformacoesPessoais);
            InscreverAlunoCommand = new Command<long>(Inscrever);
            TransferirAlunoCommand = new Command<long>(Transferir);
            DesinscreverAlunoCommand = new Command<long>(Desinscrever);

            Pesquisar();
        }

        private void Desinscrever(long alunoId)
        {
            var viewModel = new DesinscreverAlunoViewModel(alunoId, 1);
            _dialogService.ShowDialog(viewModel);

            Pesquisar();
        }

        private void Transferir(long alunoId)
        {
            var viewModel = new TransferirAlunoViewModel(alunoId);
            _dialogService.ShowDialog(viewModel);

            Pesquisar();
        }

        private void Inscrever(long alunoId)
        {
            var viewModel = new InscreverAlunoViewModel(alunoId);
            _dialogService.ShowDialog(viewModel);

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

            foreach (var aluno in Alunos)
            {
                aluno.InscreverAlunoCommand = InscreverAlunoCommand;
                aluno.TransferirAlunoCommand = TransferirAlunoCommand;
                aluno.DesinscreverAlunoCommand = DesinscreverAlunoCommand;
            }

            Notify(nameof(Alunos));
        }
    }
}
