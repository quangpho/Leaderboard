namespace LeaderboardApi.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<T> GetByIdAsync(object id, CancellationToken cancellationToken);
        Task DeleteAllRecords(CancellationToken cancellationToken);
    }
}