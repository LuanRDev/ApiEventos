namespace ApiEventos.Domain.Models
{
    public class Evento : BaseEntity
    {
        public int? TipoEvento { get; set; }
        public string? Descricao { get; private set; }
        public string? Empresa { get; private set; }
        public string? Instrutor { get; private set; }
        public DateTime DataRealizado { get; private set; }
        public float CargaHoraria { get; private set; }
        public int ParticipantesEsperados { get; private set; }
        public string? UrlConteudo { get; private set; }

        public Evento() { }

        public Evento(int? tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime dataRealizado, float cargaHoraria, int participantesEsperados, string? urlConteudo)
        {
            ValidaEvento(tipoEvento, descricao, empresa, instrutor, dataRealizado, cargaHoraria, participantesEsperados, urlConteudo);
            TipoEvento = tipoEvento;
            Descricao = descricao;
            Empresa = empresa;
            Instrutor = instrutor;
            DataRealizado = dataRealizado;
            CargaHoraria = cargaHoraria;
            ParticipantesEsperados = participantesEsperados;
            UrlConteudo = urlConteudo;
        }

        private void ValidaEvento(int? tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime dataRealizado, float cargaHoraria, int? participantesEsperados, string? urlConteudo)
        {
            if (string.IsNullOrEmpty(empresa))
            {
                throw new InvalidOperationException("O campo Empresa não pode ser vazio ou nulo");
            }
            if (string.IsNullOrEmpty(descricao))
            {
                throw new InvalidOperationException("O campo Descricao não pode ser vazio ou nulo");
            }
            if (string.IsNullOrEmpty(instrutor))
            {
                throw new InvalidOperationException("O campo Instrutor não pode ser vazio ou nulo");
            }
            if (string.IsNullOrEmpty(urlConteudo))
            {
                throw new InvalidOperationException("O campo UrlConteudo não pode ser vazio ou nulo");
            }
            if (tipoEvento == null) 
            {
                throw new InvalidOperationException("O campo TipoEvento não pode ser vazio ou nulo");
            }
            if (participantesEsperados == null)
            {
                throw new InvalidOperationException("O campo TipoEvento não pode ser vazio ou nulo");
            }
            if (dataRealizado == null)
            {
                throw new InvalidOperationException("O campo DataRealizado não pode ser vazio ou nulo");
            }
            if(cargaHoraria == null)
            {
                throw new InvalidOperationException("O campo CargaHoraria não pode ser vazio ou nulo");
            }
        }
    }
}
