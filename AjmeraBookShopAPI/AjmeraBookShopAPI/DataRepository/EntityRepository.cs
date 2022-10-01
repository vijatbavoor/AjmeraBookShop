using AjmeraBookShopAPI.DataRepository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AjmeraBookShopAPI.DataRepository
{
    public class EntityRepository<T>: IRepository<T> where T : class 
    {
        protected ApplicationDBContext _dBContext;
        protected DbSet<T> dbSet;
        protected readonly ILogger _logger;

        public EntityRepository(ApplicationDBContext dBContext, ILogger logger)
        {
           _dBContext = dBContext;
            _logger = logger;
            dbSet = _dBContext.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(Guid Id)
        {
            return await dbSet.FindAsync(Id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(T entity)
        {
            dbSet.Remove(entity);
            return true;
        }

        public virtual Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
