using AutoMapper;
using AD.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            //CreateMap<Tag, Tag>();
            //CreateMap<Currency, Currency>();
            //CreateMap<City, City>();

            //CreateMap<Country, Country>()
            //    .ForMember(x => x.Image, option => option.Ignore());
            //CreateMap<Category, Category>()
            //    .ForMember(x => x.Image, option => option.Ignore());
            //CreateMap<Product, Product>()
            //   .ForMember(x => x.Image, option => option.Ignore());
            //CreateMap<Store, Store>()
            //   .ForMember(x => x.Image, option => option.Ignore());
        }
    }
}
