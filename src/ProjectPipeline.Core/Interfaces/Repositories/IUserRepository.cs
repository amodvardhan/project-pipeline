using ProjectPipeline.Core.Entities;

namespace ProjectPipeline.Core.Interfaces.Repositories;

/// <summary>
/// User repository interface for user-specific operations
/// </summary>
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersByBusinessUnitAsync(int businessUnitId);
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<User?> GetUserWithBusinessUnitAsync(string userId);
}
