namespace ApiEventos.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);
        TEntity GetLastEntity();
        IEnumerable<TEntity> GetAll();
        Task Save(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(int id);
    }
}
