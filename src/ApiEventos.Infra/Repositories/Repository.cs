using ApiEventos.Domain.Interfaces;
using ApiEventos.Domain.Models;
using ApiEventos.Infra.Context;

namespace ApiEventos.Infra.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context) => _context = context;

        public virtual TEntity GetById(int id)
        {
            var query = _context.Set<TEntity>().Where(e => e.Id == id);
            if (query.Any())
                return query.FirstOrDefault();
            return null;
        }

        public virtual TEntity GetLastEntity()
        {
            var query = _context.Set<TEntity>().OrderByDescending(e => e.Id);
            if (query.Any())
                return query.FirstOrDefault();
            return null;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var query = _context.Set<TEntity>();
            if (query.Any())
                return query.ToList();
            return new List<TEntity>();
        }

        public virtual async Task Save(TEntity entity)
        {
           _context.Set<TEntity>().Add(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            var query = _context.Set<TEntity>().Where(e => e.Id == entity.Id);
            if (query.Any())
            {
                var selectedEntity = query.FirstOrDefault();
                _context.Set<TEntity>().Update(entity);
            }
        }

        public virtual async Task Delete(int id)
        {
            var query = _context.Set<TEntity>().Where(e => e.Id == id);
            if(query.Any())
            {
                var selectedEntity = query.FirstOrDefault();
                _context.Set<TEntity>().Remove(selectedEntity);
            }
        }
    }
}
