namespace AjmeraBookShopAPI.DataRepository.Interface
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }

        Task SaveChanges();
    }
}
