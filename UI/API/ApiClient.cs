using API.Models;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UI.API
{
    public static class ApiClient
    {
        private static readonly HttpClient _client = new HttpClient();
        private static string _endpointUrl;

        public static void Init(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
        }

        public static async Task<IReadOnlyList<AlunoDto>> RecuperarLista(string cursoNome, string numeroDeCursos)
        {
            var result = await SendRequest<List<AlunoDto>>($"?cursoNome={cursoNome}&numero={numeroDeCursos}", HttpMethod.Get).ConfigureAwait(false);
            return result.Value;
        }

        public static async Task<Result> EditarInformacoesPessoais(AlunoInformacoesPessoaisDto dto)
        {
            var result = await SendRequest<string>("/" + dto.Id, HttpMethod.Put, dto).ConfigureAwait(false);
            return result;
        }

        public static async Task<Result> Inscrever(AlunoInscricaoDto dto)
        {
            Result result = await SendRequest<string>($"/{dto.Id}/inscricoes", HttpMethod.Post, dto).ConfigureAwait(false);
            return result;
        }

        public static async Task<Result> Transferir(AlunoTransferenciaDto dto)
        {
            Result result = await SendRequest<string>($"/{dto.Id}/inscricoes/{dto.NumeroInscricao}", HttpMethod.Put, dto).ConfigureAwait(false);
            return result;
        }

        public static async Task<Result> Registrar(AlunoNovoDto dto)
        {
            Result result = await SendRequest<string>("/", HttpMethod.Post, dto).ConfigureAwait(false);
            return result;
        }

        public static async Task<Result> Desinscrever(AlunoDesinscricaoDto dto)
        {
            Result result = await SendRequest<string>($"/{dto.Id}/inscricoes/{dto.NumeroInscricao}/excluir", HttpMethod.Post, dto).ConfigureAwait(false);
            return result;
        }

        public static async Task<Result> Desregistrar(long id)
        {
            Result result = await SendRequest<string>("/" + id, HttpMethod.Delete).ConfigureAwait(false);
            return result;
        }

        private static async Task<Result<T>> SendRequest<T>(string url, HttpMethod method, object content = null)
             where T : class
        {
            var request = new HttpRequestMessage(method, $"{_endpointUrl}/{url}");

            if (content != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }

            var message = await _client.SendAsync(request).ConfigureAwait(false);
            string response = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (message.StatusCode == HttpStatusCode.InternalServerError)
                throw new Exception(response);

            var envelope = JsonConvert.DeserializeObject<Envelope<T>>(response);

            if (!message.IsSuccessStatusCode)
                return Result.Fail<T>(envelope.ErrorMessage);

            T result = envelope.Result;

            if (result == null && typeof(T) == typeof(string))
            {
                result = string.Empty as T;
            }

            return Result.Ok(result);
        }
    }
}
