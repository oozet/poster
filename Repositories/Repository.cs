using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task RemoveAsync(object id);
    Task RemoveAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetAsync();
    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = ""
    );
    Task UpdateAsync(TEntity entityToUpdate);
}

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task RemoveAsync(object id)
    {
        TEntity? entityToDelete = await _dbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            await RemoveAsync(entityToDelete);
        }
    }

    public virtual Task RemoveAsync(TEntity entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);

        return Task.CompletedTask;
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = ""
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (
            var includeProperty in includeProperties.Split(
                new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries
            )
        )
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public virtual Task UpdateAsync(TEntity entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;

        return Task.CompletedTask;
    }
}
