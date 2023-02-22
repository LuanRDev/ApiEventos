using ApiEventos.Domain.Models;
using ApiEventos.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiEventos.Infra.Repositories
{
    public class EventoRepository : Repository<Evento>
    {
        private readonly IConfiguration _configuration;
        public EventoRepository(AppDbContext appDbContext, IConfiguration configuration) : base(appDbContext) 
        {
            _configuration = configuration;
        }

        public override Evento GetById(int id)
        {
            var query = _context.Set<Evento>().Where(e => e.Id == id).Where(e => e.Inativo == false).Include(e => e.ConteudoEventos);
            if (query.Any())
            {
                if(query.FirstOrDefault().ConteudoEventos.Any())
                {
                    foreach (var arquivo in query.FirstOrDefault().ConteudoEventos)
                    {
                        arquivo.Url = _configuration["ApiStorageManager:PublicObjectsBaseUrl"] + arquivo.Url;
                    }
                }
                return query.FirstOrDefault();
            }
            return null;
        }

        public override Evento GetLastEntity()
        {
            var query = _context.Set<Evento>().Where(e => e.Inativo == false).OrderByDescending(e => e.Id);
            if (query.Any())
            {
                if (query.FirstOrDefault().ConteudoEventos.Any())
                {
                    foreach (var arquivo in query.FirstOrDefault().ConteudoEventos)
                    {
                        arquivo.Url = _configuration["ApiStorageManager:PublicObjectsBaseUrl"] + arquivo.Url;
                    }
                }
                return query.FirstOrDefault();
            }
            return null;
        }

        public override IEnumerable<Evento> GetLimit(int limit)
        {
            var query = _context.Set<Evento>().Where(e => e.Inativo == false).Take(limit).OrderByDescending(e => e.Id);
            if (query.Any())
            {
                foreach(var evento in query)
                {
                    if (evento.ConteudoEventos.Any())
                    {
                        foreach (var arquivo in evento.ConteudoEventos)
                        {
                            arquivo.Url = _configuration["ApiStorageManager:PublicObjectsBaseUrl"] + arquivo.Url;
                        }
                    }
                }
                return query.ToList();
            }
            return new List<Evento>();
        }

        public override IEnumerable<Evento> GetAll()
        {
            var query = _context.Set<Evento>().Where(e => e.Inativo == false).Include(e => e.ConteudoEventos).OrderByDescending(e => e.Id);
            if (query.Any())
            {

                foreach (var evento in query)
                {
                    if (evento.ConteudoEventos.Any())
                    {
                        foreach (var arquivo in evento.ConteudoEventos)
                        {
                            arquivo.Url = _configuration["ApiStorageManager:PublicObjectsBaseUrl"] + arquivo.Url;
                        }
                    }
                }
                return query.ToList();
            }
            return new List<Evento>();
        }

        public IEnumerable<Evento> GetDate(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate.HasValue)
            {
                var query = _context.Set<Evento>().Where(e => e.Inativo == false).Where(e => e.DataRealizado < endDate);
                if (query.Any())
                {
                    foreach (var evento in query)
                    {
                        if (evento.ConteudoEventos.Any())
                        {
                            foreach (var arquivo in evento.ConteudoEventos)
                            {
                                arquivo.Url = _configuration["ApiStorageManager:PublicObjectsBaseUrl"] + arquivo.Url;
                            }
                        }
                    }
                    return query.ToList();
                }
                return new List<Evento>();
            }
            if (endDate == null && startDate.HasValue)
            {
                var query = _context.Set<Evento>().Where(e => e.Inativo == false).Where(e => e.DataRealizado > startDate);
                if (query.Any())
                {

                    foreach (var evento in query)
                    {
                        if (evento.ConteudoEventos.Any())
                        {
                            foreach (var arquivo in evento.ConteudoEventos)
                            {
                                arquivo.Url = _configuration["ApiStorageManager:PublicObjectsBaseUrl"] + arquivo.Url;
                            }
                        }
                    }
                    return query.ToList();
                }
                return new List<Evento>();
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                var query = _context.Set<Evento>().Where(e => e.Inativo == false).Where(e => e.DataRealizado > startDate && e.DataRealizado < endDate);
                if (query.Any())
                {

                    foreach (var evento in query)
                    {
                        if (evento.ConteudoEventos.Any())
                        {
                            foreach (var arquivo in evento.ConteudoEventos)
                            {
                                arquivo.Url = _configuration["ApiStorageManager:PublicObjectsBaseUrl"] + arquivo.Url;
                            }
                        }
                    }
                    return query.ToList();
                }
                return new List<Evento>();
            }
            return new List<Evento>();
        }

        public IEnumerable<int?> GetReports(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate.HasValue)
            {
                var query = _context.Set<Evento>().Where(e => e.Inativo == false).Where(e => e.DataRealizado < endDate).Select(e => e.ParticipacoesConfirmadas);
                if (query.Any())
                    return query.ToList();
                return new List<int?>();
            }
            if (endDate == null && startDate.HasValue)
            {
                var query = _context.Set<Evento>().Where(e => e.Inativo == false).Where(e => e.DataRealizado > startDate).Select(e => e.ParticipacoesConfirmadas);
                if (query.Any())
                    return query.ToList();
                return new List<int?>();
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                var query = _context.Set<Evento>().Where(e => e.Inativo == false).Where(e => e.DataRealizado > startDate && e.DataRealizado < endDate).Select(e => e.ParticipacoesConfirmadas);
                if (query.Any())
                    return query.ToList();
                return new List<int?>();
            }
            return new List<int?>();
        }
    }
}
