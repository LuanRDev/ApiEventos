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
        private readonly EventoRepository _repo;
        private readonly DatabaseFileService _databaseFileService;
        private readonly IRepository<Evento> _eventoRepository;
        private readonly IRepository<DatabaseFile> _databaseFileRepository;
        private readonly IUnitOfWork _unitOfWork;
        public EventosController(EventoRepository repo, EventoService eventoService, DatabaseFileService databaseFileService, IRepository<Evento> eventoRepository, IRepository<DatabaseFile> databaseFileRepository, IUnitOfWork unitOfWork) 
        {
            _repo = repo;
            _eventoService = eventoService;
            _databaseFileService = databaseFileService;
            _eventoRepository = eventoRepository;
            _databaseFileRepository = databaseFileRepository;
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
            var evento = _repo.GetById(id);
            if(evento == null)
            {
                return NotFound(new { message = $"Evento com o id {id} não foi encontrado." });
            }
            return Ok(evento);
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            var eventos = _eventoRepository.GetAll();
            return Ok(eventos);
        }

        [HttpPost()]
        public async Task<ActionResult> NewEvento([FromBody]EventoDTO newEvento)
        {
            var mapper = InitializeAutomapper();
            var entity = mapper.Map<Evento>(newEvento);
            await _eventoService.Save(newEvento.TipoEvento, newEvento.Descricao, newEvento.Empresa, newEvento.Instrutor, newEvento.DataRealizado, newEvento.CargaHoraria, newEvento.ParticipantesEsperados, newEvento.ParticipacoesConfirmadas, newEvento.Inativo);
            _unitOfWork.Commit();
            var createdEvento = _eventoRepository.GetLastEntity();
            await _databaseFileService.Save(createdEvento.Id, newEvento.Empresa, newEvento.ArquivosBase64);
            _unitOfWork.Commit();
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
            return Ok();
        }
    }
}
