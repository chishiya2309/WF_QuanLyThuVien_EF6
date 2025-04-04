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
    public partial class FormEditBook: Form
    {
        private DBBooks dbBooks;
        private DBCategory dbCategory;
        private DataTable dtDanhMuc;
        private string _currentMaSach;

        public FormEditBook(DataRow bookData)
        {
            InitializeComponent();
            dbBooks = new DBBooks();
            dbCategory = new DBCategory();

            _currentMaSach = bookData["MaSach"].ToString();

            LoadCategories();
            PopulateBookData(bookData);
        }
        private void LoadCategories()
        {
            try
            {
                dtDanhMuc = dbCategory.LayDanhMuc().Tables[0];
                cmbDanhMuc.DataSource = dtDanhMuc;
                cmbDanhMuc.DisplayMember = "TenDanhMuc";
                cmbDanhMuc.ValueMember = "MaDanhMuc";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateBookData(DataRow bookData)
        {
            txtMaSach.Text = bookData["MaSach"].ToString();
            txtMaSach.ReadOnly = true; // Không cho phép sửa mã sách
            txtISBN.Text = bookData["ISBN"].ToString();
            txtTenSach.Text = bookData["TenSach"].ToString();
            txtTacGia.Text = bookData["TacGia"].ToString();

            // Thiết lập giá trị cho ComboBox danh mục
            if (bookData["MaDanhMuc"] != DBNull.Value)
            {
                string maDanhMuc = (string)bookData["MaDanhMuc"];
                cmbDanhMuc.SelectedValue = maDanhMuc;
            }

            txtNamXuatBan.Text = bookData["NamXuatBan"].ToString();
            txtNXB.Text = bookData["NXB"].ToString();
            txtSoBan.Text = bookData["SoBan"].ToString();
            txtKhaDung.Text = bookData["KhaDung"].ToString();
            txtViTri.Text = bookData["ViTri"].ToString();
        }

        private void FormEditBook_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;
            try
            {
                string maSach = txtMaSach.Text.Trim();
                string isbn = txtISBN.Text.Trim();
                string tenSach = txtTenSach.Text.Trim();
                string tacGia = txtTacGia.Text.Trim();
                string maDanhMuc = (string)cmbDanhMuc.SelectedValue;
                int namXuatBan = Convert.ToInt32(txtNamXuatBan.Text.Trim());
                string nxb = txtNXB.Text.Trim();
                int soBan = Convert.ToInt32(txtSoBan.Text.Trim());
                int khaDung = Convert.ToInt32(txtKhaDung.Text.Trim());
                string viTri = txtViTri.Text.Trim();

                string err = "";
                bool success = dbBooks.SuaSach(ref err, maSach, isbn, tenSach, tacGia, maDanhMuc,
                                              namXuatBan, nxb, soBan, khaDung, viTri);
                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi cập nhật sách: " + err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            // Kiểm tra thông tin bắt buộc
            if (string.IsNullOrWhiteSpace(txtTenSach.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSach.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show("Vui lòng nhập mã ISBN!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtISBN.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTacGia.Text))
            {
                MessageBox.Show("Vui lòng nhập tên tác giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTacGia.Focus();
                return false;
            }

            // Kiểm tra định dạng số
            if (!int.TryParse(txtNamXuatBan.Text, out _))
            {
                MessageBox.Show("Năm xuất bản phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamXuatBan.Focus();
                return false;
            }

            if (!int.TryParse(txtSoBan.Text, out _))
            {
                MessageBox.Show("Số bản phải là số nguyên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoBan.Focus();
                return false;
            }

            if (!int.TryParse(txtKhaDung.Text, out int khaDung))
            {
                MessageBox.Show("Số lượng khả dụng phải là số nguyên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKhaDung.Focus();
                return false;
            }

            int soBan = int.Parse(txtSoBan.Text);
            if (khaDung > soBan)
            {
                MessageBox.Show("Số lượng khả dụng không thể lớn hơn tổng số bản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKhaDung.Focus();
                return false;
            }

            return true;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtISBN_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtMaSach_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtTacGia_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenSach_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void cmbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txtNamXuatBan_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txtNXB_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void txtSoBan_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtViTri_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void txtKhaDung_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
