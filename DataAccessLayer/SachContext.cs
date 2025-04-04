using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
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
            danhMucSachConfig.ToTable("DanhMucSach"); //Tên bảng trong cơ sở dữ liệu là "DanhMucSach"

            //Cấu hình sách
            var sachConfig = modelBuilder.Entity<Sach>();
            sachConfig.HasKey(s => s.MaSach); //Khóa chính
            sachConfig.Property(s => s.MaSach)
                .HasMaxLength(20); //Mã sách không được null và có độ dài tối đa 20 ký tự
            sachConfig.Property(s => s.ISBN)
                .IsRequired() //ISBN không được null
                .HasMaxLength(20)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute() { IsUnique = true })); // Đảm bảo ISBN là duy nhất
            sachConfig.Property(s => s.TenSach)
                .IsRequired() //Tên sách không được null
                .HasMaxLength(255); //Độ dài tối đa 255 ký tự
            sachConfig.Property(s => s.TacGia)
                .IsRequired() //Tác giả không được null
                .HasMaxLength(255); //Độ dài tối đa 255 ký tự
            sachConfig.Property(s => s.MaDanhMuc)
                .IsRequired() //Mã danh mục không được null
                .HasMaxLength(20); //Độ dài tối đa 20 ký tự
            sachConfig.Property(s => s.NamXuatBan)
                .IsRequired()
                .HasColumnAnnotation("Check", "NamXuatBan >= 1900 AND NamXuatBan <= YEAR(GETDATE())"); // Kiểm tra năm xuất bản hợp lệ
            sachConfig.Property(s => s.NXB)
                .IsRequired() //Nhà xuất bản không được null
                .HasMaxLength(255); //Độ dài tối đa 255 ký tự
            sachConfig.Property(s => s.SoBan)
                .IsRequired() //Số bản không được null
                .HasColumnAnnotation("Default", 0) // Giá trị mặc định là 0
                .HasColumnAnnotation("Check", "SoBan >= 0"); // Kiểm tra số bản không âm
            sachConfig .Property(s => s.KhaDung)
                .IsRequired() //Khả dụng không được null
                .HasColumnAnnotation("Default", 0) // Giá trị mặc định là 0
                .HasColumnAnnotation("Check", "KhaDung >= 0 AND KhaDung <= SoBan"); // Kiểm tra khả dụng không âm
            sachConfig.Property(s => s.ViTri)
                .IsRequired() //Vị trí không được null
                .HasMaxLength(255); //Độ dài tối đa 255 ký tự

            sachConfig.HasRequired(s => s.DanhMucSanh) //Sách phải có danh mục sách
                .WithMany(dm => dm.Sachs) //Danh mục sách có nhiều sách
                .HasForeignKey(s => s.MaDanhMuc) //Khóa ngoại là mã danh mục sách
                .WillCascadeOnDelete(true); // Thêm hành vi xóa cascade

            sachConfig.ToTable("Sach"); //Tên bảng trong cơ sở dữ liệu là "Sach"


        }


    }
}
