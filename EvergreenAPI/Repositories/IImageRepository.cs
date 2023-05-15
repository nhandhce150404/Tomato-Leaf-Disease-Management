using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IImageRepository
    {
        ICollection<Image> GetImages();
        Image GetImage(int id);
        bool ImageExist(int id);
        bool CreateImage(Image image);
        bool UpdateImage(Image image);
        bool DeleteImage(Image image);
        bool Save();
    }
}
