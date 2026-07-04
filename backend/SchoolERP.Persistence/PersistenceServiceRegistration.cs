using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolERP.Application.Interfaces;
using SchoolERP.Persistence.Context;
using SchoolERP.Persistence.Repositories;

namespace SchoolERP.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<SchoolERPDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(SchoolERPDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
