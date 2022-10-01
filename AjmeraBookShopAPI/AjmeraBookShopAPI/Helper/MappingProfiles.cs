using AjmeraBookShopAPI.DataModel;
using AjmeraBookShopAPI.ServiceModel;
using AutoMapper;

namespace AjmeraBookShopAPI.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<BookModel, BookSeviceModel>();
            CreateMap<BookSeviceModel, BookModel>();
        }
    }
}
