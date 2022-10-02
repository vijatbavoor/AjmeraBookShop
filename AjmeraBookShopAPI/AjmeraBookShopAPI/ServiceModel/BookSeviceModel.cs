using System.ComponentModel.DataAnnotations;

namespace AjmeraBookShopAPI.ServiceModel
{
    public class BookSeviceModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AuthorName { get; set; }
    }
}
