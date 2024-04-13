using BuisnesLogic.Dto;
using BuisnesLogic.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnesLogic.Sevices
{
    public class AccountService : IAccountServices
    {
        private readonly IWorkWithData _workWithData;
        private readonly UserManager<IdentityUser> userManager;
        public AccountService(IWorkWithData _workWithData,UserManager<IdentityUser> userManager)
        {
            this._workWithData = _workWithData;
            this.userManager = userManager;
        }
        public Task Login()
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task Register(RegisterDto register)
        {
            IdentityUser userReg = new IdentityUser() { UserName=register.Email,Email = register.Email };
            var result = await userManager.CreateAsync(userReg, register.Password);
        }
    }
}
