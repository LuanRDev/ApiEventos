using ApiEventos.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ApiEventos.Domain.Models
{
    public class DatabaseFileService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<DatabaseFile> _databaseFileRepository;
        public DatabaseFileService(IRepository<DatabaseFile> databaseFileRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseFileRepository = databaseFileRepository;
        }
        
        public async Task Save(int codigoEvento, string empresa, IEnumerable<string> Base64Files)
        {
            foreach (string base64File in Base64Files)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_configuration["ApiStorageManager:UploadEndpoint"]}");
                var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                request.Headers.Accept.Add(mediaType);
                StorageFile storageFile = ProcessFile(base64File, empresa, codigoEvento);
                var payload = JsonConvert.SerializeObject(new
                {
                    Id = storageFile.GuidStorageId,
                    storageFile.Name,
                    Empresa = empresa,
                    CodigoEvento = codigoEvento,
                    storageFile.Url,
                    storageFile.Metadata,
                    storageFile.Type,
                    storageFile.Extension,
                    storageFile.Bytes
                });
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiStorageManager:BaseUrl"]!);
                    request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    if (response.IsSuccessStatusCode)
                    {
                        var details = await response.Content.ReadAsStringAsync();
                        DatabaseFile databaseFile = new DatabaseFile(storageFile.CodigoEvento, storageFile.Name, storageFile.Url);
                        await _databaseFileRepository.Save(databaseFile);
                    }
                    else
                    {
                        throw new Exception(await response.Content.ReadAsStringAsync());
                    }
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
