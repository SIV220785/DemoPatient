using Microsoft.OpenApi.Models;
using System.Reflection;
using Patient.API.Middleware;
using Patient.BLL.Interfaces;
using Patient.BLL.Mapping;
using Patient.BLL.Services;
using Patient.DAL.Extensions;


namespace Patient.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Patient API",
                Version = "v1",
                Description = "API for managing patient data",
                Contact = new OpenApiContact
                {
                    Name = "Name",
                    Email = "email@example.com",
                    Url = new Uri("https://example.com/")
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddDatabaseAndUnitOfWorkServices(builder.Configuration);
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddScoped<IPatientService, PatientService>();

        var app = builder.Build();

        ServiceExtensions.InitializeDatabase(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
