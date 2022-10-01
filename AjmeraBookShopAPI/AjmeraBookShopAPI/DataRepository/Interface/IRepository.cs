namespace AjmeraBookShopAPI.DataRepository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(Guid Id);

        Task<bool> Add(T entity);

        Task<bool> Delete(T entity);

        Task<bool> Upsert(T entity);
    }
}

