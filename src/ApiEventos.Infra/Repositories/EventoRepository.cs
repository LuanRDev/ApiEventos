using ApiEventos.Domain.Models;
using ApiEventos.Infra.Context;

namespace ApiEventos.Infra.Repositories
{
    public class EventoRepository : Repository<Evento>
    {
        public EventoRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public override Evento GetById(int id)
        {
            var query = _context.Set<Evento>().Where(e => e.Id == id);
            if(query.Any())
                return query.FirstOrDefault();
            return null;
        }

        public override Evento GetLastEntity()
        {
            var query = _context.Set<Evento>().OrderByDescending(e => e.Id);
            if (query.Any())
                return query.FirstOrDefault();
            return null;
        }

        public override IEnumerable<Evento> GetAll()
        {
            var query = _context.Set<Evento>();
            if(query.Any())
                return query.ToList();
            return new List<Evento>();
        }
    }
}
