using AjmeraBookShopAPI.DataRepository.Interface;

namespace AjmeraBookShopAPI.DataRepository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ILogger _logger;

        public IBookRepository Books { get; private set; }

        public UnitOfWork(ApplicationDBContext dbContext, ILoggerFactory logger)
        {
            _dbContext = dbContext;
            _logger = logger.CreateLogger("logs");

            Books = new BookRepository(dbContext, _logger);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public  async void Dispose()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
