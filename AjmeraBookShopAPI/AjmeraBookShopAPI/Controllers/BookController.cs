using AjmeraBookShopAPI.DataModel;
using AjmeraBookShopAPI.DataRepository.Interface;
using AjmeraBookShopAPI.ServiceModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AjmeraBookShopAPI.Controllers
{
    [ApiController]
    [Route("[Controller]s")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookController(ILogger<BookController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("AddBook")]
        public async Task<IActionResult> AddBook(BookSeviceModel book)
        {
            if (book == null || ModelState.IsValid == false)
                return BadRequest(ModelState);

            book.Id = Guid.NewGuid();
            var bookMap = _mapper.Map<BookModel>(book);
            bool isBookExist = await _unitOfWork.Books.IsBookExistByName(book);
            if (isBookExist)
            {
                ModelState.AddModelError("", $"Book with the name '{book.Name}' already exists");
                return StatusCode(422, ModelState);
            }
            await _unitOfWork.Books.Add(bookMap);
            await _unitOfWork.SaveChanges();

            return CreatedAtAction("GetBook", new { bookMap.Id }, bookMap);
        }

        [HttpGet("{id}")]
        //[Route("GetBook1")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var book = await _unitOfWork.Books.GetById(id);
            if (book == null)
                return NotFound();
            else
            {
                var bookMap = _mapper.Map<BookSeviceModel>(book);
                return Ok(bookMap);
            }
        }

        [HttpGet()]
        [Route("GetAllBooks")]
        public async Task<IActionResult> GetAll()
        {
            var books = await _unitOfWork.Books.GetAll();
            var booksMap = _mapper.Map<List<BookSeviceModel>>(books);
            return Ok(booksMap);
        }

        [HttpPost("{id}")]
        //[Route("UpsertBook")]
        public async Task<IActionResult> UpsertBook(Guid id, BookSeviceModel book)
        {
            if (id != book.Id)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bookMap = _mapper.Map<BookModel>(book);
            await _unitOfWork.Books.Upsert(bookMap);
            await _unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            if (!await _unitOfWork.Books.IsBookExistById(id))
                return NotFound();
            var book = await _unitOfWork.Books.GetById(id);
            if (await _unitOfWork.Books.Delete(book))
                await _unitOfWork.SaveChanges();
            else
                ModelState.AddModelError("", "Something went wrong deleting category");

            return NoContent();
        }
    }
}
