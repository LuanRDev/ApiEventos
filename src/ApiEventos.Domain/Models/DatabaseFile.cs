using Newtonsoft.Json;

namespace ApiEventos.Domain.Models
{
    public class DatabaseFile : BaseEntity
    {
        [JsonIgnore]
        public int CodigoEvento { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonIgnore]
        public virtual Evento? CodigoEventoNavigation { get; set; }

        public DatabaseFile() { }

        public DatabaseFile(int codigoEvento, string name, string url)
        {
            ValidaDatabaseFile(codigoEvento, name, url);
            CodigoEvento = codigoEvento;
            Name = name;
            Url = url;
        }

        private static void ValidaDatabaseFile(int codigoEvento, string name, string url)
        {
            if (codigoEvento <= 0)
            {
                throw new InvalidOperationException("O campo codigo evento não pode ser menor ou igual a zero.");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("O campo name não pode ser vazio ou nulo.");
            }
            if (string.IsNullOrEmpty(url))
            {
                throw new InvalidOperationException("O campo Url não pode ser vazio ou nulo.");
            }
        }
    }
}
