using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnesLogic.Dto
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
