using AjmeraBookShopAPI.DataModel;
using AjmeraBookShopAPI.DataRepository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AjmeraBookShopAPI.DataRepository
{
    public class BookRepository : EntityRepository<BookModel>, IBookRepository
    {
        public BookRepository(ApplicationDBContext dbContaxt, ILogger logger) : base(dbContaxt, logger)
        {

        }

        public override async Task<IEnumerable<BookModel>> GetAll()
        {
            try
            {
                return await base.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAll", typeof(BookRepository));
                return new List<BookModel>();
            }
        }

        public override async Task<bool> Upsert(BookModel book)
        {
            try
            {
                var existingBook = await dbSet.Where(p => p.Id == book.Id).FirstOrDefaultAsync();
                if (existingBook == null)
                    return await base.Add(book);
                else
                {
                    existingBook.AuthorName = book.AuthorName;
                    existingBook.Name = book.Name;
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upsert", typeof(BookRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(BookModel book)
        {
            try
            {
                var existingBook = await dbSet.FindAsync(book.Id);
                if (existingBook != null)
                    return await base.Delete(existingBook);
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete", typeof(BookRepository));
                return false;
            }
        }

        public async Task<bool> IsBookExistByName(BookModel book)
        {
            try
            {
                var isExist = await dbSet.AnyAsync(p => p.Name.ToLower() == book.Name.ToLower() && p.AuthorName.ToLower() == book.AuthorName.ToLower());
                if (isExist)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "IsBookExistByName", typeof(BookRepository));
                return false;
            }
        }

        public async Task<bool> IsBookExistById(Guid id)
        {
            return await dbSet.AnyAsync(p => p.Id == id);
        }
    }
}
