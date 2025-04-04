using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Sach
    {
        public string MaSach { get; set; }
        public string ISBN { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string MaDanhMuc { get; set; }
        public string NhaXuatBan { get; set; }
        public int NamXuatBan { get; set; }
        public string NXB { get; set; }
        public int SoLuong { get; set; }
        public int KhaDung { get; set; }
        public string ViTri { get; set; }

        public virtual DanhMucSach DanhMucSanh { get; set; } 
    }
}
