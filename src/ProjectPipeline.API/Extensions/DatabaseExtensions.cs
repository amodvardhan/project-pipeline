using Microsoft.AspNetCore.Identity;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Infrastructure.Data.Context;
using ProjectPipeline.Infrastructure.Data.Seed;

namespace ProjectPipeline.API.Extensions
{
    /// <summary>
    /// Extension methods for database operations
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Seeds the database with initial data
        /// </summary>
        /// <param name="app">Web application</param>
        /// <returns>Task</returns>
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Starting database seeding...");
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await DataSeeder.SeedAsync(context, userManager, roleManager);
                logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }
    }
}
