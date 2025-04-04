using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Collections.Generic;
using System.Linq;


namespace BusinessAccessLayer
{
    public class DanhMucSachService
    {
        private readonly SachContext _context;

        public DanhMucSachService()
        {
            _context = new SachContext();
        }


        public List<DanhMucSach> LayTatCaDanhMucSach()
        {
            return _context.DanhMucSachs.ToList();
        }

        public void ThemDanhMucSach(DanhMucSach danhMucSachObj)
        {
            _context.DanhMucSachs.Add(danhMucSachObj);
            _context.SaveChanges();
        }
    }
}
