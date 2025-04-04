using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using DataAccessLayer;
using BusinessAccessLayer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class AdminControl_Categories: UserControl
    {
        DBCategory dbdm;
        // Đối tượng đưa dữ liệu vào DataTable dtDanhMuc
        DataTable dtDanhMuc = null;
        public AdminControl_Categories()
        {
            InitializeComponent();
            Adjust();
            dbdm = new DBCategory();
            LoadData();
        }



        private void Adjust()
        {
            searchPanel.Size = new Size(panel.Width - 40, 60);
            categoriesGridView.Size = new Size(panel.Width - 40, panel.Height - 280);
            MenuButton.Width = panel.Width - 40;
            lblNoData.Location = new Point(
       categoriesGridView.Location.X + (categoriesGridView.Width - lblNoData.Width) / 2,
      categoriesGridView.Location.Y + (categoriesGridView.Height - lblNoData.Height) / 2);
        }

        private void membersGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void searchPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            ShowAddCategoryForm();
        }

        private void ShowAddCategoryForm()
        {
            FormAddCategory formAddCategory = new FormAddCategory();
            if(formAddCategory.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }    
        }

        private void LoadData()
        {
            dtDanhMuc = new DataTable();
            dtDanhMuc.Clear();
            dtDanhMuc = dbdm.LayDanhMuc().Tables[0];

            (categoriesGridView.Columns["DanhMucCha"] as DataGridViewComboBoxColumn).DataSource = dtDanhMuc;
            (categoriesGridView.Columns["DanhMucCha"] as DataGridViewComboBoxColumn).DisplayMember = "TenDanhMuc";
            (categoriesGridView.Columns["DanhMucCha"] as DataGridViewComboBoxColumn).ValueMember = "MaDanhMuc";

            // Đưa dữ liệu lên DataGridView  
            categoriesGridView.DataSource = dtDanhMuc;
        }

        private void panel_Resize(object sender, EventArgs e)
        {
            Adjust();
        }

        private void xemChiTiếtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewCategoryDetails();
        }

        private void ViewCategoryDetails()
        {
            MessageBox.Show("Chức năng xem chi tiết danh mục sẽ được triển khai sau.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void categoriesGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) ViewCategoryDetails();
        }

        private void chỉnhSửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedCategory();
        }

        private void EditSelectedCategory()
        {
            if (categoriesGridView.SelectedRows.Count == 0) return;

            // Lấy dòng đang chọn
            DataGridViewRow selectedRow = categoriesGridView.SelectedRows[0];
            string maDanhMuc = selectedRow.Cells["MaDanhMuc"].Value.ToString();

           FormEditCategory formEditCategory = new FormEditCategory(maDanhMuc);
            if (formEditCategory.ShowDialog() == DialogResult.OK)
            {
                LoadData(); // Tải lại dữ liệu sau khi cập nhật thành công
            }
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedCategory();
        }

        private void DeleteSelectedCategory()
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa danh mục này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Chức năng xóa danh mục sẽ được triển khai sau.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void xemSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewCategoryBooks();
        }

        private void ViewCategoryBooks()
        {
            MessageBox.Show("Chức năng xem sách trong danh mục sẽ được triển khai sau.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            if (categoriesGridView.SelectedRows.Count > 0)
            {
                EditSelectedCategory();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một danh mục để chỉnh sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (categoriesGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Lấy dòng đã chọn
                DataGridViewRow selectedRow = categoriesGridView.SelectedRows[0];
                // Lấy mã danh mục cần xóa
                string maDanhMuc = selectedRow.Cells["MaDanhMuc"].Value.ToString();
                string tenDanhMuc = selectedRow.Cells["TenDanhMuc"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa danh mục '{tenDanhMuc}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string err = "";
                    // Thực hiện xóa sách từ cơ sở dữ liệu
                    bool success = dbdm.XoaDanhMuc(ref err, maDanhMuc);

                    if (success)
                    {
                        // Cập nhật dữ liệu trong DataGridView sau khi xóa
                        LoadData();

                        MessageBox.Show($"Đã xóa danh mục '{tenDanhMuc}' thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa danh mục. Danh mục có thể có ràng buộc dữ liệu khác!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa danh mục: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadCategory(searchTerm);
        }

        private void LoadCategory(string searchTerm)
        {
            try
            {
                dtDanhMuc = dbdm.LayDanhMuc().Tables[0];

                var filteredCategories = dtDanhMuc.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        searchTerm = searchTerm.ToLower();
                        filteredCategories = filteredCategories.Where(row =>
                            row.Field<string>("MaDanhMuc")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("MoTa")?.ToLower().Contains(searchTerm) == true ||

                            row.Field<string>("TenDanhMuc")?.ToLower().Contains(searchTerm) == true);
                    }
                }

                // Nếu không có dữ liệu, tạo DataTable rỗng thay vì CopyToDataTable() gây lỗi
                DataTable filteredCategory = filteredCategories.Any() ? filteredCategories.CopyToDataTable() : dtDanhMuc.Clone();

                // Kiểm tra nếu không có dữ liệu thì ẩn DataGridView, hiển thị Label
                if (filteredCategory.Rows.Count == 0)
                {
                    lblNoData.Visible = true;
                    categoriesGridView.Visible = false;
                }
                else
                {
                    lblNoData.Visible = false;
                    categoriesGridView.Visible = true;
                }

                categoriesGridView.DataSource = filteredCategory;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void categoriesGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra xem cột hiện tại có phải là cột "TrangThai" không
            if (categoriesGridView.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value is string trangThai)
            {
                switch (trangThai)
                {
                    case "Hoạt động":
                        e.CellStyle.ForeColor = Color.Green;
                        break;
                    case "Không hoạt động":
                        e.CellStyle.ForeColor = Color.Red;
                        break;
                    default:
                        e.CellStyle.ForeColor = Color.Black; // Mặc định nếu có giá trị khác
                        break;
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void categoriesGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
