namespace MP4.Repositories;


public interface IRepository<T>
    where T : class
{
    Task<List<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T entity);

    Task DeleteAsync(Guid id);

    Task SaveChangesAsync();
}