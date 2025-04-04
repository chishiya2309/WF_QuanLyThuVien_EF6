using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BusinessAccessLayer
{
    public class DBLoanAndReturn
    {
        DAL db = null;
        public DBLoanAndReturn()
        {
            db = new DAL();
        }

        public DataSet LayPhieuMuon()
        {
            return db.ExecuteQueryDataSet(
                "SELECT * From PhieuMuon", CommandType.Text, null);
        }

        public bool MuonSach(ref string err, string MaThanhVien, string MaSach, int SoLuong, DateTime NgayMuon, DateTime HanTra)
        {
            return db.MyExecuteNonQuery("sp_MuonSach",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaThanhVien", MaThanhVien),
                new SqlParameter("@MaSach", MaSach),
                new SqlParameter("@SoLuong", SoLuong),
                new SqlParameter("@NgayMuon", NgayMuon),
                new SqlParameter("@HanTra", HanTra));
        }

        public bool TraSach(ref string err, int MaPhieu, DateTime NgayTra)
        {
            return db.MyExecuteNonQuery("sp_TraSach",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaPhieu", MaPhieu),
                new SqlParameter("@NgayTra", NgayTra));
        }

        public bool SuaPhieuMuon(ref string err, int MaPhieu, DateTime NgayMuon, DateTime HanTra, DateTime? NgayTraThucTe, string TrangThai, int SoLuong)
        {
            SqlParameter ngayTraParam = new SqlParameter("@NgayTraThucTe", SqlDbType.Date);
            if (NgayTraThucTe.HasValue)
                ngayTraParam.Value = NgayTraThucTe.Value;
            else
                ngayTraParam.Value = DBNull.Value;
            return db.MyExecuteNonQuery("sp_SuaPhieuMuon",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaPhieu", MaPhieu),
                new SqlParameter("@NgayMuon", NgayMuon),
                new SqlParameter("@HanTra", HanTra),
                ngayTraParam,
                new SqlParameter("@TrangThai", TrangThai),
                new SqlParameter("@SoLuong", SoLuong));
        }

        public bool XoaPhieuMuon(ref string err, int MaPhieu)
        {
            return db.MyExecuteNonQuery("sp_XoaPhieuMuon",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaPhieu", MaPhieu));
        }

        public (int TongSoPhieu, int SoPhieuDungHan, int SoPhieuQuaHan) ThongKeTraSach()
        {
            DataSet dsPhieuMuon = LayPhieuMuon();
            if (dsPhieuMuon == null || dsPhieuMuon.Tables.Count == 0 || dsPhieuMuon.Tables[0].Rows.Count == 0)
                return (0, 0, 0);

            DataTable dtPhieuMuon = dsPhieuMuon.Tables[0];

            int tongSoPhieu = dtPhieuMuon.AsEnumerable()
                .Count(pm => pm.Field<string>("TrangThai") != "Đang mượn");

            int soPhieuDungHan = dtPhieuMuon.AsEnumerable()
                .Count(pm => pm.Field<string>("TrangThai") == "Đã trả"
                    && pm.Field<DateTime>("NgayTraThucTe") <= pm.Field<DateTime>("HanTra"));

            int soPhieuQuaHan = dtPhieuMuon.AsEnumerable()
                .Count(pm => pm.Field<string>("TrangThai") == "Quá hạn"
                    || (pm.Field<string>("TrangThai") == "Đã trả"
                    && pm.Field<DateTime>("NgayTraThucTe") > pm.Field<DateTime>("HanTra")));

            return (tongSoPhieu, soPhieuDungHan, soPhieuQuaHan);
        }



        public DataTable LayPhieuMuonQuaHan()
        {
            string query = "SELECT * FROM PhieuMuon WHERE TrangThai = N'Quá hạn'";

            DataSet ds = db.ExecuteQueryDataSet(query, CommandType.Text);
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }


        public DataTable LayLichSuMuonTheoThanhVien(string memberId)
        {
            DataSet ds = LayPhieuMuon();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            DataTable dtPhieuMuon = ds.Tables[0];

            var memberLoanHistory = dtPhieuMuon.AsEnumerable()
                .Where(row => row.Field<string>("MaThanhVien") == memberId)
                .OrderByDescending(row => row.Field<DateTime>("NgayMuon"));

            return memberLoanHistory.Any() ? memberLoanHistory.CopyToDataTable() : dtPhieuMuon.Clone();
        }

        public DataTable LayTop5ThanhVienMuonNhieuNhat()
        {
            DataSet dsPhieuMuon = LayPhieuMuon();
            DataSet dsThanhVien = new DBMembers().LayThanhVien();

            if (dsPhieuMuon == null || dsPhieuMuon.Tables.Count == 0 || dsPhieuMuon.Tables[0].Rows.Count == 0)
                return null;

            if (dsThanhVien == null || dsThanhVien.Tables.Count == 0 || dsThanhVien.Tables[0].Rows.Count == 0)
                return null;

            DataTable dtPhieuMuon = dsPhieuMuon.Tables[0];
            DataTable dtThanhVien = dsThanhVien.Tables[0];

            var topBorrowers = dtPhieuMuon.AsEnumerable()
               .Where(pm => pm.Field<string>("TrangThai") == "Đang mượn" || pm.Field<string>("TrangThai") == "Quá hạn")
                .GroupBy(pm => pm.Field<string>("MaThanhVien"))
                .Select(g => new
                {
                    MaThanhVien = g.Key,
                    TongSoSach = g.Sum(pm => Convert.ToInt32(pm["SoLuong"] ?? 0)),
                    HoTen = dtThanhVien.AsEnumerable()
                              .FirstOrDefault(tv => tv.Field<string>("MaThanhVien") == g.Key)?
                              .Field<string>("HoTen") ?? "Không xác định"
                })
                .OrderByDescending(x => x.TongSoSach)
                .Take(5)
                .ToList();

            DataTable dtTopBorrowers = new DataTable();
            dtTopBorrowers.Columns.Add("STT", typeof(int));
            dtTopBorrowers.Columns.Add("MaThanhVien", typeof(string));
            dtTopBorrowers.Columns.Add("HoTen", typeof(string));
            dtTopBorrowers.Columns.Add("TongSoSach", typeof(int));

            int stt = 1;
            foreach (var item in topBorrowers)
            {
                dtTopBorrowers.Rows.Add(stt++, item.MaThanhVien, item.HoTen, item.TongSoSach);
            }

            return dtTopBorrowers;
        }

        public DataTable LayTop10SachPhoBien()
        {
            DataSet dsPhieuMuon = LayPhieuMuon();
            DataSet dsSach = new DBBooks().LaySach();

            if (dsPhieuMuon == null || dsPhieuMuon.Tables.Count == 0 || dsPhieuMuon.Tables[0].Rows.Count == 0)
                return null;

            if (dsSach == null || dsSach.Tables.Count == 0 || dsSach.Tables[0].Rows.Count == 0)
                return null;

            DataTable dtPhieuMuon = dsPhieuMuon.Tables[0];
            DataTable dtSach = dsSach.Tables[0];

            var popularBooks = dtPhieuMuon.AsEnumerable()
                .GroupBy(pm => pm.Field<string>("MaSach"))
                .Select(g => new
                {
                    MaSach = g.Key,
                    SoLanMuon = g.Count(),
                    TenSach = dtSach.AsEnumerable()
                                .FirstOrDefault(s => s.Field<string>("MaSach") == g.Key)?
                                .Field<string>("TenSach") ?? "Không xác định"
                })
                .OrderByDescending(x => x.SoLanMuon)
                .Take(10)
                .ToList();

            DataTable dtPopularBooks = new DataTable();
            dtPopularBooks.Columns.Add("STT", typeof(int));
            dtPopularBooks.Columns.Add("MaSach", typeof(string));
            dtPopularBooks.Columns.Add("TenSach", typeof(string));
            dtPopularBooks.Columns.Add("SoLanMuon", typeof(int));

            int stt = 1;
            foreach (var item in popularBooks)
            {
                dtPopularBooks.Rows.Add(stt++, item.MaSach, item.TenSach, item.SoLanMuon);
            }

            return dtPopularBooks;
        }

        public DataTable ThongKeSachMuonTheoThang(int? selectedYear = null)
        {
            DataSet dsPhieuMuon = LayPhieuMuon();
            if (dsPhieuMuon == null || dsPhieuMuon.Tables.Count == 0 || dsPhieuMuon.Tables[0].Rows.Count == 0)
                return null;

            DataTable dtPhieuMuon = dsPhieuMuon.Tables[0];

            var loansByMonth = dtPhieuMuon.AsEnumerable()
                .GroupBy(pm => new
                {
                    Thang = pm.Field<DateTime>("NgayMuon").Month,
                    Nam = pm.Field<DateTime>("NgayMuon").Year
                })
                .Select(g => new
                {
                    Thang = g.Key.Thang,
                    Nam = g.Key.Nam,
                    SoLuongMuon = g.Sum(pm => pm.Field<int>("SoLuong"))
                })
                .OrderBy(x => x.Thang)
                .ToList();

            if (selectedYear.HasValue)
            {
                loansByMonth = loansByMonth.Where(x => x.Nam == selectedYear.Value).ToList();
            }

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("ThangNam", typeof(string));
            dtResult.Columns.Add("SoLuongMuon", typeof(int));

            foreach (var item in loansByMonth)
            {
                dtResult.Rows.Add($"{item.Thang}/{item.Nam}", item.SoLuongMuon);
            }

            return dtResult;
        }

        public List<int> LayDanhSachNam()
        {
            DataSet dsPhieuMuon = LayPhieuMuon();
            if (dsPhieuMuon == null || dsPhieuMuon.Tables.Count == 0 || dsPhieuMuon.Tables[0].Rows.Count == 0)
                return new List<int>();

            DataTable dtPhieuMuon = dsPhieuMuon.Tables[0];

            return dtPhieuMuon.AsEnumerable()
                .Select(pm => pm.Field<DateTime>("NgayMuon").Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();
        }
    }
}
