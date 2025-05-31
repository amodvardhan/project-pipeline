namespace ProjectPipeline.Core.Interfaces.Repositories;

/// <summary>
/// Unit of Work pattern interface for managing transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IProjectRepository Projects { get; }
    IUserRepository Users { get; }
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
