namespace ApiEventos.Domain.Models
{
    public class TipoEvento : BaseEntity
    {
        public string TipoDescricao { get; set; }
        public TipoEvento(string tipoDescricao)
        {
            ValidaTipo(tipoDescricao);
            TipoDescricao = tipoDescricao;
        }
        public TipoEvento() { }
        private void ValidaTipo(string tipoDescricao)
        {
            if(string.IsNullOrEmpty(tipoDescricao))
            {
                throw new InvalidOperationException("O campo tipo descrição não pode ser nulo.");
            }
        }
    }
}
