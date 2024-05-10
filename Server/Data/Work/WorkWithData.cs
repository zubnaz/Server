
using AutoMapper;
using BuisnesLogic.Constants;
using BuisnesLogic.Dto;
using BuisnesLogic.Interfaces;
using Data.DBContext;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.Work
{
    public class WorkWithData : IWorkWithData
    {
        private readonly UserDbContext _userDbContext;
        private readonly IImageSaving imageSaving;
        private readonly IMapper mapper;
        private readonly UserManager<UserEntity> _userManager;

        public WorkWithData(UserDbContext _userDbContext,IImageSaving imageSaving,IMapper mapper,UserManager<UserEntity> _userManager)
        {
            this._userDbContext = _userDbContext;
            this.imageSaving = imageSaving;
            this.mapper = mapper;
            this._userManager = _userManager;
        }

        public async Task<List<ShowCategoryDto>> Get(UserEntity user)
        {
            List<ShowCategoryDto> categories;
            if (await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                categories = mapper.Map<List<ShowCategoryDto>>(_userDbContext.Categories.Include(x => x.User).ToList());
            }
            else
            {
                categories = mapper.Map<List<ShowCategoryDto>>(_userDbContext.Categories.Include(x => x.User).Where(x=>x.UserId==user.Id).ToList());
            }
            return categories;
        }

        public async Task<ShowCategoryDto> Create(CreateCategoryDto model, long user)
        {
            var newCategory = mapper.Map<Category>(model);
            newCategory.UserId = user;
            newCategory.Image = await imageSaving.Save(model.Image);
            _userDbContext.Categories.Add(newCategory);
            _userDbContext.SaveChanges();
            return mapper.Map<ShowCategoryDto>(newCategory);
        }

        public async Task<ShowCategoryDto> Update(UpdateCategoryDto model)
        {
            var category = _userDbContext.Categories.FirstOrDefault(m => m.Id == model.Id);
            if(category == null) { return null; }
            
            if(!String.IsNullOrEmpty(category.Image)) { imageSaving.Delete(category.Image); }
            category.Name=String.IsNullOrEmpty(model.Name)? category.Name:model.Name;
            category.Description = String.IsNullOrEmpty(model.Description) ? category.Description : model.Description;
            if (model.Image != null)
            {
                category.Image =await imageSaving.Save(model.Image);
            }
            _userDbContext.Update(category);
            _userDbContext.SaveChanges();
            return mapper.Map<ShowCategoryDto>(category);
        }
        public void Delete(int id)
        {
            var category = _userDbContext.Categories.FirstOrDefault(m => m.Id == id);
            if (category != null) {
                if (!String.IsNullOrEmpty(category.Image)) { imageSaving.Delete(category.Image); }
                _userDbContext.Remove(category);
                _userDbContext.SaveChanges();
            }
        }

        public async Task Register(RegisterDto register)
        {
            UserEntity newUser = new UserEntity()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Email,
                Image = await imageSaving.Save(register.Image)
            };
            
            var result = await _userManager.CreateAsync(newUser,register.Password);
            await _userManager.AddToRoleAsync(newUser, Roles.User);
            Console.WriteLine("Errors :");
            foreach (var item in result.Errors.ToList())
            {
                Console.WriteLine(item.Code+" - "+item.Description);
            }
        }
        public async Task<UserDto> GetInfoAboutUser(UserEntity user)
        {
            try
            {
                UserDto userDto = new UserDto()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Image = user.Image,
                    Role = _userDbContext.UserRoles.Include(x => x.Role).FirstOrDefault(x => x.UserId == user.Id).Role.Name
                };
                return userDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
           
        }
    }
}
