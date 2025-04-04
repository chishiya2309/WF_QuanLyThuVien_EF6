using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class SachService
    {
        private readonly SachContext _context;
        public SachService()
        {
            _context = new SachContext();
        }

        public void ThemSach(Sach sach)
        {
            _context.Sachs.Add(sach);
            _context.SaveChanges();
        }

        
    }
}
