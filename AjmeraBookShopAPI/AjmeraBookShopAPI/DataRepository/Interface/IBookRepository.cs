using AjmeraBookShopAPI.DataModel;
using AjmeraBookShopAPI.ServiceModel;

namespace AjmeraBookShopAPI.DataRepository.Interface
{
    public interface IBookRepository : IRepository<BookModel>
    {
        Task<bool> IsBookExistById(Guid id);
        Task<bool> IsBookExistByName(BookSeviceModel book);
    }
}
