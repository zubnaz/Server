﻿using BuisnesLogic.Dto;
using BuisnesLogic.Interfaces;
using Data.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly UserDbContext _userDbContext;
        private readonly IWorkWithData workWithData;

        public CategoriesController(UserDbContext _userDbContext,IWorkWithData workWithData)
        {
            this._userDbContext = _userDbContext;
            this.workWithData = workWithData;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var list = _userDbContext.Categories.ToList();
            Console.WriteLine("--- New connect ---");
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto model)
        {
            var newCategory = await workWithData.Create(model);
            return Ok(newCategory);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto model)
        {
            var updateCategory = await workWithData.Update(model);
            return Ok(updateCategory);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            workWithData.Delete(id);
            return Ok();
        }
    }
}
