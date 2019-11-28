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
        public IActionResult Criar([FromBody] AlunoDto dto)
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
        public IActionResult Excluir(long id)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            _alunoRepositorio.Excluir(aluno);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(long id, [FromBody] AlunoDto dto)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            aluno.Nome = dto.Nome;
            aluno.Email = dto.Email;

            var primeiraInscricao = aluno.PrimeiraInscricao;
            var segundaInscricao = aluno.SegundaInscricao;

            if (TemInscricaoAlterada(dto.Curso1, dto.Curso1Grade, primeiraInscricao))
            {
                if (string.IsNullOrWhiteSpace(dto.Curso1)) // Desinscrever o aluno.
                {
                    if (string.IsNullOrWhiteSpace(dto.Curso1ComentarioDesincricao))
                        return Error("Comentário na desincrição é obrigatório.");

                    var inscricao = primeiraInscricao;
                    aluno.RemoverInscricao(inscricao);
                    aluno.AdicionarComentarioDeDesincricao(inscricao, dto.Curso1ComentarioDesincricao);
                }

                if (string.IsNullOrWhiteSpace(dto.Curso1Grade))
                    return Error("Grade é obrigatória.");

                var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso1);

                if (primeiraInscricao == null)
                {
                    aluno.Inscrever(curso, Enum.Parse<Grade>(dto.Curso1Grade)); // Inscreve o aluno.
                }
                else
                {
                    primeiraInscricao.Atualizar(curso, Enum.Parse<Grade>(dto.Curso1Grade)); // Transfere o aluno.
                }
            }

            if (TemInscricaoAlterada(dto.Curso2, dto.Curso2Grade, segundaInscricao))
            {
                if (string.IsNullOrWhiteSpace(dto.Curso2)) // Desincrever aluno.
                {
                    if (string.IsNullOrWhiteSpace(dto.Curso2ComentarioDesincricao))
                        return Error("Comentário na desincrição é obrigatório.");

                    var inscricao = segundaInscricao;
                    aluno.RemoverInscricao(inscricao);
                    aluno.AdicionarComentarioDeDesincricao(inscricao, dto.Curso2ComentarioDesincricao);
                }

                if (string.IsNullOrWhiteSpace(dto.Curso2Grade))
                    return Error("Grade é obrigatória.");

                var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso2);

                if (segundaInscricao == null)
                {          
                    aluno.Inscrever(curso, Enum.Parse<Grade>(dto.Curso2Grade));  // Inscreve o aluno.
                }
                else
                {          
                    segundaInscricao.Atualizar(curso, Enum.Parse<Grade>(dto.Curso2Grade));  // Transfere o aluno.
                }
            }

            _unitOfWork.Commit();

            return Ok();
        }

        private bool TemInscricaoAlterada(string novoCursoNome, string novaGrade, Inscricao inscricao)
        {
            if (string.IsNullOrWhiteSpace(novoCursoNome) && inscricao == null)
                return false;

            if (string.IsNullOrWhiteSpace(novoCursoNome) || inscricao == null)
                return true;

            return novoCursoNome != inscricao.Curso.Nome || novaGrade != inscricao.Grade.ToString();
        }
    }
}
