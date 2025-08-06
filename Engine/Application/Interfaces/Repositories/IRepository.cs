namespace Engine.Application.Interfaces.Repositories;

public interface IRepository<T>
{
    public Task<T?> GetByIdAsync(Guid id);
    public Task AddAsync(T entity);
    public void Update(T entity);
    public Task DeleteAsync(Guid id);
}
