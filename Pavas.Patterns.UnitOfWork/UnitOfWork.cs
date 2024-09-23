using Microsoft.EntityFrameworkCore.Storage;
using Pavas.Patterns.UnitOfWork.Abstracts;
using Pavas.Patterns.UnitOfWork.Contracts;

namespace Pavas.Patterns.UnitOfWork;

internal class UnitOfWork(DatabaseContext context) : IUnitOfWork
{
    public IRepository<TEntity> GetRepository<TEntity>(CancellationToken token = new()) where TEntity : class, IEntity
    {
        return new Repository<TEntity>(context, token);
    }

    public IDbContextTransaction BeginTransaction()
    {
        return context.Database.BeginTransaction();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = new())
    {
        return context.Database.BeginTransactionAsync(token);
    }

    public int SaveChanges()
    {
        return context.SaveChanges();
    }

    public Task<int> SaveChangesAsync(CancellationToken token = new())
    {
        return context.SaveChangesAsync(token);
    }
}