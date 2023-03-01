using ApiEventos.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
                empresa = empresa,
                codigoEvento = codigoEvento,
                url = storageFile.Url,
                metadata = storageFile.Metadata,
                type = storageFile.Type,
                extension = storageFile.Extension,
                bytes = storageFile.Bytes
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiStorageManager:BaseUrl"]!);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsJsonAsync("/file", payload).Result;
                if (response.IsSuccessStatusCode)
                {
                    var details = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Completed successfuly!" + details);
                    DatabaseFile databaseFile = new DatabaseFile(storageFile.CodigoEvento, storageFile.Name, storageFile.Url);
                }
                else
                {
                    Console.WriteLine("ERROR WHILE TRYING TO SEND REQUEST");
                    Console.WriteLine("RESPONSE GIVEN FROM HOST: " + response.Content.ReadAsStringAsync().Result);
                    //throw new Exception(await response.Content.ReadAsStringAsync());
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
