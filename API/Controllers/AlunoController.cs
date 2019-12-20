using API.Models;
using Logica.Alunos;
using Logica.Utils;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/alunos")]
    public sealed class AlunoController : BaseController
    {
        private readonly Messages _messages;

        public AlunoController(Messages messages)
        {
            _messages = messages;
        }

        [HttpGet]
        public IActionResult GetLista(string cursoNome, int? numero)
        {
            var query = new RecuperarAlunosQuery(cursoNome, numero);
            var result = _messages.Dispatch(query);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Registrar([FromBody] AlunoNovoDto dto)
        {
            var comando = new RegistrarAlunoCommand(dto.Nome, dto.Email, dto.Curso1, dto.Curso1Grade, dto.Curso2, dto.Curso2Grade);

            var result = _messages.Dispatch(comando);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public IActionResult Desregistrar(long id)
        {
            var comando = new DesregistrarAlunoCommand(id);

            var result = _messages.Dispatch(comando);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("{id}/inscricoes")]
        public IActionResult Inscrever(long id, [FromBody]AlunoInscricaoDto dto)
        {
            var comando = new InscreverAlunoCommand(id, dto.Curso, dto.Grade);

            var result = _messages.Dispatch(comando);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{id}/inscricoes/{numeroInscricao}")]
        public IActionResult Transferir(long id, int numeroInscricao, [FromBody]AlunoTransferenciaDto dto)
        {
            var comando = new TransferirAlunoCommand(id, numeroInscricao, dto.Curso, dto.Grade);

            var result = _messages.Dispatch(comando);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("{id}/inscricoes/{numeroInscricao}/excluir")]
        public IActionResult Desinscrever(long id, int numeroInscricao, [FromBody]AlunoDesinscricaoDto dto)
        {
            var comando = new DesinscreverAlunoCommand(id, numeroInscricao, dto.Comentario);

            var result = _messages.Dispatch(comando);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public IActionResult EditarInformacoesPessoais(long id, [FromBody]AlunoInformacoesPessoaisDto dto)
        {
            var comando = new EditarInformacoesPessoaisCommand(id, dto.Nome, dto.Email);

            var result = _messages.Dispatch(comando);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
