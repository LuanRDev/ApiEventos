using Newtonsoft.Json;

namespace ApiEventos.Domain.Models
{
    public abstract class BaseEntity
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
    }
}
