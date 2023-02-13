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

        public async Task Save(int? tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime? dataRealizado, float? cargaHoraria, int? participantesEsperados, int? participacoesConfirmadas, bool? inativo)
        {
            Evento evento = new(tipoEvento, descricao, empresa, instrutor, dataRealizado, cargaHoraria, participantesEsperados, participacoesConfirmadas, inativo);
            await _eventoRepository.Save(evento);
        }

        public async Task Update(int codigoEvento, int? tipoEvento, string? descricao, string? empresa, string? instrutor, DateTime? dataRealizado, float? cargaHoraria, int? participantesEsperados, int? participacoesConfirmadas, bool? inativo)
        {
            var evento = _eventoRepository.GetById(codigoEvento);
            if (evento != null)
            {
                evento.UpdateEvento(tipoEvento, descricao, empresa, instrutor, dataRealizado, cargaHoraria, participantesEsperados, participacoesConfirmadas, inativo);
                await _eventoRepository.Update(evento);
            }
        }

        public async Task Delete(int codigoEvento)
        {
            var evento = _eventoRepository.GetById(codigoEvento);
            if(evento != null)
            {
                bool inativo = true;
                evento.UpdateEvento(null, null, null, null, null, null, null, null, inativo);
                await _eventoRepository.Update(evento);
            }
        }
    }
}
