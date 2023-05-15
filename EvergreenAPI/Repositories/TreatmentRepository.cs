using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly AppDbContext _context;

        public TreatmentRepository(AppDbContext context)
        {
            _context = context;
        }






        public bool CreateTreatment(Treatment treatment)
        {
            _context.Add(treatment);
            return Save();
        }







        public bool DeleteTreatment(Treatment treatment)
        {
            _context.Remove(treatment);
            return Save();
        }






        public bool TreatmentExist(int id)
        {
            return _context.Treatments.Any(f => f.TreatmentId == id);
        }







        public Treatment GetTreatment(int id)
        {
            
            return _context.Treatments.Include(d => d.Thumbnail).FirstOrDefault(s => s.TreatmentId == id);
        }






        public ICollection<Thumbnail> GetThumbnails()
        {
            return _context.Thumbnails.ToList();
        }






        public ICollection<Treatment> GetTreatments()
        {
            return _context.Treatments.Include(d => d.Thumbnail).ToList();
        }






        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }






        public bool UpdateTreatment(Treatment treatment)
        {
            _context.Update(treatment);
            return Save();
        }







        public List<Treatment> Search(string search)
        {
            List<Treatment> d = new List<Treatment>();
            try
            {
                d = _context.Treatments.Where(d => d.TreatmentName.ToLower().Contains(search.ToLower())).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return d;
        }
    }
}
