using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Validation;


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

        public bool ThemDanhMucSach(DanhMucSach danhMucSachObj)
        {
            try
            {
                _context.DanhMucSachs.Add(danhMucSachObj);
                _context.SaveChanges();
                return true;
            }catch(DbEntityValidationException ex)
            {
                // Xử lý lỗi validation
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var validationError in error.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error: {validationError.ErrorMessage}");
                    }
                }
                return false;
            }
        }
    }
}
