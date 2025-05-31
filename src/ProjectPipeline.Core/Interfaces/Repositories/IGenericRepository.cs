using System.Linq.Expressions;

namespace ProjectPipeline.Core.Interfaces.Repositories;

/// <summary>
/// Generic repository interface for common CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> expression);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
}
