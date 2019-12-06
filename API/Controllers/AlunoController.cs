using API.Models;
using Logica.Alunos;
using Logica.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [Route("api/alunos")]
    public sealed class AlunoController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly AlunoRepositorio _alunoRepositorio;
        private readonly CursoRepositorio _cursoRepositorio;

        public AlunoController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _alunoRepositorio = new AlunoRepositorio(unitOfWork);
            _cursoRepositorio = new CursoRepositorio(unitOfWork);
        }

        [HttpGet]
        public IActionResult GetLista(string cursoNome, int? numero)
        {
            IReadOnlyList<Aluno> alunos = _alunoRepositorio.RecuperarLista(cursoNome, numero);
            List<AlunoDto> dtos = alunos.Select(x => ConverterParaDto(x)).ToList();
            return Ok(dtos);
        }

        private AlunoDto ConverterParaDto(Aluno aluno)
        {
            return new AlunoDto
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Curso1 = aluno.PrimeiraInscricao?.Curso?.Nome,
                Curso1Grade = aluno.PrimeiraInscricao?.Grade.ToString(),
                Curso1Creditos = aluno.PrimeiraInscricao?.Curso?.Creditos,
                Curso2 = aluno.SegundaInscricao?.Curso?.Nome,
                Curso2Grade = aluno.SegundaInscricao?.Grade.ToString(),
                Curso2Creditos = aluno.SegundaInscricao?.Curso?.Creditos,
            };
        }

        [HttpPost]
        public IActionResult Registrar([FromBody] AlunoNovoDto dto)
        {
            var aluno = new Aluno(dto.Nome, dto.Email);

            if (dto.Curso1 != null && dto.Curso1Grade != null)
            {
                var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso1);
                aluno.Inscrever(curso, Enum.Parse<Grade>(dto.Curso1Grade));
            }

            if (dto.Curso2 != null && dto.Curso2Grade != null)
            {
                var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso2);
                aluno.Inscrever(curso, Enum.Parse<Grade>(dto.Curso2Grade));
            }

            _alunoRepositorio.Salvar(aluno);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Desregistrar(long id)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            _alunoRepositorio.Excluir(aluno);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPost("{id}/inscricoes")]
        public IActionResult Inscrever(long id, [FromBody]AlunoInscricaoDto dto)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso);

            if (curso == null)
                return Error($"O curso é incorreto: {dto.Curso}.");

            var gradeSucesso = Enum.TryParse(dto.Grade, out Grade grade);

            if (!gradeSucesso)
                return Error($"A grade é incorreta: {dto.Grade}.");

            aluno.Inscrever(curso, grade);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPut("{id}/inscricoes/{numeroInscricao}")]
        public IActionResult Transferir(long id, int numeroInscricao, [FromBody]AlunoTransferenciaDto dto)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso);

            if (curso == null)
                return Error($"O curso é incorreto: {dto.Curso}.");

            var gradeSucesso = Enum.TryParse(dto.Grade, out Grade grade);

            if (!gradeSucesso)
                return Error($"A grade é incorreta: {dto.Grade}.");

            var inscricao = aluno.RecuperarInscricao(numeroInscricao);

            if (inscricao == null)
                return Error($"Nenhuma inscrição encontrada com o número: {numeroInscricao}");

            inscricao.Atualizar(curso, grade);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPost("{id}/inscricoes/{numeroInscricao}/excluir")]
        public IActionResult Desinscricao(long id, int numeroInscricao, [FromBody]AlunoDesinscricaoDto dto)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            if (string.IsNullOrEmpty(dto.Comentario))
                return Error($"É necessario informar um comentário para desinscrever de um curso");

            var inscricao = aluno.RecuperarInscricao(numeroInscricao);

            if (inscricao == null)
                return Error($"Nenhuma inscrição encontrada com o número: {numeroInscricao}");

            aluno.RemoverInscricao(inscricao, dto.Comentario);

            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditarInformacoesPessoais(long id, [FromBody]AlunoInformacoesPessoaisDto dto)
        {
            var comando =new EditarInformacoesPessoaisCommand(id, dto.Nome, dto.Email);
            var handler = new EditarInformacoesPessoaisHandler(_unitOfWork);

            var result = handler.Handle(comando);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
