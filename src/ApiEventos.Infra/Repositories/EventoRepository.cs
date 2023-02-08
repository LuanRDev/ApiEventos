using ApiEventos.Domain.Models;
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
            if(query.Any())
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

        public override IEnumerable<Evento> GetAll()
        {
            var query = _context.Set<Evento>().Where(e => e.Inativo == false).Include(e => e.ConteudoEventos);
            if(query.Any())
                return query.ToList();
            return new List<Evento>();
        }
    }
}
