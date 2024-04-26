using AutoMapper;
using BuisnesLogic.Dto;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnesLogic.Helpers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Category,ShowCategoryDto>().ReverseMap().ForPath(x=>x.User.Email,opt=>opt.MapFrom(y=>y.User));
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
        }
    }
}
