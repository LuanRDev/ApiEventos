using ApiEventos.Domain.Interfaces;

namespace ApiEventos.Domain.Models
{
    public class EventoService
    {
        private readonly IRepository<Evento> _eventoRepository;
        public EventoService(IRepository<Evento> eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public void Save(int tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime dataRealizado, float cargaHoraria, int participantesEsperados, string? urlConteudo)
        {
            Evento evento = new(tipoEvento, descricao, empresa, instrutor, dataRealizado, cargaHoraria, participantesEsperados, urlConteudo);
            _eventoRepository.Save(evento);
        }

        public void Update(int codigoEvento, int tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime dataRealizado, float cargaHoraria, int participantesEsperados, string? urlConteudo)
        {
            var evento = _eventoRepository.GetById(codigoEvento);
            if (evento != null)
            {
                evento = new Evento(tipoEvento, descricao, empresa, instrutor, dataRealizado, cargaHoraria, participantesEsperados, urlConteudo);
                _eventoRepository.Update(evento);
            }
        }
    }
}
