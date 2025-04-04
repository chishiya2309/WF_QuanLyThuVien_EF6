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
    public partial class FormAddBook : Form
    {
        private DBBooks dbBooks;
        private DBCategory dbCategory;
        private DataTable dtDanhMuc;
        public FormAddBook()
        {
            InitializeComponent();
            dbBooks = new DBBooks();
            dbCategory = new DBCategory();
            LoadCategories();
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



        private void FormAddBook_Load(object sender, EventArgs e)
        {

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
                string viTri = txtViTri.Text.Trim();

                string err = "";
                bool success = dbBooks.ThemSach(ref err, maSach, isbn, tenSach, tacGia, maDanhMuc,
                                             namXuatBan, nxb, soBan, viTri);
                if (success)
                {
                    MessageBox.Show("Thêm sách mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi thêm sách: " + err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (string.IsNullOrWhiteSpace(txtMaSach.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSach.Focus();
                return false;
            }

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
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
