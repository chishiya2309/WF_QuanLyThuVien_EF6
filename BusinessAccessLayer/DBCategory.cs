using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BusinessAccessLayer
{
    public class DBCategory
    {
        DAL db = null;

        public DBCategory()
        {
            db = new DAL();
        }

        //CRUD cho danh muc
        public DataSet LayDanhMuc()
        {
            return db.ExecuteQueryDataSet(
                "SELECT * FROM DanhMucSach", CommandType.Text, null);
        }

        public bool ThemDanhMuc(ref string err, string MaDanhMuc, string TenDanhMuc, string MoTa, string DanhMucCha, int SoLuongSach, string TrangThai)
        {
            return db.MyExecuteNonQuery("sp_ThemDanhMucSach",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaDanhMuc", MaDanhMuc),
                new SqlParameter("@TenDanhMuc", TenDanhMuc),
                new SqlParameter("@MoTa", MoTa),
                new SqlParameter("@DanhMucCha", DanhMucCha),
                new SqlParameter("@SoLuongSach", SoLuongSach),
                new SqlParameter("@TrangThai", TrangThai));
        }

        public bool SuaDanhMuc(ref string err, string MaDanhMuc, string TenDanhMuc, string MoTa, string DanhMucCha, int SoLuongSach, string TrangThai)
        {
            return db.MyExecuteNonQuery("sp_SuaDanhMucSach",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaDanhMuc", MaDanhMuc),
                new SqlParameter("@TenDanhMuc", TenDanhMuc),
                new SqlParameter("@MoTa", MoTa),
                new SqlParameter("@DanhMucCha", DanhMucCha),
                new SqlParameter("@SoLuongSach", SoLuongSach),
                new SqlParameter("@TrangThai", TrangThai));
        }

        public bool XoaDanhMuc(ref string err, string MaDanhMuc)
        {
            return db.MyExecuteNonQuery("sp_XoaDanhMucSach",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaDanhMuc", MaDanhMuc));
        }
    }
}
