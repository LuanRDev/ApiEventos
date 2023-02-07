using ApiEventos.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ApiEventos.Domain.Models
{
    public class EventoService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Evento> _eventoRepository;
        private readonly IRepository<StorageFile> _storageFileRepository;
        public EventoService(IRepository<Evento> eventoRepository, IRepository<StorageFile> fileRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _eventoRepository = eventoRepository;
            _storageFileRepository = fileRepository;
        }

        public void Save(int tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime dataRealizado, float cargaHoraria, int participantesEsperados, int participacoesConfirmadas, bool inativo)
        {
            Evento evento = new(tipoEvento, descricao, empresa, instrutor, dataRealizado, cargaHoraria, participantesEsperados, participacoesConfirmadas, inativo);
            _eventoRepository.Save(evento);
        }

        public void Update(int codigoEvento, int tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime dataRealizado, float cargaHoraria, int participantesEsperados, int participacoesConfirmadas, bool inativo)
        {
            var evento = _eventoRepository.GetById(codigoEvento);
            if (evento != null)
            {
                evento = new Evento(tipoEvento, descricao, empresa, instrutor, dataRealizado, cargaHoraria, participantesEsperados, participacoesConfirmadas, inativo);
                _eventoRepository.Update(evento);
            }
        }

        public async void SaveFiles(IEnumerable<string> Base64Files, string empresa, int codigoEvento)
        {
            foreach(string base64File in Base64Files)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_configuration["ApiStorageManager:UploadEndpoint"]}");
                var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                request.Headers.Accept.Add(mediaType);
                StorageFile file = ProcessFile(base64File, empresa, codigoEvento);
                var payload = JsonConvert.SerializeObject(file);
                _storageFileRepository.Save(file);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiStorageManager:BaseUrl"]!);
                    request.Content = new StringContent(payload, Encoding.UTF8);
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    if(response.IsSuccessStatusCode)
                    {
                        var details = await response.Content.ReadAsStringAsync();
                        _storageFileRepository.Save(file);
                    }
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
        }

        public static StorageFile ProcessFile(string base64File, string empresa, int codigoEvento)
        {
            try
            {
                string name = base64File.Substring(0, base64File.IndexOf('|'));
                string metadata = base64File.Substring(base64File.IndexOf('|') + 1);
                string type = base64File.Substring(base64File.IndexOf(':') + 1, base64File.IndexOf(';') - base64File.IndexOf(':') - 1);
                string extensions = base64File.Substring(base64File.IndexOf('.') + 1, base64File.IndexOf('|') - base64File.IndexOf('.') - 1);
                Byte[] bytes = Convert.FromBase64String(base64File.Substring(base64File.IndexOf(',') + 1));
                StorageFile processedFile = new(codigoEvento, empresa, name, metadata, type, extensions, bytes);
                return processedFile;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
