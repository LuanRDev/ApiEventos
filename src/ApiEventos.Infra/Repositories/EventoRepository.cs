﻿using ApiEventos.Domain.Models;
using ApiEventos.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiEventos.Infra.Repositories
{
    public class EventoRepository : Repository<Evento>
    {
        public EventoRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public override Evento GetById(int id)
        {
            var query = _context.Set<Evento>().Where(e => e.Id == id).Where(e => e.Inativo == false).Include(e => e.ConteudoEventos);
            if (query.Any())
                return query.FirstOrDefault();
            return null;
        }

        public override Evento GetLastEntity()
        {
            var query = _context.Set<Evento>().Where(e => e.Inativo == false).OrderByDescending(e => e.Id);
            if (query.Any())
                return query.FirstOrDefault();
            return null;
        }

        public override IEnumerable<Evento> GetLimit(int limit)
        {
            var query = _context.Set<Evento>().Where(e => e.Inativo == false).Take(limit);
            if (query.Any())
                return query.ToList();
            return new List<Evento>();
        }

        public override IEnumerable<Evento> GetAll()
        {
            var query = _context.Set<Evento>().Where(e => e.Inativo == false).Include(e => e.ConteudoEventos);
            if (query.Any())
                return query.ToList();
            return new List<Evento>();
        }

        public IEnumerable<Evento> GetDate(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate.HasValue)
            {
                var query = _context.Set<Evento>().Where(e => e.Inativo == false).Where(e => e.DataRealizado < endDate);
                if (query.Any())
                    return query.ToList();
                return new List<Evento>();
            }
            if (endDate == null && startDate.HasValue)
            {
                var query = _context.Set<Evento>().Where(e => e.Inativo == false).Where(e => e.DataRealizado > startDate);
                if (query.Any())
                    return query.ToList();
                return new List<Evento>();
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                var query = _context.Set<Evento>().Where(e => e.Inativo == false).Where(e => e.DataRealizado > startDate && e.DataRealizado < endDate);
                if (query.Any())
                    return query.ToList();
                return new List<Evento>();
            }
            return new List<Evento>();
        }
    }
}
