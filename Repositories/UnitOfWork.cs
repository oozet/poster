using Microsoft.EntityFrameworkCore;

public interface IUnitOfWork<out TContext>
    where TContext : DbContext, new()
{
    //The following Property is going to hold the context object
    TContext Context { get; }

    //Start the database Transaction

    //Commit the database Transaction

    //Rollback the database Transaction

    //DbContext Class SaveChanges method
    void CreateTransactionAsync();
    void CommitAsync();
    void RollbackAsync();
    void SaveAsync();
}

public class UnitOfWork<TContext> : IUnitOfWork<TContext>
    where TContext : DbContext, new()
{
    public TContext Context => throw new NotImplementedException();

    public void Commit()
    {
        throw new NotImplementedException();
    }

    public void CreateTransaction()
    {
        throw new NotImplementedException();
    }

    public void Rollback()
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }
}

    Task CreateTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task SaveAsync();
    public class UnitOfWork : IUnitOfWork
{
    private readonly MyDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(MyDbContext context)
    {
        _context = context;
    }

    public async Task CreateTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
