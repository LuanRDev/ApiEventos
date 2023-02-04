using ApiEventos.Domain.Interfaces;
using ApiEventos.Domain.Models;
using ApiEventos.Infra.Context;
using ApiEventos.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiEventos.Application.DI
{
    public class Initializer
    {
        public static void Configure(IServiceCollection services, string connection)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<Evento>), typeof(Repository<Evento>));
            services.AddScoped(typeof(EventoService));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        }
    }
}
