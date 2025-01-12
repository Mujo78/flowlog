using server.Data;
using server.Repository.IRepository;

namespace server.Repository;

public class Repository<T>(ApplicationDBContext context) : IRepository<T> where T : class
{
    private readonly ApplicationDBContext _context = context;

    public async Task CreateAsync(T data)
    {
        await _context.Set<T>().AddAsync(data);
        await SaveChangesAsync();
    }

    public async Task<T?> GetAsync(Guid Id)
    {
        return await _context.Set<T>().FindAsync(Id);
    }

    public async Task RemoveAsync(T data)
    {
        _context.Set<T>().Remove(data);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(T data)
    {
        _context.Set<T>().Update(data);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
