using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context;
        } 

        public bool CreateBlog(Blog blog)
        {
            _context.Add(blog);
            return Save();
        }

        public bool DeleteBlog(Blog blog)
        {
            _context.Remove(blog);
            return Save();
        }

        public bool BlogExist(int id)
        {
            return _context.Blogs.Any(f => f.BlogId == id);
        }

        public Blog GetBlog(int id)
        {
            return _context.Blogs.Include(d => d.Thumbnail).FirstOrDefault(s => s.BlogId == id); ;
        }

        public ICollection<Thumbnail> GetThumbnails()
        {
            return _context.Thumbnails.ToList();
        }

        public ICollection<Blog> GetBlogs()
        {

            return _context.Blogs.Include(d => d.Thumbnail).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateBlog(Blog blog)
        {
            _context.Update(blog);
            return Save();
        }




        public bool UpdateViewBlog(int blogId)
        {
            var blog = _context.Blogs.SingleOrDefault(b => b.BlogId == blogId);
            blog.ViewCount++;
            _context.Update(blog);
            return Save();
        }



        public int GetViewBlog(int blogId)
        {
            var blog = _context.Blogs.SingleOrDefault(b => b.BlogId == blogId);
            return blog.ViewCount;
           
        }


        public List<Blog> Search(string search)
        {
            List<Blog> d = new List<Blog>();
            try
            {
                d = _context.Blogs.Where(d => d.Title.ToLower().Contains(search.ToLower())).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return d;
        }

    }
}
