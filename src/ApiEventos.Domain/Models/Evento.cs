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
        public int ParticipacoesConfirmadas { get; private set; }
        public bool Inativo { get; private set; }

        public Evento() { }

        public Evento(int? tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime dataRealizado, float cargaHoraria, int participantesEsperados, int participacoesConfirmadas, bool inativo)
        {
            ValidaEvento(tipoEvento, descricao, empresa, instrutor, dataRealizado, cargaHoraria, participantesEsperados, participacoesConfirmadas, inativo);
            TipoEvento = tipoEvento;
            Descricao = descricao;
            Empresa = empresa;
            Instrutor = instrutor;
            DataRealizado = dataRealizado;
            CargaHoraria = cargaHoraria;
            ParticipantesEsperados = participantesEsperados;
            ParticipacoesConfirmadas = participacoesConfirmadas;
            Inativo = inativo;
        }

        private void ValidaEvento(int? tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime dataRealizado, float cargaHoraria, int? participantesEsperados, int? participacoesConfirmadas, bool? inativo)
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
            if (tipoEvento == null) 
            {
                throw new InvalidOperationException("O campo TipoEvento não pode ser vazio ou nulo");
            }
            if (participantesEsperados == null)
            {
                throw new InvalidOperationException("O campo ParticipantesEsperados não pode ser vazio ou nulo");
            }
            if (participacoesConfirmadas == null)
            {
                throw new InvalidOperationException("O campo ParticipacoesConfirmadas não pode ser vazio ou nulo");
            }
            if (dataRealizado == null)
            {
                throw new InvalidOperationException("O campo DataRealizado não pode ser vazio ou nulo");
            }
            if(cargaHoraria == null)
            {
                throw new InvalidOperationException("O campo CargaHoraria não pode ser vazio ou nulo");
            }
            if(inativo == null)
            {
                throw new InvalidOperationException("O campo Inativo não pode ser vazio ou nulo");
            }
        }
    }
}
