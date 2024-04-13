using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnesLogic.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> CreateToken(UserEntity user);
    }
}
