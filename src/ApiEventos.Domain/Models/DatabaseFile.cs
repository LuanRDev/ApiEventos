namespace ApiEventos.Domain.Models
{
    public class DatabaseFile : BaseEntity
    {
        public int CodigoEvento { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
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
