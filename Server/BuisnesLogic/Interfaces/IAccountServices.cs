using BuisnesLogic.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnesLogic.Interfaces
{
    public interface IAccountServices
    {
        Task Register(RegisterDto register);
        Task Login();
        Task Logout();
    }
}
