using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class SachContext : DbContext
    {
        public SachContext() : base("SachDbConnection") { }

        public DbSet<DanhMucSach> DanhMucSachs { get; set; }
        public DbSet<Sach> Sachs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Cấu hình danh mục sách
            var danhMucSachConfig = modelBuilder.Entity<DanhMucSach>();
            danhMucSachConfig.HasKey(dm => dm.MaDanhMuc); //Khóa chính
            danhMucSachConfig.Property(dm => dm.MaDanhMuc)
                .HasMaxLength(20); //Mã danh mục sách không được null và có độ dài tối đa 20 ký tự

            danhMucSachConfig.Property(dm => dm.TenDanhMuc)
                .IsRequired() //Tên danh mục sách không được null
                .HasMaxLength(255); //Độ dài tối đa 255 ký tự

            danhMucSachConfig.Property(dm => dm.MoTa)
                .HasMaxLength(500); //Độ dài tối đa 500 ký tự

            danhMucSachConfig.Property(dm => dm.DanhMucCha)
                .HasMaxLength(20); //Mã danh mục cha có độ dài tối đa 20 ký tự

            danhMucSachConfig.Property(dm => dm.SoLuongSach)
                .IsRequired() // Số lượng sách không được null
                .HasColumnAnnotation("Default", 0) // Giá trị mặc định là 0
                .HasColumnAnnotation("Check", "SoLuongSach >= 0"); // Kiểm tra số lượng sách không âm

            danhMucSachConfig.Property(dm => dm.NgayTao)
                .IsRequired() // Ngày tạo không được null
                .HasColumnAnnotation("Default", DateTime.Now); // Giá trị mặc định là ngày hiện tại

            danhMucSachConfig.Property(dm => dm.CapNhatLanCuoi)
                .IsRequired() // Ngày cập nhật không được null
                .HasColumnAnnotation("Default", DateTime.Now); // Giá trị mặc định là ngày hiện tại

            danhMucSachConfig.Property(dm => dm.TrangThai)
                .IsRequired() // Trạng thái không được null
                .HasMaxLength(20) // Độ dài tối đa 20 ký tự
                .HasColumnAnnotation("Default", "Hoạt động") // Giá trị mặc định là "Active"
                .HasColumnAnnotation("Check", "TrangThai IN ('Hoạt động', 'Ngừng hoạt động')"); // Kiểm tra trạng thái hợp lệ




        }


    }
}
