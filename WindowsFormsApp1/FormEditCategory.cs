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
using DataAccessLayer;

namespace WindowsFormsApp1
{
    public partial class FormEditCategory: Form
    {
        private DBCategory dbCategory;
        private DataTable dtDanhMuc;
        private string maDanhMuc;
        private DataRow categoryData;
        public FormEditCategory(string maDanhMuc)
        {
            InitializeComponent();
            dbCategory = new DBCategory();
            this.maDanhMuc = maDanhMuc;
            LoadData();
        }

        private void FormEditCategory_Load(object sender, EventArgs e)
        {

        }

        private void LoadData()
        {
            try
            {
                //Lấy dữ liệu danh mục
                dtDanhMuc = dbCategory.LayDanhMuc().Tables[0];

                // Tìm danh mục cần chỉnh sửa
                foreach (DataRow r in dtDanhMuc.Rows)
                {
                    if (r["MaDanhMuc"].ToString() == maDanhMuc)
                    {
                        categoryData = r;
                        break;
                    }
                }

                if (categoryData == null)
                {
                    MessageBox.Show("Không tìm thấy danh mục cần chỉnh sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                // Tạo một DataTable mới cho danh mục cha, tránh sửa đổi trực tiếp trên bản gốc
                DataTable dtDanhMucCha = dtDanhMuc.Copy();

                // Thêm tùy chọn "Không có danh mục cha"
                DataRow row = dtDanhMucCha.NewRow();
                row["MaDanhMuc"] = DBNull.Value;
                row["TenDanhMuc"] = "-- Không có danh mục cha --";
                dtDanhMucCha.Rows.InsertAt(row, 0);

                cmbDanhMucCha.DataSource = dtDanhMucCha;
                cmbDanhMucCha.DisplayMember = "TenDanhMuc";
                cmbDanhMucCha.ValueMember = "MaDanhMuc";

                // Điền dữ liệu vào các control
                txtMaDanhMuc.Text = categoryData["MaDanhMuc"].ToString();
                txtMaDanhMuc.ReadOnly = true; // Không cho phép sửa mã danh mục
                txtTenDanhMuc.Text = categoryData["TenDanhMuc"].ToString();
                txtMoTa.Text = categoryData["MoTa"] != DBNull.Value ? categoryData["MoTa"].ToString() : "";

                // Chọn danh mục cha
                if (categoryData["DanhMucCha"] == DBNull.Value)
                {
                    cmbDanhMucCha.SelectedIndex = 0; // Chọn "Không có danh mục cha"
                }
                else
                {
                    string danhMucCha = categoryData["DanhMucCha"].ToString();
                    for (int i = 0; i < cmbDanhMucCha.Items.Count; i++)
                    {
                        DataRowView item = cmbDanhMucCha.Items[i] as DataRowView;
                        if (item != null && item["MaDanhMuc"] != DBNull.Value && item["MaDanhMuc"].ToString() == danhMucCha)
                        {
                            cmbDanhMucCha.SelectedIndex = i;
                            break;
                        }
                    }
                }
                txtSoLuongSach.Text = categoryData["SoLuongSach"].ToString();
                string trangThai = categoryData["TrangThai"].ToString();
                cmbTrangThai.SelectedIndex = (trangThai == "Hoạt động") ? 0 : 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
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
                string tenDanhMuc = txtTenDanhMuc.Text.Trim();
                string moTa = txtMoTa.Text.Trim();

                // Xử lý danh mục cha (có thể là null)
                object selectedValue = cmbDanhMucCha.SelectedValue;
                string danhMucCha = null;

                if (selectedValue != null && selectedValue != DBNull.Value)
                {
                    danhMucCha = selectedValue.ToString();
                }

                // Kiểm tra không cho phép chọn chính nó làm danh mục cha
                if (danhMucCha == maDanhMuc)
                {
                    MessageBox.Show("Không thể chọn chính danh mục này làm danh mục cha!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int soLuongSach = Convert.ToInt32(txtSoLuongSach.Text.Trim());
                string trangThai = cmbTrangThai.SelectedItem.ToString();

                string err = "";
                bool success = dbCategory.SuaDanhMuc(ref err, maDanhMuc, tenDanhMuc, moTa, danhMucCha, soLuongSach, trangThai);

                if (success)
                {
                    MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi cập nhật danh mục: " + err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTenDanhMuc.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDanhMuc.Focus();
                return false;
            }

            if (txtTenDanhMuc.Text.Length > 255)
            {
                MessageBox.Show("Tên danh mục không được vượt quá 255 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDanhMuc.Focus();
                return false;
            }

            if (txtMoTa.Text.Length > 500)
            {
                MessageBox.Show("Mô tả không được vượt quá 500 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMoTa.Focus();
                return false;
            }

            if (!int.TryParse(txtSoLuongSach.Text, out int soLuong) || soLuong < 0)
            {
                MessageBox.Show("Số lượng sách phải là số nguyên không âm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongSach.Focus();
                return false;
            }
            return true;
        }

        private void txtMaDanhMuc_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblMoTa_Click(object sender, EventArgs e)
        {

        }

        private void lblMaDanhMuc_Click(object sender, EventArgs e)
        {

        }

        private void lblTrangThai_Click(object sender, EventArgs e)
        {

        }

        private void cmbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblTenDanhMuc_Click(object sender, EventArgs e)
        {

        }

        private void txtMoTa_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenDanhMuc_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblSoLuongSach_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblDanhMucCha_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbDanhMucCha_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSoLuongSach_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
