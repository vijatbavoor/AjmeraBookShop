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

        public int Price { get; set; }

        //you can have the relationship model 
    }
}
