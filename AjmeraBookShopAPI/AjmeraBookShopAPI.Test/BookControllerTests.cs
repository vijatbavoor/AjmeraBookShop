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
        private readonly BookSeviceModel _newBook;
        private readonly BookModel _book;
        private readonly BookController _bookController;
        private readonly Guid _bookId;
        private readonly List<BookSeviceModel> _books;
        private readonly List<BookModel> _dbBooks;

        public BookControllerTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _logger = A.Fake<ILogger<BookController>>();
            _mapper = A.Fake<IMapper>();

            _newBook = A.Fake<BookSeviceModel>();
            _book = A.Fake<BookModel>();
            _bookId = new Guid();
            _books = A.Fake<List<BookSeviceModel>>();

            _bookController = new BookController(_logger, _unitOfWork, _mapper);
        }

        [Fact]
        public async void Should_AddBook_If_Same_BookName_With_SameAuthore_Not_Exist_Return_Created_Responce()
        {

            // Arrange
            A.CallTo(() => _mapper.Map<BookModel>(_newBook)).Returns(_book);
            A.CallTo(() => _unitOfWork.Books.IsBookExistByName(_newBook)).Returns(false);
            A.CallTo(() => _unitOfWork.Books.Add(_book)).Returns(true);

            // Act
            var actionResult = await _bookController.AddBook(_newBook);
            var result = actionResult as CreatedAtActionResult;
            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(201, result.StatusCode);
            A.CallTo(() => _unitOfWork.SaveChanges()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.Books.Add(_book)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Should_Not_AddBook_If_Same_BookName_With_SameAuthore_Exist_Return_BookExist_Responce()
        {
            // Arrange
            A.CallTo(() => _mapper.Map<BookModel>(_newBook)).Returns(_book);
            A.CallTo(() => _unitOfWork.Books.IsBookExistByName(_newBook)).Returns(true);

            // Act
            var actionResult = await _bookController.AddBook(_newBook);
            var result = actionResult as ObjectResult;
            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(422, result.StatusCode);
            A.CallTo(() => _unitOfWork.Books.IsBookExistByName(_newBook)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.SaveChanges()).MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.Books.Add(_book)).MustNotHaveHappened();
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
            A.CallTo(() => _unitOfWork.Books.Add(_book)).MustNotHaveHappened();
        }

        [Fact]
        public async void Should_Get_Book_By_Id_If_Exist_Return_Ok_Reponce()
        {
            // Arrange
            A.CallTo(() => _unitOfWork.Books.GetById(A<Guid>._)).Returns(_book);
            A.CallTo(() => _mapper.Map<BookSeviceModel>(_book)).Returns(_newBook);

            // Act
            var actionResult = await _bookController.GetBook(_bookId);
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(200, result.StatusCode);
            A.CallTo(() => _unitOfWork.Books.GetById(A<Guid>._)).MustHaveHappenedOnceExactly();
        }

        //[Fact]
        //public async void Should_Get_Book_By_Id_If_Not_Exist_Return_Not_Found_Reponce()
        //{
        //    A.CallTo(() => _unitOfWork.Books.GetById(A<Guid>._)).Returns(null);
        //    A.CallTo(() => _mapper.Map<BookSeviceModel>(_book)).Returns(_newBook);

        //    // Act
        //    var actionResult = await _bookController.GetBook(_bookId);
        //    var result = actionResult as OkObjectResult;

        //}
        
    }
}