using BuisnesLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnesLogic.Helpers
{
    public class ImageSaving : IImageSaving
    {
        private readonly IOptions<Root> storage;

        public ImageSaving(IOptions<Root> storage)
        {
            this.storage = storage;
        }
        public void Delete(string name)
        {
           if(File.Exists(Path.Combine(storage.Value.root, name)))
            {
                File.Delete(Path.Combine(storage.Value.root, name));
            }
        }

        public string Save(IFormFile file)
        {
            string name = Guid.NewGuid().ToString();
            string extension = ".jpg";
            string fullName = name+extension;

            string fullPath = Path.Combine(storage.Value.root, fullName);

            using(FileStream fs = new FileStream(fullPath,FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return fullName;
        }
    }
}
