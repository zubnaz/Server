using BuisnesLogic.Constants;
using BuisnesLogic.Dto;
using BuisnesLogic.Helpers;
using BuisnesLogic.Interfaces;
using BuisnesLogic.Sevices;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
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
        public AccountController(UserManager<UserEntity> userManager,SignInManager<UserEntity> signInManager, IJwtTokenService jwtTokenService,IImageSaving imageSaving,IWorkWithData workWithData) {
        //this.accountServices = accountServices;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._jwtTokenService = jwtTokenService;
            this.imageSaving = imageSaving;
            this.workWithData = workWithData;
        }
        
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IImageSaving imageSaving;
        private readonly IWorkWithData workWithData;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]RegisterDto register)
        {
            //await accountServices.Register(register);
            await workWithData.Register(register);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login( LoginViewModel model)
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
        [HttpGet("get_info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var email = User.Claims.FirstOrDefault().Value;
            Console.WriteLine(email);
            UserEntity user = await _userManager.FindByNameAsync(email);
            UserDto userInfo = new UserDto();
            if (user != null)
            {
                userInfo = await workWithData.GetInfoAboutUser(user);
            }
            return Ok(userInfo);

        }
        [HttpGet("exit")]
        [Authorize]
        public async Task<IActionResult> Exit()
        {
            await _signInManager.SignOutAsync();
            return Ok();

        }
        [HttpGet("check_role")]
        [Authorize]
        public async Task<IActionResult> CheckRole()
        {
            var email = User.Claims.FirstOrDefault().Value;
            var user = await _userManager.FindByEmailAsync(email);
            if(await _userManager.IsInRoleAsync(user,Roles.Admin))
            {
                return Ok(true);
            }
            return Ok(false);

        }
    }
}
