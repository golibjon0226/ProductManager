namespace ProductManaer.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    TEntity? GetById(ulong id);
    IQueryable<TEntity> GetAll();
    ValueTask<TEntity> AddAsync(TEntity entity);
    ValueTask<TEntity> Remove(TEntity entity);
    ValueTask<TEntity> Update(TEntity entity);
}