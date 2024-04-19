
using BuisnesLogic.Dto;
using BuisnesLogic.Interfaces;
using Data.DBContext;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Work
{
    public class WorkWithData:IWorkWithData
    {
        private readonly UserDbContext _userDbContext;
        private readonly IImageSaving imageSaving;

        public WorkWithData(UserDbContext _userDbContext,IImageSaving imageSaving)
        {
            this._userDbContext = _userDbContext;
            this.imageSaving = imageSaving;
        }

        public async Task<Category> Create(CreateCategoryDto model)
        {
            var newCategory = new Category()
            {
                Name = model.Name,
                Description = model.Description,
                Image = await imageSaving.Save(model.Image)
            };
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
    }
}
