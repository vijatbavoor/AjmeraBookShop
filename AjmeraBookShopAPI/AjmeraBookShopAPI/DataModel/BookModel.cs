using System.ComponentModel.DataAnnotations;

namespace AjmeraBookShopAPI.DataModel
{
    public class BookModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AuthorName { get; set; }
    }
}
