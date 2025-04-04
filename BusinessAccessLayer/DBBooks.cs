using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessAccessLayer
{
    public class DBBooks
    {
        DAL db = null;
        public DBBooks()
        {
            db = new DAL();
        }

        public DataSet LaySach()
        {
            return db.ExecuteQueryDataSet(
                "SELECT * From Sach", CommandType.Text, null);
        }

        public bool ThemSach(ref string err, string MaSach, string ISBN, string TenSach, string TacGia, string MaDanhMuc, int NamXuatBan, string NXB, int SoBan, string ViTri)
        {
            return db.MyExecuteNonQuery("sp_ThemSach",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaSach", MaSach),
                new SqlParameter("@ISBN", ISBN),
                new SqlParameter("@TenSach", TenSach),
                new SqlParameter("@TacGia", TacGia),
                new SqlParameter("@MaDanhMuc", MaDanhMuc),
                new SqlParameter("@NamXuatBan", NamXuatBan),
                new SqlParameter("@NXB", NXB),
                new SqlParameter("@SoBan", SoBan),
                new SqlParameter("@ViTri", ViTri));
        }

        public bool SuaSach(ref string err, string maSach, string isbn, string tenSach, string tacGia, string maDanhMuc,
                             int namXuatBan, string nxb, int soBan, int khaDung, string viTri)
        {
            return db.MyExecuteNonQuery("sp_SuaSach",
               CommandType.StoredProcedure, ref err,
               new SqlParameter("@MaSach", maSach),
               new SqlParameter("@ISBN", isbn),
               new SqlParameter("@TenSach", tenSach),
               new SqlParameter("@TacGia", tacGia),
               new SqlParameter("@MaDanhMuc", maDanhMuc),
               new SqlParameter("@NamXuatBan", namXuatBan),
               new SqlParameter("@NXB", nxb),
               new SqlParameter("@SoBan", soBan),
               new SqlParameter("@KhaDung", khaDung),
               new SqlParameter("@ViTri", viTri));
        }

        public bool XoaSach(ref string err, string MaSach)
        {
            return db.MyExecuteNonQuery("sp_XoaSach",
              CommandType.StoredProcedure, ref err,
              new SqlParameter("@MaSach", MaSach));
        }

    }
}
