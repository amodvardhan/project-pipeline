using ProjectPipeline.Core.Entities;

namespace ProjectPipeline.Core.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for user operations
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByIdAsync(string userId);
        Task<User?> GetUserWithBusinessUnitAsync(string userId); // Added missing method
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByBusinessUnitAsync(int businessUnitId);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<bool> IsEmailUniqueAsync(string email, string? excludeUserId = null);
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
    }
}
