using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessAccessLayer
{
    public class DBStaff
    {
        DAL db = null;
        public DBStaff()
        {
            db = new DAL();
        }

        // Lấy danh sách tất cả nhân viên
        public DataSet LayNhanVien()
        {
            return db.ExecuteQueryDataSet(
               "SELECT * FROM NhanVien", CommandType.Text, null);
        }

        public bool ThemNhanVien(ref string err, string ID, string HoTen, string GioiTinh, string ChucVu,
             string Email, string SoDienThoai, DateTime NgayVaoLam, string TrangThai)
        {
            return db.MyExecuteNonQuery("sp_ThemNhanVien",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@ID", ID),
                new SqlParameter("@HoTen", HoTen),
                new SqlParameter("@GioiTinh", GioiTinh),
                new SqlParameter("@ChucVu", ChucVu),
                new SqlParameter("@Email", Email),
                new SqlParameter("@SoDienThoai", SoDienThoai),
                new SqlParameter("@NgayVaoLam", NgayVaoLam),
                new SqlParameter("@TrangThai", TrangThai));
        }

        public bool SuaNhanVien(ref string err, string ID, string HoTen, string GioiTinh, string ChucVu,
             string Email, string SoDienThoai, DateTime NgayVaoLam, string TrangThai)
        {
            return db.MyExecuteNonQuery("sp_SuaNhanVien",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@ID", ID),
                new SqlParameter("@HoTen", HoTen),
                new SqlParameter("@GioiTinh", GioiTinh),
                new SqlParameter("@ChucVu", ChucVu),
                new SqlParameter("@Email", Email),
                new SqlParameter("@SoDienThoai", SoDienThoai),
                new SqlParameter("@NgayVaoLam", NgayVaoLam),
                new SqlParameter("@TrangThai", TrangThai));
        }

        public bool XoaNhanVien(ref string err, string ID)
        {
            return db.MyExecuteNonQuery("sp_XoaNhanVien",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@ID", ID));
        }
    }
}
