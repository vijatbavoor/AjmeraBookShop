using AjmeraBookShopAPI.Controllers;
using AjmeraBookShopAPI.DataRepository.Interface;
using AutoMapper;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace AjmeraBookShopAPI.Test
{
    public class BookControllerTests
    {
        [Fact]
        public void Test1()
        {

        }

        //[Fact]
        //public void GetReturnsProductWithSameId()
        //{     // Arrange
        //    var mockRepository = A.Fake<IUnitOfWork>();
        //    var mockLoger = A.Fake<ILogger<BookController>>();
        //    var mockMapper = A.Fake<IMapper>();

        //    A.CallTo(() =>  mockRepository.Books.IsBookExistByName())
        //    mockRepository.call(x => x.GetById(42)).Returns(new Product { Id = 42 });
        //    var controller = new BookController(mockLoger, mockRepository, mockMapper);
        //    // Act
        //    IHttpActionResult actionResult = controller.Get(42);
        //    var contentResult = actionResult as OkNegotiatedContentResult<Product>;
        //    // Assert
        //    Assert.IsNotNull(contentResult);
        //    Assert.IsNotNull(contentResult.Content);
        //    Assert.AreEqual(42, contentResult.Content.Id);
        //}
    }
}