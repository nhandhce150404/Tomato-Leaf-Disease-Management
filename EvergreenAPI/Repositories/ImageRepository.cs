using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;

        public ImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateImage(Image image)
        {
            _context.Add(image);
            return Save();
        }

        public bool DeleteImage(Image image)
        {
            _context.Remove(image);
            return Save();
        }

        public bool ImageExist(int id)
        {
            return _context.Images.Any(f => f.ImageId == id);
        }

        public Image GetImage(int id)
        {
            return _context.Images.FirstOrDefault(s => s.ImageId == id); ;
        }

        public ICollection<Image> GetImages()
        {
            return _context.Images.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateImage(Image image)
        {
            _context.Update(image);
            return Save();
        }
    }
}
