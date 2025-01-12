namespace server.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<T?> GetAsync(Guid Id);
    Task CreateAsync(T data);
    Task RemoveAsync(T data);
    Task UpdateAsync(T data);
    Task SaveChangesAsync();
}
