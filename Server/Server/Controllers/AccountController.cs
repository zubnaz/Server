using BuisnesLogic.Constants;
using BuisnesLogic.Dto;
using BuisnesLogic.Helpers;
using BuisnesLogic.Interfaces;
using BuisnesLogic.Sevices;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //private readonly IAccountServices accountServices;
        public AccountController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService,IImageSaving imageSaving,IWorkWithData workWithData) {
        //this.accountServices = accountServices;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            this.imageSaving = imageSaving;
            this.workWithData = workWithData;
        }
        
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IImageSaving imageSaving;
        private readonly IWorkWithData workWithData;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]RegisterDto register)
        {
            //await accountServices.Register(register);
            await workWithData.Resgister(register);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest();
            }
            var isAuth = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isAuth)
            {
                return BadRequest();
            }
            var token = await _jwtTokenService.CreateToken(user);
            return Ok(new { token });
        }
    }
}
