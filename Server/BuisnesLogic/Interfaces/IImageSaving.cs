using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnesLogic.Interfaces
{
    public interface IImageSaving
    {
        Task<string> Save(IFormFile file);
        void Delete(string name);
    }
}
