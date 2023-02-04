namespace ApiEventos.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        int Commit();
    }
}
