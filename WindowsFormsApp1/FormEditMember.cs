using BusinessAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormEditMember: Form
    {
        DBMembers dbMembers;
        string maThanhVien;
        DataTable dtThanhVien;
        DataRow memberData;
        public FormEditMember(string maThanhVien)
        {
            InitializeComponent();
            this.maThanhVien = maThanhVien;
            dbMembers = new DBMembers();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                //Lấy dữ liệu thành viên
                dtThanhVien = dbMembers.LayThanhVien().Tables[0];

                // Tìm danh mục cần chỉnh sửa
                foreach (DataRow r in dtThanhVien.Rows)
                {
                    if (r["MaThanhVien"].ToString() == maThanhVien)
                    {
                        memberData = r;
                        break;
                    }
                }

                if (memberData == null)
                {
                    MessageBox.Show("Không tìm thấy thành viên cần chỉnh sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                txtMaThanhVien.Text = memberData["MaThanhVien"].ToString();
                txtMaThanhVien.ReadOnly = true;
                txtHoTen.Text = memberData["HoTen"].ToString();

                string gioiTinh = memberData["GioiTinh"].ToString();
                cmbGioiTinh.SelectedIndex = (gioiTinh == "Nữ") ? 1 : 0;

                txtSoDienThoai.Text = memberData["SoDienThoai"].ToString();

                txtEmail.Text = memberData["Email"].ToString();

                // Thiết lập loại thành viên
                string loaiThanhVien = memberData["LoaiThanhVien"].ToString();
                if (loaiThanhVien == "Sinh viên")
                {
                    cmbLoaiThanhVien.SelectedIndex = 0;
                }
                else if (loaiThanhVien == "Giảng viên")
                {
                    cmbLoaiThanhVien.SelectedIndex = 1;
                }
                else if (loaiThanhVien == "Thường")
                {
                    cmbLoaiThanhVien.SelectedIndex = 2;
                }    
                    




                if (memberData["NgayDangKy"] != DBNull.Value)
                    dtpNgayDangKy.Value = Convert.ToDateTime(memberData["NgayDangKy"]);

                if (memberData["NgayHetHan"] != DBNull.Value)
                    dtpNgayHetHan.Value = Convert.ToDateTime(memberData["NgayHetHan"]);

                string trangThai = memberData["TrangThai"].ToString();
                switch (trangThai)
                {
                    case "Hoạt động":
                        cmbTrangThai.SelectedIndex = 0;
                        break;
                    case "Hết hạn":
                        cmbTrangThai.SelectedIndex = 1;
                        break;
                    case "Khóa":
                        cmbTrangThai.SelectedIndex = 2;
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin thành viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void FormEditMember_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!ValidateInputs())
            {
                return;
            }

            try
            {
                string maThanhVien = txtMaThanhVien.Text.Trim();
                string hoTen = txtHoTen.Text.Trim();

                string gioiTinh = cmbGioiTinh.SelectedItem.ToString();

                string soDienThoai = txtSoDienThoai.Text.Trim();
                string email = txtEmail.Text.Trim();
                string loaiThanhVien = cmbLoaiThanhVien.SelectedItem.ToString();
                DateTime ngayDangKy = dtpNgayDangKy.Value;
                DateTime ngayHetHan = dtpNgayHetHan.Value;
                string trangThai = cmbTrangThai.SelectedItem.ToString();

                string err = "";
                bool success = dbMembers.SuaThanhVien(ref err, maThanhVien, hoTen, gioiTinh, soDienThoai,
                    email, loaiThanhVien, ngayDangKy, ngayHetHan, trangThai);

                if(success)
                {
                    MessageBox.Show("Cập nhật thông tin thành viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi cập nhật thông tin thành viên: " + err, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }       
            }catch(Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInputs()
        {
            // Kiểm tra họ tên
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên thành viên!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            // Kiểm tra số điện thoại
            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }

            // Kiểm tra định dạng số điện thoại
            string phonePattern = @"^0\d{9,10}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, phonePattern))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại phải bắt đầu bằng số 0 và có 10-11 chữ số.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }

            // Kiểm tra email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ email!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Kiểm tra định dạng email
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, emailPattern))
            {
                MessageBox.Show("Địa chỉ email không hợp lệ!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Kiểm tra ngày hết hạn không được nhỏ hơn ngày đăng ký
            if (dtpNgayHetHan.Value < dtpNgayDangKy.Value)
            {
                MessageBox.Show("Ngày hết hạn không được nhỏ hơn ngày đăng ký!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayHetHan.Focus();
                return false;
            }
            return true;
        }
    }
}
