using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Core.Interfaces.Services;
using ProjectPipeline.Infrastructure.Data.Repositories;
using ProjectPipeline.Infrastructure.Services;

namespace ProjectPipeline.API.Extensions
{
    /// <summary>
    /// Extension methods for service registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            // Register Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileSubmissionRepository, ProfileSubmissionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileSubmissionService, ProfileSubmissionService>();

            // Add AutoMapper if you're using it
            // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
