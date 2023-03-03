namespace ApiEventos.Domain.Models
{
    public class KeycloakTokenObject
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }
}
