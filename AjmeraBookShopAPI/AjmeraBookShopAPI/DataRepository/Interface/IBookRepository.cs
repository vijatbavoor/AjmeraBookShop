using AjmeraBookShopAPI.DataModel;

namespace AjmeraBookShopAPI.DataRepository.Interface
{
    public interface IBookRepository : IRepository<BookModel>
    {
        Task<bool> IsBookExistById(Guid id);
        Task<bool> IsBookExistByName(BookModel book);
    }
}
