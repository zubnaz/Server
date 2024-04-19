using BuisnesLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
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

        public async Task<string> Save(IFormFile file)
        {
            string name = Guid.NewGuid().ToString();
            string extension = ".jpg";
            string fullName = name+extension;

            string fullPath = Path.Combine(storage.Value.root, fullName);

            using MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);

            using Image image = Image.Load(ms.ToArray());

            image.Mutate(
                x => x.Resize(new ResizeOptions()
                {
                    Size = new Size(1200),
                    Mode = ResizeMode.Max
                })
                );
            using var stream = System.IO.File.Create(fullPath);
            await image.SaveAsJpegAsync(stream);


            return fullName;
        }
    }
}
