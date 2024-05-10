using BuisnesLogic.Constants;
using BuisnesLogic.Dto;
using BuisnesLogic.Interfaces;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Server.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IWorkWithData workWithData;
        private readonly UserManager<UserEntity> _userManager;

        public CategoriesController(IWorkWithData workWithData, UserManager<UserEntity> userManager)
        {
            this.workWithData = workWithData;
            this._userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var email = User.Claims.FirstOrDefault().Value;
            var user = await _userManager.FindByEmailAsync(email);

            var list = await workWithData.Get(user);
                Console.WriteLine("--- New connect ---");
                return Ok(list);

        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateCategoryDto model)
        {
            var email = User.Claims.FirstOrDefault().Value;
            var user = await _userManager.FindByEmailAsync(email);
            var newCategory = await workWithData.Create(model, user.Id);
            return Ok(newCategory);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateCategoryDto model)
        {
            //var email = User.Claims.FirstOrDefault().Value;
            //var user = await _userManager.FindByEmailAsync(email);
            //if (await _userManager.IsInRoleAsync(user, Roles.Admin))
            //{
                var updateCategory = await workWithData.Update(model);
            return Ok(updateCategory);
            //}
            //return BadRequest("You have no right!");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            //var email = User.Claims.FirstOrDefault().Value;
            //var user = await _userManager.FindByEmailAsync(email);
            //if (await _userManager.IsInRoleAsync(user, Roles.Admin))
            //{
                workWithData.Delete(id);
                return Ok();
            //}
            //return BadRequest("You have no right!");
        }
    }
}
