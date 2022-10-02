using AjmeraBookShopAPI.Controllers;
using AjmeraBookShopAPI.DataModel;
using AjmeraBookShopAPI.DataRepository.Interface;
using AjmeraBookShopAPI.ServiceModel;
using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AjmeraBookShopAPI.Test
{
    public class BookControllerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookController> _logger;
        private readonly IMapper _mapper;
        private readonly BookSeviceModel _book;
        private readonly BookModel _dbBook;
        private readonly BookController _bookController;
        private readonly Guid _bookId;
        private readonly List<BookSeviceModel> _books;
        private readonly List<BookModel> _dbBooks;

        public BookControllerTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _logger = A.Fake<ILogger<BookController>>();
            _mapper = A.Fake<IMapper>();

            _book = A.Fake<BookSeviceModel>();
            _dbBook = A.Fake<BookModel>();
            _bookId = Guid.Parse("9ca50451-d07f-4e74-a0ee-718721668275");
            _books = A.Fake<List<BookSeviceModel>>();

            _bookController = new BookController(_logger, _unitOfWork, _mapper);
        }

        [Fact]
        public async void Should_AddBook_If_Same_BookName_With_SameAuthore_Not_Exist_Return_Created_Responce()
        {

            // Arrange
            A.CallTo(() => _mapper.Map<BookModel>(_book)).Returns(_dbBook);
            A.CallTo(() => _unitOfWork.Books.IsBookExistByName(_book)).Returns(false);
            A.CallTo(() => _unitOfWork.Books.Add(_dbBook)).Returns(true);

            // Act
            var actionResult = await _bookController.AddBook(_book);
            var result = actionResult as CreatedAtActionResult;
            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(201, result.StatusCode);
            A.CallTo(() => _unitOfWork.SaveChanges()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.Books.Add(_dbBook)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Should_Not_AddBook_If_Same_BookName_With_SameAuthore_Exist_Return_BookExist_Responce()
        {
            // Arrange
            A.CallTo(() => _mapper.Map<BookModel>(_book)).Returns(_dbBook);
            A.CallTo(() => _unitOfWork.Books.IsBookExistByName(_book)).Returns(true);

            // Act
            var actionResult = await _bookController.AddBook(_book);
            var result = actionResult as ObjectResult;
            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(422, result.StatusCode);
            A.CallTo(() => _unitOfWork.Books.IsBookExistByName(_book)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.SaveChanges()).MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.Books.Add(_dbBook)).MustNotHaveHappened();
        }

        [Fact]
        public async void Should_Not_AddBook_If_BookObject_Is_Null__Return_Bad_Reponce()
        {
            // Act
            var actionResult = await _bookController.AddBook(null);
            var result = actionResult as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(400, result.StatusCode);
            A.CallTo(() => _unitOfWork.Books.IsBookExistByName(null)).MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.SaveChanges()).MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.Books.Add(_dbBook)).MustNotHaveHappened();
        }

        [Fact]
        public async void Should_Get_Book_By_Id_If_Exist_Return_Ok_Reponce()
        {
            // Arrange
            A.CallTo(() => _unitOfWork.Books.GetById(A<Guid>._)).Returns(_dbBook);
            A.CallTo(() => _mapper.Map<BookSeviceModel>(_dbBook)).Returns(_book);

            // Act
            var actionResult = await _bookController.GetBook(_bookId);
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(200, result.StatusCode);
            A.CallTo(() => _unitOfWork.Books.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Should_Get_Book_By_Id_If_Not_Exist_Return_Not_Found_Reponce()
        {
            BookModel? book = null;
            A.CallTo(() => _unitOfWork.Books.GetById(A<Guid>._)).Returns(book);
            A.CallTo(() => _mapper.Map<BookSeviceModel>(_dbBook)).Returns(_book);

            // Act
            var actionResult = await _bookController.GetBook(_bookId);
            var result = actionResult as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(404, result.StatusCode);
            A.CallTo(() => _unitOfWork.Books.GetById(_bookId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<BookSeviceModel>(_dbBook)).MustNotHaveHappened();
        }

        [Fact]
        public async void Should_Get_All_Books_Return_Ok_Reponce()
        {
            A.CallTo(() => _unitOfWork.Books.GetAll()).Returns(_dbBooks);
            A.CallTo(() => _mapper.Map<List<BookSeviceModel>>(_dbBooks)).Returns(_books);

            // Act
            var actionResult = await _bookController.GetAll();
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(200, result.StatusCode);
            A.CallTo(() => _unitOfWork.Books.GetAll()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Should_Not_Upser_Book_If_Id_Is_Different_Than_BookId_Return_Bad_Reponce()
        {
            // Act
            var actionResult = await _bookController.UpsertBook(_bookId, _book);
            var result = actionResult as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(400, result.StatusCode);
            A.CallTo(() => _unitOfWork.Books.Upsert(_dbBook)).MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.SaveChanges()).MustNotHaveHappened();
        }

        [Fact]
        public async void Should_Upser_Book_If_Id_Is_SameAS_BookId_Return_NoContent_Reponce()
        {
            // Arrange
            _book.Id = _bookId;
            A.CallTo(() => _mapper.Map<BookModel>(_book)).Returns(_dbBook);
            A.CallTo(() => _unitOfWork.Books.Upsert(_dbBook)).Returns(true);

            // Act
            var actionResult = await _bookController.UpsertBook(_bookId, _book);
            var result = actionResult as NoContentResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(204, result.StatusCode);
            A.CallTo(() => _unitOfWork.Books.Upsert(_dbBook)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.SaveChanges()).MustHaveHappenedOnceExactly();
        }
    }
}