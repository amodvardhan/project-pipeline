using ProjectPipeline.Infrastructure.Data.Seeders;

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
                await DataSeeder.SeedAsync(scope.ServiceProvider);
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
