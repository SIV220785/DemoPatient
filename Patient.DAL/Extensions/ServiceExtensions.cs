using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Patient.DAL.Context;
using Patient.DAL.Interfaces;
using Patient.DAL.Repository;

namespace Patient.DAL.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabaseAndUnitOfWorkServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PatientDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.CommandTimeout(130);
                sqlServerOptions.EnableRetryOnFailure(3);
                sqlServerOptions.ExecutionStrategy(dependencies =>
                    new SqlServerRetryingExecutionStrategy(dependencies, maxRetryCount: 3));
            });
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static void InitializeDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<PatientDbContext>();

        try
        {
            if (!context.Database.CanConnect())
            {
                try
                {
                    context.Database.EnsureCreated();
                }
                catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 1801)
                { }
            }
            else
            { }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Ошибка при инициализации базы данных", ex);
        }
    }
}
