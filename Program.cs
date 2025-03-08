using Microsoft.EntityFrameworkCore;
using SideSeams.Data; // Ensure your Data project is referenced properly.
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Data.Repositories;
using System.Diagnostics;

namespace SideSeams.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ? Add Controllers to the services
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });


            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages",
    "com.companyname.sideseams_9zz4h110yvjzm", "LocalState", "SideSeams.db");

            // Print for verification
            Console.WriteLine($"?? API Database Path: {dbPath}");
            Debug.WriteLine($"?? API Database Path: {dbPath}");

            builder.Services.AddDbContext<ClientContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // ? Register Client Repository
            builder.Services.AddScoped<IClientRepository, ClientRepository>();

            // ? Add Swagger Service
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SideSeams API", Version = "v1" });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ClientContext>();
                dbContext.Database.Migrate();
            }

            // ? Enable Swagger UI (for API testing)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
