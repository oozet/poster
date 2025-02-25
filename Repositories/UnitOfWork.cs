using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<Post> PostRepository { get; }

    // Task CreateTransactionAsync();
    // Task CommitAsync();
    // Task RollbackAsync();
    Task SaveAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction? _transaction;

    private Repository<Post>? _postRepository;
    public IRepository<Post> PostRepository =>
        _postRepository ?? (_postRepository = new Repository<Post>(_context));

    public UnitOfWork(PosterDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // public async Task CreateTransactionAsync()
    // {
    //     _transaction = await _context.Database.BeginTransactionAsync();
    // }

    // public async Task CommitAsync()
    // {
    //     if (_transaction != null)
    //     {
    //         await _transaction.CommitAsync();
    //         await _transaction.DisposeAsync();
    //         _transaction = null;
    //     }
    // }

    // public async Task RollbackAsync()
    // {
    //     if (_transaction != null)
    //     {
    //         await _transaction.RollbackAsync();
    //         await _transaction.DisposeAsync();
    //         _transaction = null;
    //     }
    // }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    private bool _disposed;

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);

        // Take this object off the finalization queue to prevent
        // finalization code for this object from executing a second time.
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources.
                await _context.DisposeAsync();
            }

            // Dispose any unmanaged resources here...

            _disposed = true;
        }
    }
}
