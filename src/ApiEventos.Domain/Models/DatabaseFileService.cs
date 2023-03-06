using ApiEventos.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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
                StorageFile storageFile = ProcessFile(base64File, empresa, codigoEvento);
                SendFile(storageFile, empresa, codigoEvento);
                DatabaseFile databaseFile = new DatabaseFile(storageFile.CodigoEvento, storageFile.Name, storageFile.Url);
                await _databaseFileRepository.Save(databaseFile);
            }
        }

        private void SendFile(StorageFile storageFile, string empresa, int codigoEvento)
        {
            var payload = new
            {
                id = storageFile.GuidStorageId,
                name = storageFile.Name,
                empresa,
                codigoEvento,
                url = storageFile.Url,
                metadata = storageFile.Metadata,
                type = storageFile.Type,
                extension = storageFile.Extension,
                bytes = storageFile.Bytes
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiStorageManager:BaseUrl"]!);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
                var response = client.PostAsJsonAsync("/file", payload).Result;
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    var details = response.Content.ReadAsStringAsync().Result;
                    DatabaseFile databaseFile = new DatabaseFile(storageFile.CodigoEvento, storageFile.Name, storageFile.Url);
                }
                else
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
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

        public string GetToken()
        {
            using (var client = new HttpClient())
            {
                var body = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", _configuration["Keycloak:ApiStorageManagerClientId"]!),
                    new KeyValuePair<string, string>("client_secret", _configuration["Keycloak:ApiStorageManagerClientSecret"]!)
                });

                client.BaseAddress = new Uri(_configuration["Keycloak:UrlBase"]!);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = client.PostAsync(_configuration["Keycloak:SessionStartUrl"], body).Result;
                var resultObject = result.Content.ReadAsStringAsync().Result;
                if (result.IsSuccessStatusCode) 
                {
                    return JsonConvert.DeserializeObject<KeycloakTokenObject>(resultObject)!.access_token;
                }
                throw new Exception(resultObject);
            }
        }
    }
}
