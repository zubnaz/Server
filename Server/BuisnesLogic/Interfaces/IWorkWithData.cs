using BuisnesLogic.Dto;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnesLogic.Interfaces
{
    public interface IWorkWithData
    {
        Category Create(CreateCategoryDto model);
        Category Update(UpdateCategoryDto model);
        void Delete(int id);
    }
}
