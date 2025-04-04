using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class DBThongKe
    {
        DAL db = null;
        public DBThongKe()
        {
            db = new DAL();
        }

        public DataRow LayThongKeTongQuan()
        {
            string query = @"
        SELECT 
            (SELECT SUM(KhaDung) FROM Sach) AS TongSachKhaDung,
            (SELECT COUNT(*) FROM ThanhVien) AS TongThanhVien,
            (SELECT COUNT(*) FROM NhanVien) AS TongNhanVien,
            (SELECT SUM(SoLuong) FROM PhieuMuon WHERE CONVERT(date, NgayMuon) = CONVERT(date, GETDATE())) AS SachMuonHomNay,
            (SELECT SUM(SoLuong) FROM PhieuMuon WHERE TrangThai = N'Đã trả' AND CONVERT(date, NgayTraThucTe) = CONVERT(date, GETDATE())) AS SachTraHomNay,
            (SELECT SUM(SoLuong) FROM PhieuMuon WHERE TrangThai = N'Quá hạn') AS SachQuaHan";

            DataSet ds = db.ExecuteQueryDataSet(query, CommandType.Text);

            // Tạo bảng với các cột cần thiết
            DataTable dt = new DataTable();
            dt.Columns.Add("TongSachKhaDung", typeof(int));
            dt.Columns.Add("TongThanhVien", typeof(int));
            dt.Columns.Add("TongNhanVien", typeof(int));
            dt.Columns.Add("SachMuonHomNay", typeof(int));
            dt.Columns.Add("SachTraHomNay", typeof(int));
            dt.Columns.Add("SachQuaHan", typeof(int));

            // Tạo một dòng dữ liệu mặc định (toàn bộ giá trị = 0)
            DataRow row = dt.NewRow();
            row["TongSachKhaDung"] = 0;
            row["TongThanhVien"] = 0;
            row["TongNhanVien"] = 0;
            row["SachMuonHomNay"] = 0;
            row["SachTraHomNay"] = 0;
            row["SachQuaHan"] = 0;

            // Nếu có dữ liệu từ database, cập nhật giá trị
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow data = ds.Tables[0].Rows[0];

                row["TongSachKhaDung"] = data.IsNull("TongSachKhaDung") ? 0 : Convert.ToInt32(data["TongSachKhaDung"]);
                row["TongThanhVien"] = data.IsNull("TongThanhVien") ? 0 : Convert.ToInt32(data["TongThanhVien"]);
                row["TongNhanVien"] = data.IsNull("TongNhanVien") ? 0 : Convert.ToInt32(data["TongNhanVien"]);
                row["SachMuonHomNay"] = data.IsNull("SachMuonHomNay") ? 0 : Convert.ToInt32(data["SachMuonHomNay"]);
                row["SachTraHomNay"] = data.IsNull("SachTraHomNay") ? 0 : Convert.ToInt32(data["SachTraHomNay"]);
                row["SachQuaHan"] = data.IsNull("SachQuaHan") ? 0 : Convert.ToInt32(data["SachQuaHan"]);
            }

            dt.Rows.Add(row);
            return row;
        }

    }
}
