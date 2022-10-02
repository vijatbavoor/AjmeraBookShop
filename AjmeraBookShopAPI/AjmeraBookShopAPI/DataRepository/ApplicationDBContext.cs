using AjmeraBookShopAPI.DataModel;
using Microsoft.EntityFrameworkCore;

namespace AjmeraBookShopAPI.DataRepository
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            base.Database.EnsureCreated();
        }

        public virtual DbSet<BookModel> Books { get; set; }
    }
}
