namespace ApiEventos.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);
        TEntity GetLastEntity();
        IEnumerable<TEntity> GetAll();
        void Save(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
    }
}
