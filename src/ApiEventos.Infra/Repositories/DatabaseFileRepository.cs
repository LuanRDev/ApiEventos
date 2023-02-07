using ApiEventos.Domain.Models;
using ApiEventos.Infra.Context;

namespace ApiEventos.Infra.Repositories
{
    public class DatabaseFileRepository : Repository<DatabaseFile>
    {
        public DatabaseFileRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
