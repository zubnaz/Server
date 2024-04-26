
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

        public async Task<List<ShowCategoryDto>> Get()
        {
            var categories = mapper.Map<List<ShowCategoryDto>>(_userDbContext.Categories.Include(x=>x.User).ToList());
            foreach (var item in _userDbContext.Categories.Include(x=>x.User).ToList())
            {
                Console.WriteLine(item.User.Email);
            }
            return categories;
        }

        public async Task<Category> Create(CreateCategoryDto model)
        {
            var newCategory = mapper.Map<Category>(model);
            newCategory.Image = await imageSaving.Save(model.Image);
            _userDbContext.Categories.Add(newCategory);
            _userDbContext.SaveChanges();
            return newCategory;
        }

        public async Task<Category> Update(UpdateCategoryDto model)
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
            return category;
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

        public async Task Resgister(RegisterDto register)
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
            _userManager.AddToRoleAsync(newUser, Roles.User);
            Console.WriteLine("Errors :");
            foreach (var item in result.Errors.ToList())
            {
                Console.WriteLine(item.Code+" - "+item.Description);
            }
        }
    }
}
