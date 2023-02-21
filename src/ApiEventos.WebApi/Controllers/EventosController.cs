using ApiEventos.Domain.Interfaces;
using ApiEventos.Domain.Models;
using ApiEventos.Infra.Repositories;
using ApiEventos.WebApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiEventos.WebApi.Controllers
{
    public class EventosController : EventosControllerBase
    {
        private readonly EventoService _eventoService;
        private readonly EventoRepository _eventoRepository;
        private readonly DatabaseFileService _databaseFileService;
        private readonly IRepository<TipoEvento> _tiposEventoRepository;
        private readonly IUnitOfWork _unitOfWork;
        public EventosController(EventoRepository eventoRepository, 
            EventoService eventoService, 
            DatabaseFileService databaseFileService,
            IRepository<TipoEvento> tiposEventoRepository, 
            IUnitOfWork unitOfWork) 
        {
            _eventoService = eventoService;
            _databaseFileService = databaseFileService;
            _eventoRepository = eventoRepository;
            _tiposEventoRepository = tiposEventoRepository;
            _unitOfWork = unitOfWork;
        }

        static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventoDTO, Evento>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = _eventoRepository.GetById(id);
            if(evento == null)
            {
                return NotFound(new { message = $"Evento com o id {id} não foi encontrado." });
            }
            return Ok(evento);
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos(int limit)
        {
            if(limit > 0)
            {
                var eventosLimit = _eventoRepository.GetLimit(limit);
                return Ok(eventosLimit);
            }
            var eventos = _eventoRepository.GetAll();
            return Ok(eventos);
        }

        [HttpGet("tipos")]
        public async Task<ActionResult<IEnumerable<TipoEvento>>> GetTipos()
        {
            var tipos = _tiposEventoRepository.GetAll();
            return Ok(tipos);
        }

        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<IEnumerable<Evento>>>> GetReports()
        {
            List<IEnumerable<Evento>> eventosReports = new List<IEnumerable<Evento>>();
            DateTime lastWeekDate = DateTime.Today.AddDays(-7);
            var lastWeek = _eventoRepository.GetDate(lastWeekDate, DateTime.Today);
            eventosReports.Add(lastWeek);

            DateTime lastMonthDate = DateTime.Today.AddDays(-30);
            var lastMonth = _eventoRepository.GetDate(lastMonthDate, DateTime.Today);
            eventosReports.Add(lastMonth);

            DateTime lastTrimesterDate = DateTime.Today.AddDays(-90);
            var lastTrimester = _eventoRepository.GetDate(lastTrimesterDate, DateTime.Today);
            eventosReports.Add(lastTrimester);
            return Ok(eventosReports.AsEnumerable());
        }

        [HttpPost()]
        public async Task<ActionResult> NewEvento([FromBody]EventoDTO newEvento)
        {
            var mapper = InitializeAutomapper();
            var entity = mapper.Map<Evento>(newEvento);
            await _eventoService.Save(newEvento.TipoEvento, newEvento.Descricao, newEvento.Empresa, newEvento.Instrutor, newEvento.DataRealizado, newEvento.CargaHoraria, newEvento.ParticipantesEsperados, newEvento.ParticipacoesConfirmadas, newEvento.Inativo);
            _unitOfWork.Commit();
            if (newEvento.ArquivosBase64 != null)
            {
                var createdEvento = _eventoRepository.GetLastEntity();
                await _databaseFileService.Save(createdEvento.Id, newEvento.Empresa, newEvento.ArquivosBase64);
                _unitOfWork.Commit();
            }
            return Ok();
        }

        [HttpPut()]
        public async Task<ActionResult> EditEvento([FromBody] EventoDTO editEvento)
        {
            var mapper = InitializeAutomapper();
            var entity = mapper.Map<Evento>(editEvento);
            var evento = _eventoRepository.GetById(editEvento.Id);
            if(evento == null)
                return NotFound(new { message = $"Evento com o id {entity.Id} não foi encontrado." });
            await _eventoService.Update(editEvento.Id, editEvento.TipoEvento, editEvento.Descricao, editEvento.Empresa, editEvento.Instrutor, editEvento.DataRealizado, editEvento.CargaHoraria, editEvento.ParticipantesEsperados, editEvento.ParticipacoesConfirmadas, editEvento.Inativo);
            _unitOfWork.Commit();
            if (editEvento.ArquivosBase64 != null)
            {
                if (evento.Empresa == null)
                {
                    return BadRequest(new { message = $"Empresa informada é inválida" });
                }
                await _databaseFileService.Save(evento.Id, evento.Empresa, editEvento.ArquivosBase64);
                _unitOfWork.Commit();
            }
                
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvento(int id)
        {
            var evento = _eventoRepository.GetById(id);
            if (evento == null)
                return NotFound(new { message = $"Evento com o id {id} não foi encontrado." });
            await _eventoService.Delete(id);
            _unitOfWork.Commit();
            return Ok();
        }
    }
}
