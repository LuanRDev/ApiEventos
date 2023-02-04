using ApiEventos.Domain.Interfaces;
using ApiEventos.Domain.Models;
using ApiEventos.WebApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiEventos.WebApi.Controllers
{
    public class EventosController : EventosControllerBase
    {
        private readonly EventoService _eventoService;
        private readonly IRepository<Evento> _eventoRepository;
        private readonly IUnitOfWork _unitOfWork;
        public EventosController(EventoService eventoService, IRepository<Evento> eventoRepository, IUnitOfWork unitOfWork) 
        {
            _eventoService = eventoService;
            _eventoRepository = eventoRepository;
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
        public ActionResult<Evento> GetEvento(int id)
        {
            var evento = _eventoRepository.GetById(id);
            if(evento == null)
            {
                return NotFound(new { message = $"Evento com o id {id} não foi encontrado." });
            }
            return Ok(evento);
        }

        [HttpGet()]
        public ActionResult<IEnumerable<Evento>> GetEventos()
        {
            var eventos = _eventoRepository.GetAll();
            return Ok(eventos);
        }

        [HttpPost()]
        public ActionResult NewEvento([FromBody]EventoDTO newEvento)
        {
            var mapper = InitializeAutomapper();
            var entity = mapper.Map<Evento>(newEvento);
            _eventoService.Save(newEvento.TipoEvento, newEvento.Descricao, newEvento.Empresa, newEvento.Instrutor, newEvento.DataRealizado, newEvento.CargaHoraria, newEvento.ParticipantesEsperados, newEvento.UrlConteudo);
            _unitOfWork.Commit();
            return Ok();
        }

        [HttpPut()]
        public ActionResult EditEvento([FromBody] EventoDTO editEvento)
        {
            var mapper = InitializeAutomapper();
            var entity = mapper.Map<Evento>(editEvento);
            var evento = _eventoRepository.GetById(editEvento.Id);
            if(evento == null)
                return NotFound(new { message = $"Evento com o id {entity.Id} não foi encontrado." });
            _eventoService.Update(editEvento.TipoEvento, editEvento.Descricao, editEvento.Empresa, editEvento.Instrutor, editEvento.DataRealizado, editEvento.CargaHoraria, editEvento.ParticipantesEsperados, editEvento.UrlConteudo);
            _unitOfWork.Commit();
            return Ok();
        }
    }
}
