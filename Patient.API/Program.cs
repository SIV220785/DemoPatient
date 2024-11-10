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

        builder.WebHost.UseUrls("http://0.0.0.0:5000");

        // Configure Services
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure Middleware and Web Application pipeline
        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddLogging();
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
                    Name = "Your Name",
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
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        // Ensuring database is initialized
        ServiceExtensions.InitializeDatabase(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();  // Ensure UseRouting() is called before UseAuthorization()

        app.UseAuthorization();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.MapControllers();

        // Set URLs here
        app.Urls.Add("http://*:80");
    }
}
