using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patient.DAL.Context;
using Patient.DAL.Interfaces;

namespace Patient.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterMssqlInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PatientDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Database");

            options.UseSqlServer(connectionString, o =>
            {
                o.CommandTimeout(130);
                o.UseCompatibilityLevel(120);
                o.EnableRetryOnFailure(3);
                o.ExecutionStrategy(dependencies => new NonRetryingExecutionStrategy(dependencies));
            });
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}