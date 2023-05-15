using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IBlogRepository
    {
        ICollection<Thumbnail> GetThumbnails();
        ICollection<Blog> GetBlogs();
        Blog GetBlog(int id);
        bool BlogExist(int id);
        bool CreateBlog(Blog b);
        bool DeleteBlog(Blog b);
        bool UpdateBlog(Blog b);
        bool Save();
        List<Blog> Search(string search);
        bool UpdateViewBlog(int blogId);
        int GetViewBlog(int blogId);

    }
}
