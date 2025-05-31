using Microsoft.EntityFrameworkCore.Storage;
using ProjectPipeline.Core.Interfaces.Repositories;
using ProjectPipeline.Infrastructure.Data.Context;

namespace ProjectPipeline.Infrastructure.Data.Repositories;

/// <summary>
/// Unit of Work implementation for managing transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Projects = new ProjectRepository(_context);
        Users = new UserRepository(_context);
    }

    public IProjectRepository Projects { get; private set; }
    public IUserRepository Users { get; private set; }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        if (_repositories.ContainsKey(typeof(T)))
        {
            return (IGenericRepository<T>)_repositories[typeof(T)];
        }

        var repository = new GenericRepository<T>(_context);
        _repositories.Add(typeof(T), repository);
        return repository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
