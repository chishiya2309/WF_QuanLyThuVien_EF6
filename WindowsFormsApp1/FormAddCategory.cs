using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;
using BusinessAccessLayer;

namespace WindowsFormsApp1
{
    public partial class FormAddCategory: Form
    {
        DBCategory dbCategory;
        DataTable dtDanhMuc;

        public FormAddCategory()
        {
            InitializeComponent();
            dbCategory = new DBCategory();
            LoadParentCatengories();
        }

        private void LoadParentCatengories()
        {
            try
            {
                dtDanhMuc = dbCategory.LayDanhMuc().Tables[0];

                // Tạo một DataTable mới để tránh sửa đổi trực tiếp trên bản gốc
                DataTable dtDanhMucCha = dtDanhMuc.Copy();

                // Thêm tùy chọn "Không có danh mục cha"
                DataRow row = dtDanhMucCha.NewRow();
                row["MaDanhMuc"] = DBNull.Value;
                row["TenDanhMuc"] = "-- Không có danh mục cha --";
                dtDanhMucCha.Rows.InsertAt(row, 0);

                cmbDanhMucCha.DataSource = dtDanhMucCha;
                cmbDanhMucCha.DisplayMember = "TenDanhMuc";
                cmbDanhMucCha.ValueMember = "MaDanhMuc";
                cmbDanhMucCha.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục cha: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!ValidateInputs())
            {
                return;
            }  
            
            try
            {
                string maDanhMuc = txtMaDanhMuc.Text.Trim();
                string tenDanhMuc = txtTenDanhMuc.Text.Trim();
                string moTa = txtMoTa.Text.Trim();

                // Xử lý danh mục cha (có thể là null)
                object selectedValue = cmbDanhMucCha.SelectedValue;
                string danhMucCha = null;

                if (selectedValue != null && selectedValue != DBNull.Value)
                {
                    danhMucCha = selectedValue.ToString();
                }


                int soLuongSach = Convert.ToInt32(txtSoLuongSach.Text.Trim());
                string trangThai = (string)cmbTrangThai.SelectedValue;

                string err = "";
                bool success = dbCategory.ThemDanhMuc(ref err, maDanhMuc, tenDanhMuc, moTa, danhMucCha, soLuongSach, trangThai);

                if (success)
                {
                    MessageBox.Show("Thêm danh mục mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi thêm danh mục: " + err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormAddCategory_Load(object sender, EventArgs e)
        {
            // Khởi tạo các giá trị mặc định
            txtSoLuongSach.Text = "0";
            cmbTrangThai.SelectedIndex = 0; //Cho trạng thái mặc định là Hoạt động
        }

        private bool ValidateInputs()
        {
            // Kiểm tra thông tin bắt buộc
            if (string.IsNullOrWhiteSpace(txtMaDanhMuc.Text))
            {
                MessageBox.Show("Vui lòng nhập mã danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaDanhMuc.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenDanhMuc.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDanhMuc.Focus();
                return false;
            }

            // Kiểm tra định dạng số lượng sách
            if (!int.TryParse(txtSoLuongSach.Text, out int soLuong) || soLuong < 0)
            {
                MessageBox.Show("Số lượng sách phải là số nguyên không âm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongSach.Focus();
                return false;
            }

            // Kiểm tra trùng mã danh mục
            foreach (DataRow row in dtDanhMuc.Rows)
            {
                if (row["MaDanhMuc"] != DBNull.Value && row["MaDanhMuc"].ToString() == txtMaDanhMuc.Text.Trim())
                {
                    MessageBox.Show("Mã danh mục đã tồn tại, vui lòng chọn mã khác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaDanhMuc.Focus();
                    return false;
                }
            }
            return true;
        }
    }
}
