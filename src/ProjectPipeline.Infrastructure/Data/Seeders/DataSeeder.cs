using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;
using ProjectPipeline.Infrastructure.Data.Context;

namespace ProjectPipeline.Infrastructure.Data.Seeders
{
    /// <summary>
    /// Data seeder for initial application data
    /// </summary>
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Seed roles
                await SeedRolesAsync(roleManager, logger);

                // Seed business units and save changes
                await SeedBusinessUnitsAsync(context, logger);
                await context.SaveChangesAsync(); // Save business units first

                // Seed admin user (now business units exist)
                await SeedAdminUserAsync(userManager, context, logger);

                // Seed sample data
                await SeedSampleDataAsync(context, userManager, logger);

                await context.SaveChangesAsync();
                logger.LogInformation("Data seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding data");
                throw;
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            logger.LogInformation("Seeding roles...");

            var roles = new[]
            {
                "SystemAdmin",
                "BusinessUnitHead", 
                "DeliveryDirector",
                "DeliveryManager",
                "AccountManager"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation($"Created role: {role}");
                }
            }
        }

        private static async Task SeedBusinessUnitsAsync(ApplicationDbContext context, ILogger logger)
        {
            logger.LogInformation("Seeding business units...");

            if (!await context.BusinessUnits.AnyAsync())
            {
                var businessUnits = new[]
                {
                    new BusinessUnit
                    {
                        Name = "Digital Solutions",
                        Code = "DS",
                        Description = "Digital transformation and web solutions",
                        HeadOfUnit = "John Smith",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    new BusinessUnit
                    {
                        Name = "Enterprise Applications",
                        Code = "EA",
                        Description = "Enterprise software development and integration",
                        HeadOfUnit = "Sarah Johnson",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    new BusinessUnit
                    {
                        Name = "Cloud Services",
                        Code = "CS",
                        Description = "Cloud migration and infrastructure services",
                        HeadOfUnit = "Mike Wilson",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    new BusinessUnit
                    {
                        Name = "Data Analytics",
                        Code = "DA",
                        Description = "Data science and analytics solutions",
                        HeadOfUnit = "Emily Davis",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    }
                };

                await context.BusinessUnits.AddRangeAsync(businessUnits);
                logger.LogInformation($"Created {businessUnits.Length} business units");
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<User> userManager, ApplicationDbContext context, ILogger logger)
        {
            logger.LogInformation("Seeding admin user...");

            const string adminEmail = "admin@projectpipeline.com";
            const string adminPassword = "Admin@123456";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                // Get the first business unit for admin (now it exists in database)
                var firstBusinessUnit = await context.BusinessUnits.FirstOrDefaultAsync();
                if (firstBusinessUnit == null)
                {
                    logger.LogError("No business units found for admin user creation");
                    return;
                }

                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Administrator",
                    Department = "IT",
                    Designation = "System Administrator",
                    BusinessUnitId = firstBusinessUnit.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "SystemAdmin");
                    logger.LogInformation($"Created admin user: {adminEmail}");
                }
                else
                {
                    logger.LogError($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        private static async Task SeedSampleDataAsync(ApplicationDbContext context, UserManager<User> userManager, ILogger logger)
        {
            logger.LogInformation("Seeding sample data...");

            // Create sample users if they don't exist
            await SeedSampleUsersAsync(userManager, context, logger);

            // Create sample projects if they don't exist
            if (!await context.Projects.AnyAsync())
            {
                await SeedSampleProjectsAsync(context, userManager, logger);
            }
        }

        private static async Task SeedSampleUsersAsync(UserManager<User> userManager, ApplicationDbContext context, ILogger logger)
        {
            var businessUnits = await context.BusinessUnits.ToListAsync();
            var sampleUsers = new[]
            {
                new { Email = "john.doe@company.com", FirstName = "John", LastName = "Doe", Role = "AccountManager", Department = "Sales", Designation = "Senior Account Manager" },
                new { Email = "jane.smith@company.com", FirstName = "Jane", LastName = "Smith", Role = "DeliveryManager", Department = "Delivery", Designation = "Delivery Manager" },
                new { Email = "bob.wilson@company.com", FirstName = "Bob", LastName = "Wilson", Role = "DeliveryDirector", Department = "Delivery", Designation = "Delivery Director" },
                new { Email = "alice.brown@company.com", FirstName = "Alice", LastName = "Brown", Role = "BusinessUnitHead", Department = "Management", Designation = "BU Head" }
            };

            foreach (var userData in sampleUsers)
            {
                var existingUser = await userManager.FindByEmailAsync(userData.Email);
                if (existingUser == null)
                {
                    var user = new User
                    {
                        UserName = userData.Email,
                        Email = userData.Email,
                        EmailConfirmed = true,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        Department = userData.Department,
                        Designation = userData.Designation,
                        BusinessUnitId = businessUnits[Random.Shared.Next(businessUnits.Count)].Id,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, "User@123456");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userData.Role);
                        logger.LogInformation($"Created sample user: {userData.Email}");
                    }
                }
            }
        }

        private static async Task SeedSampleProjectsAsync(ApplicationDbContext context, UserManager<User> userManager, ILogger logger)
        {
            var businessUnits = await context.BusinessUnits.ToListAsync();
            var adminUser = await userManager.FindByEmailAsync("admin@projectpipeline.com");
            
            if (adminUser == null)
            {
                logger.LogWarning("Admin user not found, skipping sample projects");
                return;
            }

            var sampleProjects = new[]
            {
                new Project
                {
                    Name = "E-Commerce Platform Modernization",
                    Description = "Modernize legacy e-commerce platform with microservices architecture",
                    ClientName = "RetailCorp Inc.",
                    EstimatedValue = 500000m,
                    Status = ProjectStatusEnum.Pipeline,
                    Technology = ".NET Core, React, Azure",
                    ProjectType = "Development",
                    BusinessUnitId = businessUnits[0].Id,
                    ExpectedClosureDate = DateTime.UtcNow.AddMonths(3),
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = adminUser.Id
                },
                new Project
                {
                    Name = "Cloud Migration Project",
                    Description = "Migrate on-premise applications to Azure cloud",
                    ClientName = "TechStart Solutions",
                    EstimatedValue = 750000m,
                    ActualValue = 720000m,
                    Status = ProjectStatusEnum.Won,
                    StatusReason = "Competitive pricing and strong technical proposal",
                    Technology = "Azure, Docker, Kubernetes",
                    ProjectType = "Migration",
                    BusinessUnitId = businessUnits[2].Id,
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    ExpectedClosureDate = DateTime.UtcNow.AddMonths(6),
                    CreatedAt = DateTime.UtcNow.AddDays(-45),
                    CreatedBy = adminUser.Id
                },
                new Project
                {
                    Name = "Data Analytics Dashboard",
                    Description = "Build real-time analytics dashboard for business intelligence",
                    ClientName = "DataDriven Corp",
                    EstimatedValue = 300000m,
                    Status = ProjectStatusEnum.Lost,
                    StatusReason = "Client chose competitor due to budget constraints",
                    Technology = "Power BI, SQL Server, Python",
                    ProjectType = "Analytics",
                    BusinessUnitId = businessUnits[3].Id,
                    ExpectedClosureDate = DateTime.UtcNow.AddMonths(2),
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    CreatedBy = adminUser.Id
                },
                new Project
                {
                    Name = "Mobile App Development",
                    Description = "Cross-platform mobile application for customer engagement",
                    ClientName = "MobileFirst Ltd",
                    EstimatedValue = 400000m,
                    Status = ProjectStatusEnum.Pipeline,
                    Technology = "React Native, Node.js, MongoDB",
                    ProjectType = "Mobile Development",
                    BusinessUnitId = businessUnits[0].Id,
                    ExpectedClosureDate = DateTime.UtcNow.AddMonths(4),
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    CreatedBy = adminUser.Id
                }
            };

            await context.Projects.AddRangeAsync(sampleProjects);
            logger.LogInformation($"Created {sampleProjects.Length} sample projects");
        }
    }
}
