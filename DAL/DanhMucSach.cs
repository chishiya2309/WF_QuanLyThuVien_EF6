using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DanhMucSach
    {
        public string MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTa { get; set; }
        public string DanhMucCha { get; set; }
        public int SoLuongSach { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime CapNhatLanCuoi { get; set; }
        public string TrangThai { get; set; }

        public virtual ICollection<Sach> Sachs { get; set; } //Quan hệ 1 - n
    }

}
