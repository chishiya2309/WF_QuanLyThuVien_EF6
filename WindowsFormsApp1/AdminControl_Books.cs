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

namespace WindowsFormsApp1
{
    public partial class AdminControl_Books : UserControl
    {
        DBBooks dbs;
        // Đối tượng đưa dữ liệu vào DataTable dtThanhPho 
        DataTable dtSach = null;

        DBCategory dbdm;
        // Đối tượng đưa dữ liệu vào DataTable dtDanhMuc
        DataTable dtDanhMuc = null;
        public AdminControl_Books()
        {
            InitializeComponent();
            Adjust();
            dbs = new DBBooks();
            dbdm = new DBCategory();

            dtDanhMuc = new DataTable();
            dtDanhMuc.Clear();
            dtDanhMuc = dbdm.LayDanhMuc().Tables[0];

            (booksGridView.Columns["MaDanhMuc"] as DataGridViewComboBoxColumn).DataSource = dtDanhMuc;
            (booksGridView.Columns["MaDanhMuc"] as DataGridViewComboBoxColumn).DisplayMember = "TenDanhMuc";
            (booksGridView.Columns["MaDanhMuc"] as DataGridViewComboBoxColumn).ValueMember = "MaDanhMuc";

            dtSach = new DataTable();
            dtSach.Clear();
            dtSach = dbs.LaySach().Tables[0];


            // Đưa dữ liệu lên DataGridView  
            booksGridView.DataSource = dtSach;
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            ShowAddBookForm();
        }

        private void ShowAddBookForm()
        {
            FormAddBook formAddBook = new FormAddBook();
            if (formAddBook.ShowDialog() == DialogResult.OK)
            {
                // Reload lại dữ liệu
                LoadData();
            }
        }

        private void Adjust()
        {
            searchPanel.Width = panel.Width - 40;
            booksGridView.Size = new Size(panel.Width - 40, panel.Height - 280);
            MenuButton.Width = panel.Width - 40;
            lblNoData.Location = new Point(
        booksGridView.Location.X + (booksGridView.Width - lblNoData.Width) / 2,
        booksGridView.Location.Y + (booksGridView.Height - lblNoData.Height) / 2
    );
        }

        private void panel_Resize(object sender, EventArgs e)
        {
            Adjust();
        }

        private void xemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewBookDetails();
        }

        private void ViewBookDetails()
        {
            MessageBox.Show("Chức năng xem chi tiết sách vẫn đang được phát triển. Vui lòng đợi thêm.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void booksGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ViewBookDetails();
            }
        }

        private void chỉnhSửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedBook();
        }

        private void EditSelectedBook()
        {
            if (booksGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sách cần chỉnh sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Lấy dòng đã chọn
                DataGridViewRow selectedRow = booksGridView.SelectedRows[0];

                // Lấy mã sách để tìm kiếm trong DataTable
                string maSach = selectedRow.Cells["MaSach"].Value.ToString();

                // Tìm dòng dữ liệu trong DataTable
                DataRow[] rows = dtSach.Select($"MaSach = '{maSach}'");
                if (rows.Length > 0)
                {
                    // Tạo và hiển thị form chỉnh sửa với dữ liệu sách đã chọn
                    FormEditBook formEditBook = new FormEditBook(rows[0]);

                    if (formEditBook.ShowDialog() == DialogResult.OK)
                    {
                        // Cập nhật dữ liệu trong DataGridView
                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin sách!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chỉnh sửa thông tin sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnDeleteBook_Click(sender, e);
        }

        private void cậpNhậtTrạngTháiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng cập nhật trạng thái sách vẫn đang được phát triển.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dtDanhMuc = new DataTable();
            dtDanhMuc.Clear();
            dtDanhMuc = dbdm.LayDanhMuc().Tables[0];

            (booksGridView.Columns["MaDanhMuc"] as DataGridViewComboBoxColumn).DataSource = dtDanhMuc;
            (booksGridView.Columns["MaDanhMuc"] as DataGridViewComboBoxColumn).DisplayMember = "TenDanhMuc";
            (booksGridView.Columns["MaDanhMuc"] as DataGridViewComboBoxColumn).ValueMember = "MaDanhMuc";

            dtSach = new DataTable();
            dtSach.Clear();
            dtSach = dbs.LaySach().Tables[0];


            // Đưa dữ liệu lên DataGridView  
            booksGridView.DataSource = dtSach;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadBooks(searchTerm);
        }

        private void LoadBooks(string searchTerm = "")
        {
            try
            {
                dtSach = dbs.LaySach().Tables[0];

                var filteredBooks = dtSach.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        searchTerm = searchTerm.ToLower();
                        filteredBooks = filteredBooks.Where(row =>
                            row.Field<string>("MaSach")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("TenSach")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("TacGia")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("NXB")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<int>("NamXuatBan").ToString().Contains(searchTerm) == true ||
                            row.Field<string>("ViTri")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("ISBN")?.ToLower().Contains(searchTerm) == true);
                    }
                }

                DataTable filteredTable = filteredBooks.Any() ? filteredBooks.CopyToDataTable() : dtSach.Clone();

                // Kiểm tra nếu không có dữ liệu thì ẩn DataGridView, hiển thị Label
                if (filteredTable.Rows.Count == 0)
                {
                    lblNoData.Visible = true;
                    booksGridView.Visible = false;
                }
                else
                {
                    lblNoData.Visible = false;
                    booksGridView.Visible = true;
                }

                booksGridView.DataSource = filteredTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void btnEditBook_Click_1(object sender, EventArgs e)
        {
            EditSelectedBook();
        }

        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            if (booksGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sách cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Lấy dòng đã chọn
                DataGridViewRow selectedRow = booksGridView.SelectedRows[0];
                // Lấy mã sách cần xóa
                string maSach = selectedRow.Cells["MaSach"].Value.ToString();
                string tenSach = selectedRow.Cells["TenSach"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa sách '{tenSach}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string err = "";
                    // Thực hiện xóa sách từ cơ sở dữ liệu
                    bool success = dbs.XoaSach(ref err, maSach);

                    if (success)
                    {
                        // Cập nhật dữ liệu trong DataGridView sau khi xóa
                        LoadData();

                        MessageBox.Show($"Đã xóa sách '{tenSach}' thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa sách. Sách có thể đang được mượn hoặc có ràng buộc dữ liệu khác!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void booksGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
