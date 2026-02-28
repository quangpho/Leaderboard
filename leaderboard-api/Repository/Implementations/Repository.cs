using LeaderboardApi.Infrastructures.Database;
using LeaderboardApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace LeaderboardApi.Repository.Implementations;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly LeaderboardDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(LeaderboardDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        return await DbSet.FindAsync(id, cancellationToken);
    }
    public async Task DeleteAllRecords(CancellationToken cancellationToken)
    {
        await DbSet.ExecuteDeleteAsync(cancellationToken);
    }
}