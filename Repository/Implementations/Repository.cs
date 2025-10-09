using Database;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
namespace Repository.Implementations;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly LeaderboardDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(LeaderboardDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<T> GetByIdAsync(object id)
    {
        return await DbSet.FindAsync(id);
    }
    public async Task DeleteAllRecords()
    {
        await DbSet.ExecuteDeleteAsync();
    }
}