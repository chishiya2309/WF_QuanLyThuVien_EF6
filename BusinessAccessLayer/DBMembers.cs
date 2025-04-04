using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessAccessLayer
{
    public class DBMembers
    {
        DAL db = null;

        public DBMembers()
        {
            db = new DAL();
        }

        //CRUD cho danh muc
        public DataSet LayThanhVien()
        {
            return db.ExecuteQueryDataSet(
                "SELECT * FROM ThanhVien", CommandType.Text, null);
        }

        public bool ThemThanhVien(ref string err, string MaThanhVien, string HoTen, string GioiTinh,
             string SoDienThoai, string Email, string LoaiThanhVien, DateTime NgayDangKy,
             DateTime NgayHetHan, string TrangThai)
        {
            return db.MyExecuteNonQuery("sp_ThemThanhVien",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaThanhVien", MaThanhVien),
                new SqlParameter("@HoTen", HoTen),
                new SqlParameter("@GioiTinh", GioiTinh),
                new SqlParameter("@SoDienThoai", SoDienThoai),
                new SqlParameter("@Email", Email),
                new SqlParameter("@LoaiThanhVien", LoaiThanhVien),
                new SqlParameter("@NgayDangKy", NgayDangKy),
                new SqlParameter("@NgayHetHan", NgayHetHan),
                new SqlParameter("@TrangThai", TrangThai));
        }

        public bool SuaThanhVien(ref string err, string MaThanhVien, string HoTen, string GioiTinh,
            string SoDienThoai, string Email, string LoaiThanhVien, DateTime NgayDangKy,
            DateTime NgayHetHan, string TrangThai)
        {
            return db.MyExecuteNonQuery("sp_SuaThanhVien",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaThanhVien", MaThanhVien),
                new SqlParameter("@HoTen", HoTen),
                new SqlParameter("@GioiTinh", GioiTinh),
                new SqlParameter("@SoDienThoai", SoDienThoai),
                new SqlParameter("@Email", Email),
                new SqlParameter("@LoaiThanhVien", LoaiThanhVien),
                new SqlParameter("@NgayDangKy", NgayDangKy),
                new SqlParameter("@NgayHetHan", NgayHetHan),
                new SqlParameter("@TrangThai", TrangThai));
        }

        public bool XoaThanhVien(ref string err, string MaThanhVien)
        {
            return db.MyExecuteNonQuery("sp_XoaThanhVien",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaThanhVien", MaThanhVien));
        }

        public DataTable LayThanhVienSapHetHan(int days)
        {
            string query = $@"
                SELECT * FROM ThanhVien
                WHERE NgayHetHan < DATEADD(DAY, {days}, GETDATE())
                AND TrangThai = N'Hoạt động'";

            DataSet ds = db.ExecuteQueryDataSet(query, CommandType.Text);
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        public DataTable ThongKeThanhVienTheoLoai()
        {
            DataSet dsThanhVien = LayThanhVien();
            if (dsThanhVien == null || dsThanhVien.Tables.Count == 0 || dsThanhVien.Tables[0].Rows.Count == 0)
                return null;

            DataTable dtThanhVien = dsThanhVien.Tables[0]; // Lấy bảng chính
            int totalMembers = dtThanhVien.Rows.Count;

            var membersByType = dtThanhVien.AsEnumerable()
                .GroupBy(row => row.Field<string>("LoaiThanhVien"))
                .Select(g => new
                {
                    LoaiThanhVien = g.Key,
                    SoLuong = g.Count(),
                    TiLe = totalMembers > 0 ? (g.Count() * 100.0 / totalMembers) : 0.0
                }).ToList();

            // Tạo bảng kết quả
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("LoaiThanhVien", typeof(string));
            dtResult.Columns.Add("SoLuong", typeof(int));
            dtResult.Columns.Add("TiLe", typeof(double));

            foreach (var item in membersByType)
            {
                dtResult.Rows.Add(item.LoaiThanhVien, item.SoLuong, item.TiLe);
            }

            return dtResult;
        }
    }
}
