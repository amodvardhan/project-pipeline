using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Enums;
using ProjectPipeline.Infrastructure.Data.Context;

namespace ProjectPipeline.Infrastructure.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed Roles
        await SeedRolesAsync(roleManager);

        // Seed Business Units
        await SeedBusinessUnitsAsync(context);

        // Seed Users
        await SeedUsersAsync(userManager, context);

        // Seed Sample Projects
        await SeedProjectsAsync(context, userManager);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "SystemAdmin", "DeliveryDirector", "DeliveryManager", "AccountManager" };

        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedBusinessUnitsAsync(ApplicationDbContext context)
    {
        if (!await context.BusinessUnits.AnyAsync())
        {
            var businessUnits = new List<BusinessUnit>
            {
                new BusinessUnit
                {
                    Name = "International Organization",
                    Code = "IO",
                    Description = "Impact organization with global reach",
                    HeadOfUnit = "Swapnil Gade",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new BusinessUnit
                {
                    Name = "Enterprise Applications",
                    Code = "EA",
                    Description = "Enterprise software development",
                    HeadOfUnit = "Sarah Johnson",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new BusinessUnit
                {
                    Name = "Digital Product Engineering",
                    Code = "DPES",
                    Description = "Cloud infrastructure and services",
                    HeadOfUnit = "Mike Wilson",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new BusinessUnit
                {
                    Name = "Data Analytics",
                    Code = "DA",
                    Description = "Business intelligence and analytics",
                    HeadOfUnit = "Lisa Brown",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.BusinessUnits.AddRangeAsync(businessUnits);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager, ApplicationDbContext context)
    {
        // Get business units for assignment
        var businessUnits = await context.BusinessUnits.ToListAsync();
        var digitalSolutions = businessUnits.FirstOrDefault(bu => bu.Code == "DPES");

        // Seed Admin User
        if (await userManager.FindByEmailAsync("admin@projectpipeline.com") == null)
        {
            var adminUser = new User
            {
                UserName = "admin@projectpipeline.com",
                Email = "admin@projectpipeline.com",
                FirstName = "System",
                LastName = "Administrator",
                Department = "IT",
                Designation = "System Admin",
                BusinessUnitId = digitalSolutions?.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "SystemAdmin");
            }
        }

        // Seed Account Manager
        if (await userManager.FindByEmailAsync("am@projectpipeline.com") == null)
        {
            var amUser = new User
            {
                UserName = "am@projectpipeline.com",
                Email = "am@projectpipeline.com",
                FirstName = "Alice",
                LastName = "Manager",
                Department = "Sales",
                Designation = "Account Manager",
                BusinessUnitId = digitalSolutions?.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(amUser, "AM@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(amUser, "AccountManager");
            }
        }

        // Seed Delivery Manager
        if (await userManager.FindByEmailAsync("dm@projectpipeline.com") == null)
        {
            var dmUser = new User
            {
                UserName = "dm@projectpipeline.com",
                Email = "dm@projectpipeline.com",
                FirstName = "David",
                LastName = "Manager",
                Department = "Delivery",
                Designation = "Delivery Manager",
                BusinessUnitId = digitalSolutions?.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(dmUser, "DM@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(dmUser, "DeliveryManager");
            }
        }
    }

    private static async Task SeedProjectsAsync(ApplicationDbContext context, UserManager<User> userManager)
    {
        if (!await context.Projects.AnyAsync())
        {
            var adminUser = await userManager.FindByEmailAsync("admin@projectpipeline.com");
            var businessUnits = await context.BusinessUnits.ToListAsync();
            var digitalSolutions = businessUnits.FirstOrDefault(bu => bu.Code == "DS");
            var enterpriseApps = businessUnits.FirstOrDefault(bu => bu.Code == "EA");

            var projects = new List<Project>
            {
                new Project
                {
                    Name = "E-Commerce Platform Modernization",
                    Description = "Modernize legacy e-commerce platform with microservices architecture",
                    ClientName = "RetailCorp Inc",
                    Technology = "React, .NET Core, Azure, Docker",
                    ProjectType = "Development",
                    EstimatedValue = 250000,
                    Status = ProjectStatusEnum.Pipeline,
                    BusinessUnitId = digitalSolutions?.Id ?? 1,
                    ExpectedClosureDate = DateTime.UtcNow.AddMonths(3),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = adminUser?.Id,
                    ProfilesSubmitted = 5,
                    ProfilesShortlisted = 2,
                    ProfilesSelected = 0
                },
                new Project
                {
                    Name = "CRM System Integration",
                    Description = "Integrate Salesforce CRM with existing ERP system",
                    ClientName = "TechSolutions Ltd",
                    Technology = "Salesforce, .NET, SQL Server",
                    ProjectType = "Integration",
                    EstimatedValue = 150000,
                    Status = ProjectStatusEnum.Won,
                    ActualValue = 145000,
                    BusinessUnitId = enterpriseApps?.Id ?? 2,
                    ExpectedClosureDate = DateTime.UtcNow.AddMonths(2),
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    CreatedAt = DateTime.UtcNow.AddDays(-45),
                    CreatedBy = adminUser?.Id,
                    ProfilesSubmitted = 8,
                    ProfilesShortlisted = 4,
                    ProfilesSelected = 2
                },
                new Project
                {
                    Name = "Mobile Banking App",
                    Description = "Develop secure mobile banking application for iOS and Android",
                    ClientName = "FinanceBank",
                    Technology = "React Native, Node.js, MongoDB",
                    ProjectType = "Mobile Development",
                    EstimatedValue = 400000,
                    Status = ProjectStatusEnum.Pipeline,
                    BusinessUnitId = digitalSolutions?.Id ?? 1,
                    ExpectedClosureDate = DateTime.UtcNow.AddMonths(6),
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = adminUser?.Id,
                    ProfilesSubmitted = 12,
                    ProfilesShortlisted = 6,
                    ProfilesSelected = 1
                }
            };

            await context.Projects.AddRangeAsync(projects);
            await context.SaveChangesAsync();
        }
    }
}