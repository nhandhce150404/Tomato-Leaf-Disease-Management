using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class ThumbnailRepository : IThumbnailRepository
    {
        private readonly AppDbContext _context;

        public ThumbnailRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateThumbnail(Thumbnail thumbnail)
        {
            _context.Add(thumbnail);
            return Save();
        }

        public bool DeleteThumbnail(Thumbnail thumbnail)
        {
            _context.Remove(thumbnail);
            return Save();
        }

        public bool ThumbnailExist(int id)
        {
            return _context.Thumbnails.Any(f => f.ThumbnailId == id);
        }

        public Thumbnail GetThumbnail(int id)
        {
            return _context.Thumbnails.FirstOrDefault(s => s.ThumbnailId == id); ;
        }

        public ICollection<Thumbnail> GetThumbnails()
        {
            return _context.Thumbnails.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateThumbnail(Thumbnail thumbnail)
        {
            _context.Update(thumbnail);
            return Save();
        }
    }
}
