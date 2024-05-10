using BuisnesLogic.Dto;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnesLogic.Interfaces
{
    public interface IWorkWithData
    {
        Task Register(RegisterDto register);
        Task<ShowCategoryDto> Create(CreateCategoryDto model, long user);
        Task<ShowCategoryDto> Update(UpdateCategoryDto model);
        Task<List<ShowCategoryDto>> Get(UserEntity user);
        void Delete(int id);
        Task<UserDto> GetInfoAboutUser(UserEntity user);
    }
}
