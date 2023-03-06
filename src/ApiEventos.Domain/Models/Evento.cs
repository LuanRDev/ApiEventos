using Newtonsoft.Json;

namespace ApiEventos.Domain.Models
{
    public class Evento : BaseEntity
    {
        [JsonProperty("eventoHash")]
        public string? EventoHash { get; set; }
        [JsonProperty("tipoEvento")]
        public int? TipoEvento { get; set; }
        [JsonProperty("descricao")]
        public string? Descricao { get; private set; }
        [JsonProperty("empresa")]
        public string? Empresa { get; private set; }
        [JsonProperty("instrutor")]
        public string? Instrutor { get; private set; }
        [JsonProperty("dataRealizado")]
        public DateTime? DataRealizado { get; private set; }
        [JsonProperty("cargaHoraria")]
        public float? CargaHoraria { get; private set; }
        [JsonProperty("participantesEsperados")]
        public int? ParticipantesEsperados { get; private set; }
        [JsonProperty("participacoesConfirmadas")]
        public int? ParticipacoesConfirmadas { get; private set; }
        [JsonProperty("inativo")]
        public bool? Inativo { get; private set; }
        [JsonProperty("conteudoEventos")]
        public virtual ICollection<DatabaseFile> ConteudoEventos { get; set; } = new List<DatabaseFile>();

        public Evento() { }

        public Evento(int? tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime? dataRealizado, float? cargaHoraria, int? participantesEsperados, int? participacoesConfirmadas, bool? inativo)
        {
            ValidaEvento(tipoEvento, descricao, empresa, instrutor, dataRealizado, cargaHoraria, participantesEsperados, participacoesConfirmadas, inativo);
            EventoHash = Guid.NewGuid().ToString().Remove(6);    
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
        
        public void UpdateEvento(int? tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime? dataRealizado, float? cargaHoraria, int? participantesEsperados, int? participacoesConfirmadas, bool? inativo)
        {
            if (tipoEvento != null)
            {
                TipoEvento = tipoEvento;
            }
            if (cargaHoraria != null)
            {
                CargaHoraria = cargaHoraria;
            }
            if(participantesEsperados != null)
            {
                ParticipantesEsperados = participantesEsperados;
            }
            if(participacoesConfirmadas != null)
            {
                ParticipacoesConfirmadas = participacoesConfirmadas;
            }
            if (dataRealizado != null)
            {
                DataRealizado = dataRealizado;
            }
            if(inativo != null)
            {
                Inativo = inativo;
            }
            if (!string.IsNullOrEmpty(descricao))
            {
                Descricao = descricao;
            }
            if (!string.IsNullOrEmpty(empresa))
            {
                Empresa = empresa;
            }
            if (!string.IsNullOrEmpty(instrutor))
            {
                Instrutor = instrutor;
            }
        }

        private void ValidaEvento(int? tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime? dataRealizado, float? cargaHoraria, int? participantesEsperados, int? participacoesConfirmadas, bool? inativo)
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
