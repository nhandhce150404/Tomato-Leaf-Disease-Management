using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IThumbnailRepository
    {
        ICollection<Thumbnail> GetThumbnails();
        Thumbnail GetThumbnail(int id);
        bool ThumbnailExist(int id);
        bool CreateThumbnail(Thumbnail thumbnail);
        bool UpdateThumbnail(Thumbnail thumbnail);
        bool DeleteThumbnail(Thumbnail thumbnail);
        bool Save();
    }
}
